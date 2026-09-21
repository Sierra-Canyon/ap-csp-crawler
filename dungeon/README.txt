DUNGEON CRAWLER: every function in the game
============================================================
130 functions in 16 files.

HOW TO RUN        open a terminal in this folder and type:   dotnet run
HOW TO PLAY       w a s d + Enter to move. Walk into M to fight, into N to talk. Type help for the rest.
YOUR FILES        MyLevel.cs, MyCharacter.cs, MySpell.cs and MyDialogue.cs
YOUR QUESTS       Quest00.cs to Quest16.cs, one each class. They are not listed below: the sheet
                  for the day describes them. QuestBoard.cs is teacher code that calls them.
SKIP TO MY LEVEL  in Program.cs change   int levelNumber = 1;   to   = 4;

THE TILE ALPHABET   # wall   . floor   @ player start   M monster   $ loot   + door
                    N someone to talk to   * a powerup   B the boss (one at most)
A LEVEL IS          a List<string>, one string per row, read as level[y][x] (row first!)
A CHARACTER IS      an index i shared by names[i], health[i], attack[i]
A CONVERSATION IS   an index i shared by says[i], choiceA[i], nextA[i], choiceB[i], nextB[i], gives[i]

------------------------------------------------------------
Program.cs: the game loop and nothing else (no functions live here)
------------------------------------------------------------

------------------------------------------------------------
Screens.cs: title, messages, reading commands
------------------------------------------------------------
void PrintTitle()
      Prints the title banner.

void PrintControls()
      Prints the list of commands.

void PrintLevelBanner(int levelNumber)
      Prints the 'LEVEL 2' heading when a level starts.

void PrintGameOver()
      Prints the losing message.

void PrintWin(int gold)
      Prints the winning message and the gold you kept.

string ReadCommand()
      Reads one typed line. Answers "quit" if the input has ended.

int StepX(string command)
      Sideways movement for a command: a = -1, d = 1, anything else = 0.

int StepY(string command)
      Up/down movement for a command: w = -1, s = 1, anything else = 0.

string FirstWord(string text)
      The first word of a command: "cast spark d" -> "cast".

string RestOfWords(string text)
      Everything after the first word: "lead Mote" -> "Mote".

string SecondWord(string text)
      The second word of a command, or "" if there is none.

string ThirdWord(string text)
      The third word of a command, or "" if there is none.

------------------------------------------------------------
Levels.cs: the map
------------------------------------------------------------
List<string> MakeLevel1()
      Builds and returns template level 1.

List<string> MakeLevel2()
      Builds and returns template level 2.

List<string> MakeLevel3()
      Builds and returns template level 3.

int LevelCount()
      How many levels the game has (4; the last is MyLevel).

List<string> LoadLevel(int number)
      Returns the level with that number. The last number returns MyLevel().

char TileAt(List<string> level, int x, int y)
      The tile at column x, row y. Off the map counts as '#', so it never crashes.

bool IsWall(List<string> level, int x, int y)
      True if the tile at (x, y) is a wall. Built on TileAt.

int CountTiles(List<string> level, char tile)
      How many copies of one tile are in the level. Nested loop with a counter.

int FindTileX(List<string> level, char tile)
      Column of the first copy of a tile, or -1.

int FindTileY(List<string> level, char tile)
      Row of the first copy of a tile, or -1.

void SetTile(List<string> level, int x, int y, char tile)
      Replaces one tile in the level (used when loot is taken or a monster dies).

void DrawLevel(List<string> level, int playerX, int playerY)
      Prints the level with the player drawn on top. Nested loop.

void PrintTile(char tile)
      Prints one tile in square brackets.

string Describe(char tile)
      Turns a tile into words, e.g. '#' -> "a wall".

bool IsKnownTile(char tile)
      True if the tile is one of the nine in the tile alphabet.

void PrintLegend()
      Prints all nine tiles with their descriptions.

bool CheckLevel(List<string> level)
      True if a level obeys the contracts. Prints what is wrong if not.

------------------------------------------------------------
Characters.cs: the party (parallel lists)
------------------------------------------------------------
bool IsAlive(int hp)
      True if hp is more than 0.

int Heal(int hp)
      Returns hp + 3. Does NOT change the variable you pass in. Keep the answer!

string HealthBar(string name, int hp, int maxHp)
      Returns one line like  Rook     [#######---] 7/10

void AddCharacter(List<string> names, List<int> health, List<int> attack, string name, int hp, int atk)
      Adds one character to the three party lists (hp limited to 1-10, atk to 1-5).

void MakeStartingParty(List<string> names, List<int> health, List<int> attack)
      Adds the two template characters, Rook and Mote.

int PartySize(List<string> names)
      How many characters are in the party.

int FindCharacter(List<string> names, string name)
      Index of the character with that name, or -1.

int TotalHealth(List<int> health)
      All the party's health added together.

int StrongestIndex(List<int> attack)
      INDEX of the character with the biggest attack.

bool AnyAlive(List<int> health)
      True if at least one character is alive. Stops looking at the first one.

int NextAliveIndex(List<int> health)
      Index of the first living character, or -1.

int ChooseLeader(List<string> names, List<int> health, int leader, string name)
      Returns the index of the named character if they can lead, else the old leader.

void PrintParty(List<string> names, List<int> health, int maxHp)
      Prints a health bar for every character.

------------------------------------------------------------
Combat.cs: damage, attacks, monsters
------------------------------------------------------------
int Roll(int low, int high)
      A random whole number from low to high, including both ends.

int Clamp(int value, int low, int high)
      Forces value to stay between low and high.

int Damage(int attackPower, int defense)
      attackPower minus defense, but never less than 1. ORDER MATTERS.

string WinnerName(string a, int aHp, string b, int bHp)
      The name of whoever has more hp, or "nobody" on a tie.

string MonsterName(int kind)
      Name for monster kind 0, 1 or 2: Grub, Bat, Ogre.

int MonsterHealth(int kind, int levelNumber)
      Starting health for that kind of monster on that level.

int MonsterAttack(int kind, int levelNumber)
      Attack for that kind of monster on that level.

int MonsterDefense(int levelNumber)
      Monster defense on that level (0, or 1 from level 3).

void SpawnMonsters(List<string> level, int levelNumber, List<string> mNames, List<int> mHealth, List<int> mAttack, List<int> mX, List<int> mY)
      Finds every M and B in the level and fills the five monster lists.

int MonsterAt(List<int> mX, List<int> mY, int x, int y)
      Index of the monster standing on (x, y), or -1.

void HeroAttacks(List<string> names, List<int> attack, int hero, List<string> mNames, List<int> mHealth, int m, int monsterDefense, int bonus)
      One character hits one monster; lowers mHealth[m] and prints what happened. bonus is extra power.

void MonsterAttacks(List<string> mNames, List<int> mAttack, int m, List<string> names, List<int> health, int hero, int partyDefense)
      One monster hits one character; lowers health[hero] and prints what happened.

void ClearDefeated(List<string> level, List<string> mNames, List<int> mHealth, List<int> mX, List<int> mY, List<string> inventory)
      Takes every monster with no health left off the map. Each one drops 1 gold.

------------------------------------------------------------
Loot.cs: the inventory
------------------------------------------------------------
void MakeStartingInventory(List<string> inventory)
      What a new game starts with: 2 potions, 3 arrows, 2 stones.

string RandomLoot()
      Returns "potion", "gold", "shield", "arrow" or "stone" at random.

void AddLoot(List<string> inventory, string item)
      Adds an item to the inventory and announces it.

int CountItem(List<string> inventory, string item)
      How many of one item are in the inventory.

bool RemoveItem(List<string> inventory, string item)
      Removes ONE copy of an item. False if there was none.

void DrinkPotion(List<string> inventory, List<string> names, List<int> health, int leader, int maxHp)
      The leader drinks a potion: removes one and heals. Uses  health[i] = Heal(health[i]).

void PrintInventory(List<string> inventory)
      Prints potions, shields, gold, arrows and stones on one line.

------------------------------------------------------------
Ranged.cs: arrows and stones
------------------------------------------------------------
int ShotPower(string ammo)
      How hard that ammo hits: arrow 3, stone 1.

int ShotRange(string ammo)
      How far that ammo flies: arrow 6, stone 3.

int TargetInLine(List<string> level, List<int> mX, List<int> mY, int x, int y, int stepX, int stepY, int range)
      Index of the first monster in a straight line from (x, y), or -1 if a wall comes first.

int DistanceTo(List<int> mX, List<int> mY, int m, int x, int y)
      How many squares away monster m is.

bool Shoot(List<string> inventory, string ammo, List<string> level, List<string> names, int hero, List<string> mNames, List<int> mHealth, List<int> mX, List<int> mY, int x, int y, int stepX, int stepY, int bonus, int monsterDefense)
      Uses one piece of ammo and shoots it. ONE function for arrows and stones. True if a turn passed.

------------------------------------------------------------
Magic.cs: mana and spells
------------------------------------------------------------
void AddSpell(List<string> spellNames, List<int> spellCosts, string name, int cost)
      Adds one spell to the two spell book lists (cost limited to 1-6).

void MakeSpellBook(List<string> spellNames, List<int> spellCosts)
      Adds the three template spells: spark, mend, quake.

int FindSpell(List<string> spellNames, string name)
      Index of the spell with that name, or -1.

bool CanCast(int mana, int cost)
      True if mana is at least cost.

bool IsAimedSpell(string name)
      True if the spell needs a direction. mend and quake do not.

int SpellDamage(string name, int casterAttack, int distance)
      Damage of an aimed spell. spark is 4. Anything else asks MySpellDamage.

int ManaAfterTurn(int mana, int maxMana, int turns)
      RETURNS the mana after a turn: 1 more on every 4th turn. Keep the answer!

string ManaBar(int mana, int maxMana)
      Returns one line like  Mana     [****..] 4/6

void PrintSpells(List<string> spellNames, List<int> spellCosts)
      Prints every spell you know and its cost.

int HurtNearby(List<string> mNames, List<int> mHealth, List<int> mX, List<int> mY, int x, int y, int reach, int amount)
      Every living monster within reach squares takes amount damage. Returns how many.

int CastSpell(string name, List<string> spellNames, List<int> spellCosts, int mana, List<string> level, List<string> names, List<int> health, List<int> attack, int hero, int maxHp, List<string> mNames, List<int> mHealth, List<int> mX, List<int> mY, int x, int y, int stepX, int stepY)
      Casts one spell by name. RETURNS the mana it cost, or 0 if it was not cast.

------------------------------------------------------------
Powerups.cs: short boosts from the * tile
------------------------------------------------------------
string PowerupName(int kind)
      Name for powerup kind 0, 1 or 2: might, guard, regen.

int FindPowerup(List<string> powerNames, string name)
      Index of a switched-on powerup, or -1.

void AddPowerup(List<string> powerNames, List<int> powerTurns, string name, int turns)
      Switches a powerup on. If it is already on, tops its turns back up instead of adding it twice.

int PowerupBonus(List<string> powerNames, string name, int amount)
      amount if that powerup is on, 0 if it is not.

void TickPowerups(List<string> powerNames, List<int> powerTurns)
      One turn passes: every powerup loses a turn, and spent ones are removed. Loops BACKWARDS.

void PrintPowerups(List<string> powerNames, List<int> powerTurns)
      Prints each switched-on powerup and its turns left.

------------------------------------------------------------
Boss.cs: the B tile
------------------------------------------------------------
string BossName(int levelNumber)
      The boss's name on that level. Level 4 asks MyBossName.

int BossHealth(int levelNumber)
      Boss starting health on that level.

int BossAttack(int levelNumber)
      Boss attack on that level.

int FindBoss(List<string> level, List<int> mX, List<int> mY)
      Index of the boss in the monster lists, or -1 if the level has no B.

int BossPhase(int hp, int maxHp)
      1, 2 or 3, from how much of its health the boss has left.

int ShockwaveEvery(int phase)
      Turns between shockwaves in that phase. 0 means never.

bool IsShockwaveTurn(int phase, int turns)
      True if the boss sends a shockwave on this turn.

void Shockwave(string bossName, List<string> names, List<int> health, int amount)
      Every living party member takes amount damage.

void PrintBossPhase(string bossName, int phase)
      Announces phase 2 and phase 3.

string BossBar(string bossName, int hp, int maxHp)
      Returns the boss's health bar line.

bool IsDoorSealed(List<int> mHealth, int boss)
      True while this level's boss is alive.

------------------------------------------------------------
Dialogue.cs: talking to the N tile, and quests
------------------------------------------------------------
void AddLine(List<string> says, List<string> choiceA, List<int> nextA, List<string> choiceB, List<int> nextB, List<string> gives, string text, string answerA, int leadsA, string answerB, int leadsB, string action)
      Adds one line of conversation to the six dialogue lists.

void GuideDialogue(List<string> says, List<string> choiceA, List<int> nextA, List<string> choiceB, List<int> nextB, List<string> gives)
      The level 1 conversation.

void SageDialogue(List<string> says, List<string> choiceA, List<int> nextA, List<string> choiceB, List<int> nextB, List<string> gives)
      The level 2 conversation, with the gold quest.

void GhostDialogue(List<string> says, List<string> choiceA, List<int> nextA, List<string> choiceB, List<int> nextB, List<string> gives)
      The level 3 conversation, with the boss quest.

void LoadDialogue(int levelNumber, List<string> says, List<string> choiceA, List<int> nextA, List<string> choiceB, List<int> nextB, List<string> gives)
      Empties the six lists and fills them with that level's conversation.

string NpcName(int levelNumber)
      Who the N is on that level.

string QuestNeed(int levelNumber)
      What that level's quest needs: "", "gold", "boss" or "monsters".

string QuestNeedWords(string need)
      A quest need in words, for the screen: "gold" -> "bring 3 gold".

int DialogueStart(int levelNumber, int questState)
      Which line the talk opens on, for that level and quest state (0 to 3).

bool HasFlag(List<string> flags, string flag)
      True if the game remembers that flag.

void SetFlag(List<string> flags, string flag)
      Remembers a flag. Never adds it twice.

bool AllDefeated(List<int> mHealth)
      True if no monster on the level is alive.

bool IsQuestReady(string need, List<string> flags, List<string> inventory, List<int> mHealth, int levelNumber)
      True if what the quest needs has been done.

int QuestState(List<string> flags, List<string> inventory, List<int> mHealth, int levelNumber)
      0 not started, 1 started, 2 ready to finish, 3 done.

void PrintDialogue(string npcName, List<string> says, List<string> choiceA, List<string> choiceB, int line)
      Prints one line of conversation and its answers.

int NextLine(List<string> choiceA, List<int> nextA, List<string> choiceB, List<int> nextB, int line, string answer)
      The line that follows, given what the player typed. The SAME line means ask again.

bool IsKnownAction(string action)
      True if the game knows what that "gives" word means.

void AddMany(List<string> inventory, string item, int count)
      Adds the same item to the inventory count times. A loop with a parameter for its limit.

void GiveItems(List<string> inventory, string item, int count)
      AddMany, and says so.

void DoAction(string action, int levelNumber, List<string> flags, List<string> inventory, List<int> health, int maxHp, List<string> powerNames, List<int> powerTurns)
      Does what a line of conversation gives: items, healing, powerups, quest flags.

int ManaAfterAction(string action, int mana, int maxMana)
      RETURNS the mana after an action. Only "restore mana" changes it.

bool CheckDialogue(int levelNumber, List<string> says, List<string> choiceA, List<int> nextA, List<string> choiceB, List<int> nextB, List<string> gives)
      True if a conversation obeys the contract. Prints what is wrong if not.

------------------------------------------------------------
Export.cs: saving the game for the web player
------------------------------------------------------------
void ExportItem(List<string> lines, List<string> inventory, string item)
      Writes one starting-inventory line of the export.

string SpellKind(string name)
      spark, mend, quake, or bolt for your own spell.

void ExportGame(int maxHp, int maxMana)
      Writes mygame.txt (levels, party, monsters, spells, conversations) for the web player. Always a NEW game.

------------------------------------------------------------
MyLevel.cs: YOUR FILE
------------------------------------------------------------
List<string> MyLevel()
      YOURS. Returns your level. The game uses it as the final level.

------------------------------------------------------------
MyCharacter.cs: YOUR FILE
------------------------------------------------------------
bool IsFairCharacter(int hp, int atk)
      YOURS (Milestone 1). True when hp + atk * 2 is 18 or less.

void MyCharacter(List<string> names, List<int> health, List<int> attack)
      YOURS. Adds your character to the party lists.

------------------------------------------------------------
MySpell.cs: YOUR FILE
------------------------------------------------------------
void MySpell(List<string> spellNames, List<int> spellCosts)
      YOURS. Adds your spell to the spell book.

int MySpellDamage(int casterAttack, int distance)
      YOURS. How hard your spell hits, from the caster's attack and the distance.

------------------------------------------------------------
MyDialogue.cs: YOUR FILE
------------------------------------------------------------
string MyNpcName()
      YOURS. The name of the N on your level.

string MyBossName()
      YOURS. The name of the B on your level.

string MyQuestNeed()
      YOURS. What your quest needs: "", "gold", "boss" or "monsters".

int MyDialogueStart(int questState)
      YOURS. Which line your talk opens on for each quest state.

void MyDialogue(List<string> says, List<string> choiceA, List<int> nextA, List<string> choiceB, List<int> nextB, List<string> gives)
      YOURS. Adds your lines of conversation with AddLine.

