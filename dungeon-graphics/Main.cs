// ======================================================================
//  Main.cs: the graphical game loop.  TEACHER / ENGINE CODE.
//
//  This plays the SAME game as ../dungeon/Program.cs, using the SAME functions
//  from the students' files (LoadLevel, TileAt, IsWall, MonsterAt, HeroAttacks,
//  MonsterAttacks, DrinkPotion, Shoot, CastSpell, DoAction, BossPhase, MyLevel,
//  MyCharacter, MySpell, MyDialogue ...). If a student rewrites Damage or Heal, draws
//  a new MyLevel or writes a new conversation, this game changes with it.
//
//  Keys: arrows or WASD move. Walk into a monster to attack, into a person to talk.
//        F arrow, T stone, 1 to 4 spells (all in the direction you are facing).
//        P potion, TAB leader, M music. In a conversation: 1, 2 or ENTER.
//
//  Anything the students' code prints appears in the message box at the bottom.
//
//  Run it from this folder:      dotnet run
//  Jump straight to a level:     dotnet run -- 4
// ======================================================================

int maxHp = 10;
int firstLevel = 1;
if (args.Length > 0 && int.TryParse(args[0], out int asked)) firstLevel = Math.Clamp(asked, 1, LevelCount());

List<string> names = new List<string>();
List<int> health = new List<int>();
List<int> attack = new List<int>();
List<string> mNames = new List<string>();
List<int> mHealth = new List<int>();
List<int> mAttack = new List<int>();
List<int> mX = new List<int>();
List<int> mY = new List<int>();
List<int> mMaxHealth = new List<int>();
List<string> inventory = new List<string>();
List<string> spellNames = new List<string>();
List<int> spellCosts = new List<int>();
List<string> powerNames = new List<string>();
List<int> powerTurns = new List<int>();
List<string> says = new List<string>();
List<string> choiceA = new List<string>();
List<int> nextA = new List<int>();
List<string> choiceB = new List<string>();
List<int> nextB = new List<int>();
List<string> gives = new List<string>();
List<string> flags = new List<string>();
List<string> level = new List<string>();
List<string> oldLevel = new List<string>();

int levelNumber = firstLevel;
int oldLevelNumber = firstLevel;
int leader = 0;
int partyDefense = 0;
int playerX = 0, playerY = 0;
int fromX = 0, fromY = 0;
int facing = 0;                 // 0 down, 1 left, 2 right, 3 up
int target = -1;                // index of the monster being fought
bool levelIsBroken = false;
int maxMana = 6, mana = 6, turns = 0;
int boss = -1, bossMaxHp = 1, bossPhase = 1;
int talkLine = -1;
bool lineIsNew = false;

// A shot in flight: an arrow, a stone or an aimed spell.
string shotKind = "";           // "arrow", "stone", or "spell_" + the spell's name
string shotSpell = "";
int shotStepX = 0, shotStepY = 0, shotLength = 0;
float shake = 0;

string mode = "title";          // title, play, walk, bump, swing, counter, shot, talk, slide, won, lost, broken
float timer = 0;
bool applied = false;
float playerFlash = 0, monsterFlash = 0, bumpQuiet = 0;
float oldCamX = 0, oldCamY = 0;
int oldTile = 32;
int walkFrame = 0;

const float WalkTime = 0.14f, BumpTime = 0.12f, SwingTime = 0.24f, CounterTime = 0.30f, SlideTime = 0.70f, ShotTimePerTile = 0.045f;

// Small levels are drawn with big tiles, so an 8 x 3 student level still fills the screen.
int GfxTileFor(List<string> lvl)
{
    int cols = lvl.Count > 0 ? lvl[0].Length : 1;
    if (cols * 64 <= Engine.ScreenW && lvl.Count * 64 <= Engine.ViewH) return 64;
    if (cols * 48 <= Engine.ScreenW && lvl.Count * 48 <= Engine.ViewH) return 48;
    return 32;
}

void GfxStartLevel()
{
    level = LoadLevel(levelNumber);
    Engine.Tile = GfxTileFor(level);
    Engine.CurrentLevel = levelNumber;
    levelIsBroken = !CheckLevel(level);
    SpawnMonsters(level, levelNumber, mNames, mHealth, mAttack, mX, mY);
    LoadDialogue(levelNumber, says, choiceA, nextA, choiceB, nextB, gives);
    if (CountTiles(level, 'N') > 0 && !CheckDialogue(levelNumber, says, choiceA, nextA, choiceB, nextB, gives)) levelIsBroken = true;
    boss = FindBoss(level, mX, mY);
    bossPhase = 1;
    bossMaxHp = boss != -1 ? mHealth[boss] : 1;
    mana = maxMana;
    talkLine = -1;
    mMaxHealth.Clear();
    for (int i = 0; i < mHealth.Count; i++) mMaxHealth.Add(mHealth[i]);
    playerX = FindTileX(level, '@');
    playerY = FindTileY(level, '@');
    fromX = playerX;
    fromY = playerY;
    PrintLevelBanner(levelNumber);
    Engine.PlayMusic(levelNumber);
}

void GfxNewGame()
{
    names.Clear(); health.Clear(); attack.Clear(); inventory.Clear();
    spellNames.Clear(); spellCosts.Clear(); powerNames.Clear(); powerTurns.Clear(); flags.Clear();
    MakeSpellBook(spellNames, spellCosts);
    MySpell(spellNames, spellCosts);
    turns = 0;
    MakeStartingParty(names, health, attack);
    MyCharacter(names, health, attack);
    leader = StrongestIndex(attack);
    MakeStartingInventory(inventory);
    partyDefense = 0;
    levelNumber = firstLevel;
    facing = 0;
    GfxStartLevel();
}

float GfxCamX(List<string> lvl, float px)
{
    float width = (lvl.Count > 0 ? lvl[0].Length : 1) * Engine.Tile;
    if (width <= Engine.ScreenW) return -(Engine.ScreenW - width) / 2f;
    return Math.Clamp(px + Engine.Tile / 2f - Engine.ScreenW / 2f, 0, width - Engine.ScreenW);
}

float GfxCamY(List<string> lvl, float py)
{
    float height = lvl.Count * Engine.Tile;
    if (height <= Engine.ViewH) return -(Engine.ViewH - height) / 2f;
    return Math.Clamp(py + Engine.Tile / 2f - Engine.ViewH / 2f, 0, height - Engine.ViewH);
}

void GfxDrawMap(List<string> lvl, int number, float offX, float offY)
{
    for (int y = 0; y < lvl.Count; y++)
    {
        for (int x = 0; x < lvl[y].Length; x++)
        {
            float sx = x * Engine.Tile + offX, sy = y * Engine.Tile + offY;
            if (sx < -Engine.Tile || sx > Engine.ScreenW || sy < -Engine.Tile || sy > Engine.ScreenH) continue;
            char tile = TileAt(lvl, x, y);
            if (tile == '#') Engine.DrawTile("wall", number, sx, sy);
            else
            {
                Engine.DrawTile("floor", number, sx, sy);
                if (tile == '+') Engine.DrawTile("door", number, sx, sy);
                else if (tile == '$') Engine.DrawTile("loot", number, sx, sy);
                else if (tile == '*') Engine.DrawTile("powerup", number, sx, sy);
                else if (tile == 'N') Engine.DrawTile("npc", number, sx, sy);
            }
        }
    }
}

// How many squares does a shot travel before it stops? It stops ON a monster, or just before a wall.
int GfxShotLength(int stepX, int stepY, int range)
{
    for (int i = 1; i <= range; i++)
    {
        char tile = TileAt(level, playerX + stepX * i, playerY + stepY * i);
        if (tile == '#' || tile == 'N' || tile == '+') return i - 1;
        if (tile == 'M' || tile == 'B') return i;
    }
    return range;
}

int GfxAlive()
{
    int alive = 0;
    for (int i = 0; i < mHealth.Count; i++) if (IsAlive(mHealth[i])) alive++;
    return alive;
}

// The same end of turn as Program.cs, with sounds.
void GfxEndTurn()
{
    turns = turns + 1;
    mana = ManaAfterTurn(mana, maxMana, turns);

    int onMap = CountTiles(level, 'M') + CountTiles(level, 'B');
    ClearDefeated(level, mNames, mHealth, mX, mY, inventory);
    if (CountTiles(level, 'M') + CountTiles(level, 'B') < onMap) Engine.Sfx("defeat");
    if (boss != -1 && !IsAlive(mHealth[boss]) && !HasFlag(flags, "boss down " + levelNumber))
    {
        SetFlag(flags, "boss down " + levelNumber);
        Engine.Sfx("door");
        Console.WriteLine("The door is open.");
    }

    int guard = PowerupBonus(powerNames, "guard", 2);
    if (IsDoorSealed(mHealth, boss))
    {
        int phase = BossPhase(mHealth[boss], bossMaxHp);
        if (phase != bossPhase)
        {
            bossPhase = phase;
            PrintBossPhase(mNames[boss], phase);
            if (phase == 3) mAttack[boss] = mAttack[boss] + 2;
            Engine.Sfx("roar");
            shake = 0.5f;
        }
        if (IsShockwaveTurn(phase, turns))
        {
            int before = health[leader];
            Shockwave(mNames[boss], names, health, Damage(2, guard));
            Engine.Sfx("quake");
            shake = 0.35f;
            playerFlash = 0.3f;
            Engine.Float("-" + (before - health[leader]), playerX * Engine.Tile + 10, playerY * Engine.Tile, 255, 110, 110);
        }
    }

    if (IsAlive(health[leader])) health[leader] = Clamp(health[leader] + PowerupBonus(powerNames, "regen", 1), 0, maxHp);
    TickPowerups(powerNames, powerTurns);

    if (!IsAlive(health[leader]))
    {
        Console.WriteLine(names[leader] + " has fallen!");
        if (AnyAlive(health)) leader = NextAliveIndex(health);
    }
    if (!AnyAlive(health))
    {
        PrintGameOver();
        Engine.Sfx("lose");
        mode = "lost";
        timer = 0;
    }
}

Engine.Open("Dungeon Crawler");
PrintTitle();
GfxNewGame();

while (Engine.Running())
{
    float dt = Engine.Delta();
    timer += dt;
    if (playerFlash > 0) playerFlash -= dt;
    if (monsterFlash > 0) monsterFlash -= dt;
    if (bumpQuiet > 0) bumpQuiet -= dt;
    if (Engine.Pressed("mute")) Engine.ToggleMute();

    int dirX = facing == 1 ? -1 : facing == 2 ? 1 : 0;
    int dirY = facing == 3 ? -1 : facing == 0 ? 1 : 0;

    // ---------------------------------------------------------------- update
    if (levelIsBroken && mode != "broken") mode = "broken";

    if (mode == "title")
    {
        if (Engine.Pressed("confirm")) mode = "play";
    }
    else if (mode == "play")
    {
        if (Engine.Pressed("potion"))
        {
            int before = health[leader];
            DrinkPotion(inventory, names, health, leader, maxHp);
            if (health[leader] > before)
            {
                Engine.Sfx("potion");
                Engine.Float("+" + (health[leader] - before), playerX * Engine.Tile + 8, playerY * Engine.Tile, 120, 240, 140);
            }
        }
        else if (Engine.Pressed("leader"))
        {
            for (int k = 1; k <= names.Count; k++)
            {
                int next = (leader + k) % names.Count;
                if (IsAlive(health[next]))
                {
                    leader = ChooseLeader(names, health, leader, names[next]);
                    break;
                }
            }
        }
        else if (Engine.Pressed("fire") || Engine.Pressed("throw"))
        {
            string ammo = Engine.Pressed("throw") && !Engine.Pressed("fire") ? "stone" : "arrow";
            if (CountItem(inventory, ammo) == 0)
            {
                Console.WriteLine("You have no " + ammo + "s.");
                Engine.Sfx("bump");
            }
            else
            {
                shotKind = ammo; shotSpell = "";
                shotStepX = dirX; shotStepY = dirY;
                shotLength = GfxShotLength(dirX, dirY, ShotRange(ammo));
                Engine.Sfx("shoot");
                mode = "shot"; timer = 0;
            }
        }
        else if (Engine.Pressed("spell1") || Engine.Pressed("spell2") || Engine.Pressed("spell3") || Engine.Pressed("spell4"))
        {
            int slot = Engine.Pressed("spell1") ? 0 : Engine.Pressed("spell2") ? 1 : Engine.Pressed("spell3") ? 2 : 3;
            if (slot < spellNames.Count)
            {
                string spell = spellNames[slot];
                if (!CanCast(mana, spellCosts[slot]))
                {
                    Console.WriteLine("Not enough mana. " + spell + " costs " + spellCosts[slot] + " and you have " + mana + ".");
                    Engine.Sfx("bump");
                }
                else if (IsAimedSpell(spell))
                {
                    shotKind = "spell_" + spell; shotSpell = spell;
                    shotStepX = dirX; shotStepY = dirY;
                    shotLength = GfxShotLength(dirX, dirY, 4);
                    Engine.Sfx("cast");
                    mode = "shot"; timer = 0;
                }
                else
                {
                    int before = health[leader];
                    int cost = CastSpell(spell, spellNames, spellCosts, mana, level, names, health, attack, leader, maxHp, mNames, mHealth, mX, mY, playerX, playerY, 0, 0);
                    mana = mana - cost;
                    Engine.Sfx(spell == "quake" ? "quake" : "potion");
                    if (spell == "quake") shake = 0.35f;
                    if (health[leader] > before) Engine.Float("+" + (health[leader] - before), playerX * Engine.Tile + 8, playerY * Engine.Tile, 120, 240, 140);
                    if (cost > 0) GfxEndTurn();
                }
            }
        }
        else
        {
            int stepX = Engine.HeldX();
            int stepY = stepX == 0 ? Engine.HeldY() : 0;
            if (stepX != 0 || stepY != 0)
            {
                facing = stepX < 0 ? 1 : stepX > 0 ? 2 : stepY < 0 ? 3 : 0;
                int newX = playerX + stepX, newY = playerY + stepY;
                char tile = TileAt(level, newX, newY);
                timer = 0;
                applied = false;
                if (IsWall(level, newX, newY))
                {
                    mode = "bump";
                    if (bumpQuiet <= 0) { Engine.Sfx("bump"); bumpQuiet = 0.4f; }
                }
                else if (tile == 'N')
                {
                    talkLine = DialogueStart(levelNumber, QuestState(flags, inventory, mHealth, levelNumber));
                    lineIsNew = true;
                    Engine.Sfx("talk");
                    mode = "talk";
                }
                else if (tile == '+' && IsDoorSealed(mHealth, boss))
                {
                    mode = "bump";
                    if (bumpQuiet <= 0)
                    {
                        Console.WriteLine("The door is sealed while " + mNames[boss] + " lives.");
                        Engine.Sfx("bump");
                        bumpQuiet = 0.8f;
                    }
                }
                else if ((tile == 'M' || tile == 'B') && MonsterAt(mX, mY, newX, newY) != -1)
                {
                    target = MonsterAt(mX, mY, newX, newY);
                    mode = "swing";
                }
                else
                {
                    fromX = playerX; fromY = playerY;
                    playerX = newX; playerY = newY;
                    walkFrame++;
                    mode = "walk";
                }
            }
        }
    }
    else if (mode == "bump")
    {
        if (timer >= BumpTime) mode = "play";
    }
    else if (mode == "walk")
    {
        if (timer >= WalkTime)
        {
            fromX = playerX; fromY = playerY;
            mode = "play";
            char tile = TileAt(level, playerX, playerY);
            if (tile == '$')
            {
                string item = RandomLoot();
                AddLoot(inventory, item);
                if (item == "arrow") AddMany(inventory, "arrow", 2);
                SetTile(level, playerX, playerY, '.');
                Engine.Sfx("loot");
                Engine.Float(item + "!", playerX * Engine.Tile - 4, playerY * Engine.Tile, 250, 220, 110);
            }
            else if (tile == '*')
            {
                string power = PowerupName((playerX + playerY) % 3);
                AddPowerup(powerNames, powerTurns, power, 10);
                SetTile(level, playerX, playerY, '.');
                Engine.Sfx("powerup");
                Engine.Float(power + "!", playerX * Engine.Tile - 4, playerY * Engine.Tile, 250, 214, 80);
            }
            else if (tile == '+')
            {
                Engine.Sfx("door");
                if (levelNumber >= LevelCount())
                {
                    PrintWin(CountItem(inventory, "gold"));
                    Engine.Sfx("win");
                    mode = "won";
                }
                else
                {
                    oldLevel = new List<string>(level);
                    oldLevelNumber = levelNumber;
                    oldTile = Engine.Tile;
                    oldCamX = GfxCamX(level, playerX * Engine.Tile);
                    oldCamY = GfxCamY(level, playerY * Engine.Tile);
                    levelNumber = levelNumber + 1;
                    GfxStartLevel();
                    mode = "slide";
                }
                timer = 0;
            }
            if (mode == "play") GfxEndTurn();
        }
    }
    else if (mode == "shot")
    {
        if (timer >= Math.Max(1, shotLength) * ShotTimePerTile)
        {
            int m = TargetInLine(level, mX, mY, playerX, playerY, shotStepX, shotStepY, shotSpell == "" ? ShotRange(shotKind) : 4);
            int before = m != -1 ? mHealth[m] : 0;
            bool took = true;
            if (shotSpell == "")
            {
                took = Shoot(inventory, shotKind, level, names, leader, mNames, mHealth, mX, mY, playerX, playerY, shotStepX, shotStepY, PowerupBonus(powerNames, "might", 2), MonsterDefense(levelNumber));
            }
            else
            {
                int cost = CastSpell(shotSpell, spellNames, spellCosts, mana, level, names, health, attack, leader, maxHp, mNames, mHealth, mX, mY, playerX, playerY, shotStepX, shotStepY);
                mana = mana - cost;
                took = cost > 0;
            }
            if (m != -1 && mHealth[m] < before)
            {
                target = m;
                monsterFlash = 0.25f;
                Engine.Sfx("hit");
                Engine.Float("-" + (before - mHealth[m]), mX[m] * Engine.Tile + 10, mY[m] * Engine.Tile, 255, 255, 255);
            }
            mode = "play";
            if (took) GfxEndTurn();
        }
    }
    else if (mode == "talk")
    {
        if (lineIsNew)
        {
            int manaBefore = mana;
            DoAction(gives[talkLine], levelNumber, flags, inventory, health, maxHp, powerNames, powerTurns);
            mana = ManaAfterAction(gives[talkLine], mana, maxMana);
            if (gives[talkLine] != "") Engine.Sfx(gives[talkLine].StartsWith("power") ? "powerup" : mana > manaBefore ? "potion" : "loot");
            lineIsNew = false;
        }
        string answer = "none";
        if (choiceA[talkLine] == "") { if (Engine.Pressed("confirm")) answer = ""; }
        else if (Engine.Pressed("choice1")) answer = "1";
        else if (Engine.Pressed("choice2")) answer = "2";
        if (answer != "none")
        {
            int next = NextLine(choiceA, nextA, choiceB, nextB, talkLine, answer);
            if (next != talkLine)
            {
                talkLine = next;
                lineIsNew = true;
                if (talkLine == -1) mode = "play";
                else Engine.Sfx("talk");
            }
        }
    }
    else if (mode == "swing")
    {
        if (!applied && timer >= SwingTime * 0.5f)
        {
            applied = true;
            int before = mHealth[target];
            HeroAttacks(names, attack, leader, mNames, mHealth, target, MonsterDefense(levelNumber), PowerupBonus(powerNames, "might", 2));
            Engine.Sfx("hit");
            monsterFlash = 0.25f;
            Engine.Float("-" + (before - mHealth[target]), mX[target] * Engine.Tile + 10, mY[target] * Engine.Tile, 255, 255, 255);
        }
        if (timer >= SwingTime)
        {
            timer = 0;
            applied = false;
            if (IsAlive(mHealth[target])) mode = "counter";
            else
            {
                mode = "play";
                GfxEndTurn();
            }
        }
    }
    else if (mode == "counter")
    {
        if (!applied && timer >= CounterTime * 0.5f)
        {
            applied = true;
            int before = health[leader];
            partyDefense = Clamp(CountItem(inventory, "shield"), 0, 3);
            MonsterAttacks(mNames, mAttack, target, names, health, leader, partyDefense + PowerupBonus(powerNames, "guard", 2));
            Engine.Sfx("hurt");
            playerFlash = 0.3f;
            Engine.Float("-" + (before - health[leader]), playerX * Engine.Tile + 10, playerY * Engine.Tile, 255, 110, 110);
        }
        if (timer >= CounterTime)
        {
            mode = "play";
            GfxEndTurn();
        }
    }
    else if (mode == "slide")
    {
        if (timer >= SlideTime) mode = "play";
    }
    else if (mode == "won" || mode == "lost")
    {
        if (Engine.Pressed("restart")) { GfxNewGame(); mode = "play"; }
    }

    // ---------------------------------------------------------------- draw
    Engine.BeginFrame();

    float t = mode == "walk" ? Math.Clamp(timer / WalkTime, 0, 1) : 1;
    float px = (fromX + (playerX - fromX) * t) * Engine.Tile;
    float py = (fromY + (playerY - fromY) * t) * Engine.Tile;
    if (mode == "bump")
    {
        float nudge = MathF.Sin(Math.Clamp(timer / BumpTime, 0, 1) * MathF.PI) * Engine.Tile * 0.16f;
        px += dirX * nudge; py += dirY * nudge;
    }
    if (mode == "swing")
    {
        float lunge = MathF.Sin(Math.Clamp(timer / SwingTime, 0, 1) * MathF.PI) * Engine.Tile * 0.2f;
        px += dirX * lunge; py += dirY * lunge;
    }

    float camX = GfxCamX(level, px), camY = GfxCamY(level, py);
    float slide = mode == "slide" ? Math.Clamp(timer / SlideTime, 0, 1) : 1;
    slide = slide * slide * (3 - 2 * slide);
    float offX = -camX + (1 - slide) * Engine.ScreenW;
    float offY = -camY + Engine.HudH;
    if (shake > 0)
    {
        shake -= dt;
        offX += MathF.Sin(Engine.Clock() * 90) * 4;
        offY += MathF.Cos(Engine.Clock() * 70) * 3;
    }

    if (mode == "slide")
    {
        int tileNow = Engine.Tile;
        Engine.Tile = oldTile;
        GfxDrawMap(oldLevel, oldLevelNumber, -oldCamX - slide * Engine.ScreenW, -oldCamY + Engine.HudH);
        Engine.Tile = tileNow;
    }
    GfxDrawMap(level, levelNumber, offX, offY);

    for (int i = 0; i < mNames.Count; i++)
    {
        if (!IsAlive(mHealth[i])) continue;
        float mx = mX[i] * Engine.Tile + offX, my = mY[i] * Engine.Tile + offY;
        if (mode == "counter" && i == target)
        {
            float lunge = MathF.Sin(Math.Clamp(timer / CounterTime, 0, 1) * MathF.PI) * Engine.Tile * 0.38f;
            mx -= dirX * lunge; my -= dirY * lunge;
        }
        int frame = (int)(Engine.Clock() * 3 + i);
        if (i == boss) Engine.DrawBoss(mNames[i], mx, my, frame, i == target ? monsterFlash : 0);
        else Engine.DrawActor(mNames[i], mx, my, frame, 0, i == target ? monsterFlash : 0);
        if (mHealth[i] < mMaxHealth[i]) Engine.DrawSmallBar(mx, my, mHealth[i], mMaxHealth[i]);
    }

    if (mode != "lost" && playerX >= 0)
    {
        Engine.DrawActor("player", px + offX, py + offY, mode == "walk" ? walkFrame : 0, facing, playerFlash);
        if (mode == "swing")
        {
            float swing = Math.Clamp(timer / SwingTime, 0, 1);
            float baseAngle = facing == 2 ? 0 : facing == 0 ? 90 : facing == 1 ? 180 : 270;
            float angle = baseAngle - 60 + 120 * swing;
            float rad = angle * MathF.PI / 180f;
            Engine.DrawSword(px + offX + MathF.Cos(rad) * Engine.Tile * 0.7f, py + offY + MathF.Sin(rad) * Engine.Tile * 0.7f, angle);
        }
    }
    if (mode == "shot")
    {
        float along = Math.Clamp(timer / ShotTimePerTile, 0, Math.Max(1, shotLength));
        float angle = shotStepX > 0 ? 0 : shotStepY > 0 ? 90 : shotStepX < 0 ? 180 : 270;
        Engine.DrawProjectile(shotKind, px + offX + shotStepX * along * Engine.Tile, py + offY + shotStepY * along * Engine.Tile, angle);
    }
    Engine.DrawFloats(dt, offX, offY);

    Engine.DrawHudBackground();
    for (int i = 0; i < names.Count; i++) Engine.DrawPartyMember(i, names[i], health[i], maxHp, i == leader);
    Engine.DrawCounters(levelNumber, CountItem(inventory, "potion"), CountItem(inventory, "shield"), CountItem(inventory, "gold"), CountItem(inventory, "arrow"), CountItem(inventory, "stone"));
    Engine.DrawMessages();
    Engine.DrawMana(mana, maxMana);
    Engine.DrawPowerups(powerNames, powerTurns);
    if (mode != "title" && mode != "broken") Engine.DrawSpellKeys(spellNames, spellCosts, mana);
    if (IsDoorSealed(mHealth, boss) && mode != "title") Engine.DrawBossBar(mNames[boss], mHealth[boss], bossMaxHp, bossPhase);
    if (mode == "talk" && talkLine != -1) Engine.DrawDialogue(NpcName(levelNumber), says[talkLine], choiceA[talkLine], choiceB[talkLine]);

    if (mode == "title")
    {
        Engine.Shade(170);
        Engine.TextCentered("DUNGEON CRAWLER", 200, 48, 255, 226, 120);
        Engine.TextCentered("Find the door on every level. Stay alive.", 270, 20, 220, 220, 235);
        Engine.TextCentered("Press ENTER to begin", 340, 20, 255, 255, 255);
    }
    else if (mode == "won")
    {
        Engine.Shade((int)Math.Clamp(timer * 300, 0, 170));
        Engine.TextCentered("YOU WIN!", 220, 48, 255, 226, 120);
        Engine.TextCentered("Gold: " + CountItem(inventory, "gold") + "      Press R to play again", 300, 20, 255, 255, 255);
    }
    else if (mode == "lost")
    {
        Engine.Shade((int)Math.Clamp(timer * 300, 0, 190));
        Engine.TextCentered("GAME OVER", 220, 48, 230, 80, 80);
        Engine.TextCentered("Press R to try again", 300, 20, 255, 255, 255);
    }
    else if (mode == "broken")
    {
        Engine.Shade(200);
        Engine.TextCentered("LEVEL " + levelNumber + " BREAKS A CONTRACT", 200, 30, 230, 80, 80);
        Engine.TextCentered("Read the LEVEL PROBLEM lines below (and in the terminal), fix the level, run again.", 260, 16, 255, 255, 255);
    }

    Engine.EndFrame();
}

Engine.Close();
