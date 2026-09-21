// ============================================================
//  Program.cs: the game loop, and nothing else.
//  Every job is done by a function that lives in another file.
// ============================================================

int maxHp = 10;
int levelNumber = 1;          // change to 4 to jump straight to MyLevel

// The party: three parallel lists. names[i], health[i] and attack[i] are one character.
List<string> names = new List<string>();
List<int> health = new List<int>();
List<int> attack = new List<int>();
MakeStartingParty(names, health, attack);
MyCharacter(names, health, attack);
int leader = StrongestIndex(attack);

// The monsters on the current level: five parallel lists. The boss, if there is one, is in here too.
List<string> mNames = new List<string>();
List<int> mHealth = new List<int>();
List<int> mAttack = new List<int>();
List<int> mX = new List<int>();
List<int> mY = new List<int>();
int boss = -1;
int bossMaxHp = 1;
int bossPhase = 1;

List<string> inventory = new List<string>();
MakeStartingInventory(inventory);
int partyDefense = 0;

// Magic: the spell book is two parallel lists.
int maxMana = 6;
int mana = maxMana;
List<string> spellNames = new List<string>();
List<int> spellCosts = new List<int>();
MakeSpellBook(spellNames, spellCosts);
MySpell(spellNames, spellCosts);

// Powerups that are switched on, and how many turns each has left.
List<string> powerNames = new List<string>();
List<int> powerTurns = new List<int>();

// Talking: six parallel lists for this level's conversation, and what the game remembers.
List<string> says = new List<string>();
List<string> choiceA = new List<string>();
List<int> nextA = new List<int>();
List<string> choiceB = new List<string>();
List<int> nextB = new List<int>();
List<string> gives = new List<string>();
List<string> flags = new List<string>();
int talkLine = -1;            // -1 means nobody is talking
bool lineIsNew = false;       // true until this line's action has happened

List<string> level = new List<string>();
int playerX = 0;
int playerY = 0;
int turns = 0;
bool needLevel = true;
int questSteps = 0;            // Quest 5's challenge keeps its count here

PrintTitle();
PrintControls();
QuestsAtStart();

bool playing = true;
while (playing)
{
    if (needLevel)
    {
        level = LoadLevel(levelNumber);
        playing = CheckLevel(level);
        SpawnMonsters(level, levelNumber, mNames, mHealth, mAttack, mX, mY);
        LoadDialogue(levelNumber, says, choiceA, nextA, choiceB, nextB, gives);
        if (CountTiles(level, 'N') > 0 && !CheckDialogue(levelNumber, says, choiceA, nextA, choiceB, nextB, gives)) playing = false;
        boss = FindBoss(level, mX, mY);
        bossPhase = 1;
        if (boss != -1) bossMaxHp = mHealth[boss];
        playerX = FindTileX(level, '@');
        playerY = FindTileY(level, '@');
        mana = maxMana;
        PrintLevelBanner(levelNumber);
        needLevel = false;
    }
    else if (talkLine != -1)
    {
        // Someone is talking. Show their line, read an answer, move to the next line.
        PrintDialogue(NpcName(levelNumber), says, choiceA, choiceB, talkLine);
        if (lineIsNew)
        {
            DoAction(gives[talkLine], levelNumber, flags, inventory, health, maxHp, powerNames, powerTurns);
            mana = ManaAfterAction(gives[talkLine], mana, maxMana);
            lineIsNew = false;
        }

        string answer = ReadCommand();
        int nextLine = NextLine(choiceA, nextA, choiceB, nextB, talkLine, answer);
        if (answer == "quit")
        {
            talkLine = -1;
            playing = false;
        }
        else if (nextLine == talkLine)
        {
            Console.WriteLine("Type 1 or 2.");
        }
        else
        {
            talkLine = nextLine;
            lineIsNew = true;
        }
    }
    else
    {
        DrawLevel(level, playerX, playerY);
        PrintParty(names, health, maxHp);
        Console.WriteLine(ManaBar(mana, maxMana));
        if (IsDoorSealed(mHealth, boss)) Console.WriteLine(BossBar(mNames[boss], mHealth[boss], bossMaxHp));
        PrintInventory(inventory);
        PrintPowerups(powerNames, powerTurns);
        Console.WriteLine("Leader: " + names[leader] + "   Party of " + PartySize(names) + "   Total health: " + TotalHealth(health));
        QuestsOnHud(names, health, leader, inventory);

        string command = ReadCommand();
        questSteps = CountStep(questSteps, command);
        string verb = FirstWord(command);
        int newX = playerX + StepX(command);
        int newY = playerY + StepY(command);
        char target = TileAt(level, newX, newY);
        partyDefense = Clamp(CountItem(inventory, "shield"), 0, 3);
        int might = PowerupBonus(powerNames, "might", 2);
        int guard = PowerupBonus(powerNames, "guard", 2);
        bool tookTurn = false;

        if (command == "quit")
        {
            playing = false;
        }
        else if (command == "help")
        {
            PrintControls();
            PrintLegend();
        }
        else if (command == "quests")
        {
            PrintQuestBoard(names, health, attack, leader, maxHp, inventory, mana, maxMana, level, playerX, playerY, levelNumber, questSteps);
        }
        else if (command == "spells")
        {
            PrintSpells(spellNames, spellCosts);
        }
        else if (command == "export")
        {
            ExportGame(maxHp, maxMana);
        }
        else if (command == "potion")
        {
            DrinkPotion(inventory, names, health, leader, maxHp);
        }
        else if (verb == "lead")
        {
            leader = ChooseLeader(names, health, leader, RestOfWords(command));
        }
        else if (verb == "fire")
        {
            tookTurn = Shoot(inventory, "arrow", level, names, leader, mNames, mHealth, mX, mY, playerX, playerY, StepX(SecondWord(command)), StepY(SecondWord(command)), might, MonsterDefense(levelNumber));
        }
        else if (verb == "throw")
        {
            tookTurn = Shoot(inventory, "stone", level, names, leader, mNames, mHealth, mX, mY, playerX, playerY, StepX(SecondWord(command)), StepY(SecondWord(command)), might, MonsterDefense(levelNumber));
        }
        else if (verb == "cast")
        {
            int cost = CastSpell(SecondWord(command), spellNames, spellCosts, mana, level, names, health, attack, leader, maxHp, mNames, mHealth, mX, mY, playerX, playerY, StepX(ThirdWord(command)), StepY(ThirdWord(command)));
            mana = mana - cost;
            tookTurn = cost > 0;
        }
        else if (newX == playerX && newY == playerY)
        {
            Console.WriteLine("I don't know that command. Type help.");
        }
        else if (IsWall(level, newX, newY))
        {
            Console.WriteLine("You bump into a wall.");
        }
        else if (target == 'N')
        {
            int state = QuestState(flags, inventory, mHealth, levelNumber);
            talkLine = DialogueStart(levelNumber, state);
            lineIsNew = true;
        }
        else if (target == '+' && IsDoorSealed(mHealth, boss))
        {
            Console.WriteLine("The door is sealed while " + mNames[boss] + " lives.");
        }
        else if (target == 'M' || target == 'B')
        {
            int m = MonsterAt(mX, mY, newX, newY);
            HeroAttacks(names, attack, leader, mNames, mHealth, m, MonsterDefense(levelNumber), might);
            if (IsAlive(mHealth[m]))
            {
                MonsterAttacks(mNames, mAttack, m, names, health, leader, partyDefense + guard);
                Console.WriteLine("Winning so far: " + WinnerName(names[leader], health[leader], mNames[m], mHealth[m]));
            }
            tookTurn = true;
        }
        else
        {
            playerX = newX;
            playerY = newY;
            tookTurn = true;
            if (target == '$')
            {
                string item = RandomLoot();
                AddLoot(inventory, item);
                if (item == "arrow") AddMany(inventory, "arrow", 2);
                SetTile(level, newX, newY, '.');
            }
            else if (target == '*')
            {
                AddPowerup(powerNames, powerTurns, PowerupName((newX + newY) % 3), 10);
                SetTile(level, newX, newY, '.');
            }
            else if (target == '+')
            {
                levelNumber = levelNumber + 1;
                needLevel = true;
                tookTurn = false;
                if (levelNumber > LevelCount())
                {
                    PrintWin(CountItem(inventory, "gold"));
                    playing = false;
                }
            }
        }

        // ---------- the end of a turn ----------
        if (tookTurn)
        {
            turns = turns + 1;
            mana = ManaAfterTurn(mana, maxMana, turns);
            ClearDefeated(level, mNames, mHealth, mX, mY, inventory);
            if (boss != -1 && !IsAlive(mHealth[boss]) && !HasFlag(flags, "boss down " + levelNumber))
            {
                SetFlag(flags, "boss down " + levelNumber);
                Console.WriteLine("The door is open.");
            }

            if (IsDoorSealed(mHealth, boss))
            {
                int phase = BossPhase(mHealth[boss], bossMaxHp);
                if (phase != bossPhase)
                {
                    bossPhase = phase;
                    PrintBossPhase(mNames[boss], phase);
                    if (phase == 3) mAttack[boss] = mAttack[boss] + 2;
                }
                if (IsShockwaveTurn(phase, turns)) Shockwave(mNames[boss], names, health, Damage(2, PowerupBonus(powerNames, "guard", 2)));
            }

            if (IsAlive(health[leader])) health[leader] = Clamp(health[leader] + PowerupBonus(powerNames, "regen", 1), 0, maxHp);
            TickPowerups(powerNames, powerTurns);

            if (!IsAlive(health[leader]))
            {
                Console.WriteLine(names[leader] + " has fallen!");
                leader = NextAliveIndex(health);
            }
            if (!AnyAlive(health))
            {
                PrintGameOver();
                playing = false;
            }
        }
    }
}
