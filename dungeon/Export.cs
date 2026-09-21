// ============================================================
//  Export.cs: saves the whole game as text, for the web player.
//
//  Type  export  while playing. It writes mygame.txt next to this file.
//  Drag mygame.txt onto dungeon-web/player.html to play it with graphics.
// ============================================================

void ExportItem(List<string> lines, List<string> inventory, string item)
{
    lines.Add("START " + CountItem(inventory, item) + " " + item);
}

string SpellKind(string name)
{
    if (name == "spark" || name == "mend" || name == "quake") return name;
    else return "bolt";
}

// The export is always a NEW game: a fresh party, the starting inventory and your spell book.
// It does not matter how hurt you are or what you are carrying when you type  export.
//
// The web player cannot run your C#. It knows your levels, characters, conversations and your
// spell's damage (it asks MySpellDamage for every attack and distance). The rules inside the
// template's own functions, such as Damage and Heal, are built into the web player as they were
// written, so a rewrite of those shows up in the text game and the graphical game only.
void ExportGame(int maxHp, int maxMana)
{
    List<string> names = new List<string>();
    List<int> health = new List<int>();
    List<int> attack = new List<int>();
    MakeStartingParty(names, health, attack);
    MyCharacter(names, health, attack);
    List<string> inventory = new List<string>();
    MakeStartingInventory(inventory);
    List<string> spellNames = new List<string>();
    List<int> spellCosts = new List<int>();
    MakeSpellBook(spellNames, spellCosts);
    MySpell(spellNames, spellCosts);

    List<string> lines = new List<string>();
    lines.Add("DUNGEON-EXPORT 2");
    lines.Add("MAXHP " + maxHp);
    lines.Add("MAXMANA " + maxMana);

    for (int i = 0; i < names.Count; i++)
    {
        lines.Add("CHARACTER " + health[i] + " " + attack[i] + " " + names[i]);
    }

    ExportItem(lines, inventory, "potion");
    ExportItem(lines, inventory, "shield");
    ExportItem(lines, inventory, "gold");
    ExportItem(lines, inventory, "arrow");
    ExportItem(lines, inventory, "stone");
    lines.Add("SHOT arrow " + ShotPower("arrow") + " " + ShotRange("arrow"));
    lines.Add("SHOT stone " + ShotPower("stone") + " " + ShotRange("stone"));

    for (int i = 0; i < spellNames.Count; i++)
    {
        lines.Add("SPELL " + spellCosts[i] + " " + SpellKind(spellNames[i]) + " " + spellNames[i]);
    }
    // Your spell's damage, for every attack from 1 to 5 and every distance from 1 to 4.
    for (int atk = 1; atk <= 5; atk++)
    {
        for (int distance = 1; distance <= 4; distance++)
        {
            lines.Add("BOLT " + atk + " " + distance + " " + SpellDamage("bolt", atk, distance));
        }
    }
    for (int phase = 1; phase <= 3; phase++)
    {
        lines.Add("BOSSPHASE " + phase + " " + ShockwaveEvery(phase));
    }

    List<string> mNames = new List<string>();
    List<int> mHealth = new List<int>();
    List<int> mAttack = new List<int>();
    List<int> mX = new List<int>();
    List<int> mY = new List<int>();
    List<string> says = new List<string>();
    List<string> choiceA = new List<string>();
    List<int> nextA = new List<int>();
    List<string> choiceB = new List<string>();
    List<int> nextB = new List<int>();
    List<string> gives = new List<string>();

    for (int number = 1; number <= LevelCount(); number++)
    {
        List<string> level = LoadLevel(number);
        lines.Add("LEVEL " + number + " " + MonsterDefense(number));
        for (int y = 0; y < level.Count; y++)
        {
            lines.Add(level[y]);
        }
        lines.Add("END");

        SpawnMonsters(level, number, mNames, mHealth, mAttack, mX, mY);
        int boss = FindBoss(level, mX, mY);
        for (int i = 0; i < mNames.Count; i++)
        {
            if (i == boss) lines.Add("BOSS " + number + " " + mHealth[i] + " " + mAttack[i] + " " + mX[i] + " " + mY[i] + " " + mNames[i]);
            else lines.Add("MONSTER " + number + " " + mHealth[i] + " " + mAttack[i] + " " + mX[i] + " " + mY[i] + " " + mNames[i]);
        }
        for (int y = 0; y < level.Count; y++)
        {
            for (int x = 0; x < level[y].Length; x++)
            {
                if (TileAt(level, x, y) == '*') lines.Add("POWERUP " + number + " " + x + " " + y + " " + PowerupName((x + y) % 3));
            }
        }

        LoadDialogue(number, says, choiceA, nextA, choiceB, nextB, gives);
        if (!CheckLevel(level)) Console.WriteLine("EXPORT WARNING: level " + number + " breaks a contract. The web player will refuse it.");
        if (CountTiles(level, 'N') > 0 && !CheckDialogue(number, says, choiceA, nextA, choiceB, nextB, gives)) Console.WriteLine("EXPORT WARNING: the conversation on level " + number + " breaks a contract.");
        lines.Add("NPC " + number + " " + NpcName(number));
        lines.Add("QUEST " + number + " " + QuestNeed(number));
        lines.Add("STARTS " + number + " " + DialogueStart(number, 0) + " " + DialogueStart(number, 1) + " " + DialogueStart(number, 2) + " " + DialogueStart(number, 3));
        for (int i = 0; i < says.Count; i++)
        {
            lines.Add("SAY " + number + " " + nextA[i] + " " + nextB[i]);
            lines.Add("T " + says[i]);
            lines.Add("A " + choiceA[i]);
            lines.Add("B " + choiceB[i]);
            lines.Add("G " + gives[i]);
        }
    }

    File.WriteAllLines("mygame.txt", lines);
    Console.WriteLine("Saved mygame.txt (" + lines.Count + " lines).");
}
