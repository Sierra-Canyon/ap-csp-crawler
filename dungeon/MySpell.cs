// ============================================================
//  MySpell.cs: THIS FILE IS YOURS.
//
//  Your spell is a bolt in a straight line, like spark. You choose its name,
//  what it costs, and how hard it hits. Cast it with:   cast zap d
//
//  The spell book holds four spells: the game's three and yours. The graphical game
//  casts them with the keys 1 to 4.
//
//  A spell is fair when it never does more than   cost * 3   damage.
//  A cost can be 1 to 6.
// ============================================================

void MySpell(List<string> spellNames, List<int> spellCosts)
{
    AddSpell(spellNames, spellCosts, "zap", 2);
}

// How much damage your spell does. The game hands you the caster's attack
// and how many squares away the monster is. Use them, or do not.
int MySpellDamage(int casterAttack, int distance)
{
    return 3;
}
