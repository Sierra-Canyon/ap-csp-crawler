# Dungeon Crawler

**AP Computer Science Principles · Period D · 2026-27 · Mr. DeVaughn-Brown · Room U 108**

A top-down dungeon game that already works: monsters, a boss, arrows, spells, powerups, and people
to talk to. From the first day you write small functions that the game calls: one quest each class.
By Tuesday 12/1 you also add a character, rewrite eight of its functions, and design a level.

The game is built from **five tools and nothing else**: variables, `if / else if / else`,
functions, `List<T>`, and `for` loops. No classes, no arrays, no `foreach`, no `switch`,
no `var`.

---

## Once, at the start

Everything for this class lives in `~/version_control`. Not Downloads, not Desktop, and
never iCloud Drive or Google Drive.

```
cd ~/version_control
git clone <your repo url>
cd ~/version_control/apcsp-2026-2027-crawler-<your-username>/dungeon
dotnet run
```

Type `d` and press Enter. If the `@` moved, your machine is ready. Type `quit` to stop.

Then start your branch. You do this once, and every quest and milestone goes on it.

```
git switch -c crawler
git push -u origin crawler
```

## Where things are

| Folder | What it is | Do you edit it? |
|---|---|---|
| **`dungeon/`** | The game. `README.txt` lists every function, its file and what it does. | Yes, from Day 0 |
| **`dungeon/Quest00.cs`** to **`Quest16.cs`** | **One quest a class.** The sheet for the day has the rules and the rows you are graded on. The game calls what you write. | Yes, one each class |
| **`dungeon/QuestBoard.cs`** | Teacher code that calls your quest functions from inside the game. | No |
| **`dungeon/MyCharacter.cs`**, **`dungeon/MyLevel.cs`** | **Your two files.** | Yes |
| **`dungeon/MySpell.cs`**, **`dungeon/MyDialogue.cs`** | Two more files that are yours: your own spell, and what the person on your level says. Optional. | Yes |
| **`dungeon/practice/`** | Scratch space. Nothing in here is compiled into the game. | Yes |
| **`drills/`** | Starter programs for class. Your sheet tells you which one to copy into your work repository. | No, you copy them |
| **`dungeon-tests/`** | The self-check. It calls every function and prints PASS or FAIL. | No |
| **`specs/`** | The assignment and the milestone sheets. | No |
| **`playtest/`** | Your playtest report, written on Tuesday 11/24. | Yes, on 11/24 |
| **`CREDITS.md`** | Who made the art and music you used, and any help you had. | Yes |
| **`dungeon-graphics/`**, **`assets/`**, **`asset-library/`** | The graphical version and its art. We open these together on **Tuesday 11/10**. | Not the engine. Yes, `assets/` |
| **`dungeon-web/`** | The same game in a browser. | No |

## The three commands you will use every day

Every `cd` below starts from your home folder, so they work no matter which folder the terminal is in.

```
cd ~/version_control/apcsp-2026-2027-crawler-<your-username>/dungeon
dotnet run                play the text game

cd ~/version_control/apcsp-2026-2027-crawler-<your-username>/dungeon-tests
dotnet run                check every function
dotnet run Damage         check only the functions with Damage in the name
dotnet run Quest03        check only the rows for Quest 3 and its challenge
```

Inside the game, type `quests` to see what your quest functions do with the real party, inventory, and level.

## The debugger

From Tue 9/29. Open VS Code on this folder (the one with `dungeon/` in it), with the **C#** extension by Microsoft installed. Click in the margin to the left of a line number so a red dot appears, then press **F5** and pick **Play the game** or **Check one quest** (it asks for a quest name, such as `Quest04`). The program stops before the red dot, with the line in yellow; the **Variables** pane on the left shows what every variable holds. **F10** runs one line, **F11** steps into a function call, **Shift+F11** steps back out, **F5** continues, **Shift+F5** stops. Every quest sheet has a line that starts "Stuck on a row?" that says where the red dot goes.

A line that says **FAIL** shows the call, the right answer and what your function gave.
Read the first one. A line that says **TO DO** names a quest or a milestone you have not reached yet. It is a reminder, not a mistake.
The rows for a quest are not run until you delete the `QUEST NOT STARTED` line at the top of its file.
A line that says **EXTRA** is optional and is never graded.

## Updating from the template

When the template gets a fix, it does not reach your repository by itself. The one-page sheet "Updating from the template" has the commands; here they are again. Every command runs in a terminal inside your repository folder.

Once, the first time only (with nothing uncommitted):

```
git remote add template https://github.com/Sierra-Canyon/ap-csp-crawler.git
git fetch template --tags
git checkout main
git merge -s ours --allow-unrelated-histories -m "Link template history" baseline
git push
```

Every time there is an update (with nothing uncommitted):

```
git checkout main
git pull --no-rebase template main
git push
git checkout crawler
git merge main
git push
```

## The four contracts

Every level, character and conversation in the class follows these.

1. **A level is a `List<string>`.** One string per row. Every row the same length.
   It is read as `level[y][x]`: row first, then column.
2. **Nine tiles, and only nine.** `#` wall · `.` floor · `@` player start · `M` monster · `$` loot · `+` door · `N` someone to talk to · `*` a powerup · `B` the boss, one at most
3. **A character is an index.** `names[2]`, `health[2]` and `attack[2]` are one character.
4. **A line of conversation is an index too.** `says[2]`, `choiceA[2]`, `nextA[2]`, `choiceB[2]`, `nextB[2]` and `gives[2]` are one line. The top of `Dialogue.cs` explains each list.

## Branch, commits, pull request

`main` is the record of finished work. **You never work on it directly.** This project has
one branch, called exactly `crawler`, and one pull request for the whole project.
You start the branch on Day 0. Milestone 1 walks you through the pull request.

**Commit while you work.** One commit per function you write or rewrite, and a push before you leave the room. A project that
arrives as one commit tells me nothing about how you got there. I look at this.

## Help, and what kind

**Your C# is yours.** You may ask a chatbot to explain an error message or explain code you
already wrote. You may not turn in code you did not write. This is the same rule as the syllabus.

**Art and music are different.** You may use the ready-made library, draw your own, or use a
chatbot with the prompts in `asset-library/`. Art and music are not graded. **Saying where
they came from is.** Every picture, song and chatbot you used goes in `CREDITS.md`.

**What your characters say is your writing.** The lines in `MyDialogue.cs` are yours, like the rest of your C#.

## If something breaks

**`dotnet run` prints red errors.** Read the first one. It names a file and a line number.

**The self-check says FAIL after you rewrote a function.** Read the call on that line and trace your function by hand with those inputs.

**I changed something and now nothing works.** `git status` shows what you touched.
`git restore dungeon/Levels.cs` puts one file back to your last commit.

**If any git step fails, ask.** Do not paste the error into a chatbot and run whatever it
tells you.
