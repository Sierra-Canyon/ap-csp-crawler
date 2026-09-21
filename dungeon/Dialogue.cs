// ============================================================
//  Dialogue.cs: talking to the N tile, and quests.
//
//  A conversation is SIX parallel lists. Index i is one thing the character says:
//
//      says[i]      the words
//      choiceA[i]   the player's first answer      nextA[i]   which index it leads to
//      choiceB[i]   the player's second answer     nextB[i]   which index it leads to
//      gives[i]     what happens when this line is reached ("" for nothing)
//
//  A next of -1 ends the conversation.
//  If choiceA[i] is "" there is nothing to choose: press Enter and it moves on to nextA[i].
//  A line with nothing to choose must lead FORWARDS (to a bigger index) or to -1.
//
//  The game remembers things in one more list, flags. A quest is two flags:
//  "started 2" and "done 2" are the quest on level 2.
// ============================================================

void AddLine(List<string> says, List<string> choiceA, List<int> nextA, List<string> choiceB, List<int> nextB, List<string> gives, string text, string answerA, int leadsA, string answerB, int leadsB, string action)
{
    says.Add(text);
    choiceA.Add(answerA);
    nextA.Add(leadsA);
    choiceB.Add(answerB);
    nextB.Add(leadsB);
    gives.Add(action);
}


// ---------- the three template conversations ----------

void GuideDialogue(List<string> says, List<string> choiceA, List<int> nextA, List<string> choiceB, List<int> nextB, List<string> gives)
{
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "You are new down here. Want some advice?", "Yes, please.", 1, "I will manage.", 5, "");
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "Do not let them touch you. An arrow flies 6 squares, and a monster you shoot cannot hit back.", "What about magic?", 2, "Thanks.", 3, "");
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "A spark costs 2 mana and goes straight through armor. Mana comes back slowly. The controls list has the keys.", "", 3, "", -1, "");
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "Take these. You will need them.", "", 4, "", -1, "give arrows");
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "Off you go.", "", -1, "", -1, "finish quest");
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "Suit yourself. The offer stands.", "", -1, "", -1, "");
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "Still alive? Good. Keep moving.", "", -1, "", -1, "");
}

void SageDialogue(List<string> says, List<string> choiceA, List<int> nextA, List<string> choiceB, List<int> nextB, List<string> gives)
{
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "I am building a shield, and I am 3 gold short.", "I will find it.", 1, "Not my problem.", -1, "");
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "Monsters carry gold. Come back with 3.", "", -1, "", -1, "start quest");
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "You do not have 3 gold yet.", "", -1, "", -1, "");
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "You have the gold! Will you trade it?", "Here you go.", 4, "I will keep it.", -1, "");
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "Done. Give me a moment.", "", 5, "", -1, "finish quest");
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "There. It will take a hit for you.", "", -1, "", -1, "give shield");
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "The shield is holding up, I hope?", "", -1, "", -1, "");
}

void GhostDialogue(List<string> says, List<string> choiceA, List<int> nextA, List<string> choiceB, List<int> nextB, List<string> gives)
{
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "The Warden locked me in here. While it lives, the door stays shut.", "I will deal with it.", 1, "How do I beat it?", 2, "");
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "Then go. Come back when it is over.", "", -1, "", -1, "start quest");
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "Hurt it from far away while you can. When it roars, the floor will hurt all of you.", "I will deal with it.", 1, "Maybe later.", -1, "");
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "It still lives. I can hear it.", "", -1, "", -1, "");
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "It is gone. I can feel it. Let me help you, before I go.", "", 5, "", -1, "finish quest");
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "Be well.", "", 6, "", -1, "heal party");
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "And be ready.", "", -1, "", -1, "restore mana");
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "...", "", -1, "", -1, "");
}


// ---------- choosing a conversation by level number ----------

void LoadDialogue(int levelNumber, List<string> says, List<string> choiceA, List<int> nextA, List<string> choiceB, List<int> nextB, List<string> gives)
{
    says.Clear();
    choiceA.Clear();
    nextA.Clear();
    choiceB.Clear();
    nextB.Clear();
    gives.Clear();

    if (levelNumber == 1) GuideDialogue(says, choiceA, nextA, choiceB, nextB, gives);
    else if (levelNumber == 2) SageDialogue(says, choiceA, nextA, choiceB, nextB, gives);
    else if (levelNumber == 3) GhostDialogue(says, choiceA, nextA, choiceB, nextB, gives);
    else MyDialogue(says, choiceA, nextA, choiceB, nextB, gives);
}

string NpcName(int levelNumber)
{
    if (levelNumber == 1) return "Guide";
    else if (levelNumber == 2) return "Sage";
    else if (levelNumber == 3) return "Ghost";
    else return MyNpcName();
}

// What does this level's quest need before it can be finished?
//   ""          there is no quest          "gold"       3 gold in the inventory
//   "boss"      this level's boss beaten   "monsters"   every monster on the level beaten
string QuestNeed(int levelNumber)
{
    if (levelNumber == 1) return "";
    else if (levelNumber == 2) return "gold";
    else if (levelNumber == 3) return "boss";
    else return MyQuestNeed();
}

// The same thing in words, for the screen.
string QuestNeedWords(string need)
{
    if (need == "gold") return "bring 3 gold";
    else if (need == "boss") return "beat the boss";
    else if (need == "monsters") return "beat every monster on this level";
    else return "nothing";
}

// Which line does the conversation open on? It depends on how far the quest has got:
//   0 not started   1 started, not ready to finish   2 ready to finish   3 done
int DialogueStart(int levelNumber, int questState)
{
    if (levelNumber == 1)
    {
        if (questState == 3) return 6;
        else return 0;
    }
    else if (levelNumber == 2)
    {
        if (questState == 0) return 0;
        else if (questState == 1) return 2;
        else if (questState == 2) return 3;
        else return 6;
    }
    else if (levelNumber == 3)
    {
        if (questState == 0) return 0;
        else if (questState == 1) return 3;
        else if (questState == 2) return 4;
        else return 7;
    }
    else
    {
        return MyDialogueStart(questState);
    }
}


// ---------- flags: what the game remembers ----------

bool HasFlag(List<string> flags, string flag)
{
    for (int i = 0; i < flags.Count; i++)
    {
        if (flags[i] == flag) return true;
    }
    return false;
}

// A flag is only ever in the list once.
void SetFlag(List<string> flags, string flag)
{
    if (!HasFlag(flags, flag)) flags.Add(flag);
}

bool AllDefeated(List<int> mHealth)
{
    for (int i = 0; i < mHealth.Count; i++)
    {
        if (IsAlive(mHealth[i])) return false;
    }
    return true;
}

bool IsQuestReady(string need, List<string> flags, List<string> inventory, List<int> mHealth, int levelNumber)
{
    if (need == "gold") return CountItem(inventory, "gold") >= 3;
    else if (need == "boss") return HasFlag(flags, "boss down " + levelNumber);
    else if (need == "monsters") return AllDefeated(mHealth);
    else return true;
}

int QuestState(List<string> flags, List<string> inventory, List<int> mHealth, int levelNumber)
{
    if (HasFlag(flags, "done " + levelNumber)) return 3;
    else if (!HasFlag(flags, "started " + levelNumber)) return 0;
    else if (IsQuestReady(QuestNeed(levelNumber), flags, inventory, mHealth, levelNumber)) return 2;
    else return 1;
}


// ---------- one step of a conversation ----------

void PrintDialogue(string npcName, List<string> says, List<string> choiceA, List<string> choiceB, int line)
{
    Console.WriteLine();
    Console.WriteLine(npcName + ": " + says[line]);
    if (choiceA[line] == "")
    {
        Console.WriteLine("  (press Enter)");
    }
    else
    {
        Console.WriteLine("  1. " + choiceA[line]);
        if (choiceB[line] != "") Console.WriteLine("  2. " + choiceB[line]);
    }
}

// Which line comes next, given what the player typed? Returning the SAME line means "ask again".
int NextLine(List<string> choiceA, List<int> nextA, List<string> choiceB, List<int> nextB, int line, string answer)
{
    if (choiceA[line] == "") return nextA[line];
    else if (answer == "1") return nextA[line];
    else if (answer == "2" && choiceB[line] != "") return nextB[line];
    else return line;
}

bool IsKnownAction(string action)
{
    if (action == "" || action == "start quest" || action == "finish quest") return true;
    else if (action == "give potion" || action == "give arrows" || action == "give stones") return true;
    else if (action == "give shield" || action == "give gold") return true;
    else if (action == "heal party" || action == "restore mana") return true;
    else if (action == "power might" || action == "power guard" || action == "power regen") return true;
    else return false;
}

// Adds the same item several times.
void AddMany(List<string> inventory, string item, int count)
{
    for (int i = 0; i < count; i++)
    {
        inventory.Add(item);
    }
}

// Adds them and says so.
void GiveItems(List<string> inventory, string item, int count)
{
    AddMany(inventory, item, count);
    Console.WriteLine("(You get " + count + " x " + item + ".)");
}

// Does whatever a line of conversation "gives". Everything here changes a LIST,
// which a function is able to do. Mana is an int, so it is handled by ManaAfterAction.
void DoAction(string action, int levelNumber, List<string> flags, List<string> inventory, List<int> health, int maxHp, List<string> powerNames, List<int> powerTurns)
{
    if (action == "start quest")
    {
        SetFlag(flags, "started " + levelNumber);
        Console.WriteLine("(Quest started: " + QuestNeedWords(QuestNeed(levelNumber)) + ".)");
    }
    else if (action == "finish quest")
    {
        if (QuestNeed(levelNumber) == "gold")
        {
            RemoveItem(inventory, "gold");
            RemoveItem(inventory, "gold");
            RemoveItem(inventory, "gold");
        }
        SetFlag(flags, "done " + levelNumber);
        if (QuestNeed(levelNumber) != "") Console.WriteLine("(Quest finished!)");
    }
    else if (action == "give potion") GiveItems(inventory, "potion", 1);
    else if (action == "give arrows") GiveItems(inventory, "arrow", 3);
    else if (action == "give stones") GiveItems(inventory, "stone", 3);
    else if (action == "give shield") GiveItems(inventory, "shield", 1);
    else if (action == "give gold") GiveItems(inventory, "gold", 2);
    else if (action == "heal party")
    {
        for (int i = 0; i < health.Count; i++)
        {
            if (IsAlive(health[i])) health[i] = maxHp;
        }
        Console.WriteLine("Everyone still standing is back to full health.");
    }
    else if (action == "power might") AddPowerup(powerNames, powerTurns, "might", 12);
    else if (action == "power guard") AddPowerup(powerNames, powerTurns, "guard", 12);
    else if (action == "power regen") AddPowerup(powerNames, powerTurns, "regen", 12);
}

int ManaAfterAction(string action, int mana, int maxMana)
{
    if (action == "restore mana")
    {
        Console.WriteLine("Your mana is full.");
        return maxMana;
    }
    else
    {
        return mana;
    }
}


// ---------- checking a conversation obeys the contract ----------

bool CheckDialogue(int levelNumber, List<string> says, List<string> choiceA, List<int> nextA, List<string> choiceB, List<int> nextB, List<string> gives)
{
    bool ok = true;
    int count = says.Count;

    if (count == 0)
    {
        Console.WriteLine("DIALOGUE PROBLEM: the conversation has no lines.");
        return false;
    }
    if (choiceA.Count != count || nextA.Count != count || choiceB.Count != count || nextB.Count != count || gives.Count != count)
    {
        Console.WriteLine("DIALOGUE PROBLEM: the six lists are not the same length. Add every line with AddLine.");
        return false;
    }

    for (int state = 0; state <= 3; state++)
    {
        int start = DialogueStart(levelNumber, state);
        if (start < 0 || start >= count)
        {
            Console.WriteLine("DIALOGUE PROBLEM: for quest state " + state + " the talk opens on line " + start + ", but the lines are 0 to " + (count - 1) + ".");
            ok = false;
        }
    }

    for (int i = 0; i < count; i++)
    {
        if (nextA[i] < -1 || nextA[i] >= count || nextB[i] < -1 || nextB[i] >= count)
        {
            Console.WriteLine("DIALOGUE PROBLEM: line " + i + " leads to a line that does not exist. Lines are 0 to " + (count - 1) + ", and -1 ends the talk.");
            ok = false;
        }
        if (choiceA[i] == "" && nextA[i] != -1 && nextA[i] <= i)
        {
            Console.WriteLine("DIALOGUE PROBLEM: line " + i + " has no choice, so it must lead forwards or to -1. It leads to " + nextA[i] + ".");
            ok = false;
        }
        if (choiceA[i] != "" && (nextA[i] == i || (choiceB[i] != "" && nextB[i] == i)))
        {
            Console.WriteLine("DIALOGUE PROBLEM: an answer on line " + i + " leads straight back to line " + i + ". Send it to another line, or to -1.");
            ok = false;
        }
        if (choiceA[i] == "" && choiceB[i] != "")
        {
            Console.WriteLine("DIALOGUE PROBLEM: line " + i + " has a second answer but no first answer.");
            ok = false;
        }
        if (!IsKnownAction(gives[i]))
        {
            Console.WriteLine("DIALOGUE PROBLEM: line " + i + " gives \"" + gives[i] + "\", which is not an action the game knows.");
            ok = false;
        }
    }
    return ok;
}
