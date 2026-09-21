// ============================================================
//  Powerups.cs: short boosts, picked up from a * tile.
//
//  Two parallel lists. powerNames[i] is a boost that is switched on,
//  and powerTurns[i] is how many turns it has left.
//
//      might   the leader hits 2 harder, with a sword or an arrow
//      guard   every hit on the party does 2 less
//      regen   the leader gets 1 health back every turn
// ============================================================

// Which boost a * gives depends on where it is: kind = (x + y) % 3. Move the * one square and it changes.
string PowerupName(int kind)
{
    if (kind == 0) return "might";
    else if (kind == 1) return "guard";
    else return "regen";
}

// Where is this boost in the list? -1 means it is not switched on.
int FindPowerup(List<string> powerNames, string name)
{
    for (int i = 0; i < powerNames.Count; i++)
    {
        if (powerNames[i] == name) return i;
    }
    return -1;
}

// Picking up a boost you already have does not add it twice. It tops the turns back up.
void AddPowerup(List<string> powerNames, List<int> powerTurns, string name, int turns)
{
    int found = FindPowerup(powerNames, name);
    if (found == -1)
    {
        powerNames.Add(name);
        powerTurns.Add(turns);
    }
    else
    {
        powerTurns[found] = turns;
    }
    Console.WriteLine("Power up: " + name + " for " + turns + " turns!");
}

// How much extra does this boost give right now? "amount" when it is on, 0 when it is not.
int PowerupBonus(List<string> powerNames, string name, int amount)
{
    if (FindPowerup(powerNames, name) == -1) return 0;
    else return amount;
}

// One turn passes. Every boost loses a turn, and a boost with none left is removed.
// The loop runs BACKWARDS so that RemoveAt never makes it skip the next one.
void TickPowerups(List<string> powerNames, List<int> powerTurns)
{
    for (int i = powerNames.Count - 1; i >= 0; i--)
    {
        powerTurns[i] = powerTurns[i] - 1;
        if (powerTurns[i] <= 0)
        {
            Console.WriteLine("The " + powerNames[i] + " wears off.");
            powerNames.RemoveAt(i);
            powerTurns.RemoveAt(i);
        }
    }
}

void PrintPowerups(List<string> powerNames, List<int> powerTurns)
{
    for (int i = 0; i < powerNames.Count; i++)
    {
        Console.WriteLine("  * " + powerNames[i] + " (" + powerTurns[i] + " turns left)");
    }
}
