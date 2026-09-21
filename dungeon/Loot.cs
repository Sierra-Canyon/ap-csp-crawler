// ============================================================
//  Loot.cs: the inventory. One List<string>, one item per entry.
// ============================================================

// What the party carries when a new game starts.
void MakeStartingInventory(List<string> inventory)
{
    inventory.Add("potion");
    inventory.Add("potion");
    AddMany(inventory, "arrow", 3);
    AddMany(inventory, "stone", 2);
}

string RandomLoot()
{
    int roll = Roll(1, 10);
    if (roll <= 3) return "potion";
    else if (roll <= 5) return "gold";
    else if (roll == 6) return "shield";
    else if (roll <= 8) return "arrow";
    else return "stone";
}

void AddLoot(List<string> inventory, string item)
{
    inventory.Add(item);
    Console.WriteLine("You found: " + item + "!");
}

int CountItem(List<string> inventory, string item)
{
    int count = 0;
    for (int i = 0; i < inventory.Count; i++)
    {
        if (inventory[i] == item) count = count + 1;
    }
    return count;
}

// Removes ONE copy of the item. Returns false if there was none to remove.
bool RemoveItem(List<string> inventory, string item)
{
    for (int i = 0; i < inventory.Count; i++)
    {
        if (inventory[i] == item)
        {
            inventory.RemoveAt(i);
            return true;
        }
    }
    return false;
}

// The leader drinks one potion, if there is one. Lists can be changed from inside
// a function, so health[leader] really does go up, as long as we KEEP what Heal returns.
void DrinkPotion(List<string> inventory, List<string> names, List<int> health, int leader, int maxHp)
{
    if (RemoveItem(inventory, "potion"))
    {
        health[leader] = Clamp(Heal(health[leader]), 0, maxHp);
        Console.WriteLine(names[leader] + " drinks a potion.");
    }
    else
    {
        Console.WriteLine("You have no potions.");
    }
}

void PrintInventory(List<string> inventory)
{
    Console.WriteLine("Potions: " + CountItem(inventory, "potion")
        + "   Shields: " + CountItem(inventory, "shield")
        + "   Gold: " + CountItem(inventory, "gold")
        + "   Arrows: " + CountItem(inventory, "arrow")
        + "   Stones: " + CountItem(inventory, "stone"));
}
