// This program has a bug. Your sheet shows what it should print.

string attacker = "Rook";
string target = "Grub";
int power = 4;
int defense = 0;
PrintHit();

attacker = "Mote";
target = "Bat";
power = 3;
defense = 1;
PrintHit();

attacker = "Rook";
power = 4;
PrintHit();

attacker = "Ogre";
target = "Mote";
power = 6;
defense = 2;
PrintHit();

void PrintHit()
{
    int dealt = power - defense;
    if (dealt < 1)
    {
        dealt = 1;
    }
    Console.WriteLine(attacker + " hits " + target + " for " + dealt + ".");
}
