// Who hits hardest? No list: three attack values, three variables.

int attack1 = 4;
int attack2 = 3;
int attack3 = 5;

Console.WriteLine("Strongest is hero number " + StrongestOfThree(attack1, attack2, attack3));

int StrongestOfThree(int a1, int a2, int a3)
{
    if (a1 >= a2 && a1 >= a3)
    {
        return 1;
    }
    else if (a2 >= a3)
    {
        return 2;
    }
    else
    {
        return 3;
    }
}
