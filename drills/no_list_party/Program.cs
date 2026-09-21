// A party of two with no lists. Every value is its own variable.

string name1 = "Rook";
int health1 = 10;
int attack1 = 4;

string name2 = "Mote";
int health2 = 7;
int attack2 = 3;

PrintParty(name1, health1, attack1, name2, health2, attack2);

void PrintParty(string n1, int h1, int a1, string n2, int h2, int a2)
{
    Console.WriteLine("Party size: 2");
    Console.WriteLine(n1 + ": health " + h1 + ", attack " + a1);
    Console.WriteLine(n2 + ": health " + h2 + ", attack " + a2);
}
