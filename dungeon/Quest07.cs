// ======================================================================
//  Quest 7: Three Gates, One Function.   Review, Mon 10/5
//  The sheet for this day has the rules and the rows it is graded on.
//  Check it:   cd ~/version_control/apcsp-2026-2027-crawler-<your-username>/dungeon-tests
//              then   dotnet run Quest07
//
//  QUEST NOT STARTED      <- delete this whole line when you begin the quest
// ======================================================================
// These three are finished and they work. Read them first. Once `PrintGate` works, the quest has you change their bodies.
void PrintRedGate()
{
    Console.WriteLine("The red gate needs 1 key.");
}

void PrintBlueGate()
{
    Console.WriteLine("The blue gate needs 2 keys.");
}

void PrintGoldGate()
{
    Console.WriteLine("The gold gate needs 3 keys.");
}

void PrintGate(string color, int keys)
{
    // QUEST 7: your lines go here
}

// ----------------------------------------------------------------------
//  Challenge 7: The Other Direction.   Extra. Not graded.
//  CHALLENGE NOT STARTED  <- delete this whole line when you begin the challenge
// ----------------------------------------------------------------------
// Finished. Do not change it, and do not call it from the two below.
string PotionLabel(string size, int heals)
{
    return size + " potion (+" + heals + ")";
}

string SmallPotionLabel()
{
    return "";     // CHALLENGE 7: replace this line
}

string LargePotionLabel()
{
    return "";     // CHALLENGE 7: replace this line
}
