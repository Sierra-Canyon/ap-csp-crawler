PrintHit("Rook", "Grub", 4, 0);
PrintHit("Mote", "Bat", 3, 1);
PrintHit("Ogre", "Rook", 6, 2);

void PrintHit(string attacker, string target, int power, int defense)
{
    int dealt = power - defense;
    if (dealt < 1)
    {
        dealt = 1;
    }
    Console.WriteLine(attacker + " hits " + target + " for " + dealt + ".");
}
