// ============================================================
//  Levels.cs: the map.
//
//  CONTRACT A: a level is a List<string>. One string per row.
//              Every row is the same length.
//  CONTRACT B: the tile alphabet. Nine tiles, and only nine.
//
//      #  wall          M  a monster            N  someone to talk to
//      .  floor         $  loot                 *  a powerup
//      @  player start  +  door to next level   B  the boss (one at most)
//
//  Indexing is level[y][x]: ROW first, then COLUMN.
// ============================================================


// ---------- the three template levels ----------

List<string> MakeLevel1()
{
    List<string> level = new List<string>();
    level.Add("##########");
    level.Add("#@...#..+#");
    level.Add("#.##.#.#.#");
    level.Add("#.#..M.#.#");
    level.Add("#.#.##.#.#");
    level.Add("#..N#..$.#");
    level.Add("##########");
    return level;
}

List<string> MakeLevel2()
{
    List<string> level = new List<string>();
    level.Add("################");
    level.Add("#@..#.....#...$#");
    level.Add("#.#.#.###.#.##.#");
    level.Add("#.#..*#$..M....#");
    level.Add("#.#####.####.#.#");
    level.Add("#...M...#....#.#");
    level.Add("#.#####.#.####.#");
    level.Add("#N......#....M+#");
    level.Add("################");
    return level;
}

List<string> MakeLevel3()
{
    List<string> level = new List<string>();
    level.Add("####################");
    level.Add("#@.....#$..M.......#");
    level.Add("#.####.#.#########.#");
    level.Add("#.#$N#.#.#..*....#.#");
    level.Add("#.#..M.#.#.#####.#.#");
    level.Add("#.####.#.#.#+B.#.#.#");
    level.Add("#......#.#.##M.#.#.#");
    level.Add("#.######.#...$.#.#.#");
    level.Add("#.#$..M..#######.#.#");
    level.Add("#...#............M.#");
    level.Add("####################");
    return level;
}


// ---------- choosing a level by number ----------

// How many levels the game has. The last one is YOUR level, from MyLevel.cs.
int LevelCount()
{
    return 4;
}

List<string> LoadLevel(int number)
{
    if (number == 1) return MakeLevel1();
    else if (number == 2) return MakeLevel2();
    else if (number == 3) return MakeLevel3();
    else return MyLevel();
}


// ---------- asking questions about a level ----------

// Off the edge of the map counts as a wall. That one decision means
// nothing in this game can ever crash by looking outside the level.
char TileAt(List<string> level, int x, int y)
{
    if (y < 0 || y >= level.Count) return '#';
    if (x < 0 || x >= level[y].Length) return '#';
    return level[y][x];
}

bool IsWall(List<string> level, int x, int y)
{
    return TileAt(level, x, y) == '#';
}

int CountTiles(List<string> level, char tile)
{
    int count = 0;
    for (int y = 0; y < level.Count; y++)
    {
        for (int x = 0; x < level[y].Length; x++)
        {
            if (TileAt(level, x, y) == tile) count = count + 1;
        }
    }
    return count;
}

// Which column is the first copy of this tile in? -1 means "not found".
int FindTileX(List<string> level, char tile)
{
    for (int y = 0; y < level.Count; y++)
    {
        for (int x = 0; x < level[y].Length; x++)
        {
            if (TileAt(level, x, y) == tile) return x;
        }
    }
    return -1;
}

// Which row is the first copy of this tile in? -1 means "not found".
int FindTileY(List<string> level, char tile)
{
    for (int y = 0; y < level.Count; y++)
    {
        for (int x = 0; x < level[y].Length; x++)
        {
            if (TileAt(level, x, y) == tile) return y;
        }
    }
    return -1;
}


// ---------- changing a level ----------

// Strings cannot be edited in place, so we build a new row and put it back.
void SetTile(List<string> level, int x, int y, char tile)
{
    if (y < 0 || y >= level.Count) return;
    if (x < 0 || x >= level[y].Length) return;
    string row = level[y];
    level[y] = row.Substring(0, x) + tile + row.Substring(x + 1);
}


// ---------- drawing ----------

void DrawLevel(List<string> level, int playerX, int playerY)
{
    for (int y = 0; y < level.Count; y++)
    {
        string row = "";
        for (int x = 0; x < level[y].Length; x++)
        {
            if (x == playerX && y == playerY) row = row + "@";
            else if (TileAt(level, x, y) == '@') row = row + ".";
            else row = row + TileAt(level, x, y);
        }
        Console.WriteLine(row);
    }
}

void PrintTile(char tile)
{
    Console.Write("[" + tile + "] ");
}

string Describe(char tile)
{
    if (tile == '#') return "a wall";
    else if (tile == '.') return "open floor";
    else if (tile == '@') return "where the player starts";
    else if (tile == 'M') return "a monster";
    else if (tile == '$') return "loot";
    else if (tile == '+') return "the door to the next level";
    else if (tile == 'N') return "someone to talk to";
    else if (tile == '*') return "a powerup";
    else if (tile == 'B') return "the boss";
    else return "something that is NOT in the tile alphabet";
}

bool IsKnownTile(char tile)
{
    string alphabet = "#.@M$+N*B";
    for (int i = 0; i < alphabet.Length; i++)
    {
        if (alphabet[i] == tile) return true;
    }
    return false;
}

void PrintLegend()
{
    string alphabet = "#.@M$+N*B";
    for (int i = 0; i < alphabet.Length; i++)
    {
        PrintTile(alphabet[i]);
        Console.WriteLine(Describe(alphabet[i]));
    }
}


// ---------- checking a level obeys the contracts ----------

// Returns true when the level is safe to play. Prints what is wrong when it is not.
bool CheckLevel(List<string> level)
{
    bool ok = true;

    if (level.Count == 0)
    {
        Console.WriteLine("LEVEL PROBLEM: the level has no rows.");
        return false;
    }

    for (int y = 0; y < level.Count; y++)
    {
        if (level[y].Length != level[0].Length)
        {
            Console.WriteLine("LEVEL PROBLEM: row " + y + " is " + level[y].Length + " long but row 0 is " + level[0].Length + ".");
            ok = false;
        }
        for (int x = 0; x < level[y].Length; x++)
        {
            if (!IsKnownTile(level[y][x]))
            {
                Console.WriteLine("LEVEL PROBLEM: row " + y + ", column " + x + " is '" + level[y][x] + "', which is " + Describe(level[y][x]) + ".");
                ok = false;
            }
        }
    }

    if (CountTiles(level, '@') != 1)
    {
        Console.WriteLine("LEVEL PROBLEM: there must be exactly one @ but there are " + CountTiles(level, '@') + ".");
        ok = false;
    }
    if (CountTiles(level, '+') < 1)
    {
        Console.WriteLine("LEVEL PROBLEM: there is no + door, so nobody can ever leave.");
        ok = false;
    }
    if (CountTiles(level, 'B') > 1)
    {
        Console.WriteLine("LEVEL PROBLEM: a level can have one B boss but there are " + CountTiles(level, 'B') + ".");
        ok = false;
    }
    return ok;
}
