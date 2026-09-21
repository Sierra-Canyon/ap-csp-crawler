// ============================================================
//  Ranged.cs: arrows and stones. Hitting a monster from a distance.
//
//  ONE function, Shoot, does both. What is different about an arrow and
//  a stone arrives as ARGUMENTS: the ammo's name, its power and its range.
//
//      fire d     shoots an arrow to the right    (power 3, reaches 6 squares)
//      throw w    throws a stone upwards          (power 1, reaches 3 squares)
//
//  A monster that is shot from a distance does not get to hit back.
// ============================================================

int ShotPower(string ammo)
{
    if (ammo == "arrow") return 3;
    else return 1;
}

int ShotRange(string ammo)
{
    if (ammo == "arrow") return 6;
    else return 3;
}

// Looks along a straight line from (x, y). Returns the index of the first living
// monster it meets, or -1 if the shot hits a wall or runs out of range first.
int TargetInLine(List<string> level, List<int> mX, List<int> mY, int x, int y, int stepX, int stepY, int range)
{
    for (int i = 1; i <= range; i++)
    {
        int lookX = x + stepX * i;
        int lookY = y + stepY * i;
        char tile = TileAt(level, lookX, lookY);
        if (tile == '#' || tile == 'N' || tile == '+') return -1;
        if (tile == 'M' || tile == 'B') return MonsterAt(mX, mY, lookX, lookY);
    }
    return -1;
}

// How far away is that monster, in squares along the line?
int DistanceTo(List<int> mX, List<int> mY, int m, int x, int y)
{
    int across = mX[m] - x;
    int down = mY[m] - y;
    if (across < 0) across = 0 - across;
    if (down < 0) down = 0 - down;
    return across + down;
}

// Uses up one piece of ammo and shoots it. Returns true if the shot was taken
// (so a turn passes), false if there was nothing to shoot with or no direction.
bool Shoot(List<string> inventory, string ammo, List<string> level, List<string> names, int hero, List<string> mNames, List<int> mHealth, List<int> mX, List<int> mY, int x, int y, int stepX, int stepY, int bonus, int monsterDefense)
{
    if (stepX == 0 && stepY == 0)
    {
        Console.WriteLine("Which way? Add w, a, s or d.");
        return false;
    }
    if (!RemoveItem(inventory, ammo))
    {
        Console.WriteLine("You have no " + ammo + "s.");
        return false;
    }

    int m = TargetInLine(level, mX, mY, x, y, stepX, stepY, ShotRange(ammo));
    if (m == -1)
    {
        Console.WriteLine("The " + ammo + " hits nothing.");
    }
    else
    {
        int dealt = Damage(ShotPower(ammo) + bonus + Roll(0, 1), monsterDefense);
        mHealth[m] = Clamp(mHealth[m] - dealt, 0, 99);
        Console.WriteLine(names[hero] + "'s " + ammo + " hits " + mNames[m] + " for " + dealt + ".");
    }
    return true;
}
