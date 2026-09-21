// ============================================================
//  Boss.cs: the B tile. One big monster that guards the door.
//
//  A boss lives in the same five monster lists as everything else, so swords,
//  arrows and spells all work on it. What makes it a boss:
//
//    - the + door is sealed until it is defeated
//    - it has three PHASES, decided by how much health it has left
//    - from phase 2 it sends out a shockwave that hurts the WHOLE party
//    - in phase 3 it hits 2 harder
// ============================================================

string BossName(int levelNumber)
{
    if (levelNumber == 3) return "Warden";
    else return MyBossName();
}

int BossHealth(int levelNumber)
{
    return 12 + levelNumber * 2;
}

int BossAttack(int levelNumber)
{
    return 2 + levelNumber;
}

// Which monster is the boss? -1 means this level has no boss.
int FindBoss(List<string> level, List<int> mX, List<int> mY)
{
    int x = FindTileX(level, 'B');
    int y = FindTileY(level, 'B');
    if (x == -1) return -1;
    else return MonsterAt(mX, mY, x, y);
}

// Phase 1 above two thirds health, phase 2 above one third, phase 3 below that.
int BossPhase(int hp, int maxHp)
{
    if (hp * 3 > maxHp * 2) return 1;
    else if (hp * 3 > maxHp) return 2;
    else return 3;
}

// How many turns between shockwaves in this phase? 0 means never.
int ShockwaveEvery(int phase)
{
    if (phase == 1) return 0;
    else if (phase == 2) return 4;
    else return 3;
}

bool IsShockwaveTurn(int phase, int turns)
{
    if (ShockwaveEvery(phase) == 0) return false;
    else return turns % ShockwaveEvery(phase) == 0;
}

// Everyone in the party who is still standing takes the hit. Armor helps, a little.
void Shockwave(string bossName, List<string> names, List<int> health, int amount)
{
    Console.WriteLine(bossName + " slams the ground!");
    for (int i = 0; i < names.Count; i++)
    {
        if (IsAlive(health[i]))
        {
            health[i] = Clamp(health[i] - amount, 0, 99);
            Console.WriteLine("  " + names[i] + " takes " + amount + ".");
        }
    }
}

void PrintBossPhase(string bossName, int phase)
{
    if (phase == 2) Console.WriteLine(bossName + " roars. The floor starts to shake.");
    else if (phase == 3) Console.WriteLine(bossName + " is enraged!");
}

string BossBar(string bossName, int hp, int maxHp)
{
    string bar = "";
    for (int i = 0; i < maxHp; i++)
    {
        if (i < hp) bar = bar + "#";
        else bar = bar + "-";
    }
    return "BOSS " + bossName + " [" + bar + "] phase " + BossPhase(hp, maxHp);
}

// Is the door shut? It is while this level's boss is alive.
bool IsDoorSealed(List<int> mHealth, int boss)
{
    if (boss == -1) return false;
    else return IsAlive(mHealth[boss]);
}
