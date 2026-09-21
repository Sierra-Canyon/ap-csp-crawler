// One long program. No functions yet.

int rookHealth = 10;
int grubHealth = 7;
int dealt = 0;

Console.WriteLine("Round 1");
dealt = 4 - 0;
if (dealt < 1)
{
    dealt = 1;
}
grubHealth = grubHealth - dealt;
if (grubHealth < 0)
{
    grubHealth = 0;
}
Console.WriteLine("Rook hits Grub for " + dealt + ". Grub has " + grubHealth + " left.");

dealt = 2 - 5;
if (dealt < 1)
{
    dealt = 1;
}
rookHealth = rookHealth - dealt;
if (rookHealth < 0)
{
    rookHealth = 0;
}
Console.WriteLine("Grub hits Rook for " + dealt + ". Rook has " + rookHealth + " left.");

Console.WriteLine("Round 2");
dealt = 6 - 0;
if (dealt < 1)
{
    dealt = 1;
}
grubHealth = grubHealth - dealt;
if (grubHealth < 0)
{
    grubHealth = 0;
}
Console.WriteLine("Rook hits Grub for " + dealt + ". Grub has " + grubHealth + " left.");

if (grubHealth > 0)
{
    Console.WriteLine("Grub is still standing.");
}
else
{
    Console.WriteLine("Grub is defeated.");
}
