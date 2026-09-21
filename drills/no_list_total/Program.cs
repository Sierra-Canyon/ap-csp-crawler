// Two ways to total the party's health.

// Version A: no list.
int health1 = 10;
int health2 = 7;
int health3 = 5;
Console.WriteLine("A total: " + TotalOfThree(health1, health2, health3));

// Version B: a list, and a loop that stops at 3.
List<int> health = new List<int>();
health.Add(10);
health.Add(7);
health.Add(5);
Console.WriteLine("B total: " + TotalFirstThree(health));

int TotalOfThree(int h1, int h2, int h3)
{
    return h1 + h2 + h3;
}

int TotalFirstThree(List<int> health)
{
    int total = 0;
    for (int i = 0; i < 3; i++)
    {
        total = total + health[i];
    }
    return total;
}
