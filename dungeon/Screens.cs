// ============================================================
//  Screens.cs: words on the screen, and reading the keyboard.
// ============================================================

void PrintTitle()
{
    Console.WriteLine("========================================");
    Console.WriteLine("          D U N G E O N   C R A W L E R");
    Console.WriteLine("========================================");
    Console.WriteLine("Find the + door on every level. Stay alive.");
}

void PrintControls()
{
    Console.WriteLine("Type a command, then press Enter:");
    Console.WriteLine("  w a s d      move up, left, down, right");
    Console.WriteLine("               (walk INTO a monster to attack it, INTO an N to talk)");
    Console.WriteLine("  fire d       shoot an arrow to the right (or w, a, s)");
    Console.WriteLine("  throw d      throw a stone to the right");
    Console.WriteLine("  cast spark d cast a spell. Some spells need a direction");
    Console.WriteLine("  spells       list the spells you know");
    Console.WriteLine("  potion       the leader drinks a potion");
    Console.WriteLine("  lead NAME    put a different character in front");
    Console.WriteLine("  help         show this list and the map legend");
    Console.WriteLine("  quests       show what your quest functions do");
    Console.WriteLine("  export       save mygame.txt for the web player");
    Console.WriteLine("  quit         stop playing");
}

void PrintLevelBanner(int levelNumber)
{
    Console.WriteLine();
    if (levelNumber == LevelCount()) Console.WriteLine("----- LEVEL " + levelNumber + " : MY LEVEL -----");
    else Console.WriteLine("----- LEVEL " + levelNumber + " -----");
}

void PrintGameOver()
{
    Console.WriteLine();
    Console.WriteLine("The whole party has fallen. GAME OVER.");
}

void PrintWin(int gold)
{
    Console.WriteLine();
    Console.WriteLine("You escaped the dungeon with " + gold + " gold. YOU WIN!");
}

// Reads one line from the keyboard. If there is no keyboard left to read
// (the input has ended) it answers "quit" so the game can never get stuck.
string ReadCommand()
{
    Console.Write("> ");
    string command = Console.ReadLine();
    if (command == null) return "quit";
    return command.Trim();
}

// How far does this command move the player sideways? -1, 0 or 1.
int StepX(string command)
{
    if (command == "a") return -1;
    else if (command == "d") return 1;
    else return 0;
}

// How far does this command move the player up or down? -1, 0 or 1.
int StepY(string command)
{
    if (command == "w") return -1;
    else if (command == "s") return 1;
    else return 0;
}


// "cast spark d" is three words. These pull one word out of a command.
// A word that is not there comes back as "".
string FirstWord(string text)
{
    int space = text.IndexOf(' ');
    if (space == -1) return text;
    else return text.Substring(0, space);
}

// Everything after the first word, with the spaces around it trimmed off.
string RestOfWords(string text)
{
    int space = text.IndexOf(' ');
    if (space == -1) return "";
    else return text.Substring(space + 1).Trim();
}

string SecondWord(string text)
{
    return FirstWord(RestOfWords(text));
}

string ThirdWord(string text)
{
    return FirstWord(RestOfWords(RestOfWords(text)));
}
