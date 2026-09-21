// Three functions, no parameters. Each one carries its own health value.

PotionCheckRook();
PotionCheckMote();
PotionCheckBlaze();

void PotionCheckRook()
{
    int hp = 10;
    if (hp <= 0)
    {
        Console.WriteLine("Health " + hp + ": too late for a potion.");
    }
    else if (hp <= 4)
    {
        Console.WriteLine("Health " + hp + ": drink a potion.");
    }
    else
    {
        Console.WriteLine("Health " + hp + ": keep walking.");
    }
}

void PotionCheckMote()
{
    int hp = 5;
    if (hp <= 0)
    {
        Console.WriteLine("Health " + hp + ": too late for a potion.");
    }
    else if (hp <= 4)
    {
        Console.WriteLine("Health " + hp + ": drink a potion.");
    }
    else
    {
        Console.WriteLine("Health " + hp + ": keep walking.");
    }
}

void PotionCheckBlaze()
{
    int hp = 0;
    if (hp <= 0)
    {
        Console.WriteLine("Health " + hp + ": too late for a potion.");
    }
    else if (hp <= 4)
    {
        Console.WriteLine("Health " + hp + ": drink a potion.");
    }
    else
    {
        Console.WriteLine("Health " + hp + ": keep walking.");
    }
}
