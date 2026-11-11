// ============================================================
//  MyFeature.cs: THIS FILE IS YOURS.   Milestone 2, Thu 11/12 to Wed 11/18.
//
//  Your feature remembers something that happens in the game, and a
//  command of your own shows it. One part a day:
//
//    Thu 11/12   MyFeatureWord: the word the player types. One lowercase word,
//                not a word the game already uses.
//                MyFeatureTitle: the first line your command prints.
//                Then your command in Program.cs.
//    Mon 11/16   MyRecord: adds what to the end of entries. An empty what adds nothing.
//                Then your list in Program.cs, and the line that adds to it.
//    Tue 11/17   MyCount: how many entries are exactly word.
//                Then the command learns a second word.
//    Wed 11/18   MyReport: everything your command shows, as one string. An empty
//                filter shows every entry. Any other filter shows only the entries
//                that are exactly filter. An empty list says so, in your words.
//                Then your Personalized Project Reference.
//
//  Above MyRecord, MyCount and MyReport, two comment lines of your own: what the
//  function does, and what each parameter is for.
//
//  Check it:   cd ~/version_control/apcsp-2026-2027-crawler-<your-username>/dungeon-tests
//              then   dotnet run MyFeature
//  Rows for a day you have not reached yet say TO DO. That is fine.
// ============================================================

string MyFeatureWord()
{
    return "";     // MILESTONE 2, THU: replace this line
}

string MyFeatureTitle()
{
    return "";     // MILESTONE 2, THU: replace this line
}

void MyRecord(List<string> entries, string what)
{
    // MILESTONE 2, MON: write this function
}

int MyCount(List<string> entries, string word)
{
    return -99;     // MILESTONE 2, TUE: replace this line
}

string MyReport(List<string> entries, string filter)
{
    return "";     // MILESTONE 2, WED: replace this line
}
