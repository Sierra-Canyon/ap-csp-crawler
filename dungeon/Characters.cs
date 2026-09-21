// ============================================================
//  Characters.cs: the party.
//
//  CONTRACT C: a character is NOT one thing. A character is an
//  INDEX shared across several lists that stay the same length.
//
//      names[2]   health[2]   attack[2]     <- one character
//
//  So the only way to work with the party is a for loop over i.
// ============================================================


// ---------- one character ----------

bool IsAlive(int hp)
{
    return hp > 0;
}

// Returns a CHANGED COPY. It cannot change the hp you passed in.
// The caller has to keep the answer:   hp = Heal(hp);
int Heal(int hp)
{
    return hp + 3;
}

string HealthBar(string name, int hp, int maxHp)
{
    string bar = "";
    for (int i = 0; i < maxHp; i++)
    {
        if (i < hp) bar = bar + "#";
        else bar = bar + "-";
    }
    return $"{name,-8} [{bar}] {hp}/{maxHp}";
}


// ---------- building the party ----------

// Six parameters. Read the signature slowly: three lists to add TO,
// then the three values to add. One Add per list keeps them lined up.
void AddCharacter(List<string> names, List<int> health, List<int> attack, string name, int hp, int atk)
{
    names.Add(name);
    health.Add(Clamp(hp, 1, 10));
    attack.Add(Clamp(atk, 1, 5));
}

void MakeStartingParty(List<string> names, List<int> health, List<int> attack)
{
    AddCharacter(names, health, attack, "Rook", 10, 4);
    AddCharacter(names, health, attack, "Mote", 7, 3);
}


// ---------- questions about the whole party ----------

int PartySize(List<string> names)
{
    return names.Count;
}

// Returns the index of the character with this name, or -1 for "nobody".
int FindCharacter(List<string> names, string name)
{
    for (int i = 0; i < names.Count; i++)
    {
        if (names[i] == name) return i;
    }
    return -1;
}

int TotalHealth(List<int> health)
{
    int total = 0;
    for (int i = 0; i < health.Count; i++)
    {
        total = total + health[i];
    }
    return total;
}

// The biggest-so-far pattern. Returns an INDEX, not the attack value.
int StrongestIndex(List<int> attack)
{
    int best = 0;
    for (int i = 1; i < attack.Count; i++)
    {
        if (attack[i] > attack[best]) best = i;
    }
    return best;
}

// Early exit: the moment we find one survivor we already know the answer.
bool AnyAlive(List<int> health)
{
    for (int i = 0; i < health.Count; i++)
    {
        if (IsAlive(health[i])) return true;
    }
    return false;
}

// Index of the first character still standing, or -1 if there is nobody.
int NextAliveIndex(List<int> health)
{
    for (int i = 0; i < health.Count; i++)
    {
        if (IsAlive(health[i])) return i;
    }
    return -1;
}

// Who should lead now? Returns the index of the named character if they can lead,
// otherwise returns the current leader unchanged. The caller keeps the answer:
//     leader = ChooseLeader(names, health, leader, "Mote");
int ChooseLeader(List<string> names, List<int> health, int leader, string name)
{
    int chosen = FindCharacter(names, name);
    if (chosen == -1)
    {
        Console.WriteLine("Nobody in the party has that name.");
        return leader;
    }
    else if (!IsAlive(health[chosen]))
    {
        Console.WriteLine(names[chosen] + " cannot lead any more.");
        return leader;
    }
    else
    {
        Console.WriteLine(names[chosen] + " steps to the front.");
        return chosen;
    }
}


// ---------- printing ----------

void PrintParty(List<string> names, List<int> health, int maxHp)
{
    for (int i = 0; i < names.Count; i++)
    {
        Console.WriteLine(HealthBar(names[i], health[i], maxHp));
    }
}
