// ============================================================
//  MyCharacter.cs: THIS FILE IS YOURS.
//
//  The game hands you the three party lists. Add your character to them.
//  Health can be 1 to 10. Attack can be 1 to 5.
// ============================================================

// MILESTONE 1: write the real body of this function.
// A character is fair when   hp + atk * 2   is 18 or less.
bool IsFairCharacter(int hp, int atk)
{
    return true;
}

void MyCharacter(List<string> names, List<int> health, List<int> attack)
{
    AddCharacter(names, health, attack, "Hero", 8, 3);
}
