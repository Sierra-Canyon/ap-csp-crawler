// ============================================================
//  Combat.cs: damage, attacks, death.
//
//  Monsters obey CONTRACT C too. They are five parallel lists:
//      mNames[i]  mHealth[i]  mAttack[i]  mX[i]  mY[i]
//  The last two say which tile that monster is standing on.
// ============================================================


// ---------- small number tools ----------

int Roll(int low, int high)
{
    return Random.Shared.Next(low, high + 1);
}

int Clamp(int value, int low, int high)
{
    if (value < low) return low;
    else if (value > high) return high;
    else return value;
}

// ORDER MATTERS. Damage(4, 1) is 3.  Damage(1, 4) is 1.
// Both compile. Only one is what you meant.
int Damage(int attackPower, int defense)
{
    int dealt = attackPower - defense;
    if (dealt < 1) dealt = 1;
    return dealt;
}

string WinnerName(string a, int aHp, string b, int bHp)
{
    if (aHp > bHp) return a;
    else if (bHp > aHp) return b;
    else return "nobody";
}


// ---------- what kind of monster? ----------

string MonsterName(int kind)
{
    if (kind == 0) return "Grub";
    else if (kind == 1) return "Bat";
    else return "Ogre";
}

int MonsterHealth(int kind, int levelNumber)
{
    if (kind == 0) return 3 + levelNumber;
    else if (kind == 1) return 2 + levelNumber;
    else return 5 + levelNumber;
}

int MonsterAttack(int kind, int levelNumber)
{
    if (kind == 0) return 1 + levelNumber;
    else if (kind == 1) return 2 + levelNumber;
    else return 3 + levelNumber;
}

// Monsters on the deeper levels have thicker hides.
int MonsterDefense(int levelNumber)
{
    if (levelNumber >= 3) return 1;
    else return 0;
}


// ---------- putting monsters on the map ----------

// Walks the whole grid. Every M it finds becomes one monster in the lists.
void SpawnMonsters(List<string> level, int levelNumber, List<string> mNames, List<int> mHealth, List<int> mAttack, List<int> mX, List<int> mY)
{
    mNames.Clear();
    mHealth.Clear();
    mAttack.Clear();
    mX.Clear();
    mY.Clear();

    for (int y = 0; y < level.Count; y++)
    {
        for (int x = 0; x < level[y].Length; x++)
        {
            if (TileAt(level, x, y) == 'M')
            {
                int kind = mNames.Count % 3;
                mNames.Add(MonsterName(kind));
                mHealth.Add(MonsterHealth(kind, levelNumber));
                mAttack.Add(MonsterAttack(kind, levelNumber));
                mX.Add(x);
                mY.Add(y);
            }
            else if (TileAt(level, x, y) == 'B')
            {
                mNames.Add(BossName(levelNumber));
                mHealth.Add(BossHealth(levelNumber));
                mAttack.Add(BossAttack(levelNumber));
                mX.Add(x);
                mY.Add(y);
            }
        }
    }
}

// Which monster is standing on (x, y)? Returns its index, or -1 for "none".
int MonsterAt(List<int> mX, List<int> mY, int x, int y)
{
    for (int i = 0; i < mX.Count; i++)
    {
        if (mX[i] == x && mY[i] == y) return i;
    }
    return -1;
}


// ---------- one exchange of blows ----------

// Lists CAN be changed by a function. Compare that with Heal(int hp).
void HeroAttacks(List<string> names, List<int> attack, int hero, List<string> mNames, List<int> mHealth, int m, int monsterDefense, int bonus)
{
    int power = attack[hero] + bonus + Roll(0, 2);
    int dealt = Damage(power, monsterDefense);
    mHealth[m] = Clamp(mHealth[m] - dealt, 0, 99);
    Console.WriteLine(names[hero] + " hits " + mNames[m] + " for " + dealt + ".");
}

void MonsterAttacks(List<string> mNames, List<int> mAttack, int m, List<string> names, List<int> health, int hero, int partyDefense)
{
    int power = mAttack[m] + Roll(0, 1);
    int dealt = Damage(power, partyDefense);
    health[hero] = Clamp(health[hero] - dealt, 0, 99);
    Console.WriteLine(mNames[m] + " hits " + names[hero] + " for " + dealt + ".");
}


// Monsters can now fall to a sword, an arrow or a spell, so tidying up happens in ONE place.
// Any monster with no health left that is still on the map is taken off it, and drops 1 gold.
void ClearDefeated(List<string> level, List<string> mNames, List<int> mHealth, List<int> mX, List<int> mY, List<string> inventory)
{
    for (int i = 0; i < mNames.Count; i++)
    {
        char tile = TileAt(level, mX[i], mY[i]);
        if (!IsAlive(mHealth[i]) && (tile == 'M' || tile == 'B'))
        {
            Console.WriteLine(mNames[i] + " is defeated!");
            SetTile(level, mX[i], mY[i], '.');
            AddLoot(inventory, "gold");
        }
    }
}
