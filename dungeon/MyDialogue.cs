// ============================================================
//  MyDialogue.cs: THIS FILE IS YOURS.
//
//  The N tile on YOUR level says these lines. Read the top of Dialogue.cs first.
//
//  AddLine takes, after the six lists:
//      what they say, answer 1, where it leads, answer 2, where it leads, what it gives
//
//  -1 ends the talk. An answer of "" means "press Enter", and then the line
//  must lead forwards (to a bigger number) or to -1.
//
//  What a line can give:
//      ""   "give potion"   "give arrows"   "give stones"   "give shield"   "give gold"
//      "heal party"   "restore mana"   "power might"   "power guard"   "power regen"
//      "start quest"   "finish quest"
// ============================================================

string MyNpcName()
{
    return "Stranger";
}

string MyBossName()
{
    return "Dragon";
}

// What your quest needs before it can be finished:  ""  "gold"  "boss"  or  "monsters"
string MyQuestNeed()
{
    return "";
}

// Which line the talk opens on. questState is:
//   0 not started   1 started but not ready   2 ready to finish   3 done
int MyDialogueStart(int questState)
{
    return 0;
}

void MyDialogue(List<string> says, List<string> choiceA, List<int> nextA, List<string> choiceB, List<int> nextB, List<string> gives)
{
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "Hello. I did not expect anyone.", "Who are you?", 1, "Goodbye.", -1, "");
    AddLine(says, choiceA, nextA, choiceB, nextB, gives, "Nobody yet. Somebody has to write my lines.", "", -1, "", -1, "");
}
