// ======================================================================
//  QuestBoard.cs: TEACHER CODE. You run it, you do not edit it.
//  It calls YOUR quest functions so that what you write changes the game:
//  at the start, on every turn, when a level starts, when a monster is beaten, at the end,
//  in the commands your quests add, and on the quest board (type   quests   in the game).
// ======================================================================
// Built by tools/make_quests.py. Change the record in tools/quests/, not this file.

// True once the NOT STARTED line has been deleted from that file.
bool QuestStarted(string file, string marker)
{
    // the game is usually run from dungeon/, sometimes from the folder above it
    if (!File.Exists(file)) file = "dungeon/" + file;
    if (!File.Exists(file)) return true;
    return !File.ReadAllText(file).Contains(marker);
}

// try and catch are not one of the five tools. They are here, in teacher code only, so that a crash
// inside one of your quest functions is reported and the game carries on.
void QuestCrashed(string file, Exception e)
{
    Console.WriteLine("  Your code stopped with " + e.GetType().Name + ". Look in " + file + ".");
}

// Called when the game starts.
void QuestsAtStart(List<string> names, List<int> health, List<int> attack, List<string> trophies, List<string> bookNames, List<int> bookHealth, List<string> settings)
{
    List<string> added = new List<string>();
    if (QuestStarted("Quest08.cs", "QUEST NOT STARTED")) added.Add("trophies");
    if (QuestStarted("Quest10.cs", "QUEST NOT STARTED")) added.Add("riddle");
    if (QuestStarted("Quest15.cs", "QUEST NOT STARTED")) added.Add("wide");
    if (QuestStarted("Quest15.cs", "QUEST NOT STARTED")) added.Add("art");
    if (added.Count > 0) Console.WriteLine("Your quests added these commands: " + string.Join(", ", added) + ".");
    if (QuestStarted("Quest01.cs", "QUEST NOT STARTED"))
    {
        try
        {
            PrintQuestBanner();
        }
        catch (Exception e) { QuestCrashed("Quest01.cs", e); }
    }
    if (QuestStarted("Quest08.cs", "QUEST NOT STARTED"))
    {
        try
        {
            List<string> mine = MyTrophies();
            for (int i = 0; i < mine.Count; i++)
            {
                trophies.Add(mine[i]);
            }
        }
        catch (Exception e) { QuestCrashed("Quest08.cs", e); }
    }
    if (QuestStarted("Quest09.cs", "QUEST NOT STARTED"))
    {
        try
        {
            MyMonsters(bookNames, bookHealth);
        }
        catch (Exception e) { QuestCrashed("Quest09.cs", e); }
    }
    if (QuestStarted("Quest12.cs", "QUEST NOT STARTED"))
    {
        try
        {
            List<string> story = MyStory();
            for (int i = 0; i < story.Count; i++)
            {
                Console.WriteLine("  " + story[i]);
            }
            Console.WriteLine("The party: " + JoinNames(names) + ".");
        }
        catch (Exception e) { QuestCrashed("Quest12.cs", e); }
    }
}

// Called when every turn, under the party.
void QuestsOnHud(List<string> names, List<int> health, int leader, List<string> inventory, List<string> level, List<string> trophies)
{
    if (QuestStarted("Quest00.cs", "QUEST NOT STARTED"))
    {
        try
        {
            string mood = PartyMood(TotalHealth(health), CountItem(inventory, "potion"));
            if (mood != "?") Console.WriteLine("Mood: " + mood);
            string purse = GoldRank(CountItem(inventory, "gold"));
            if (purse != "?") Console.WriteLine("Purse: " + purse);
        }
        catch (Exception e) { QuestCrashed("Quest00.cs", e); }
    }
    if (QuestStarted("Quest02.cs", "QUEST NOT STARTED"))
    {
        try
        {
            if (health[leader] <= 6) PrintHealthWarning(health[leader]);
        }
        catch (Exception e) { QuestCrashed("Quest02.cs", e); }
    }
    if (QuestStarted("Quest08.cs", "QUEST NOT STARTED"))
    {
        try
        {
            if (trophies.Count > 0) Console.WriteLine("Trophies: " + trophies.Count + ". First: " + FirstTrophy(trophies) + ". Newest: " + LastTrophy(trophies) + ".");
        }
        catch (Exception e) { QuestCrashed("Quest08.cs", e); }
    }
    if (QuestStarted("Quest11.cs", "QUEST NOT STARTED"))
    {
        try
        {
            string bar = MyBar(CountTiles(level, 'M') + CountTiles(level, 'B'));
            if (bar != "") Console.WriteLine("Monsters left " + bar);
        }
        catch (Exception e) { QuestCrashed("Quest11.cs", e); }
    }
}

// Called when a level starts.
void QuestsOnLevel(int levelNumber, List<string> level, List<string> names, List<int> health, List<int> attack, int leader, List<string> mNames, List<int> mHealth, List<string> bookNames, List<int> bookHealth)
{
    if (QuestStarted("Quest02.cs", "QUEST NOT STARTED"))
    {
        try
        {
            PrintReadyLine(names[leader]);
        }
        catch (Exception e) { QuestCrashed("Quest02.cs", e); }
    }
    if (QuestStarted("Quest09.cs", "QUEST NOT STARTED"))
    {
        try
        {
            if (bookNames.Count > 0 && bookNames.Count == bookHealth.Count)
            {
                int page = 0;
                for (int i = 0; i < mNames.Count; i = i + 2)
                {
                    if (mNames[i] == "Grub" || mNames[i] == "Bat" || mNames[i] == "Ogre")
                    {
                        mNames[i] = bookNames[page % bookNames.Count];
                        mHealth[i] = bookHealth[page % bookNames.Count];
                        page = page + 1;
                    }
                }
                for (int p = 0; p < page && p < bookNames.Count; p++)
                {
                    Console.WriteLine("From your monster book: " + PageText(bookNames, bookHealth, p));
                }
            }
        }
        catch (Exception e) { QuestCrashed("Quest09.cs", e); }
    }
    if (QuestStarted("Quest13.cs", "QUEST NOT STARTED"))
    {
        try
        {
            List<int> standing = new List<int>();
            for (int i = 0; i < attack.Count; i++)
            {
                if (IsAlive(health[i])) standing.Add(attack[i]);
            }
            string cry = MyRallyCry(TotalAttack(standing));
            if (cry != "") Console.WriteLine(names[leader] + " shouts: " + cry);
        }
        catch (Exception e) { QuestCrashed("Quest13.cs", e); }
    }
    if (QuestStarted("Quest14.cs", "QUEST NOT STARTED"))
    {
        try
        {
            if (levelNumber == 0) Console.WriteLine("This is your room, from MyRoom. Loot and monsters in it: " + CountEither(level, '$', 'M') + ".");
        }
        catch (Exception e) { QuestCrashed("Quest14.cs", e); }
    }
}

// Called when the lead command changes the leader.
void QuestsOnLead(List<string> names, List<int> health, List<int> attack, int leader)
{
    if (QuestStarted("Quest03.cs", "QUEST NOT STARTED"))
    {
        try
        {
            PrintLeaderLine(names[leader], health[leader], attack[leader]);
        }
        catch (Exception e) { QuestCrashed("Quest03.cs", e); }
    }
}

// Called when the end of a turn, before the beaten monsters are cleared away.
void QuestsOnDefeat(List<string> level, List<string> mNames, List<int> mHealth, List<int> mX, List<int> mY, List<string> trophies)
{
    if (QuestStarted("Quest08.cs", "QUEST NOT STARTED"))
    {
        try
        {
            for (int i = 0; i < mNames.Count; i++)
            {
                char tile = TileAt(level, mX[i], mY[i]);
                if (!IsAlive(mHealth[i]) && (tile == 'M' || tile == 'B'))
                {
                    string won = TrophyFrom(mNames[i]);
                    int before = trophies.Count;
                    if (won != "") AddTrophy(trophies, won);
                    if (trophies.Count > before) Console.WriteLine("The party keeps: " + won + ".");
                }
            }
        }
        catch (Exception e) { QuestCrashed("Quest08.cs", e); }
    }
}

// Called when the game is won or lost.
void QuestsAtEnd(bool won, int turns, List<string> names, List<int> health)
{
    if (QuestStarted("Quest12.cs", "QUEST NOT STARTED"))
    {
        try
        {
            Console.WriteLine("Still standing at the end: " + CountAlive(health) + ".");
        }
        catch (Exception e) { QuestCrashed("Quest12.cs", e); }
    }
    if (QuestStarted("Quest16.cs", "QUEST NOT STARTED"))
    {
        try
        {
            string ending = MyEnding(won, turns);
            if (ending != "") Console.WriteLine(ending);
        }
        catch (Exception e) { QuestCrashed("Quest16.cs", e); }
    }
}

// A command one of your quests added. True when it was one, so the game does nothing else with it.
bool QuestsCommand(string command, List<string> names, List<int> health, List<int> attack, int leader, List<string> inventory, List<string> trophies, List<string> bookNames, List<int> bookHealth, List<string> settings)
{
    if (QuestStarted("Quest08.cs", "QUEST NOT STARTED"))
    {
        try
        {
            if (command == "trophies")
            {
                Console.WriteLine("The trophy shelf, " + trophies.Count + " of 5:");
                for (int i = 0; i < trophies.Count; i++)
                {
                    Console.WriteLine("  " + i + ": " + trophies[i]);
                }
                return true;
            }
        }
        catch (Exception e) { QuestCrashed("Quest08.cs", e); return true; }
    }
    if (QuestStarted("Quest10.cs", "QUEST NOT STARTED"))
    {
        try
        {
            if (command == "riddle")
            {
                string riddle = MyRiddle();
                if (riddle == "")
                {
                    Console.WriteLine("No riddle yet: MyRiddle in Quest10.cs still returns nothing.");
                    return true;
                }
                Console.WriteLine(names[leader] + " asks: " + riddle);
                string answer = ReadCommand().ToLower();
                if (FindInThree(MyRiddleAnswers(), answer) >= 0)
                {
                    Console.WriteLine("Right!");
                    if (!settings.Contains("riddle solved"))
                    {
                        settings.Add("riddle solved");
                        AddLoot(inventory, "potion");
                        Console.WriteLine("The answer is worth a potion.");
                    }
                }
                else
                {
                    Console.WriteLine("That is not one of the three answers.");
                }
                return true;
            }
        }
        catch (Exception e) { QuestCrashed("Quest10.cs", e); return true; }
    }
    if (QuestStarted("Quest15.cs", "QUEST NOT STARTED"))
    {
        try
        {
            if (command == "wide" || command == "art")
            {
                if (settings.Contains(command))
                {
                    settings.Remove(command);
                    Console.WriteLine("Back to the game's own map.");
                }
                else
                {
                    settings.Remove("wide");
                    settings.Remove("art");
                    settings.Add(command);
                    if (command == "wide") Console.WriteLine("The map is drawn by your DrawWide now. If no map appears, DrawWide is not written yet. Type wide again to go back.");
                    else Console.WriteLine("The map is drawn with your WideTile now. Type art again to go back.");
                }
                return true;
            }
        }
        catch (Exception e) { QuestCrashed("Quest15.cs", e); return true; }
    }
    return false;
}

// The mana line: your ManaDots once Quest 11 is started, the game's own ManaBar until then.
string QuestsManaLine(int mana, int maxMana)
{
    if (QuestStarted("Quest11.cs", "QUEST NOT STARTED"))
    {
        try
        {
            string dots = ManaDots(mana, maxMana);
            if (dots != "") return "Mana     [" + dots + "] " + mana + "/" + maxMana;
        }
        catch (Exception e) { QuestCrashed("Quest11.cs", e); }
    }
    return ManaBar(mana, maxMana);
}

// Your room from Quest 14 is level 0: the game starts there once it is a room the game can use.
int QuestsFirstLevel(int levelNumber)
{
    if (levelNumber != 1 || !QuestStarted("Quest14.cs", "QUEST NOT STARTED")) return levelNumber;
    try
    {
        if (QuestRoomUsable(MyRoom())) return 0;
    }
    catch (Exception e) { QuestCrashed("Quest14.cs", e); }
    return levelNumber;
}

List<string> QuestsLoadLevel(int levelNumber)
{
    if (levelNumber == 0) return MyRoom();
    return LoadLevel(levelNumber);
}

// Rows the same length, only the six tiles, one @, and a + that can be reached from it.
bool QuestRoomUsable(List<string> room)
{
    if (room.Count < 3) return false;
    int starts = 0;
    for (int y = 0; y < room.Count; y++)
    {
        if (room[y].Length != room[0].Length) return false;
        for (int x = 0; x < room[y].Length; x++)
        {
            if ("#.@M$+".IndexOf(room[y][x]) < 0) return false;
            if (room[y][x] == '@') starts = starts + 1;
        }
    }
    if (starts != 1) return false;
    List<int> openX = new List<int>();
    List<int> openY = new List<int>();
    List<string> seen = new List<string>();
    openX.Add(FindTileX(room, '@'));
    openY.Add(FindTileY(room, '@'));
    seen.Add(openX[0] + "," + openY[0]);
    for (int k = 0; k < openX.Count; k++)
    {
        for (int d = 0; d < 4; d++)
        {
            int nx = openX[k];
            int ny = openY[k];
            if (d == 0) nx = nx + 1;
            else if (d == 1) nx = nx - 1;
            else if (d == 2) ny = ny + 1;
            else ny = ny - 1;
            char t = TileAt(room, nx, ny);
            if (t == '+') return true;
            if (t != '#' && !seen.Contains(nx + "," + ny))
            {
                seen.Add(nx + "," + ny);
                openX.Add(nx);
                openY.Add(ny);
            }
        }
    }
    return false;
}

// The map: your DrawWide after  wide , your WideTile art after  art , and the game's own DrawLevel otherwise.
void QuestsDraw(List<string> level, int playerX, int playerY, List<string> settings)
{
    if (QuestStarted("Quest15.cs", "QUEST NOT STARTED") && (settings.Contains("wide") || settings.Contains("art")))
    {
        try
        {
            if (settings.Contains("wide"))
            {
                DrawWide(level, playerX, playerY);
            }
            else
            {
                for (int y = 0; y < level.Count; y++)
                {
                    string row = "";
                    for (int x = 0; x < level[y].Length; x++)
                    {
                        char tile = level[y][x];
                        if (x == playerX && y == playerY) tile = '@';
                        else if (tile == '@') tile = '.';
                        string look = WideTile(tile);
                        if (look.Length != 2) look = "" + tile + tile;
                        row = row + look;
                    }
                    Console.WriteLine(row);
                }
            }
            return;
        }
        catch (Exception e) { QuestCrashed("Quest15.cs", e); }
    }
    DrawLevel(level, playerX, playerY);
}

void PrintQuestBoard(List<string> names, List<int> health, List<int> attack, int leader, int maxHp, List<string> inventory, int mana, int maxMana, List<string> level, int playerX, int playerY, int levelNumber, int questSteps, List<string> trophies, List<string> bookNames, List<int> bookHealth, List<string> settings)
{
    Console.WriteLine("----- THE QUEST BOARD: what your functions do with the real game -----");
    int shown = 0;
    if (QuestStarted("Quest01.cs", "CHALLENGE NOT STARTED"))
    {
        Console.WriteLine("Challenge 1, Roll Call:");
        shown = shown + 1;
        try
        {
            PrintRollCall();
        }
        catch (Exception e) { QuestCrashed("Quest01.cs", e); }
    }
    if (QuestStarted("Quest02.cs", "QUEST NOT STARTED"))
    {
        Console.WriteLine("Quest 2, Health Warning:");
        shown = shown + 1;
        try
        {
            PrintReadyLine(names[leader]);
        }
        catch (Exception e) { QuestCrashed("Quest02.cs", e); }
    }
    if (QuestStarted("Quest02.cs", "CHALLENGE NOT STARTED"))
    {
        Console.WriteLine("Challenge 2, The Door Report:");
        shown = shown + 1;
        try
        {
            PrintDoorStatus(CountTiles(level, 'M') + CountTiles(level, 'B'));
        }
        catch (Exception e) { QuestCrashed("Quest02.cs", e); }
    }
    if (QuestStarted("Quest03.cs", "QUEST NOT STARTED"))
    {
        Console.WriteLine("Quest 3, Leader Line:");
        shown = shown + 1;
        try
        {
            PrintLeaderLine(names[leader], health[leader], attack[leader]);
            PrintTrade(names[0], names[1], "potion", CountItem(inventory, "potion"));
        }
        catch (Exception e) { QuestCrashed("Quest03.cs", e); }
    }
    if (QuestStarted("Quest03.cs", "CHALLENGE NOT STARTED"))
    {
        Console.WriteLine("Challenge 3, The Duel:");
        shown = shown + 1;
        try
        {
            PrintDuel(names[0], attack[0], names[1], attack[1]);
        }
        catch (Exception e) { QuestCrashed("Quest03.cs", e); }
    }
    if (QuestStarted("Quest04.cs", "QUEST NOT STARTED"))
    {
        Console.WriteLine("Quest 4, Lucky Hit:");
        shown = shown + 1;
        try
        {
            Console.WriteLine("A lucky hit from " + names[leader] + " would do " + CritDamage(attack[leader], 6) + ".");
            if (IsLowHealth(health[leader], maxHp)) Console.WriteLine(names[leader] + " is low on health.");
        }
        catch (Exception e) { QuestCrashed("Quest04.cs", e); }
    }
    if (QuestStarted("Quest04.cs", "CHALLENGE NOT STARTED"))
    {
        Console.WriteLine("Challenge 4, Status Line:");
        shown = shown + 1;
        try
        {
            Console.WriteLine(StatusLine(names[leader], health[leader], maxHp));
        }
        catch (Exception e) { QuestCrashed("Quest04.cs", e); }
    }
    if (QuestStarted("Quest05.cs", "QUEST NOT STARTED"))
    {
        Console.WriteLine("Quest 5, Two Repairs:");
        shown = shown + 1;
        try
        {
            Console.WriteLine(ManaWarning(mana, 4));
            Console.WriteLine(ShortByLine(mana, 4));
        }
        catch (Exception e) { QuestCrashed("Quest05.cs", e); }
    }
    if (QuestStarted("Quest05.cs", "CHALLENGE NOT STARTED"))
    {
        Console.WriteLine("Challenge 5, Counting Steps:");
        shown = shown + 1;
        try
        {
            Console.WriteLine("Moves typed so far: " + questSteps);
        }
        catch (Exception e) { QuestCrashed("Quest05.cs", e); }
    }
    if (QuestStarted("Quest06.cs", "QUEST NOT STARTED"))
    {
        Console.WriteLine("Quest 6, Borrowed Tools:");
        shown = shown + 1;
        try
        {
            Console.WriteLine("A potion would take " + names[leader] + " to " + HealTo(health[leader], 3, maxHp) + ".");
            Console.WriteLine("A hit of 5 through your shields: " + HitThroughShields(5, CountItem(inventory, "shield")));
        }
        catch (Exception e) { QuestCrashed("Quest06.cs", e); }
    }
    if (QuestStarted("Quest06.cs", "CHALLENGE NOT STARTED"))
    {
        Console.WriteLine("Challenge 6, Who Is Ahead:");
        shown = shown + 1;
        try
        {
            Console.WriteLine(Matchup(names[leader], health[leader], "the Grub", 4));
        }
        catch (Exception e) { QuestCrashed("Quest06.cs", e); }
    }
    if (QuestStarted("Quest07.cs", "QUEST NOT STARTED"))
    {
        Console.WriteLine("Quest 7, Three Gates, One Function:");
        shown = shown + 1;
        try
        {
            PrintGate("red", CountItem(inventory, "gold"));
        }
        catch (Exception e) { QuestCrashed("Quest07.cs", e); }
    }
    if (QuestStarted("Quest07.cs", "CHALLENGE NOT STARTED"))
    {
        Console.WriteLine("Challenge 7, The Other Direction:");
        shown = shown + 1;
        try
        {
            Console.WriteLine(SmallPotionLabel() + " and " + LargePotionLabel());
        }
        catch (Exception e) { QuestCrashed("Quest07.cs", e); }
    }
    if (QuestStarted("Quest08.cs", "QUEST NOT STARTED"))
    {
        Console.WriteLine("Quest 8, The Trophy Shelf:");
        shown = shown + 1;
        try
        {
            List<string> questShelf = MakeTrophyShelf();
            Console.WriteLine("MakeTrophyShelf: " + questShelf.Count + " trophies. First: " + FirstTrophy(questShelf) + ". Last: " + LastTrophy(questShelf) + ".");
            Console.WriteLine("Your shelf in this game: " + trophies.Count + ". First: " + FirstTrophy(trophies) + ". Last: " + LastTrophy(trophies) + ".");
            Console.WriteLine("Beat a Grub and the party keeps: " + TrophyFrom("Grub") + ".");
        }
        catch (Exception e) { QuestCrashed("Quest08.cs", e); }
    }
    if (QuestStarted("Quest08.cs", "CHALLENGE NOT STARTED"))
    {
        Console.WriteLine("Challenge 8, Reaching In:");
        shown = shown + 1;
        try
        {
            List<string> questShelf2 = new List<string>();
            questShelf2.Add("rusty key");
            questShelf2.Add("bat wing");
            questShelf2.Add("ogre tooth");
            Console.WriteLine("The middle trophy of three: " + MiddleTrophy(questShelf2) + ". Trophy 7: " + TrophyAt(questShelf2, 7) + ".");
        }
        catch (Exception e) { QuestCrashed("Quest08.cs", e); }
    }
    if (QuestStarted("Quest09.cs", "QUEST NOT STARTED"))
    {
        Console.WriteLine("Quest 9, The Monster Book:");
        shown = shown + 1;
        try
        {
            List<string> questBookNames = new List<string>();
            List<int> questBookHealth = new List<int>();
            AddPage(questBookNames, questBookHealth, "Grub", MonsterHealth(0, levelNumber));
            AddPage(questBookNames, questBookHealth, "Bat", MonsterHealth(1, levelNumber));
            Console.WriteLine(PageText(questBookNames, questBookHealth, 0) + "   " + PageText(questBookNames, questBookHealth, 1));
            Console.WriteLine("Your own book has " + bookNames.Count + " pages. " + PageText(bookNames, bookHealth, 0) + "   " + PageText(bookNames, bookHealth, bookNames.Count));
        }
        catch (Exception e) { QuestCrashed("Quest09.cs", e); }
    }
    if (QuestStarted("Quest09.cs", "CHALLENGE NOT STARTED"))
    {
        Console.WriteLine("Challenge 9, Swap Two Pages:");
        shown = shown + 1;
        try
        {
            List<string> questSwapNames = new List<string>();
            List<int> questSwapHealth = new List<int>();
            questSwapNames.Add("Grub");
            questSwapNames.Add("Ogre");
            questSwapHealth.Add(4);
            questSwapHealth.Add(8);
            SwapPages(questSwapNames, questSwapHealth, 0, 1);
            Console.WriteLine("Grub 4 and Ogre 8, swapped: " + questSwapNames[0] + " " + questSwapHealth[0] + " and " + questSwapNames[1] + " " + questSwapHealth[1] + ".");
        }
        catch (Exception e) { QuestCrashed("Quest09.cs", e); }
    }
    if (QuestStarted("Quest10.cs", "QUEST NOT STARTED"))
    {
        Console.WriteLine("Quest 10, Finding by Hand:");
        shown = shown + 1;
        try
        {
            List<string> questThree = new List<string>();
            questThree.Add("potion");
            questThree.Add("arrow");
            questThree.Add("potion");
            Console.WriteLine("In [potion, arrow, potion] the arrow is at " + FindInThree(questThree, "arrow") + " and there are " + CountInThree(questThree, "potion") + " potions.");
        }
        catch (Exception e) { QuestCrashed("Quest10.cs", e); }
    }
    if (QuestStarted("Quest10.cs", "CHALLENGE NOT STARTED"))
    {
        Console.WriteLine("Challenge 10, Take One:");
        shown = shown + 1;
        try
        {
            List<string> questTake = new List<string>();
            questTake.Add("potion");
            questTake.Add("arrow");
            questTake.Add("potion");
            Console.WriteLine(TakeFromThree(questTake, "arrow"));
        }
        catch (Exception e) { QuestCrashed("Quest10.cs", e); }
    }
    if (QuestStarted("Quest11.cs", "QUEST NOT STARTED"))
    {
        Console.WriteLine("Quest 11, Bars and Dots:");
        shown = shown + 1;
        try
        {
            Console.WriteLine("Mana " + ManaDots(mana, maxMana) + "   Level " + Stars(levelNumber));
            Console.WriteLine("Monsters left on this level " + MyBar(CountTiles(level, 'M') + CountTiles(level, 'B')));
        }
        catch (Exception e) { QuestCrashed("Quest11.cs", e); }
    }
    if (QuestStarted("Quest11.cs", "CHALLENGE NOT STARTED"))
    {
        Console.WriteLine("Challenge 11, The Ruler:");
        shown = shown + 1;
        try
        {
            Console.WriteLine(Ruler(level[0].Length));
            Console.WriteLine(level[0]);
        }
        catch (Exception e) { QuestCrashed("Quest11.cs", e); }
    }
    if (QuestStarted("Quest12.cs", "QUEST NOT STARTED"))
    {
        Console.WriteLine("Quest 12, Every Name:");
        shown = shown + 1;
        try
        {
            Console.WriteLine("The party: " + JoinNames(names) + ". Still standing: " + CountAlive(health) + ".");
        }
        catch (Exception e) { QuestCrashed("Quest12.cs", e); }
    }
    if (QuestStarted("Quest12.cs", "CHALLENGE NOT STARTED"))
    {
        Console.WriteLine("Challenge 12, The Last One:");
        shown = shown + 1;
        try
        {
            Console.WriteLine("The last potion in the pack is item " + FindLast(inventory, "potion") + ".");
        }
        catch (Exception e) { QuestCrashed("Quest12.cs", e); }
    }
    if (QuestStarted("Quest13.cs", "QUEST NOT STARTED"))
    {
        Console.WriteLine("Quest 13, Totals and Records:");
        shown = shown + 1;
        try
        {
            Console.WriteLine("Total attack " + TotalAttack(attack) + ". Everyone standing: " + AllAlive(health) + ".");
        }
        catch (Exception e) { QuestCrashed("Quest13.cs", e); }
    }
    if (QuestStarted("Quest13.cs", "CHALLENGE NOT STARTED"))
    {
        Console.WriteLine("Challenge 13, Who Needs the Potion:");
        shown = shown + 1;
        try
        {
            int questWeak = WeakestAliveIndex(health);
            if (questWeak >= 0 && questWeak < names.Count) Console.WriteLine("The next potion should go to " + names[questWeak] + ".");
        }
        catch (Exception e) { QuestCrashed("Quest13.cs", e); }
    }
    if (QuestStarted("Quest14.cs", "QUEST NOT STARTED"))
    {
        Console.WriteLine("Quest 14, Rows and Columns:");
        shown = shown + 1;
        try
        {
            Console.WriteLine("Loot and doors left on this level: " + CountEither(level, '$', '+') + ". Walls in column 0: " + CountInColumn(level, 0, '#') + ".");
        }
        catch (Exception e) { QuestCrashed("Quest14.cs", e); }
    }
    if (QuestStarted("Quest14.cs", "CHALLENGE NOT STARTED"))
    {
        Console.WriteLine("Challenge 14, The Richest Row:");
        shown = shown + 1;
        try
        {
            Console.WriteLine("The richest row is row " + RichestRow(level) + ".");
        }
        catch (Exception e) { QuestCrashed("Quest14.cs", e); }
    }
    if (QuestStarted("Quest15.cs", "QUEST NOT STARTED"))
    {
        Console.WriteLine("Quest 15, Wide Screen:");
        shown = shown + 1;
        try
        {
            DrawWide(level, playerX, playerY);
        }
        catch (Exception e) { QuestCrashed("Quest15.cs", e); }
    }
    if (QuestStarted("Quest15.cs", "CHALLENGE NOT STARTED"))
    {
        Console.WriteLine("Challenge 15, Fog:");
        shown = shown + 1;
        try
        {
            DrawFog(level, playerX, playerY);
        }
        catch (Exception e) { QuestCrashed("Quest15.cs", e); }
    }
    if (QuestStarted("Quest16.cs", "QUEST NOT STARTED"))
    {
        Console.WriteLine("Quest 16, The Walk:");
        shown = shown + 1;
        try
        {
            List<string> questWalk = new List<string>();
            questWalk.Add("d");
            questWalk.Add("d");
            questWalk.Add("s");
            Console.WriteLine("d, d, s from here would end at " + WalkX(questWalk, playerX) + "," + WalkY(questWalk, playerY) + " if nothing were in the way.");
        }
        catch (Exception e) { QuestCrashed("Quest16.cs", e); }
    }
    if (QuestStarted("Quest16.cs", "CHALLENGE NOT STARTED"))
    {
        Console.WriteLine("Challenge 16, The Walk, With Walls:");
        shown = shown + 1;
        try
        {
            List<string> questWalk2 = new List<string>();
            questWalk2.Add("d");
            questWalk2.Add("d");
            questWalk2.Add("s");
            Console.WriteLine("d, d, s from the start of this level: " + WalkReport(level, questWalk2) + ".");
        }
        catch (Exception e) { QuestCrashed("Quest16.cs", e); }
    }
    if (shown == 0) Console.WriteLine("Nothing yet. A quest shows here once its NOT STARTED line is gone.");
    Console.WriteLine("----- end of the quest board -----");
}
