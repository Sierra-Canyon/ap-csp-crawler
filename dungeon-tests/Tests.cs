// ======================================================================
//  Tests.cs: the self-check. TEACHER CODE. You run it, you do not edit it.
//
//      cd dungeon-tests
//      dotnet run                 checks every function
//      dotnet run Damage          checks only the functions whose name contains "Damage"
//
//  Each line calls one of the game's functions with inputs where the right
//  answer is known. PASS means your function gave that answer.
//  When you rewrite a function, this is how you find out whether it still works.
// ======================================================================

string only = args.Length > 0 ? args[0] : "";
int passed = 0, failed = 0, todo = 0, extras = 0;
TextWriter screen = Console.Out;

void Check(string function, string call, string expected, string actual)
{
    if (only != "" && !function.Contains(only, StringComparison.OrdinalIgnoreCase)) return;
    if (expected == actual) { passed++; screen.WriteLine("PASS  " + call + "  is  " + actual); }
    else { failed++; screen.WriteLine("FAIL  " + call + "  should be  " + expected + "  but yours gave  " + actual); }
}
void Todo(string function, string call, string expected, string actual)
{
    if (only != "" && !function.Contains(only, StringComparison.OrdinalIgnoreCase)) return;
    if (expected == actual) { passed++; screen.WriteLine("PASS  " + call + "  is  " + actual); }
    else { todo++; screen.WriteLine("TO DO " + call + "  should be  " + expected + "  (yours gives  " + actual + ")"); }
}
// An EXTRA is optional and never graded. It is not counted as TO DO.
void Extra(string function, string call, string expected, string actual)
{
    if (only != "" && !function.Contains(only, StringComparison.OrdinalIgnoreCase)) return;
    if (expected == actual) { passed++; screen.WriteLine("PASS  " + call + "  is  " + actual); }
    else { extras++; screen.WriteLine("EXTRA " + call + "  (optional, not graded)"); }
}
string Show(List<int> list) { return "[" + string.Join(", ", list) + "]"; }
string ShowS(List<string> list) { return "[" + string.Join(", ", list) + "]"; }
void Quiet() { Console.SetOut(TextWriter.Null); }        // some functions print; the check only wants answers
void Loud() { Console.SetOut(screen); }
string Printed(Action run) { StringWriter w = new StringWriter(); try { Console.SetOut(w); run(); Loud(); return w.ToString().Replace("\r", "").TrimEnd('\n').Replace("\n", " / "); } catch (Exception e) { Loud(); return "a crash (" + e.GetType().Name + ")"; } }
string Try(Func<string> run) { try { Quiet(); string r = run(); Loud(); return r; } catch (Exception e) { Loud(); return "a crash (" + e.GetType().Name + ")"; } }

List<string> room = new List<string> { "#####", "#@.$#", "#M.+#", "#####" };
List<string> hall = new List<string> { "#########", "#@...M.M#", "#########" };
List<string> arena = new List<string> { "########", "#@*M.B+#", "#N.....#", "########" };
List<string> Names() { return new List<string> { "Rook", "Mote", "Grub" }; }
List<int> Health() { return new List<int> { 10, 0, 3 }; }
List<int> Attack() { return new List<int> { 4, 3, 5 }; }
(List<string> says, List<string> a, List<int> na, List<string> b, List<int> nb, List<string> g) NewTalk() { return (new List<string>(), new List<string>(), new List<int>(), new List<string>(), new List<int>(), new List<string>()); }

// ---------------------------------------------------------------- Characters.cs
Check("IsAlive", "IsAlive(5)", "True", Try(() => IsAlive(5).ToString()));
Check("IsAlive", "IsAlive(0)", "False", Try(() => IsAlive(0).ToString()));
Check("IsAlive", "IsAlive(-2)", "False", Try(() => IsAlive(-2).ToString()));
Check("Heal", "Heal(4)", "7", Try(() => Heal(4).ToString()));
Check("HealthBar", "HealthBar(\"Mote\", 7, 10)", "Mote     [#######---] 7/10", Try(() => HealthBar("Mote", 7, 10)));
Check("HealthBar", "HealthBar(\"Grub\", 0, 4)", "Grub     [----] 0/4", Try(() => HealthBar("Grub", 0, 4)));
Check("AddCharacter", "AddCharacter(..., \"Zed\", 8, 3) then the three Counts", "4 4 4", Try(() => { List<string> n = Names(); List<int> h = Health(); List<int> a = Attack(); AddCharacter(n, h, a, "Zed", 8, 3); return n.Count + " " + h.Count + " " + a.Count; }));
Check("AddCharacter", "AddCharacter(..., \"Zed\", 8, 3) then names[3], health[3], attack[3]", "Zed 8 3", Try(() => { List<string> n = Names(); List<int> h = Health(); List<int> a = Attack(); AddCharacter(n, h, a, "Zed", 8, 3); return n[3] + " " + h[3] + " " + a[3]; }));
Check("AddCharacter", "AddCharacter(..., \"Max\", 99, 99) is limited to", "10 5", Try(() => { List<string> n = Names(); List<int> h = Health(); List<int> a = Attack(); AddCharacter(n, h, a, "Max", 99, 99); return h[3] + " " + a[3]; }));
Check("PartySize", "PartySize([Rook, Mote, Grub])", "3", Try(() => PartySize(Names()).ToString()));
Check("FindCharacter", "FindCharacter(names, \"Grub\")", "2", Try(() => FindCharacter(Names(), "Grub").ToString()));
Check("FindCharacter", "FindCharacter(names, \"Rook\")", "0", Try(() => FindCharacter(Names(), "Rook").ToString()));
Check("FindCharacter", "FindCharacter(names, \"Nobody\")", "-1", Try(() => FindCharacter(Names(), "Nobody").ToString()));
Check("TotalHealth", "TotalHealth([10, 0, 3])", "13", Try(() => TotalHealth(Health()).ToString()));
Check("TotalHealth", "TotalHealth([])", "0", Try(() => TotalHealth(new List<int>()).ToString()));
Check("StrongestIndex", "StrongestIndex([4, 3, 5])", "2", Try(() => StrongestIndex(Attack()).ToString()));
Check("StrongestIndex", "StrongestIndex([5, 3, 5])  (a tie goes to the first)", "0", Try(() => StrongestIndex(new List<int> { 5, 3, 5 }).ToString()));
Check("AnyAlive", "AnyAlive([10, 0, 3])", "True", Try(() => AnyAlive(Health()).ToString()));
Check("AnyAlive", "AnyAlive([0, 0])", "False", Try(() => AnyAlive(new List<int> { 0, 0 }).ToString()));
Check("NextAliveIndex", "NextAliveIndex([0, 0, 3])", "2", Try(() => NextAliveIndex(new List<int> { 0, 0, 3 }).ToString()));
Check("NextAliveIndex", "NextAliveIndex([0, 0])", "-1", Try(() => NextAliveIndex(new List<int> { 0, 0 }).ToString()));
Check("ChooseLeader", "ChooseLeader(names, health, 0, \"Grub\")", "2", Try(() => ChooseLeader(Names(), Health(), 0, "Grub").ToString()));
Check("ChooseLeader", "ChooseLeader(names, health, 0, \"Mote\")  (Mote has 0 health)", "0", Try(() => ChooseLeader(Names(), Health(), 0, "Mote").ToString()));
Check("ChooseLeader", "ChooseLeader(names, health, 0, \"Nobody\")", "0", Try(() => ChooseLeader(Names(), Health(), 0, "Nobody").ToString()));

// ---------------------------------------------------------------- Combat.cs
Check("Damage", "Damage(4, 1)", "3", Try(() => Damage(4, 1).ToString()));
Check("Damage", "Damage(1, 4)  (never less than 1)", "1", Try(() => Damage(1, 4).ToString()));
Check("Damage", "Damage(5, 5)", "1", Try(() => Damage(5, 5).ToString()));
Check("Clamp", "Clamp(15, 0, 10)", "10", Try(() => Clamp(15, 0, 10).ToString()));
Check("Clamp", "Clamp(-3, 0, 10)", "0", Try(() => Clamp(-3, 0, 10).ToString()));
Check("Clamp", "Clamp(7, 0, 10)", "7", Try(() => Clamp(7, 0, 10).ToString()));
Check("Roll", "Roll(2, 5), 200 times, smallest and biggest seen", "2 5", Try(() => { int lo = 99, hi = -99; for (int i = 0; i < 200; i++) { int r = Roll(2, 5); if (r < lo) lo = r; if (r > hi) hi = r; } return lo + " " + hi; }));
Check("WinnerName", "WinnerName(\"Rook\", 8, \"Grub\", 3)", "Rook", Try(() => WinnerName("Rook", 8, "Grub", 3)));
Check("WinnerName", "WinnerName(\"Rook\", 2, \"Grub\", 3)", "Grub", Try(() => WinnerName("Rook", 2, "Grub", 3)));
Check("WinnerName", "WinnerName(\"Rook\", 3, \"Grub\", 3)", "nobody", Try(() => WinnerName("Rook", 3, "Grub", 3)));
Check("MonsterAt", "MonsterAt([1, 5], [2, 3], 5, 3)", "1", Try(() => MonsterAt(new List<int> { 1, 5 }, new List<int> { 2, 3 }, 5, 3).ToString()));
Check("MonsterAt", "MonsterAt([1, 5], [2, 3], 5, 2)  (x matches one, y the other)", "-1", Try(() => MonsterAt(new List<int> { 1, 5 }, new List<int> { 2, 3 }, 5, 2).ToString()));
Check("SpawnMonsters", "SpawnMonsters(room ...) then mNames, mX, mY", "[Grub] [1] [2]", Try(() => { List<string> n = new List<string>(); List<int> h = new List<int>(); List<int> a = new List<int>(); List<int> x = new List<int>(); List<int> y = new List<int>(); SpawnMonsters(room, 1, n, h, a, x, y); return ShowS(n) + " " + Show(x) + " " + Show(y); }));
Check("SpawnMonsters", "SpawnMonsters called twice does not double the monsters", "1", Try(() => { List<string> n = new List<string>(); List<int> h = new List<int>(); List<int> a = new List<int>(); List<int> x = new List<int>(); List<int> y = new List<int>(); SpawnMonsters(room, 1, n, h, a, x, y); SpawnMonsters(room, 1, n, h, a, x, y); return n.Count.ToString(); }));
Check("HeroAttacks", "HeroAttacks: hero attack 4, monster health 20, defense 1, damage is 3 to 5", "True", Try(() => { List<int> mh = new List<int> { 20 }; HeroAttacks(Names(), Attack(), 0, new List<string> { "Ogre" }, mh, 0, 1, 0); int dealt = 20 - mh[0]; return (dealt >= 3 && dealt <= 5).ToString(); }));
Check("HeroAttacks", "HeroAttacks never leaves a monster below 0 health", "0", Try(() => { List<int> mh = new List<int> { 1 }; HeroAttacks(Names(), Attack(), 0, new List<string> { "Ogre" }, mh, 0, 0, 0); return mh[0].ToString(); }));
Check("MonsterAttacks", "MonsterAttacks: monster attack 3, hero health 10, defense 0, damage is 3 to 4", "True", Try(() => { List<int> h = Health(); MonsterAttacks(new List<string> { "Ogre" }, new List<int> { 3 }, 0, Names(), h, 0, 0); int dealt = 10 - h[0]; return (dealt >= 3 && dealt <= 4).ToString(); }));

// ---------------------------------------------------------------- Levels.cs
Check("TileAt", "TileAt(room, 1, 1)", "@", Try(() => TileAt(room, 1, 1).ToString()));
Check("TileAt", "TileAt(room, 3, 1)  (x is the column, y is the row)", "$", Try(() => TileAt(room, 3, 1).ToString()));
Check("TileAt", "TileAt(room, 1, 2)", "M", Try(() => TileAt(room, 1, 2).ToString()));
Check("TileAt", "TileAt(room, -1, 0)  (off the map is a wall)", "#", Try(() => TileAt(room, -1, 0).ToString()));
Check("TileAt", "TileAt(room, 2, 99)", "#", Try(() => TileAt(room, 2, 99).ToString()));
Check("IsWall", "IsWall(room, 0, 0)", "True", Try(() => IsWall(room, 0, 0).ToString()));
Check("IsWall", "IsWall(room, 2, 1)", "False", Try(() => IsWall(room, 2, 1).ToString()));
Check("CountTiles", "CountTiles(room, '#')", "14", Try(() => CountTiles(room, '#').ToString()));
Check("CountTiles", "CountTiles(room, '.')", "2", Try(() => CountTiles(room, '.').ToString()));
Check("FindTileX", "FindTileX(room, '+')", "3", Try(() => FindTileX(room, '+').ToString()));
Check("FindTileY", "FindTileY(room, '+')", "2", Try(() => FindTileY(room, '+').ToString()));
Check("FindTileX", "FindTileX(room, 'Z')", "-1", Try(() => FindTileX(room, 'Z').ToString()));
Check("SetTile", "SetTile(copy of room, 3, 1, '.') then row 1", "#@..#", Try(() => { List<string> copy = new List<string>(room); SetTile(copy, 3, 1, '.'); return copy[1]; }));
Check("SetTile", "SetTile off the map changes nothing and does not crash", "#####", Try(() => { List<string> copy = new List<string>(room); SetTile(copy, 50, 50, '.'); return copy[0]; }));
Check("DrawLevel", "DrawLevel(room, 2, 1) prints  (the player has walked one step right)", "##### / #.@$# / #M.+# / #####", Printed(() => DrawLevel(room, 2, 1)));
Check("PrintParty", "PrintParty([Rook, Mote], [10, 7], 10) prints", "Rook     [##########] 10/10 / Mote     [#######---] 7/10", Printed(() => PrintParty(new List<string> { "Rook", "Mote" }, new List<int> { 10, 7 }, 10)));
Check("Describe", "Describe('#')", "a wall", Try(() => Describe('#')));
Check("IsKnownTile", "IsKnownTile('M')", "True", Try(() => IsKnownTile('M').ToString()));
Check("IsKnownTile", "IsKnownTile('X')", "False", Try(() => IsKnownTile('X').ToString()));
Check("CheckLevel", "CheckLevel(room)", "True", Try(() => CheckLevel(room).ToString()));
Check("CheckLevel", "CheckLevel(a level with a short row)", "False", Try(() => CheckLevel(new List<string> { "####", "#@+", "####" }).ToString()));
Check("CheckLevel", "CheckLevel(a level with no door)", "False", Try(() => CheckLevel(new List<string> { "####", "#@.#", "####" }).ToString()));
Check("CheckLevel", "CheckLevel(a level with two @)", "False", Try(() => CheckLevel(new List<string> { "#####", "#@@+#", "#####" }).ToString()));
Check("LoadLevel", "every level from LoadLevel(1) to LoadLevel(LevelCount()) passes CheckLevel", "True", Try(() => { bool ok = true; for (int i = 1; i <= LevelCount(); i++) { if (!CheckLevel(LoadLevel(i))) ok = false; } return ok.ToString(); }));

// ---------------------------------------------------------------- Loot.cs and Screens.cs
Check("MakeStartingInventory", "MakeStartingInventory then potions, arrows, stones", "2 3 2", Try(() => { List<string> inv = new List<string>(); MakeStartingInventory(inv); return CountItem(inv, "potion") + " " + CountItem(inv, "arrow") + " " + CountItem(inv, "stone"); }));
Check("CountItem", "CountItem([potion, gold, potion], \"potion\")", "2", Try(() => CountItem(new List<string> { "potion", "gold", "potion" }, "potion").ToString()));
Check("RemoveItem", "RemoveItem([potion, gold, potion], \"potion\") removes ONE", "True [gold, potion]", Try(() => { List<string> inv = new List<string> { "potion", "gold", "potion" }; bool r = RemoveItem(inv, "potion"); return r + " " + ShowS(inv); }));
Check("RemoveItem", "RemoveItem([gold], \"potion\")", "False [gold]", Try(() => { List<string> inv = new List<string> { "gold" }; bool r = RemoveItem(inv, "potion"); return r + " " + ShowS(inv); }));
Check("RandomLoot", "RandomLoot(), 200 times, is always potion, gold, shield, arrow or stone", "True", Try(() => { bool ok = true; for (int i = 0; i < 200; i++) { string s = RandomLoot(); if (s != "potion" && s != "gold" && s != "shield" && s != "arrow" && s != "stone") ok = false; } return ok.ToString(); }));
Check("DrinkPotion", "DrinkPotion with one potion: health[0] 5 becomes", "8", Try(() => { List<int> h = new List<int> { 5 }; DrinkPotion(new List<string> { "potion" }, new List<string> { "Rook" }, h, 0, 10); return h[0].ToString(); }));
Check("DrinkPotion", "DrinkPotion never heals past maxHp: health[0] 9 becomes", "10", Try(() => { List<int> h = new List<int> { 9 }; DrinkPotion(new List<string> { "potion" }, new List<string> { "Rook" }, h, 0, 10); return h[0].ToString(); }));
Check("DrinkPotion", "DrinkPotion with no potions changes nothing", "5", Try(() => { List<int> h = new List<int> { 5 }; DrinkPotion(new List<string>(), new List<string> { "Rook" }, h, 0, 10); return h[0].ToString(); }));
Check("StepX", "StepX(\"a\"), StepX(\"d\"), StepX(\"w\")", "-1 1 0", Try(() => StepX("a") + " " + StepX("d") + " " + StepX("w")));
Check("StepY", "StepY(\"w\"), StepY(\"s\"), StepY(\"hello\")", "-1 1 0", Try(() => StepY("w") + " " + StepY("s") + " " + StepY("hello")));

// ---------------------------------------------------------------- Combat.cs, new
Check("HeroAttacks", "HeroAttacks with a bonus of 2: hero attack 4, defense 1, damage is 5 to 7", "True", Try(() => { List<int> mh = new List<int> { 20 }; HeroAttacks(Names(), Attack(), 0, new List<string> { "Ogre" }, mh, 0, 1, 2); int dealt = 20 - mh[0]; return (dealt >= 5 && dealt <= 7).ToString(); }));
Check("SpawnMonsters", "SpawnMonsters(arena ...) puts the boss in the lists too: mNames", "[Grub, " + MyBossName() + "]", Try(() => { List<string> n = new List<string>(); List<int> h = new List<int>(); List<int> a = new List<int>(); List<int> x = new List<int>(); List<int> y = new List<int>(); SpawnMonsters(arena, 4, n, h, a, x, y); return ShowS(n); }));
Check("ClearDefeated", "ClearDefeated takes a monster with 0 health off the map and drops 1 gold", "#@...# 1", Try(() => { List<string> lvl = new List<string> { "######", "#@.M.#", "######" }; List<string> inv = new List<string>(); ClearDefeated(lvl, new List<string> { "Grub" }, new List<int> { 0 }, new List<int> { 3 }, new List<int> { 1 }, inv); return lvl[1] + " " + CountItem(inv, "gold"); }));
Check("ClearDefeated", "ClearDefeated leaves a living monster alone", "#@.M.# 0", Try(() => { List<string> lvl = new List<string> { "######", "#@.M.#", "######" }; List<string> inv = new List<string>(); ClearDefeated(lvl, new List<string> { "Grub" }, new List<int> { 2 }, new List<int> { 3 }, new List<int> { 1 }, inv); return lvl[1] + " " + CountItem(inv, "gold"); }));
Check("ClearDefeated", "ClearDefeated called twice only drops the gold once", "1", Try(() => { List<string> lvl = new List<string> { "######", "#@.M.#", "######" }; List<string> inv = new List<string>(); List<string> n = new List<string> { "Grub" }; List<int> h = new List<int> { 0 }; List<int> x = new List<int> { 3 }; List<int> y = new List<int> { 1 }; ClearDefeated(lvl, n, h, x, y, inv); ClearDefeated(lvl, n, h, x, y, inv); return CountItem(inv, "gold").ToString(); }));

// ---------------------------------------------------------------- Screens.cs, words
Check("FirstWord", "FirstWord(\"cast spark d\")", "cast", Try(() => FirstWord("cast spark d")));
Check("FirstWord", "FirstWord(\"potion\")", "potion", Try(() => FirstWord("potion")));
Check("RestOfWords", "RestOfWords(\"lead Mote\")", "Mote", Try(() => RestOfWords("lead Mote")));
Check("RestOfWords", "RestOfWords(\"potion\")  (nothing after it)", "", Try(() => RestOfWords("potion")));
Check("SecondWord", "SecondWord(\"cast spark d\")", "spark", Try(() => SecondWord("cast spark d")));
Check("SecondWord", "SecondWord(\"fire\")", "", Try(() => SecondWord("fire")));
Check("ThirdWord", "ThirdWord(\"cast spark d\")", "d", Try(() => ThirdWord("cast spark d")));
Check("ThirdWord", "ThirdWord(\"cast mend\")", "", Try(() => ThirdWord("cast mend")));

// ---------------------------------------------------------------- Ranged.cs
Check("ShotPower", "ShotPower(\"arrow\"), ShotPower(\"stone\")", "3 1", Try(() => ShotPower("arrow") + " " + ShotPower("stone")));
Check("ShotRange", "ShotRange(\"arrow\"), ShotRange(\"stone\")", "6 3", Try(() => ShotRange("arrow") + " " + ShotRange("stone")));
Check("TargetInLine", "TargetInLine(hall, ..., from (1,1) going right, range 6)  finds monster", "0", Try(() => TargetInLine(hall, new List<int> { 5, 7 }, new List<int> { 1, 1 }, 1, 1, 1, 0, 6).ToString()));
Check("TargetInLine", "TargetInLine with range 3: the monster is 4 away", "-1", Try(() => TargetInLine(hall, new List<int> { 5, 7 }, new List<int> { 1, 1 }, 1, 1, 1, 0, 3).ToString()));
Check("TargetInLine", "TargetInLine going up hits the wall first", "-1", Try(() => TargetInLine(hall, new List<int> { 5, 7 }, new List<int> { 1, 1 }, 1, 1, 0, -1, 6).ToString()));
Check("TargetInLine", "TargetInLine from (6,1) going right finds the SECOND monster", "1", Try(() => TargetInLine(hall, new List<int> { 5, 7 }, new List<int> { 1, 1 }, 6, 1, 1, 0, 6).ToString()));
Check("DistanceTo", "DistanceTo(mX [5], mY [1], 0, 1, 1)", "4", Try(() => DistanceTo(new List<int> { 5 }, new List<int> { 1 }, 0, 1, 1).ToString()));
Check("DistanceTo", "DistanceTo when the monster is to the LEFT and ABOVE is still positive", "5", Try(() => DistanceTo(new List<int> { 1 }, new List<int> { 1 }, 0, 4, 3).ToString()));
Check("Shoot", "Shoot an arrow right down the hall: true, one arrow used, damage 3 to 4", "True 1 True", Try(() => { List<string> inv = new List<string> { "arrow", "arrow" }; List<int> mh = new List<int> { 20, 20 }; bool took = Shoot(inv, "arrow", hall, Names(), 0, new List<string> { "Grub", "Bat" }, mh, new List<int> { 5, 7 }, new List<int> { 1, 1 }, 1, 1, 1, 0, 0, 0); int dealt = 20 - mh[0]; return took + " " + CountItem(inv, "arrow") + " " + (dealt >= 3 && dealt <= 4); }));
Check("Shoot", "Shoot with no arrows: false, and nobody is hurt", "False 20", Try(() => { List<int> mh = new List<int> { 20, 20 }; bool took = Shoot(new List<string>(), "arrow", hall, Names(), 0, new List<string> { "Grub", "Bat" }, mh, new List<int> { 5, 7 }, new List<int> { 1, 1 }, 1, 1, 1, 0, 0, 0); return took + " " + mh[0]; }));
Check("Shoot", "Shoot with no direction: false, and the arrow is NOT used up", "False 1", Try(() => { List<string> inv = new List<string> { "arrow" }; bool took = Shoot(inv, "arrow", hall, Names(), 0, new List<string> { "Grub", "Bat" }, new List<int> { 20, 20 }, new List<int> { 5, 7 }, new List<int> { 1, 1 }, 1, 1, 0, 0, 0, 0); return took + " " + CountItem(inv, "arrow"); }));
Check("Shoot", "Shoot a stone at a monster 4 away: the stone is used, the monster is not hurt", "True 0 20", Try(() => { List<string> inv = new List<string> { "stone" }; List<int> mh = new List<int> { 20, 20 }; bool took = Shoot(inv, "stone", hall, Names(), 0, new List<string> { "Grub", "Bat" }, mh, new List<int> { 5, 7 }, new List<int> { 1, 1 }, 1, 1, 1, 0, 0, 0); return took + " " + CountItem(inv, "stone") + " " + mh[0]; }));

// ---------------------------------------------------------------- Magic.cs
Check("MakeSpellBook", "MakeSpellBook then the names and the costs", "[spark, mend, quake] [2, 3, 4]", Try(() => { List<string> n = new List<string>(); List<int> c = new List<int>(); MakeSpellBook(n, c); return ShowS(n) + " " + Show(c); }));
Check("AddSpell", "AddSpell(..., \"nova\", 99) limits the cost to", "6", Try(() => { List<string> n = new List<string>(); List<int> c = new List<int>(); AddSpell(n, c, "nova", 99); return c[0].ToString(); }));
Check("FindSpell", "FindSpell([spark, mend, quake], \"quake\")", "2", Try(() => FindSpell(new List<string> { "spark", "mend", "quake" }, "quake").ToString()));
Check("FindSpell", "FindSpell([spark, mend, quake], \"fireball\")", "-1", Try(() => FindSpell(new List<string> { "spark", "mend", "quake" }, "fireball").ToString()));
Check("CanCast", "CanCast(2, 2), CanCast(1, 2)", "True False", Try(() => CanCast(2, 2) + " " + CanCast(1, 2)));
Check("IsAimedSpell", "IsAimedSpell(\"spark\"), IsAimedSpell(\"mend\"), IsAimedSpell(\"quake\")", "True False False", Try(() => IsAimedSpell("spark") + " " + IsAimedSpell("mend") + " " + IsAimedSpell("quake")));
Check("SpellDamage", "SpellDamage(\"spark\", 4, 2)", "4", Try(() => SpellDamage("spark", 4, 2).ToString()));
Check("ManaAfterTurn", "ManaAfterTurn(3, 6, 8)  (turn 8 is a 4th turn)", "4", Try(() => ManaAfterTurn(3, 6, 8).ToString()));
Check("ManaAfterTurn", "ManaAfterTurn(3, 6, 7)", "3", Try(() => ManaAfterTurn(3, 6, 7).ToString()));
Check("ManaAfterTurn", "ManaAfterTurn(6, 6, 8)  (never past the maximum)", "6", Try(() => ManaAfterTurn(6, 6, 8).ToString()));
Check("ManaBar", "ManaBar(4, 6)", "Mana     [****..] 4/6", Try(() => ManaBar(4, 6)));
Check("HurtNearby", "HurtNearby(... at (4,1), reach 2, amount 3): monsters at x 5 and x 7", "1 [17, 20]", Try(() => { List<int> mh = new List<int> { 20, 20 }; int hit = HurtNearby(new List<string> { "Grub", "Bat" }, mh, new List<int> { 5, 7 }, new List<int> { 1, 1 }, 4, 1, 2, 3); return hit + " " + Show(mh); }));
Check("HurtNearby", "HurtNearby does not count a monster that is already defeated", "0", Try(() => HurtNearby(new List<string> { "Grub" }, new List<int> { 0 }, new List<int> { 5 }, new List<int> { 1 }, 4, 1, 2, 3).ToString()));
Check("CastSpell", "CastSpell(\"spark\" going right, mana 6): returns the cost, monster 20 becomes", "2 16", Try(() => { List<int> mh = new List<int> { 20, 20 }; int cost = CastSpell("spark", new List<string> { "spark", "mend", "quake" }, new List<int> { 2, 3, 4 }, 6, hall, Names(), Health(), Attack(), 0, 10, new List<string> { "Grub", "Bat" }, mh, new List<int> { 5, 7 }, new List<int> { 1, 1 }, 1, 1, 1, 0); return cost + " " + mh[0]; }));
Check("CastSpell", "CastSpell(\"spark\") with 1 mana: returns 0 and nobody is hurt", "0 20", Try(() => { List<int> mh = new List<int> { 20, 20 }; int cost = CastSpell("spark", new List<string> { "spark", "mend", "quake" }, new List<int> { 2, 3, 4 }, 1, hall, Names(), Health(), Attack(), 0, 10, new List<string> { "Grub", "Bat" }, mh, new List<int> { 5, 7 }, new List<int> { 1, 1 }, 1, 1, 1, 0); return cost + " " + mh[0]; }));
Check("CastSpell", "CastSpell(\"spark\") with no direction: returns 0", "0", Try(() => CastSpell("spark", new List<string> { "spark", "mend", "quake" }, new List<int> { 2, 3, 4 }, 6, hall, Names(), Health(), Attack(), 0, 10, new List<string> { "Grub" }, new List<int> { 20 }, new List<int> { 5 }, new List<int> { 1 }, 1, 1, 0, 0).ToString()));
Check("CastSpell", "CastSpell(\"mend\"): health[2] 3 becomes 8, and it costs", "3 8", Try(() => { List<int> h = Health(); int cost = CastSpell("mend", new List<string> { "spark", "mend", "quake" }, new List<int> { 2, 3, 4 }, 6, hall, Names(), h, Attack(), 2, 10, new List<string>(), new List<int>(), new List<int>(), new List<int>(), 1, 1, 0, 0); return cost + " " + h[2]; }));
Check("CastSpell", "CastSpell(\"fireball\")  (not in the book): returns", "0", Try(() => CastSpell("fireball", new List<string> { "spark", "mend", "quake" }, new List<int> { 2, 3, 4 }, 6, hall, Names(), Health(), Attack(), 0, 10, new List<string>(), new List<int>(), new List<int>(), new List<int>(), 1, 1, 1, 0).ToString()));

// ---------------------------------------------------------------- Powerups.cs
Check("PowerupName", "PowerupName(0), PowerupName(1), PowerupName(2)", "might guard regen", Try(() => PowerupName(0) + " " + PowerupName(1) + " " + PowerupName(2)));
Check("AddPowerup", "AddPowerup(might, 10) then the two lists", "[might] [10]", Try(() => { List<string> n = new List<string>(); List<int> t = new List<int>(); AddPowerup(n, t, "might", 10); return ShowS(n) + " " + Show(t); }));
Check("AddPowerup", "AddPowerup(might) when might has 3 turns left: NOT added twice, turns topped up", "[might] [10]", Try(() => { List<string> n = new List<string> { "might" }; List<int> t = new List<int> { 3 }; AddPowerup(n, t, "might", 10); return ShowS(n) + " " + Show(t); }));
Check("FindPowerup", "FindPowerup([might, regen], \"regen\"), FindPowerup([might, regen], \"guard\")", "1 -1", Try(() => FindPowerup(new List<string> { "might", "regen" }, "regen") + " " + FindPowerup(new List<string> { "might", "regen" }, "guard")));
Check("PowerupBonus", "PowerupBonus([might], \"might\", 2), PowerupBonus([might], \"guard\", 2)", "2 0", Try(() => PowerupBonus(new List<string> { "might" }, "might", 2) + " " + PowerupBonus(new List<string> { "might" }, "guard", 2)));
Check("TickPowerups", "TickPowerups([might, guard, regen], [1, 5, 1]) removes BOTH spent ones", "[guard] [4]", Try(() => { List<string> n = new List<string> { "might", "guard", "regen" }; List<int> t = new List<int> { 1, 5, 1 }; TickPowerups(n, t); return ShowS(n) + " " + Show(t); }));
Check("TickPowerups", "TickPowerups([might, guard], [1, 1])  (two in a row both end)", "[] []", Try(() => { List<string> n = new List<string> { "might", "guard" }; List<int> t = new List<int> { 1, 1 }; TickPowerups(n, t); return ShowS(n) + " " + Show(t); }));

// ---------------------------------------------------------------- Boss.cs
Check("BossPhase", "BossPhase(18, 18)", "1", Try(() => BossPhase(18, 18).ToString()));
Check("BossPhase", "BossPhase(13, 18)  (above two thirds)", "1", Try(() => BossPhase(13, 18).ToString()));
Check("BossPhase", "BossPhase(12, 18)  (exactly two thirds)", "2", Try(() => BossPhase(12, 18).ToString()));
Check("BossPhase", "BossPhase(7, 18)", "2", Try(() => BossPhase(7, 18).ToString()));
Check("BossPhase", "BossPhase(6, 18)  (exactly one third)", "3", Try(() => BossPhase(6, 18).ToString()));
Check("BossPhase", "BossPhase(1, 18)", "3", Try(() => BossPhase(1, 18).ToString()));
Check("ShockwaveEvery", "ShockwaveEvery(1), ShockwaveEvery(2), ShockwaveEvery(3)", "0 4 3", Try(() => ShockwaveEvery(1) + " " + ShockwaveEvery(2) + " " + ShockwaveEvery(3)));
Check("IsShockwaveTurn", "IsShockwaveTurn(1, 8)  (phase 1 never does, and must not divide by 0)", "False", Try(() => IsShockwaveTurn(1, 8).ToString()));
Check("IsShockwaveTurn", "IsShockwaveTurn(2, 8), IsShockwaveTurn(2, 9), IsShockwaveTurn(3, 9)", "True False True", Try(() => IsShockwaveTurn(2, 8) + " " + IsShockwaveTurn(2, 9) + " " + IsShockwaveTurn(3, 9)));
Check("Shockwave", "Shockwave(..., [10, 0, 3], 2) hurts everyone standing, and leaves the fallen at 0", "[8, 0, 1]", Try(() => { List<int> h = Health(); Shockwave("Warden", Names(), h, 2); return Show(h); }));
Check("Shockwave", "Shockwave never takes anyone below 0", "[8, 0, 0]", Try(() => { List<int> h = new List<int> { 10, 0, 1 }; Shockwave("Warden", Names(), h, 2); return Show(h); }));
Check("FindBoss", "FindBoss(arena, mX [3, 5], mY [1, 1])  (the B is at column 5)", "1", Try(() => FindBoss(arena, new List<int> { 3, 5 }, new List<int> { 1, 1 }).ToString()));
Check("FindBoss", "FindBoss(room ...)  (no B on this level)", "-1", Try(() => FindBoss(room, new List<int> { 1 }, new List<int> { 2 }).ToString()));
Check("IsDoorSealed", "IsDoorSealed([3, 9], 1), IsDoorSealed([3, 0], 1), IsDoorSealed([3], -1)", "True False False", Try(() => IsDoorSealed(new List<int> { 3, 9 }, 1) + " " + IsDoorSealed(new List<int> { 3, 0 }, 1) + " " + IsDoorSealed(new List<int> { 3 }, -1)));
Check("BossBar", "BossBar(\"Warden\", 2, 6)", "BOSS Warden [##----] phase 3", Try(() => BossBar("Warden", 2, 6)));
Check("CheckLevel", "CheckLevel(a level with two B)", "False", Try(() => CheckLevel(new List<string> { "######", "#@BB+#", "######" }).ToString()));
Check("CheckLevel", "CheckLevel(arena)  (N, * and B are all in the tile alphabet)", "True", Try(() => CheckLevel(arena).ToString()));

// ---------------------------------------------------------------- Dialogue.cs
Check("AddLine", "AddLine once, then the Count of all six lists", "1 1 1 1 1 1", Try(() => { var t = NewTalk(); AddLine(t.says, t.a, t.na, t.b, t.nb, t.g, "Hi.", "", -1, "", -1, ""); return t.says.Count + " " + t.a.Count + " " + t.na.Count + " " + t.b.Count + " " + t.nb.Count + " " + t.g.Count; }));
Check("LoadDialogue", "LoadDialogue(2 ...) twice does not double the lines", "7", Try(() => { var t = NewTalk(); LoadDialogue(2, t.says, t.a, t.na, t.b, t.nb, t.g); LoadDialogue(2, t.says, t.a, t.na, t.b, t.nb, t.g); return t.says.Count.ToString(); }));
Check("CheckDialogue", "every conversation from level 1 to LevelCount() passes CheckDialogue", "True", Try(() => { bool ok = true; for (int i = 1; i <= LevelCount(); i++) { var t = NewTalk(); LoadDialogue(i, t.says, t.a, t.na, t.b, t.nb, t.g); if (!CheckDialogue(i, t.says, t.a, t.na, t.b, t.nb, t.g)) ok = false; } return ok.ToString(); }));
Check("CheckDialogue", "CheckDialogue(a line that leads to line 9 of 1)", "False", Try(() => { var t = NewTalk(); AddLine(t.says, t.a, t.na, t.b, t.nb, t.g, "Hi.", "Yes", 9, "No", -1, ""); return CheckDialogue(1, t.says, t.a, t.na, t.b, t.nb, t.g).ToString(); }));
Check("CheckDialogue", "CheckDialogue(a press-Enter line that leads back to itself)", "False", Try(() => { var t = NewTalk(); AddLine(t.says, t.a, t.na, t.b, t.nb, t.g, "Hi.", "", 0, "", -1, ""); return CheckDialogue(1, t.says, t.a, t.na, t.b, t.nb, t.g).ToString(); }));
Check("CheckDialogue", "CheckDialogue(a line that gives \"a million gold\")", "False", Try(() => { var t = NewTalk(); AddLine(t.says, t.a, t.na, t.b, t.nb, t.g, "Hi.", "", -1, "", -1, "a million gold"); return CheckDialogue(1, t.says, t.a, t.na, t.b, t.nb, t.g).ToString(); }));
Check("CheckDialogue", "CheckDialogue(an answer that leads straight back to its own line)", "False", Try(() => { var t = NewTalk(); AddLine(t.says, t.a, t.na, t.b, t.nb, t.g, "Hi.", "Again", 0, "Bye", -1, "give potion"); return CheckDialogue(1, t.says, t.a, t.na, t.b, t.nb, t.g).ToString(); }));
Check("NextLine", "NextLine on a line with two answers: \"1\", \"2\", \"hello\"", "1 5 0", Try(() => { var t = NewTalk(); LoadDialogue(1, t.says, t.a, t.na, t.b, t.nb, t.g); return NextLine(t.a, t.na, t.b, t.nb, 0, "1") + " " + NextLine(t.a, t.na, t.b, t.nb, 0, "2") + " " + NextLine(t.a, t.na, t.b, t.nb, 0, "hello"); }));
Check("NextLine", "NextLine on a press-Enter line moves on whatever is typed", "4", Try(() => { var t = NewTalk(); LoadDialogue(1, t.says, t.a, t.na, t.b, t.nb, t.g); return NextLine(t.a, t.na, t.b, t.nb, 3, "").ToString(); }));
Check("HasFlag", "HasFlag([started 2], \"started 2\"), HasFlag([started 2], \"done 2\")", "True False", Try(() => HasFlag(new List<string> { "started 2" }, "started 2") + " " + HasFlag(new List<string> { "started 2" }, "done 2")));
Check("SetFlag", "SetFlag twice with the same flag: the list has", "1", Try(() => { List<string> f = new List<string>(); SetFlag(f, "met"); SetFlag(f, "met"); return f.Count.ToString(); }));
Check("AllDefeated", "AllDefeated([0, 0]), AllDefeated([0, 2]), AllDefeated([])", "True False True", Try(() => AllDefeated(new List<int> { 0, 0 }) + " " + AllDefeated(new List<int> { 0, 2 }) + " " + AllDefeated(new List<int>())));
Check("QuestState", "QuestState on level 2 with no flags", "0", Try(() => QuestState(new List<string>(), new List<string>(), new List<int>(), 2).ToString()));
Check("QuestState", "QuestState on level 2, started, 2 gold", "1", Try(() => QuestState(new List<string> { "started 2" }, new List<string> { "gold", "gold" }, new List<int>(), 2).ToString()));
Check("QuestState", "QuestState on level 2, started, 3 gold", "2", Try(() => QuestState(new List<string> { "started 2" }, new List<string> { "gold", "gold", "gold" }, new List<int>(), 2).ToString()));
Check("QuestState", "QuestState on level 2, done", "3", Try(() => QuestState(new List<string> { "started 2", "done 2" }, new List<string>(), new List<int>(), 2).ToString()));
Check("QuestState", "QuestState on level 3, started, then with the flag \"boss down 3\"", "1 2", Try(() => QuestState(new List<string> { "started 3" }, new List<string>(), new List<int>(), 3) + " " + QuestState(new List<string> { "started 3", "boss down 3" }, new List<string>(), new List<int>(), 3)));
Check("DialogueStart", "DialogueStart(2, 0), (2, 1), (2, 2), (2, 3)", "0 2 3 6", Try(() => DialogueStart(2, 0) + " " + DialogueStart(2, 1) + " " + DialogueStart(2, 2) + " " + DialogueStart(2, 3)));
Check("AddMany", "AddMany(inventory, \"arrow\", 3) then CountItem", "3", Try(() => { List<string> inv = new List<string>(); AddMany(inv, "arrow", 3); return CountItem(inv, "arrow").ToString(); }));
Check("DoAction", "DoAction(\"finish quest\") on level 2 takes exactly 3 of 5 gold and sets the flag", "2 True", Try(() => { List<string> inv = new List<string> { "gold", "gold", "potion", "gold", "gold", "gold" }; List<string> f = new List<string>(); DoAction("finish quest", 2, f, inv, Health(), 10, new List<string>(), new List<int>()); return CountItem(inv, "gold") + " " + HasFlag(f, "done 2"); }));
Check("DoAction", "DoAction(\"heal party\") heals the living and leaves the fallen", "[10, 0, 10]", Try(() => { List<int> h = Health(); DoAction("heal party", 1, new List<string>(), new List<string>(), h, 10, new List<string>(), new List<int>()); return Show(h); }));
Check("DoAction", "DoAction(\"give arrows\") then CountItem", "3", Try(() => { List<string> inv = new List<string>(); DoAction("give arrows", 1, new List<string>(), inv, Health(), 10, new List<string>(), new List<int>()); return CountItem(inv, "arrow").ToString(); }));
Check("DoAction", "DoAction(\"power guard\") switches guard on", "[guard]", Try(() => { List<string> pn = new List<string>(); DoAction("power guard", 1, new List<string>(), new List<string>(), Health(), 10, pn, new List<int>()); return ShowS(pn); }));
Check("ManaAfterAction", "ManaAfterAction(\"restore mana\", 1, 6), ManaAfterAction(\"give potion\", 1, 6)", "6 1", Try(() => ManaAfterAction("restore mana", 1, 6) + " " + ManaAfterAction("give potion", 1, 6)));

// ---------------------------------------------------------------- YOUR files
Todo("IsFairCharacter", "Milestone 1: IsFairCharacter(10, 4)  (10 + 4 * 2 is 18)", "True", Try(() => IsFairCharacter(10, 4).ToString()));
Todo("IsFairCharacter", "Milestone 1: IsFairCharacter(10, 5)  (10 + 5 * 2 is 20)", "False", Try(() => IsFairCharacter(10, 5).ToString()));
Todo("IsFairCharacter", "Milestone 1: IsFairCharacter(1, 1)", "True", Try(() => IsFairCharacter(1, 1).ToString()));
Todo("IsFairCharacter", "Milestone 1: IsFairCharacter(9, 5)  (9 + 5 * 2 is 19)", "False", Try(() => IsFairCharacter(9, 5).ToString()));
Todo("MyCharacter", "Milestone 1: your character is not still called Hero", "True", Try(() => { List<string> n = new List<string>(); List<int> h = new List<int>(); List<int> a = new List<int>(); MyCharacter(n, h, a); return (FindCharacter(n, "Hero") == -1).ToString(); }));
Check("MyLevel", "MyLevel() obeys the contracts (CheckLevel says True)", "True", Try(() => CheckLevel(MyLevel()).ToString()));
Check("MyCharacter", "MyCharacter adds at least one character, and the three lists stay the same length", "True", Try(() => { List<string> n = new List<string>(); List<int> h = new List<int>(); List<int> a = new List<int>(); MyCharacter(n, h, a); return (n.Count >= 1 && n.Count == h.Count && h.Count == a.Count).ToString(); }));
Check("MyDialogue", "MyDialogue() obeys the contract (CheckDialogue says True)", "True", Try(() => { var t = NewTalk(); LoadDialogue(LevelCount(), t.says, t.a, t.na, t.b, t.nb, t.g); return CheckDialogue(LevelCount(), t.says, t.a, t.na, t.b, t.nb, t.g).ToString(); }));
Check("MySpell", "the spell book (the game's three and yours) has no more than 4 spells, one for each key", "True", Try(() => { List<string> n = new List<string>(); List<int> c = new List<int>(); MakeSpellBook(n, c); MySpell(n, c); return (n.Count <= 4).ToString(); }));
Check("MySpell", "MySpell adds at least one spell, and the two lists stay the same length", "True", Try(() => { List<string> n = new List<string>(); List<int> c = new List<int>(); MySpell(n, c); return (n.Count >= 1 && n.Count == c.Count).ToString(); }));
Check("MySpellDamage", "MySpellDamage is fair: for attack 1 to 5 and distance 1 to 4 it is never below 0 or above cost * 3", "True", Try(() => { List<string> n = new List<string>(); List<int> c = new List<int>(); MySpell(n, c); bool ok = true; for (int atk = 1; atk <= 5; atk++) for (int d = 1; d <= 4; d++) { int dmg = MySpellDamage(atk, d); if (dmg < 0 || dmg > c[0] * 3) ok = false; } return ok.ToString(); }));
Extra("MySpell", "your spell is not still called zap", "True", Try(() => { List<string> n = new List<string>(); List<int> c = new List<int>(); MySpell(n, c); return (FindSpell(n, "zap") == -1).ToString(); }));
Extra("MySpellDamage", "MySpellDamage uses a parameter (it does not give the same answer for every attack and distance)", "True", Try(() => { bool differs = false; for (int atk = 1; atk <= 5; atk++) for (int d = 1; d <= 4; d++) if (MySpellDamage(atk, d) != MySpellDamage(1, 1)) differs = true; return differs.ToString(); }));
Extra("MyDialogue", "the N on your level is not still called Stranger", "True", Try(() => (MyNpcName() != "Stranger").ToString()));
Extra("MyDialogue", "your conversation has at least 4 lines and at least one line with two answers", "True", Try(() => { var t = NewTalk(); MyDialogue(t.says, t.a, t.na, t.b, t.nb, t.g); bool two = false; for (int i = 0; i < t.b.Count; i++) if (t.b[i] != "") two = true; return (t.says.Count >= 4 && two).ToString(); }));


// ---------------------------------------------------------------- can your level be finished?
string Reach(List<string> lvl, char wanted)
{
    int sx = FindTileX(lvl, '@'), sy = FindTileY(lvl, '@');
    if (sx < 0) return "there is no @";
    HashSet<string> seen = new HashSet<string> { sx + "," + sy };
    Queue<int> todo = new Queue<int>(); todo.Enqueue(sx); todo.Enqueue(sy);
    while (todo.Count > 0)
    {
        int x = todo.Dequeue(), y = todo.Dequeue();
        if (lvl[y][x] == '+') continue;      // you can walk ONTO a door but never through it
        int[] dx = { 1, -1, 0, 0 }, dy = { 0, 0, 1, -1 };
        for (int k = 0; k < 4; k++)
        {
            int nx = x + dx[k], ny = y + dy[k];
            if (ny < 0 || ny >= lvl.Count || nx < 0 || nx >= lvl[ny].Length || lvl[ny][nx] == '#' || lvl[ny][nx] == 'N') continue;
            if (seen.Add(nx + "," + ny)) { todo.Enqueue(nx); todo.Enqueue(ny); }
        }
    }
    int total = 0, reached = 0;
    for (int y = 0; y < lvl.Count; y++) for (int x = 0; x < lvl[y].Length; x++) if (lvl[y][x] == wanted) { total++; if (seen.Contains(x + "," + y)) reached++; }
    return reached + " of " + total;
}
Todo("MyLevel", "Milestone 3: MyLevel is at least 14 wide and 9 tall", "True", Try(() => { List<string> l = MyLevel(); return (l.Count >= 9 && l[0].Length >= 14).ToString(); }));
Todo("MyLevel", "Milestone 3: MyLevel has at least 3 monsters and 2 loot", "True", Try(() => (CountTiles(MyLevel(), 'M') >= 3 && CountTiles(MyLevel(), '$') >= 2).ToString()));
Check("MyLevel", "MyLevel(): doors the player can walk to", Reach(MyLevel(), '+').Split(' ')[2] + " of " + Reach(MyLevel(), '+').Split(' ')[2], Try(() => Reach(MyLevel(), '+')));
Check("MyLevel", "MyLevel(): loot the player can walk to", Reach(MyLevel(), '$').Split(' ')[2] + " of " + Reach(MyLevel(), '$').Split(' ')[2], Try(() => Reach(MyLevel(), '$')));
Check("MyLevel", "MyLevel(): monsters the player can walk to", Reach(MyLevel(), 'M').Split(' ')[2] + " of " + Reach(MyLevel(), 'M').Split(' ')[2], Try(() => Reach(MyLevel(), 'M')));
Check("MyLevel", "MyLevel(): powerups the player can walk to", Reach(MyLevel(), '*').Split(' ')[2] + " of " + Reach(MyLevel(), '*').Split(' ')[2], Try(() => Reach(MyLevel(), '*')));
Check("MyLevel", "MyLevel(): bosses the player can walk to  (an N is as solid as a wall)", Reach(MyLevel(), 'B').Split(' ')[2] + " of " + Reach(MyLevel(), 'B').Split(' ')[2], Try(() => Reach(MyLevel(), 'B')));
Extra("MyLevel", "MyLevel has someone to talk to (N), a powerup (*) and a boss (B)", "True", Try(() => (CountTiles(MyLevel(), 'N') >= 1 && CountTiles(MyLevel(), '*') >= 1 && CountTiles(MyLevel(), 'B') == 1).ToString()));

// ---------------------------------------------------------------- the five tools
// Variables, if / else, functions, List<T>, for. This reads your .cs files and looks for anything else.
string dungeonFolder = Path.Combine(Directory.GetCurrentDirectory(), "..", "dungeon");
if (!Directory.Exists(dungeonFolder)) dungeonFolder = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "dungeon");
string[,] rules = {
    { @"\b(class|struct|record|interface|enum)\b", "a type declaration (class, struct, enum ...)" }, { @"\bforeach\b", "foreach. Use for (int i = 0; ...)" },
    { @"\bswitch\b", "switch. Use if / else if / else" }, { @"\bvar\b", "var. Write the real type" }, { @"=>", "=> (a lambda or expression body)" },
    { @"\?", "? (a ternary or a nullable). Use if / else" }, { @"\w\s*\[\s*\]|\bnew\s+\w+\s*\[", "an array. Use List<T>" },
    { @"\b(Dictionary|HashSet)\b", "Dictionary or HashSet" }, { @"\.(Where|Select|Any|All|Sum|Max|Min|First|OrderBy|ToList|ToArray)\s*\(", "LINQ. Write the loop yourself" },
    { @"\b(ref|out|do|goto|try|catch|throw)\b", "something we have not learned (ref, out, do, try ...)" } };
int whileCount = 0; List<string> breaks = new List<string>();
if (Directory.Exists(dungeonFolder))
{
    foreach (string file in Directory.GetFiles(dungeonFolder, "*.cs").OrderBy(f => f))
    {
        string[] lines = File.ReadAllLines(file);
        for (int n = 0; n < lines.Length; n++)
        {
            string code = System.Text.RegularExpressions.Regex.Replace(lines[n], @"\$?""(\\.|[^""\\])*""", "\"\"");
            code = System.Text.RegularExpressions.Regex.Replace(code, @"'(\\.|[^'\\])'", "' '");
            code = System.Text.RegularExpressions.Regex.Replace(code, @"//.*", "");
            if (System.Text.RegularExpressions.Regex.IsMatch(code, @"\bwhile\b")) { whileCount++; if (Path.GetFileName(file) != "Program.cs") breaks.Add(Path.GetFileName(file) + " line " + (n + 1) + ": while. Use for. The game loop in Program.cs is the only while"); }
            for (int r = 0; r < rules.GetLength(0); r++)
                if (System.Text.RegularExpressions.Regex.IsMatch(code, rules[r, 0]) && !(Path.GetFileName(file) == "QuestBoard.cs" && r == rules.GetLength(0) - 1)) breaks.Add(Path.GetFileName(file) + " line " + (n + 1) + ": " + rules[r, 1]);
        }
    }
    if (whileCount > 1) breaks.Add("Program.cs has " + whileCount + " while loops. The game loop is the only one allowed");
    Todo("Program", "Before you hand in: Program.cs starts on level 1 again", "True", File.ReadAllText(Path.Combine(dungeonFolder, "Program.cs")).Contains("int levelNumber = 1;").ToString());
    Check("FiveTools", "Only the five tools are used in dungeon/*.cs. Things that break the rule", "none", breaks.Count == 0 ? "none" : string.Join(" | ", breaks));
}

// <quests>
// Built by tools/make_quests.py. Change the record in tools/quests/, not this file.
// ---------------------------------------------------------------- the daily quests
// A quest row is not counted in the last line of this report. Quests have their own line above it.
int questRows = 0, questPass = 0, challengeRows = 0, challengePass = 0;
string lastQuestHeader = "";
bool questHung = false;
// One row gets three seconds. A loop that never ends is reported, and the rows after it are not run,
// because the loop is still spinning in the background until this program ends.
// Under the VS Code debugger there is no limit, so a breakpoint inside a quest can be held as long as you like.
string Timed(Func<string> run)
{
    string result = "";
    System.Threading.Thread t = new System.Threading.Thread(() => { result = Try(run); });
    t.IsBackground = true; t.Start();
    if (t.Join(System.Diagnostics.Debugger.IsAttached ? -1 : 3000)) return result;
    questHung = true; Console.SetOut(TextWriter.Null);
    return "no answer after 3 seconds (a loop that never ends?)";
}
string QuestSource(string file) { string p = Path.Combine(dungeonFolder, file); return File.Exists(p) ? File.ReadAllText(p) : ""; }
string CodeOnly(string text)
{
    System.Text.StringBuilder kept = new System.Text.StringBuilder(); int i = 0;
    while (i < text.Length)
    {
        if (i + 1 < text.Length && text[i] == '/' && text[i + 1] == '/') { while (i < text.Length && text[i] != '\n') i++; }
        else if (text[i] == '"') { i++; while (i < text.Length && text[i] != '"' && text[i] != '\n') i += text[i] == '\\' ? 2 : 1; i++; kept.Append("\"\""); }
        else if (text[i] == '\'') { i++; while (i < text.Length && text[i] != '\'' && text[i] != '\n') i += text[i] == '\\' ? 2 : 1; i++; kept.Append("' '"); }
        else { kept.Append(text[i]); i++; }
    }
    return kept.ToString();
}
// How many times the body of one function in one quest file uses a name (or a symbol such as <).
int Calls(string file, string function, string what)
{
    string code = CodeOnly(QuestSource(file));
    System.Text.RegularExpressions.Match head = System.Text.RegularExpressions.Regex.Match(code, @"\b" + function + @"\s*\([^)]*\)\s*\{");
    if (!head.Success) return 0;
    int depth = 1, at = head.Index + head.Length, start = at;
    while (at < code.Length && depth > 0) { if (code[at] == '{') depth++; else if (code[at] == '}') depth--; at++; }
    string body = code.Substring(start, at - start);
    if (System.Text.RegularExpressions.Regex.IsMatch(what, @"^[A-Za-z_][\w\.]*$")) return System.Text.RegularExpressions.Regex.Matches(body, @"(?<![\w\.])" + System.Text.RegularExpressions.Regex.Escape(what) + @"\s*\(").Count;
    return System.Text.RegularExpressions.Regex.Matches(body, System.Text.RegularExpressions.Regex.Escape(what)).Count;
}
// needs: a row that an empty function would pass by luck only counts once this other row of the same function passes.
void QuestRow(bool graded, string quest, string header, string file, string marker, string call, string expected, Func<string> run, string needsCall = "", string needsExpected = "", Func<string> needs = null)
{
    if (only != "" && !quest.Contains(only, StringComparison.OrdinalIgnoreCase)) return;
    if (header != lastQuestHeader) { screen.WriteLine(); screen.WriteLine("---- " + header + " ----"); lastQuestHeader = header; }
    bool started = !QuestSource(file).Contains(marker);
    bool ran = started && !questHung;
    string actual = ran ? Timed(run) : "";
    bool ok = ran && actual == expected;
    bool waiting = ok && needs != null && Timed(needs) != needsExpected;
    if (waiting) ok = false;
    if (graded) { questRows++; if (ok) questPass++; } else { challengeRows++; if (ok) challengePass++; }
    if (ok) screen.WriteLine("PASS  " + call + "  is  " + actual);
    else if (!started) screen.WriteLine((graded ? "TO DO " : "EXTRA ") + call + "  should be  " + expected);
    else if (!ran) screen.WriteLine((graded ? "TO DO " : "EXTRA ") + call + "  should be  " + expected + "  (not run: an earlier row never finished)");
    else if (waiting) screen.WriteLine((graded ? "TO DO " : "EXTRA ") + call + "  should be  " + expected + "  (counts once this passes:  " + needsCall + ")");
    else screen.WriteLine((graded ? "TO DO " : "EXTRA ") + call + "  should be  " + expected + "  (yours gives  " + actual + ")");
}
List<string> Shelf(int n) { List<string> all = new List<string> { "rusty key", "bat wing", "ogre tooth", "silver cup", "old map" }; return all.GetRange(0, n); }
string ShelfAfterAdd(int n, string trophy) { List<string> s = Shelf(n); AddTrophy(s, trophy); return ShowS(s); }
string ShelfCountAfterAdd(int n, string trophy) { List<string> s = Shelf(n); AddTrophy(s, trophy); return s.Count.ToString(); }
string BookCounts() { List<string> n = new List<string>(); List<int> h = new List<int>(); AddPage(n, h, "Grub", 4); AddPage(n, h, "Bat", 3); return n.Count + " " + h.Count; }
string BookPage(int page) { List<string> n = new List<string>(); List<int> h = new List<int>(); AddPage(n, h, "Grub", 4); AddPage(n, h, "Bat", 3); return PageText(n, h, page); }
string BookAfterSwap(int i, int j) { List<string> n = new List<string> { "Grub", "Bat", "Ogre" }; List<int> h = new List<int> { 4, 3, 8 }; SwapPages(n, h, i, j); return n[0] + " (" + h[0] + "), " + n[1] + " (" + h[1] + "), " + n[2] + " (" + h[2] + ")"; }
List<string> Three(string a, string b, string c) { return new List<string> { a, b, c }; }
string ThreeAfterTake(string a, string b, string c, string wanted) { List<string> l = Three(a, b, c); TakeFromThree(l, wanted); return ShowS(l); }
List<string> Walk(string commands) { return new List<string>(commands.Split(' ')); }

QuestRow(true, "Quest00", "Quest 0: Party Mood", "Quest00.cs", "QUEST NOT STARTED", "PartyMood(25, 2)", "strong", () => PartyMood(25, 2));
QuestRow(true, "Quest00", "Quest 0: Party Mood", "Quest00.cs", "QUEST NOT STARTED", "PartyMood(17, 0)", "strong", () => PartyMood(17, 0));
QuestRow(true, "Quest00", "Quest 0: Party Mood", "Quest00.cs", "QUEST NOT STARTED", "PartyMood(16, 0)", "hurting", () => PartyMood(16, 0));
QuestRow(true, "Quest00", "Quest 0: Party Mood", "Quest00.cs", "QUEST NOT STARTED", "PartyMood(9, 3)", "hurting", () => PartyMood(9, 3));
QuestRow(true, "Quest00", "Quest 0: Party Mood", "Quest00.cs", "QUEST NOT STARTED", "PartyMood(8, 1)", "in trouble", () => PartyMood(8, 1));
QuestRow(true, "Quest00", "Quest 0: Party Mood", "Quest00.cs", "QUEST NOT STARTED", "PartyMood(8, 0)", "desperate", () => PartyMood(8, 0));
QuestRow(true, "Quest00", "Quest 0: Party Mood", "Quest00.cs", "QUEST NOT STARTED", "PartyMood(0, 0)", "fallen", () => PartyMood(0, 0));
QuestRow(true, "Quest00", "Quest 0: Party Mood", "Quest00.cs", "QUEST NOT STARTED", "PartyMood(0, 5)", "fallen", () => PartyMood(0, 5));
QuestRow(false, "Quest00", "Challenge 0: The Purse   (extra, not graded)", "Quest00.cs", "CHALLENGE NOT STARTED", "GoldRank(0)", "empty", () => GoldRank(0));
QuestRow(false, "Quest00", "Challenge 0: The Purse   (extra, not graded)", "Quest00.cs", "CHALLENGE NOT STARTED", "GoldRank(-3)", "empty", () => GoldRank(-3));
QuestRow(false, "Quest00", "Challenge 0: The Purse   (extra, not graded)", "Quest00.cs", "CHALLENGE NOT STARTED", "GoldRank(1)", "light", () => GoldRank(1));
QuestRow(false, "Quest00", "Challenge 0: The Purse   (extra, not graded)", "Quest00.cs", "CHALLENGE NOT STARTED", "GoldRank(2)", "light", () => GoldRank(2));
QuestRow(false, "Quest00", "Challenge 0: The Purse   (extra, not graded)", "Quest00.cs", "CHALLENGE NOT STARTED", "GoldRank(3)", "jingling", () => GoldRank(3));
QuestRow(false, "Quest00", "Challenge 0: The Purse   (extra, not graded)", "Quest00.cs", "CHALLENGE NOT STARTED", "GoldRank(9)", "jingling", () => GoldRank(9));
QuestRow(false, "Quest00", "Challenge 0: The Purse   (extra, not graded)", "Quest00.cs", "CHALLENGE NOT STARTED", "GoldRank(10)", "heavy", () => GoldRank(10));
QuestRow(true, "Quest01", "Quest 1: The Quest Banner", "Quest01.cs", "QUEST NOT STARTED", "PrintDivider() prints", "~~~~~~~~~~~~~~~~~~~~", () => Printed(() => PrintDivider()));
QuestRow(true, "Quest01", "Quest 1: The Quest Banner", "Quest01.cs", "QUEST NOT STARTED", "PrintQuestBanner() prints, line by line", "~~~~~~~~~~~~~~~~~~~~ /   QUESTS OF THE PARTY / ~~~~~~~~~~~~~~~~~~~~", () => Printed(() => PrintQuestBanner()));
QuestRow(true, "Quest01", "Quest 1: The Quest Banner", "Quest01.cs", "QUEST NOT STARTED", "PrintQuestBanner calls PrintDivider this many times", "2", () => Calls("Quest01.cs", "PrintQuestBanner", "PrintDivider").ToString());
QuestRow(true, "Quest01", "Quest 1: The Quest Banner", "Quest01.cs", "QUEST NOT STARTED", "PrintRookReady() prints", "Rook is ready.", () => Printed(() => PrintRookReady()));
QuestRow(true, "Quest01", "Quest 1: The Quest Banner", "Quest01.cs", "QUEST NOT STARTED", "PrintMoteReady() prints", "Mote is ready.", () => Printed(() => PrintMoteReady()));
QuestRow(false, "Quest01", "Challenge 1: Roll Call   (extra, not graded)", "Quest01.cs", "CHALLENGE NOT STARTED", "PrintRollCall() prints, line by line", "Rook is ready. / Mote is ready. / ~~~~~~~~~~~~~~~~~~~~", () => Printed(() => PrintRollCall()));
QuestRow(false, "Quest01", "Challenge 1: Roll Call   (extra, not graded)", "Quest01.cs", "CHALLENGE NOT STARTED", "PrintRollCall has this many Console.WriteLine in its body", "0", () => Calls("Quest01.cs", "PrintRollCall", "Console.WriteLine").ToString(), "PrintRollCall() prints, line by line", "Rook is ready. / Mote is ready. / ~~~~~~~~~~~~~~~~~~~~", () => Printed(() => PrintRollCall()));
QuestRow(true, "Quest02", "Quest 2: Health Warning", "Quest02.cs", "QUEST NOT STARTED", "PrintReadyLine(\"Rook\") prints", "Rook is ready.", () => Printed(() => PrintReadyLine("Rook")));
QuestRow(true, "Quest02", "Quest 2: Health Warning", "Quest02.cs", "QUEST NOT STARTED", "PrintReadyLine(\"Hero\") prints", "Hero is ready.", () => Printed(() => PrintReadyLine("Hero")));
QuestRow(true, "Quest02", "Quest 2: Health Warning", "Quest02.cs", "QUEST NOT STARTED", "PrintHealthWarning(10) prints", "Healthy at 10.", () => Printed(() => PrintHealthWarning(10)));
QuestRow(true, "Quest02", "Quest 2: Health Warning", "Quest02.cs", "QUEST NOT STARTED", "PrintHealthWarning(7) prints", "Healthy at 7.", () => Printed(() => PrintHealthWarning(7)));
QuestRow(true, "Quest02", "Quest 2: Health Warning", "Quest02.cs", "QUEST NOT STARTED", "PrintHealthWarning(6) prints", "Careful. Health is 6.", () => Printed(() => PrintHealthWarning(6)));
QuestRow(true, "Quest02", "Quest 2: Health Warning", "Quest02.cs", "QUEST NOT STARTED", "PrintHealthWarning(3) prints", "Danger! Health is 3.", () => Printed(() => PrintHealthWarning(3)));
QuestRow(true, "Quest02", "Quest 2: Health Warning", "Quest02.cs", "QUEST NOT STARTED", "PrintHealthWarning(1) prints", "Danger! Health is 1.", () => Printed(() => PrintHealthWarning(1)));
QuestRow(true, "Quest02", "Quest 2: Health Warning", "Quest02.cs", "QUEST NOT STARTED", "PrintHealthWarning(0) prints", "The leader is down!", () => Printed(() => PrintHealthWarning(0)));
QuestRow(false, "Quest02", "Challenge 2: The Door Report   (extra, not graded)", "Quest02.cs", "CHALLENGE NOT STARTED", "PrintDoorStatus(0) prints", "The way is clear.", () => Printed(() => PrintDoorStatus(0)));
QuestRow(false, "Quest02", "Challenge 2: The Door Report   (extra, not graded)", "Quest02.cs", "CHALLENGE NOT STARTED", "PrintDoorStatus(-1) prints", "The way is clear.", () => Printed(() => PrintDoorStatus(-1)));
QuestRow(false, "Quest02", "Challenge 2: The Door Report   (extra, not graded)", "Quest02.cs", "CHALLENGE NOT STARTED", "PrintDoorStatus(1) prints", "1 monster blocks the way.", () => Printed(() => PrintDoorStatus(1)));
QuestRow(false, "Quest02", "Challenge 2: The Door Report   (extra, not graded)", "Quest02.cs", "CHALLENGE NOT STARTED", "PrintDoorStatus(2) prints", "2 monsters block the way.", () => Printed(() => PrintDoorStatus(2)));
QuestRow(false, "Quest02", "Challenge 2: The Door Report   (extra, not graded)", "Quest02.cs", "CHALLENGE NOT STARTED", "PrintDoorStatus(7) prints", "7 monsters block the way.", () => Printed(() => PrintDoorStatus(7)));
QuestRow(true, "Quest03", "Quest 3: Leader Line", "Quest03.cs", "QUEST NOT STARTED", "PrintLeaderLine(\"Rook\", 10, 4) prints", "Leader Rook: health 10, attack 4", () => Printed(() => PrintLeaderLine("Rook", 10, 4)));
QuestRow(true, "Quest03", "Quest 3: Leader Line", "Quest03.cs", "QUEST NOT STARTED", "PrintLeaderLine(\"Mote\", 4, 10) prints", "Leader Mote: health 4, attack 10", () => Printed(() => PrintLeaderLine("Mote", 4, 10)));
QuestRow(true, "Quest03", "Quest 3: Leader Line", "Quest03.cs", "QUEST NOT STARTED", "PrintLeaderLine(\"Mote\", 0, 3) prints", "Leader Mote: health 0, attack 3 (down)", () => Printed(() => PrintLeaderLine("Mote", 0, 3)));
QuestRow(true, "Quest03", "Quest 3: Leader Line", "Quest03.cs", "QUEST NOT STARTED", "PrintTrade(\"Rook\", \"Mote\", \"potion\", 2) prints", "Rook gives Mote 2 x potion.", () => Printed(() => PrintTrade("Rook", "Mote", "potion", 2)));
QuestRow(true, "Quest03", "Quest 3: Leader Line", "Quest03.cs", "QUEST NOT STARTED", "PrintTrade(\"Mote\", \"Rook\", \"arrow\", 3) prints", "Mote gives Rook 3 x arrow.", () => Printed(() => PrintTrade("Mote", "Rook", "arrow", 3)));
QuestRow(false, "Quest03", "Challenge 3: The Duel   (extra, not graded)", "Quest03.cs", "CHALLENGE NOT STARTED", "PrintDuel(\"Rook\", 4, \"Grub\", 2) prints", "Rook hits harder than Grub.", () => Printed(() => PrintDuel("Rook", 4, "Grub", 2)));
QuestRow(false, "Quest03", "Challenge 3: The Duel   (extra, not graded)", "Quest03.cs", "CHALLENGE NOT STARTED", "PrintDuel(\"Rook\", 4, \"Ogre\", 6) prints", "Ogre hits harder than Rook.", () => Printed(() => PrintDuel("Rook", 4, "Ogre", 6)));
QuestRow(false, "Quest03", "Challenge 3: The Duel   (extra, not graded)", "Quest03.cs", "CHALLENGE NOT STARTED", "PrintDuel(\"Mote\", 3, \"Bat\", 3) prints", "Mote and Bat hit equally hard.", () => Printed(() => PrintDuel("Mote", 3, "Bat", 3)));
QuestRow(true, "Quest04", "Quest 4: Lucky Hit", "Quest04.cs", "QUEST NOT STARTED", "CritDamage(4, 6)", "8", () => CritDamage(4, 6).ToString());
QuestRow(true, "Quest04", "Quest 4: Lucky Hit", "Quest04.cs", "QUEST NOT STARTED", "CritDamage(4, 1)", "0", () => CritDamage(4, 1).ToString());
QuestRow(true, "Quest04", "Quest 4: Lucky Hit", "Quest04.cs", "QUEST NOT STARTED", "CritDamage(4, 3)", "4", () => CritDamage(4, 3).ToString());
QuestRow(true, "Quest04", "Quest 4: Lucky Hit", "Quest04.cs", "QUEST NOT STARTED", "CritDamage(3, 2)", "3", () => CritDamage(3, 2).ToString());
QuestRow(true, "Quest04", "Quest 4: Lucky Hit", "Quest04.cs", "QUEST NOT STARTED", "IsLowHealth(3, 10)", "True", () => IsLowHealth(3, 10).ToString());
QuestRow(true, "Quest04", "Quest 4: Lucky Hit", "Quest04.cs", "QUEST NOT STARTED", "IsLowHealth(4, 10)", "False", () => IsLowHealth(4, 10).ToString(), "IsLowHealth(3, 10)", "True", () => IsLowHealth(3, 10).ToString());
QuestRow(true, "Quest04", "Quest 4: Lucky Hit", "Quest04.cs", "QUEST NOT STARTED", "IsLowHealth(2, 6)  (exactly a third)", "True", () => IsLowHealth(2, 6).ToString());
QuestRow(true, "Quest04", "Quest 4: Lucky Hit", "Quest04.cs", "QUEST NOT STARTED", "IsLowHealth(0, 10)", "True", () => IsLowHealth(0, 10).ToString());
QuestRow(true, "Quest04", "Quest 4: Lucky Hit", "Quest04.cs", "QUEST NOT STARTED", "CritDamage prints nothing", "", () => Printed(() => CritDamage(4, 6)), "CritDamage(4, 6)", "8", () => CritDamage(4, 6).ToString());
QuestRow(false, "Quest04", "Challenge 4: Status Line   (extra, not graded)", "Quest04.cs", "CHALLENGE NOT STARTED", "HealthWord(10, 10)", "full", () => HealthWord(10, 10));
QuestRow(false, "Quest04", "Challenge 4: Status Line   (extra, not graded)", "Quest04.cs", "CHALLENGE NOT STARTED", "HealthWord(7, 10)", "hurt", () => HealthWord(7, 10));
QuestRow(false, "Quest04", "Challenge 4: Status Line   (extra, not graded)", "Quest04.cs", "CHALLENGE NOT STARTED", "HealthWord(3, 10)", "critical", () => HealthWord(3, 10));
QuestRow(false, "Quest04", "Challenge 4: Status Line   (extra, not graded)", "Quest04.cs", "CHALLENGE NOT STARTED", "HealthWord(0, 10)", "down", () => HealthWord(0, 10));
QuestRow(false, "Quest04", "Challenge 4: Status Line   (extra, not graded)", "Quest04.cs", "CHALLENGE NOT STARTED", "StatusLine(\"Rook\", 7, 10)", "Rook is hurt", () => StatusLine("Rook", 7, 10));
QuestRow(false, "Quest04", "Challenge 4: Status Line   (extra, not graded)", "Quest04.cs", "CHALLENGE NOT STARTED", "HealthWord calls IsLowHealth this many times", "1", () => Calls("Quest04.cs", "HealthWord", "IsLowHealth").ToString());
QuestRow(false, "Quest04", "Challenge 4: Status Line   (extra, not graded)", "Quest04.cs", "CHALLENGE NOT STARTED", "StatusLine calls HealthWord this many times", "1", () => Calls("Quest04.cs", "StatusLine", "HealthWord").ToString());
QuestRow(true, "Quest05", "Quest 5: Two Repairs", "Quest05.cs", "QUEST NOT STARTED", "ManaWarning(1, 2)", "Not enough mana: 1 of 2 needed.", () => ManaWarning(1, 2));
QuestRow(true, "Quest05", "Quest 5: Two Repairs", "Quest05.cs", "QUEST NOT STARTED", "ManaWarning(0, 4)", "Not enough mana: 0 of 4 needed.", () => ManaWarning(0, 4));
QuestRow(true, "Quest05", "Quest 5: Two Repairs", "Quest05.cs", "QUEST NOT STARTED", "ManaWarning(2, 2)", "Ready to cast.", () => ManaWarning(2, 2));
QuestRow(true, "Quest05", "Quest 5: Two Repairs", "Quest05.cs", "QUEST NOT STARTED", "ManaWarning(6, 4)", "Ready to cast.", () => ManaWarning(6, 4));
QuestRow(true, "Quest05", "Quest 5: Two Repairs", "Quest05.cs", "QUEST NOT STARTED", "ShortByLine(1, 4)", "You are short by 3.", () => ShortByLine(1, 4));
QuestRow(true, "Quest05", "Quest 5: Two Repairs", "Quest05.cs", "QUEST NOT STARTED", "ShortByLine(5, 4)", "You are short by 0.", () => ShortByLine(5, 4));
QuestRow(true, "Quest05", "Quest 5: Two Repairs", "Quest05.cs", "QUEST NOT STARTED", "ShortByLine calls ShortBy this many times", "1", () => Calls("Quest05.cs", "ShortByLine", "ShortBy").ToString());
QuestRow(false, "Quest05", "Challenge 5: Counting Steps   (extra, not graded)", "Quest05.cs", "CHALLENGE NOT STARTED", "CountStep(0, \"w\")", "1", () => CountStep(0, "w").ToString());
QuestRow(false, "Quest05", "Challenge 5: Counting Steps   (extra, not graded)", "Quest05.cs", "CHALLENGE NOT STARTED", "CountStep(5, \"d\")", "6", () => CountStep(5, "d").ToString());
QuestRow(false, "Quest05", "Challenge 5: Counting Steps   (extra, not graded)", "Quest05.cs", "CHALLENGE NOT STARTED", "CountStep(5, \"a\")", "6", () => CountStep(5, "a").ToString());
QuestRow(false, "Quest05", "Challenge 5: Counting Steps   (extra, not graded)", "Quest05.cs", "CHALLENGE NOT STARTED", "CountStep(5, \"s\")", "6", () => CountStep(5, "s").ToString());
QuestRow(false, "Quest05", "Challenge 5: Counting Steps   (extra, not graded)", "Quest05.cs", "CHALLENGE NOT STARTED", "CountStep(5, \"help\")", "5", () => CountStep(5, "help").ToString());
QuestRow(false, "Quest05", "Challenge 5: Counting Steps   (extra, not graded)", "Quest05.cs", "CHALLENGE NOT STARTED", "CountStep(5, \"fire d\")", "5", () => CountStep(5, "fire d").ToString());
QuestRow(true, "Quest06", "Quest 6: Borrowed Tools", "Quest06.cs", "QUEST NOT STARTED", "HealTo(4, 3, 10)", "7", () => HealTo(4, 3, 10).ToString());
QuestRow(true, "Quest06", "Quest 6: Borrowed Tools", "Quest06.cs", "QUEST NOT STARTED", "HealTo(9, 3, 10)", "10", () => HealTo(9, 3, 10).ToString());
QuestRow(true, "Quest06", "Quest 6: Borrowed Tools", "Quest06.cs", "QUEST NOT STARTED", "HealTo(2, -5, 10)", "0", () => HealTo(2, -5, 10).ToString());
QuestRow(true, "Quest06", "Quest 6: Borrowed Tools", "Quest06.cs", "QUEST NOT STARTED", "HitThroughShields(5, 1)", "4", () => HitThroughShields(5, 1).ToString());
QuestRow(true, "Quest06", "Quest 6: Borrowed Tools", "Quest06.cs", "QUEST NOT STARTED", "HitThroughShields(5, 9)  (only 3 shields count)", "2", () => HitThroughShields(5, 9).ToString());
QuestRow(true, "Quest06", "Quest 6: Borrowed Tools", "Quest06.cs", "QUEST NOT STARTED", "HitThroughShields(2, 3)  (a hit is never less than 1)", "1", () => HitThroughShields(2, 3).ToString());
QuestRow(true, "Quest06", "Quest 6: Borrowed Tools", "Quest06.cs", "QUEST NOT STARTED", "HitThroughShields(4, -2)", "4", () => HitThroughShields(4, -2).ToString());
QuestRow(true, "Quest06", "Quest 6: Borrowed Tools", "Quest06.cs", "QUEST NOT STARTED", "HealTo calls Clamp this many times", "1", () => Calls("Quest06.cs", "HealTo", "Clamp").ToString());
QuestRow(true, "Quest06", "Quest 6: Borrowed Tools", "Quest06.cs", "QUEST NOT STARTED", "HitThroughShields calls Damage and Clamp this many times", "1 and 1", () => Calls("Quest06.cs", "HitThroughShields", "Damage") + " and " + Calls("Quest06.cs", "HitThroughShields", "Clamp"));
QuestRow(false, "Quest06", "Challenge 6: Who Is Ahead   (extra, not graded)", "Quest06.cs", "CHALLENGE NOT STARTED", "Matchup(\"Rook\", 7, \"Grub\", 3)", "Rook is ahead.", () => Matchup("Rook", 7, "Grub", 3));
QuestRow(false, "Quest06", "Challenge 6: Who Is Ahead   (extra, not graded)", "Quest06.cs", "CHALLENGE NOT STARTED", "Matchup(\"Rook\", 2, \"Ogre\", 8)", "Ogre is ahead.", () => Matchup("Rook", 2, "Ogre", 8));
QuestRow(false, "Quest06", "Challenge 6: Who Is Ahead   (extra, not graded)", "Quest06.cs", "CHALLENGE NOT STARTED", "Matchup(\"Mote\", 5, \"Bat\", 5)", "It is even.", () => Matchup("Mote", 5, "Bat", 5));
QuestRow(false, "Quest06", "Challenge 6: Who Is Ahead   (extra, not graded)", "Quest06.cs", "CHALLENGE NOT STARTED", "Matchup calls WinnerName this many times", "1", () => Calls("Quest06.cs", "Matchup", "WinnerName").ToString());
QuestRow(false, "Quest06", "Challenge 6: Who Is Ahead   (extra, not graded)", "Quest06.cs", "CHALLENGE NOT STARTED", "Matchup compares numbers with < or > this many times", "0", () => (Calls("Quest06.cs", "Matchup", "<") + Calls("Quest06.cs", "Matchup", ">")).ToString(), "Matchup(\"Rook\", 7, \"Grub\", 3)", "Rook is ahead.", () => Matchup("Rook", 7, "Grub", 3));
QuestRow(true, "Quest07", "Quest 7: Three Gates, One Function", "Quest07.cs", "QUEST NOT STARTED", "PrintGate(\"red\", 1) prints", "The red gate needs 1 key.", () => Printed(() => PrintGate("red", 1)));
QuestRow(true, "Quest07", "Quest 7: Three Gates, One Function", "Quest07.cs", "QUEST NOT STARTED", "PrintGate(\"blue\", 2) prints", "The blue gate needs 2 keys.", () => Printed(() => PrintGate("blue", 2)));
QuestRow(true, "Quest07", "Quest 7: Three Gates, One Function", "Quest07.cs", "QUEST NOT STARTED", "PrintGate(\"iron\", 0) prints", "The iron gate needs 0 keys.", () => Printed(() => PrintGate("iron", 0)));
QuestRow(true, "Quest07", "Quest 7: Three Gates, One Function", "Quest07.cs", "QUEST NOT STARTED", "PrintGate(\"green\", 5) prints", "The green gate needs 5 keys.", () => Printed(() => PrintGate("green", 5)));
QuestRow(true, "Quest07", "Quest 7: Three Gates, One Function", "Quest07.cs", "QUEST NOT STARTED", "PrintGoldGate() still prints", "The gold gate needs 3 keys.", () => Printed(() => PrintGoldGate()), "PrintGate(\"red\", 1) prints", "The red gate needs 1 key.", () => Printed(() => PrintGate("red", 1)));
QuestRow(true, "Quest07", "Quest 7: Three Gates, One Function", "Quest07.cs", "QUEST NOT STARTED", "The three old functions call PrintGate this many times in all", "3", () => (Calls("Quest07.cs", "PrintRedGate", "PrintGate") + Calls("Quest07.cs", "PrintBlueGate", "PrintGate") + Calls("Quest07.cs", "PrintGoldGate", "PrintGate")).ToString());
QuestRow(false, "Quest07", "Challenge 7: The Other Direction   (extra, not graded)", "Quest07.cs", "CHALLENGE NOT STARTED", "SmallPotionLabel()", "small potion (+3)", () => SmallPotionLabel());
QuestRow(false, "Quest07", "Challenge 7: The Other Direction   (extra, not graded)", "Quest07.cs", "CHALLENGE NOT STARTED", "LargePotionLabel()", "large potion (+8)", () => LargePotionLabel());
QuestRow(false, "Quest07", "Challenge 7: The Other Direction   (extra, not graded)", "Quest07.cs", "CHALLENGE NOT STARTED", "The two of them call PotionLabel this many times", "0", () => (Calls("Quest07.cs", "SmallPotionLabel", "PotionLabel") + Calls("Quest07.cs", "LargePotionLabel", "PotionLabel")).ToString(), "SmallPotionLabel()", "small potion (+3)", () => SmallPotionLabel());
QuestRow(true, "Quest08", "Quest 8: The Trophy Shelf", "Quest08.cs", "QUEST NOT STARTED", "MakeTrophyShelf()", "[rusty key, bat wing, ogre tooth]", () => ShowS(MakeTrophyShelf()));
QuestRow(true, "Quest08", "Quest 8: The Trophy Shelf", "Quest08.cs", "QUEST NOT STARTED", "FirstTrophy([rusty key, bat wing, ogre tooth])", "rusty key", () => FirstTrophy(Shelf(3)));
QuestRow(true, "Quest08", "Quest 8: The Trophy Shelf", "Quest08.cs", "QUEST NOT STARTED", "FirstTrophy([])", "nothing", () => FirstTrophy(Shelf(0)));
QuestRow(true, "Quest08", "Quest 8: The Trophy Shelf", "Quest08.cs", "QUEST NOT STARTED", "LastTrophy([rusty key, bat wing, ogre tooth])", "ogre tooth", () => LastTrophy(Shelf(3)));
QuestRow(true, "Quest08", "Quest 8: The Trophy Shelf", "Quest08.cs", "QUEST NOT STARTED", "LastTrophy([rusty key])", "rusty key", () => LastTrophy(Shelf(1)));
QuestRow(true, "Quest08", "Quest 8: The Trophy Shelf", "Quest08.cs", "QUEST NOT STARTED", "LastTrophy([])", "nothing", () => LastTrophy(Shelf(0)));
QuestRow(true, "Quest08", "Quest 8: The Trophy Shelf", "Quest08.cs", "QUEST NOT STARTED", "AddTrophy(shelf of 3, \"crown\"), then the shelf", "[rusty key, bat wing, ogre tooth, crown]", () => ShelfAfterAdd(3, "crown"));
QuestRow(true, "Quest08", "Quest 8: The Trophy Shelf", "Quest08.cs", "QUEST NOT STARTED", "AddTrophy(shelf of 5, \"crown\"), then the Count", "5", () => ShelfCountAfterAdd(5, "crown"), "AddTrophy(shelf of 3, \"crown\"), then the shelf", "[rusty key, bat wing, ogre tooth, crown]", () => ShelfAfterAdd(3, "crown"));
QuestRow(true, "Quest08", "Quest 8: The Trophy Shelf", "Quest08.cs", "QUEST NOT STARTED", "AddTrophy(shelf of 5, \"crown\") prints", "The shelf is full.", () => Printed(() => AddTrophy(Shelf(5), "crown")));
QuestRow(false, "Quest08", "Challenge 8: Reaching In   (extra, not graded)", "Quest08.cs", "CHALLENGE NOT STARTED", "TrophyAt([rusty key, bat wing, ogre tooth], 1)", "bat wing", () => TrophyAt(Shelf(3), 1));
QuestRow(false, "Quest08", "Challenge 8: Reaching In   (extra, not graded)", "Quest08.cs", "CHALLENGE NOT STARTED", "TrophyAt([rusty key, bat wing, ogre tooth], 3)", "nothing", () => TrophyAt(Shelf(3), 3));
QuestRow(false, "Quest08", "Challenge 8: Reaching In   (extra, not graded)", "Quest08.cs", "CHALLENGE NOT STARTED", "TrophyAt([rusty key, bat wing, ogre tooth], -1)", "nothing", () => TrophyAt(Shelf(3), -1));
QuestRow(false, "Quest08", "Challenge 8: Reaching In   (extra, not graded)", "Quest08.cs", "CHALLENGE NOT STARTED", "MiddleTrophy(shelf of 3)", "bat wing", () => MiddleTrophy(Shelf(3)));
QuestRow(false, "Quest08", "Challenge 8: Reaching In   (extra, not graded)", "Quest08.cs", "CHALLENGE NOT STARTED", "MiddleTrophy(shelf of 4)", "ogre tooth", () => MiddleTrophy(Shelf(4)));
QuestRow(false, "Quest08", "Challenge 8: Reaching In   (extra, not graded)", "Quest08.cs", "CHALLENGE NOT STARTED", "MiddleTrophy([])", "nothing", () => MiddleTrophy(Shelf(0)));
QuestRow(true, "Quest09", "Quest 9: The Monster Book", "Quest09.cs", "QUEST NOT STARTED", "AddPage twice, then the two Counts", "2 2", () => BookCounts());
QuestRow(true, "Quest09", "Quest 9: The Monster Book", "Quest09.cs", "QUEST NOT STARTED", "AddPage(..., \"Grub\", 4) and AddPage(..., \"Bat\", 3), then PageText(..., 0)", "Page 0: Grub (4)", () => BookPage(0));
QuestRow(true, "Quest09", "Quest 9: The Monster Book", "Quest09.cs", "QUEST NOT STARTED", "the same book, PageText(..., 1)", "Page 1: Bat (3)", () => BookPage(1));
QuestRow(true, "Quest09", "Quest 9: The Monster Book", "Quest09.cs", "QUEST NOT STARTED", "the same book, PageText(..., 2)", "No such page.", () => BookPage(2));
QuestRow(true, "Quest09", "Quest 9: The Monster Book", "Quest09.cs", "QUEST NOT STARTED", "the same book, PageText(..., -1)", "No such page.", () => BookPage(-1));
QuestRow(false, "Quest09", "Challenge 9: Swap Two Pages   (extra, not graded)", "Quest09.cs", "CHALLENGE NOT STARTED", "a book of Grub 4, Bat 3, Ogre 8. SwapPages(..., 0, 2), then every page", "Ogre (8), Bat (3), Grub (4)", () => BookAfterSwap(0, 2));
QuestRow(false, "Quest09", "Challenge 9: Swap Two Pages   (extra, not graded)", "Quest09.cs", "CHALLENGE NOT STARTED", "the same book, SwapPages(..., 1, 1)", "Grub (4), Bat (3), Ogre (8)", () => BookAfterSwap(1, 1), "a book of Grub 4, Bat 3, Ogre 8. SwapPages(..., 0, 2), then every page", "Ogre (8), Bat (3), Grub (4)", () => BookAfterSwap(0, 2));
QuestRow(false, "Quest09", "Challenge 9: Swap Two Pages   (extra, not graded)", "Quest09.cs", "CHALLENGE NOT STARTED", "the same book, SwapPages(..., 0, 3)", "Grub (4), Bat (3), Ogre (8)", () => BookAfterSwap(0, 3), "a book of Grub 4, Bat 3, Ogre 8. SwapPages(..., 0, 2), then every page", "Ogre (8), Bat (3), Grub (4)", () => BookAfterSwap(0, 2));
QuestRow(false, "Quest09", "Challenge 9: Swap Two Pages   (extra, not graded)", "Quest09.cs", "CHALLENGE NOT STARTED", "the same book, SwapPages(..., -1, 2)", "Grub (4), Bat (3), Ogre (8)", () => BookAfterSwap(-1, 2), "a book of Grub 4, Bat 3, Ogre 8. SwapPages(..., 0, 2), then every page", "Ogre (8), Bat (3), Grub (4)", () => BookAfterSwap(0, 2));
QuestRow(true, "Quest10", "Quest 10: Finding by Hand", "Quest10.cs", "QUEST NOT STARTED", "FindInThree([potion, arrow, potion], \"arrow\")", "1", () => FindInThree(Three("potion", "arrow", "potion"), "arrow").ToString());
QuestRow(true, "Quest10", "Quest 10: Finding by Hand", "Quest10.cs", "QUEST NOT STARTED", "FindInThree([potion, arrow, potion], \"potion\")  (the first one)", "0", () => FindInThree(Three("potion", "arrow", "potion"), "potion").ToString());
QuestRow(true, "Quest10", "Quest 10: Finding by Hand", "Quest10.cs", "QUEST NOT STARTED", "FindInThree([gold, gold, stone], \"stone\")", "2", () => FindInThree(Three("gold", "gold", "stone"), "stone").ToString());
QuestRow(true, "Quest10", "Quest 10: Finding by Hand", "Quest10.cs", "QUEST NOT STARTED", "FindInThree([potion, arrow, potion], \"shield\")", "-1", () => FindInThree(Three("potion", "arrow", "potion"), "shield").ToString());
QuestRow(true, "Quest10", "Quest 10: Finding by Hand", "Quest10.cs", "QUEST NOT STARTED", "CountInThree([potion, arrow, potion], \"potion\")", "2", () => CountInThree(Three("potion", "arrow", "potion"), "potion").ToString());
QuestRow(true, "Quest10", "Quest 10: Finding by Hand", "Quest10.cs", "QUEST NOT STARTED", "CountInThree([potion, arrow, potion], \"arrow\")", "1", () => CountInThree(Three("potion", "arrow", "potion"), "arrow").ToString());
QuestRow(true, "Quest10", "Quest 10: Finding by Hand", "Quest10.cs", "QUEST NOT STARTED", "CountInThree([gold, gold, gold], \"gold\")", "3", () => CountInThree(Three("gold", "gold", "gold"), "gold").ToString());
QuestRow(true, "Quest10", "Quest 10: Finding by Hand", "Quest10.cs", "QUEST NOT STARTED", "CountInThree([potion, arrow, potion], \"shield\")", "0", () => CountInThree(Three("potion", "arrow", "potion"), "shield").ToString());
QuestRow(false, "Quest10", "Challenge 10: Take One   (extra, not graded)", "Quest10.cs", "CHALLENGE NOT STARTED", "TakeFromThree([potion, arrow, potion], \"arrow\")", "took arrow, 2 left", () => TakeFromThree(Three("potion", "arrow", "potion"), "arrow"));
QuestRow(false, "Quest10", "Challenge 10: Take One   (extra, not graded)", "Quest10.cs", "CHALLENGE NOT STARTED", "TakeFromThree([potion, arrow, potion], \"shield\")", "no shield here", () => TakeFromThree(Three("potion", "arrow", "potion"), "shield"));
QuestRow(false, "Quest10", "Challenge 10: Take One   (extra, not graded)", "Quest10.cs", "CHALLENGE NOT STARTED", "TakeFromThree([potion, arrow, potion], \"potion\"), then the list", "[arrow, potion]", () => ThreeAfterTake("potion", "arrow", "potion", "potion"));
QuestRow(false, "Quest10", "Challenge 10: Take One   (extra, not graded)", "Quest10.cs", "CHALLENGE NOT STARTED", "TakeFromThree calls FindInThree this many times", "1", () => Calls("Quest10.cs", "TakeFromThree", "FindInThree").ToString());
QuestRow(true, "Quest11", "Quest 11: Bars and Dots", "Quest11.cs", "QUEST NOT STARTED", "Stars(3)", "[***]", () => Stars(3));
QuestRow(true, "Quest11", "Quest 11: Bars and Dots", "Quest11.cs", "QUEST NOT STARTED", "Stars(1)", "[*]", () => Stars(1));
QuestRow(true, "Quest11", "Quest 11: Bars and Dots", "Quest11.cs", "QUEST NOT STARTED", "Stars(0)", "[]", () => Stars(0));
QuestRow(true, "Quest11", "Quest 11: Bars and Dots", "Quest11.cs", "QUEST NOT STARTED", "Stars(10)", "[**********]", () => Stars(10));
QuestRow(true, "Quest11", "Quest 11: Bars and Dots", "Quest11.cs", "QUEST NOT STARTED", "ManaDots(3, 6)", "ooo...", () => ManaDots(3, 6));
QuestRow(true, "Quest11", "Quest 11: Bars and Dots", "Quest11.cs", "QUEST NOT STARTED", "ManaDots(0, 4)", "....", () => ManaDots(0, 4));
QuestRow(true, "Quest11", "Quest 11: Bars and Dots", "Quest11.cs", "QUEST NOT STARTED", "ManaDots(4, 4)", "oooo", () => ManaDots(4, 4));
QuestRow(true, "Quest11", "Quest 11: Bars and Dots", "Quest11.cs", "QUEST NOT STARTED", "ManaDots(1, 2)", "o.", () => ManaDots(1, 2));
QuestRow(false, "Quest11", "Challenge 11: The Ruler   (extra, not graded)", "Quest11.cs", "CHALLENGE NOT STARTED", "Ruler(5)", "01234", () => Ruler(5));
QuestRow(false, "Quest11", "Challenge 11: The Ruler   (extra, not graded)", "Quest11.cs", "CHALLENGE NOT STARTED", "Ruler(10)", "0123456789", () => Ruler(10));
QuestRow(false, "Quest11", "Challenge 11: The Ruler   (extra, not graded)", "Quest11.cs", "CHALLENGE NOT STARTED", "Ruler(13)", "0123456789012", () => Ruler(13));
QuestRow(false, "Quest11", "Challenge 11: The Ruler   (extra, not graded)", "Quest11.cs", "CHALLENGE NOT STARTED", "Ruler(1)", "0", () => Ruler(1));
QuestRow(true, "Quest12", "Quest 12: Every Name", "Quest12.cs", "QUEST NOT STARTED", "JoinNames([Rook, Mote, Grub])", "Rook, Mote, Grub", () => JoinNames(Names()));
QuestRow(true, "Quest12", "Quest 12: Every Name", "Quest12.cs", "QUEST NOT STARTED", "JoinNames([Rook])", "Rook", () => JoinNames(new List<string> { "Rook" }));
QuestRow(true, "Quest12", "Quest 12: Every Name", "Quest12.cs", "QUEST NOT STARTED", "JoinNames([Rook, Mote])", "Rook, Mote", () => JoinNames(new List<string> { "Rook", "Mote" }));
QuestRow(true, "Quest12", "Quest 12: Every Name", "Quest12.cs", "QUEST NOT STARTED", "JoinNames([])", "nobody", () => JoinNames(new List<string>()));
QuestRow(true, "Quest12", "Quest 12: Every Name", "Quest12.cs", "QUEST NOT STARTED", "CountAlive([10, 0, 3])", "2", () => CountAlive(Health()).ToString());
QuestRow(true, "Quest12", "Quest 12: Every Name", "Quest12.cs", "QUEST NOT STARTED", "CountAlive([0, 0])", "0", () => CountAlive(new List<int> { 0, 0 }).ToString());
QuestRow(true, "Quest12", "Quest 12: Every Name", "Quest12.cs", "QUEST NOT STARTED", "CountAlive([5, 5, 5, 5])", "4", () => CountAlive(new List<int> { 5, 5, 5, 5 }).ToString());
QuestRow(true, "Quest12", "Quest 12: Every Name", "Quest12.cs", "QUEST NOT STARTED", "CountAlive([])", "0", () => CountAlive(new List<int>()).ToString());
QuestRow(false, "Quest12", "Challenge 12: The Last One   (extra, not graded)", "Quest12.cs", "CHALLENGE NOT STARTED", "FindLast([potion, arrow, potion, gold], \"potion\")", "2", () => FindLast(new List<string> { "potion", "arrow", "potion", "gold" }, "potion").ToString());
QuestRow(false, "Quest12", "Challenge 12: The Last One   (extra, not graded)", "Quest12.cs", "CHALLENGE NOT STARTED", "FindLast([potion, arrow, potion, gold], \"gold\")", "3", () => FindLast(new List<string> { "potion", "arrow", "potion", "gold" }, "gold").ToString());
QuestRow(false, "Quest12", "Challenge 12: The Last One   (extra, not graded)", "Quest12.cs", "CHALLENGE NOT STARTED", "FindLast([potion, arrow, potion, gold], \"shield\")", "-1", () => FindLast(new List<string> { "potion", "arrow", "potion", "gold" }, "shield").ToString());
QuestRow(false, "Quest12", "Challenge 12: The Last One   (extra, not graded)", "Quest12.cs", "CHALLENGE NOT STARTED", "FindLast([], \"gold\")", "-1", () => FindLast(new List<string>(), "gold").ToString());
QuestRow(true, "Quest13", "Quest 13: Totals and Records", "Quest13.cs", "QUEST NOT STARTED", "TotalAttack([4, 3, 5])", "12", () => TotalAttack(Attack()).ToString());
QuestRow(true, "Quest13", "Quest 13: Totals and Records", "Quest13.cs", "QUEST NOT STARTED", "TotalAttack([])", "0", () => TotalAttack(new List<int>()).ToString());
QuestRow(true, "Quest13", "Quest 13: Totals and Records", "Quest13.cs", "QUEST NOT STARTED", "WeakestIndex([10, 0, 3])", "1", () => WeakestIndex(Health()).ToString());
QuestRow(true, "Quest13", "Quest 13: Totals and Records", "Quest13.cs", "QUEST NOT STARTED", "WeakestIndex([4, 9, 4])  (a tie goes to the first)", "0", () => WeakestIndex(new List<int> { 4, 9, 4 }).ToString());
QuestRow(true, "Quest13", "Quest 13: Totals and Records", "Quest13.cs", "QUEST NOT STARTED", "WeakestIndex([9, 8, 7])", "2", () => WeakestIndex(new List<int> { 9, 8, 7 }).ToString());
QuestRow(true, "Quest13", "Quest 13: Totals and Records", "Quest13.cs", "QUEST NOT STARTED", "WeakestIndex([])", "-1", () => WeakestIndex(new List<int>()).ToString());
QuestRow(true, "Quest13", "Quest 13: Totals and Records", "Quest13.cs", "QUEST NOT STARTED", "AllAlive([10, 0, 3])", "False", () => AllAlive(Health()).ToString(), "AllAlive([10, 7, 8])", "True", () => AllAlive(new List<int> { 10, 7, 8 }).ToString());
QuestRow(true, "Quest13", "Quest 13: Totals and Records", "Quest13.cs", "QUEST NOT STARTED", "AllAlive([10, 7, 8])", "True", () => AllAlive(new List<int> { 10, 7, 8 }).ToString());
QuestRow(true, "Quest13", "Quest 13: Totals and Records", "Quest13.cs", "QUEST NOT STARTED", "AllAlive([10, 7, 0])  (the last one)", "False", () => AllAlive(new List<int> { 10, 7, 0 }).ToString(), "AllAlive([10, 7, 8])", "True", () => AllAlive(new List<int> { 10, 7, 8 }).ToString());
QuestRow(true, "Quest13", "Quest 13: Totals and Records", "Quest13.cs", "QUEST NOT STARTED", "AllAlive([])", "True", () => AllAlive(new List<int>()).ToString());
QuestRow(false, "Quest13", "Challenge 13: Who Needs the Potion   (extra, not graded)", "Quest13.cs", "CHALLENGE NOT STARTED", "WeakestAliveIndex([10, 0, 3])", "2", () => WeakestAliveIndex(Health()).ToString());
QuestRow(false, "Quest13", "Challenge 13: Who Needs the Potion   (extra, not graded)", "Quest13.cs", "CHALLENGE NOT STARTED", "WeakestAliveIndex([0, 7, 2, 0])", "2", () => WeakestAliveIndex(new List<int> { 0, 7, 2, 0 }).ToString());
QuestRow(false, "Quest13", "Challenge 13: Who Needs the Potion   (extra, not graded)", "Quest13.cs", "CHALLENGE NOT STARTED", "WeakestAliveIndex([5, 5])", "0", () => WeakestAliveIndex(new List<int> { 5, 5 }).ToString());
QuestRow(false, "Quest13", "Challenge 13: Who Needs the Potion   (extra, not graded)", "Quest13.cs", "CHALLENGE NOT STARTED", "WeakestAliveIndex([0, 0])", "-1", () => WeakestAliveIndex(new List<int> { 0, 0 }).ToString());
QuestRow(false, "Quest13", "Challenge 13: Who Needs the Potion   (extra, not graded)", "Quest13.cs", "CHALLENGE NOT STARTED", "WeakestAliveIndex([])", "-1", () => WeakestAliveIndex(new List<int>()).ToString());
QuestRow(true, "Quest14", "Quest 14: Rows and Columns", "Quest14.cs", "QUEST NOT STARTED", "CountInColumn(room, 0, '#')", "4", () => CountInColumn(room, 0, '#').ToString());
QuestRow(true, "Quest14", "Quest 14: Rows and Columns", "Quest14.cs", "QUEST NOT STARTED", "CountInColumn(room, 1, '#')", "2", () => CountInColumn(room, 1, '#').ToString());
QuestRow(true, "Quest14", "Quest 14: Rows and Columns", "Quest14.cs", "QUEST NOT STARTED", "CountInColumn(room, 2, '.')", "2", () => CountInColumn(room, 2, '.').ToString());
QuestRow(true, "Quest14", "Quest 14: Rows and Columns", "Quest14.cs", "QUEST NOT STARTED", "CountInColumn(room, 3, 'M')", "0", () => CountInColumn(room, 3, 'M').ToString());
QuestRow(true, "Quest14", "Quest 14: Rows and Columns", "Quest14.cs", "QUEST NOT STARTED", "CountEither(room, '$', '+')", "2", () => CountEither(room, '$', '+').ToString());
QuestRow(true, "Quest14", "Quest 14: Rows and Columns", "Quest14.cs", "QUEST NOT STARTED", "CountEither(room, '@', 'M')", "2", () => CountEither(room, '@', 'M').ToString());
QuestRow(true, "Quest14", "Quest 14: Rows and Columns", "Quest14.cs", "QUEST NOT STARTED", "CountEither(room, '#', '#')  (each wall once)", "14", () => CountEither(room, '#', '#').ToString());
QuestRow(true, "Quest14", "Quest 14: Rows and Columns", "Quest14.cs", "QUEST NOT STARTED", "CountEither(hall, 'M', '.')", "6", () => CountEither(hall, 'M', '.').ToString());
QuestRow(false, "Quest14", "Challenge 14: The Richest Row   (extra, not graded)", "Quest14.cs", "CHALLENGE NOT STARTED", "RichestRow(room)", "1", () => RichestRow(room).ToString());
QuestRow(false, "Quest14", "Challenge 14: The Richest Row   (extra, not graded)", "Quest14.cs", "CHALLENGE NOT STARTED", "RichestRow(hall)  (no loot at all)", "-1", () => RichestRow(hall).ToString());
QuestRow(false, "Quest14", "Challenge 14: The Richest Row   (extra, not graded)", "Quest14.cs", "CHALLENGE NOT STARTED", "RichestRow([#$#, #$$, $$#])  (a tie goes to the first)", "1", () => RichestRow(new List<string> { "#$#", "#$$", "$$#" }).ToString());
QuestRow(true, "Quest15", "Quest 15: Wide Screen", "Quest15.cs", "QUEST NOT STARTED", "DrawWide(room, 1, 1) prints, line by line", "########## / ##@@..$$## / ##MM..++## / ##########", () => Printed(() => DrawWide(room, 1, 1)));
QuestRow(true, "Quest15", "Quest 15: Wide Screen", "Quest15.cs", "QUEST NOT STARTED", "DrawWide(room, 2, 1)  (the player has stepped right)", "########## / ##..@@$$## / ##MM..++## / ##########", () => Printed(() => DrawWide(room, 2, 1)));
QuestRow(true, "Quest15", "Quest 15: Wide Screen", "Quest15.cs", "QUEST NOT STARTED", "DrawWide(room, 2, 2)", "########## / ##....$$## / ##MM@@++## / ##########", () => Printed(() => DrawWide(room, 2, 2)));
QuestRow(true, "Quest15", "Quest 15: Wide Screen", "Quest15.cs", "QUEST NOT STARTED", "DrawWide(hall, 1, 1)", "################## / ##@@......MM..MM## / ##################", () => Printed(() => DrawWide(hall, 1, 1)));
QuestRow(false, "Quest15", "Challenge 15: Fog   (extra, not graded)", "Quest15.cs", "CHALLENGE NOT STARTED", "DrawFog(room, 1, 1) prints, line by line", "###?? / #@.$? / #M.?? / ?#???", () => Printed(() => DrawFog(room, 1, 1)));
QuestRow(false, "Quest15", "Challenge 15: Fog   (extra, not graded)", "Quest15.cs", "CHALLENGE NOT STARTED", "DrawFog(hall, 3, 1)", "??###???? / ?..@.M??? / ??###????", () => Printed(() => DrawFog(hall, 3, 1)));
QuestRow(true, "Quest16", "Quest 16: The Walk", "Quest16.cs", "QUEST NOT STARTED", "WalkX([d, d, s, a], 1)", "2", () => WalkX(Walk("d d s a"), 1).ToString());
QuestRow(true, "Quest16", "Quest 16: The Walk", "Quest16.cs", "QUEST NOT STARTED", "WalkY([d, d, s, a], 1)", "2", () => WalkY(Walk("d d s a"), 1).ToString());
QuestRow(true, "Quest16", "Quest 16: The Walk", "Quest16.cs", "QUEST NOT STARTED", "WalkX([a, a, help, w], 5)", "3", () => WalkX(Walk("a a help w"), 5).ToString());
QuestRow(true, "Quest16", "Quest 16: The Walk", "Quest16.cs", "QUEST NOT STARTED", "WalkY([a, a, help, w], 5)", "4", () => WalkY(Walk("a a help w"), 5).ToString());
QuestRow(true, "Quest16", "Quest 16: The Walk", "Quest16.cs", "QUEST NOT STARTED", "WalkX([], 7)", "7", () => WalkX(new List<string>(), 7).ToString());
QuestRow(true, "Quest16", "Quest 16: The Walk", "Quest16.cs", "QUEST NOT STARTED", "KeepPlaying(\"d\", true, false)", "True", () => KeepPlaying("d", true, false).ToString());
QuestRow(true, "Quest16", "Quest 16: The Walk", "Quest16.cs", "QUEST NOT STARTED", "KeepPlaying(\"quit\", true, false)", "False", () => KeepPlaying("quit", true, false).ToString(), "KeepPlaying(\"d\", true, false)", "True", () => KeepPlaying("d", true, false).ToString());
QuestRow(true, "Quest16", "Quest 16: The Walk", "Quest16.cs", "QUEST NOT STARTED", "KeepPlaying(\"d\", false, false)", "False", () => KeepPlaying("d", false, false).ToString(), "KeepPlaying(\"d\", true, false)", "True", () => KeepPlaying("d", true, false).ToString());
QuestRow(true, "Quest16", "Quest 16: The Walk", "Quest16.cs", "QUEST NOT STARTED", "KeepPlaying(\"d\", true, true)", "False", () => KeepPlaying("d", true, true).ToString(), "KeepPlaying(\"d\", true, false)", "True", () => KeepPlaying("d", true, false).ToString());
QuestRow(false, "Quest16", "Challenge 16: The Walk, With Walls   (extra, not graded)", "Quest16.cs", "CHALLENGE NOT STARTED", "WalkReport(room, [d, d, d])", "ends at 3,1 after 2 steps and 1 bumps", () => WalkReport(room, Walk("d d d")));
QuestRow(false, "Quest16", "Challenge 16: The Walk, With Walls   (extra, not graded)", "Quest16.cs", "CHALLENGE NOT STARTED", "WalkReport(room, [w, a, s, help, d])", "ends at 2,2 after 2 steps and 2 bumps", () => WalkReport(room, Walk("w a s help d")));
QuestRow(false, "Quest16", "Challenge 16: The Walk, With Walls   (extra, not graded)", "Quest16.cs", "CHALLENGE NOT STARTED", "WalkReport(hall, [])", "ends at 1,1 after 0 steps and 0 bumps", () => WalkReport(hall, new List<string>()));
if (questRows + challengeRows > 0)
{
    screen.WriteLine();
    screen.WriteLine("Quests: " + questPass + " of " + questRows + " rows pass.   Challenges (extra): " + challengePass + " of " + challengeRows + ".");
}
// </quests>

screen.WriteLine();
if (extras > 0) screen.WriteLine(extras + " optional extras are open: a spell, a conversation, and an N, a * and a B on your level. They are not graded.");
screen.WriteLine(passed + " passed, " + failed + " failed, " + todo + " still to do.");
if (failed > 0 && only == "") screen.WriteLine("Read the first FAIL line. It shows the call, the right answer, and what your function gave.");
return failed;
