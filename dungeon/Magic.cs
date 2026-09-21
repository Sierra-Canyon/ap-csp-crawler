// ============================================================
//  Magic.cs: mana and spells.
//
//  The spell book is two parallel lists: spellNames[i] costs spellCosts[i] mana.
//
//      cast spark d    a bolt in a straight line. 4 damage. Armor does not stop it.
//      cast mend       the leader gets 5 health back.
//      cast quake      every monster within 2 squares takes 3 damage.
//      cast zap d      YOUR spell, from MySpell.cs.
//
//  Mana comes back by itself: 1 point every 4 turns, and all of it on a new level.
// ============================================================

void AddSpell(List<string> spellNames, List<int> spellCosts, string name, int cost)
{
    spellNames.Add(name);
    spellCosts.Add(Clamp(cost, 1, 6));
}

void MakeSpellBook(List<string> spellNames, List<int> spellCosts)
{
    AddSpell(spellNames, spellCosts, "spark", 2);
    AddSpell(spellNames, spellCosts, "mend", 3);
    AddSpell(spellNames, spellCosts, "quake", 4);
}

int FindSpell(List<string> spellNames, string name)
{
    for (int i = 0; i < spellNames.Count; i++)
    {
        if (spellNames[i] == name) return i;
    }
    return -1;
}

bool CanCast(int mana, int cost)
{
    return mana >= cost;
}

// Does this spell need a direction? mend and quake do not. Every other spell does.
bool IsAimedSpell(string name)
{
    if (name == "mend" || name == "quake") return false;
    else return true;
}

// How much does an aimed spell hurt a monster that is "distance" squares away?
int SpellDamage(string name, int casterAttack, int distance)
{
    if (name == "spark") return 4;
    else return Clamp(MySpellDamage(casterAttack, distance), 0, 9);
}

// An int cannot be changed from inside a function, so this RETURNS the new mana
// and Program.cs has to keep it:   mana = ManaAfterTurn(mana, maxMana, turns);
int ManaAfterTurn(int mana, int maxMana, int turns)
{
    if (turns % 4 == 0) return Clamp(mana + 1, 0, maxMana);
    else return mana;
}

string ManaBar(int mana, int maxMana)
{
    string bar = "";
    for (int i = 0; i < maxMana; i++)
    {
        if (i < mana) bar = bar + "*";
        else bar = bar + ".";
    }
    return "Mana     [" + bar + "] " + mana + "/" + maxMana;
}

void PrintSpells(List<string> spellNames, List<int> spellCosts)
{
    for (int i = 0; i < spellNames.Count; i++)
    {
        Console.WriteLine("  cast " + spellNames[i] + "   (" + spellCosts[i] + " mana)");
    }
}

// Every monster within "reach" squares of (x, y) takes "amount" damage. Returns how many were hit.
int HurtNearby(List<string> mNames, List<int> mHealth, List<int> mX, List<int> mY, int x, int y, int reach, int amount)
{
    int hit = 0;
    for (int i = 0; i < mNames.Count; i++)
    {
        int across = mX[i] - x;
        int down = mY[i] - y;
        if (across < 0) across = 0 - across;
        if (down < 0) down = 0 - down;
        if (IsAlive(mHealth[i]) && across <= reach && down <= reach)
        {
            mHealth[i] = Clamp(mHealth[i] - amount, 0, 99);
            Console.WriteLine("The quake hits " + mNames[i] + " for " + amount + ".");
            hit = hit + 1;
        }
    }
    return hit;
}

// Casts one spell. Returns the mana it cost: 0 means the spell was not cast and no turn passes.
int CastSpell(string name, List<string> spellNames, List<int> spellCosts, int mana, List<string> level, List<string> names, List<int> health, List<int> attack, int hero, int maxHp, List<string> mNames, List<int> mHealth, List<int> mX, List<int> mY, int x, int y, int stepX, int stepY)
{
    int spell = FindSpell(spellNames, name);
    if (spell == -1)
    {
        Console.WriteLine("You do not know a spell called " + name + ". Type spells.");
        return 0;
    }
    if (!CanCast(mana, spellCosts[spell]))
    {
        Console.WriteLine("Not enough mana. " + name + " costs " + spellCosts[spell] + " and you have " + mana + ".");
        return 0;
    }

    if (IsAimedSpell(name) && stepX == 0 && stepY == 0)
    {
        Console.WriteLine("Which way? Type  cast " + name + " d");
        return 0;
    }

    if (name == "mend")
    {
        health[hero] = Clamp(health[hero] + 5, 0, maxHp);
        Console.WriteLine(names[hero] + " is mended.");
    }
    else if (name == "quake")
    {
        int hit = HurtNearby(mNames, mHealth, mX, mY, x, y, 2, 3);
        if (hit == 0) Console.WriteLine("The ground shakes. Nothing is close enough.");
    }
    else
    {
        int m = TargetInLine(level, mX, mY, x, y, stepX, stepY, 4);
        if (m == -1)
        {
            Console.WriteLine("The " + name + " fizzles against the wall.");
        }
        else
        {
            int dealt = SpellDamage(name, attack[hero], DistanceTo(mX, mY, m, x, y));
            mHealth[m] = Clamp(mHealth[m] - dealt, 0, 99);
            Console.WriteLine(names[hero] + "'s " + name + " burns " + mNames[m] + " for " + dealt + ".");
        }
    }
    return spellCosts[spell];
}
