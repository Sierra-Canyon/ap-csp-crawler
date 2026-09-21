// ======================================================================
//  Quest 5: Two Repairs.   Functions 5, Wed 9/30
//  The sheet for this day has the rules and the rows it is graded on.
//  Check it:   cd ../dungeon-tests   then   dotnet run Quest05
//
//  QUEST NOT STARTED      <- delete this whole line when you begin the quest
// ======================================================================
string ManaWarning(int mana, int cost)
{
    // QUEST 5, part 1. Take the // off the next five lines and build. Read the error, then repair it.
    // if (mana < cost)
    // {
    //     string message = "Not enough mana: " + mana + " of " + cost + " needed.";
    // }
    // return message;
    return "";     // QUEST 5: replace this line
}

// This one is finished and it works. Do not change it.
int ShortBy(int mana, int cost)
{
    int missing = cost - mana;
    if (missing < 0)
    {
        missing = 0;
    }
    return missing;
}

string ShortByLine(int mana, int cost)
{
    // QUEST 5, part 2. Take the // off the next line and build. Read the error, then repair it.
    // return "You are short by " + missing + ".";
    return "";     // QUEST 5: replace this line
}

// ----------------------------------------------------------------------
//  Challenge 5: Counting Steps.   Extra. Not graded.
//  CHALLENGE NOT STARTED  <- delete this whole line when you begin the challenge
// ----------------------------------------------------------------------
int CountStep(int stepsSoFar, string command)
{
    return -99;     // CHALLENGE 5: replace this line
}
