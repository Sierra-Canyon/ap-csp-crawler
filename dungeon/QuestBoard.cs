// ======================================================================
//  QuestBoard.cs: TEACHER CODE. You run it, you do not edit it.
//  It calls YOUR quest functions so that what you write shows up in the game.
//  Type   quests   in the game to see every quest you have started.
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

void QuestsAtStart()
{
    try
    {
        if (QuestStarted("Quest01.cs", "QUEST NOT STARTED")) PrintQuestBanner();
    }
    catch (Exception e) { QuestCrashed("your quest files", e); }
}

void QuestsOnHud(List<string> names, List<int> health, int leader, List<string> inventory)
{
    try
    {
        string mood = PartyMood(TotalHealth(health), CountItem(inventory, "potion"));
        if (mood != "?") Console.WriteLine("Mood: " + mood);
        string purse = GoldRank(CountItem(inventory, "gold"));
        if (purse != "?") Console.WriteLine("Purse: " + purse);
        if (health[leader] <= 6) PrintHealthWarning(health[leader]);
    }
    catch (Exception e) { QuestCrashed("your quest files", e); }
}

void PrintQuestBoard(List<string> names, List<int> health, List<int> attack, int leader, int maxHp, List<string> inventory, int mana, int maxMana, List<string> level, int playerX, int playerY, int levelNumber, int questSteps)
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
            AddTrophy(questShelf, "level " + levelNumber + " map");
            Console.WriteLine("Trophies: " + questShelf.Count + ". First: " + FirstTrophy(questShelf) + ". Last: " + LastTrophy(questShelf) + ".");
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
