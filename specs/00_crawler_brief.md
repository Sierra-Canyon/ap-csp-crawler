# Dungeon Crawler: the project brief

**AP Computer Science Principles · Period D · handed out Friday 10/9**

Between now and **Wednesday 12/2** you add your character, rewrite eight of the game's
functions, and design a level that works in anyone's game.

## The rule

Five tools and nothing else: **variables, `if / else if / else`, functions, `List<T>`, `for` loops.**
No `class`, no arrays, no `foreach`, no `switch`, no `var`, no `while` (the game loop in
`Program.cs` is the only one). The self-check reads your files and reports anything else.

## What you hand in, and when

| | What | Built in class | Due | Points |
|---|---|---|---|---|
| **Milestone 1** | **Your character.** `IsFairCharacter` and `MyCharacter`. Branch and pull request opened. | Mon 10/12 | **Thu 10/15, end of class** | 20 |
| **Milestone 2** | **Eight rewrites.** Two functions a day for four days, each one deleted and written again by you. | Thu 11/12, Mon 11/16, Tue 11/17, Wed 11/18 | **Fri 11/20, 5:00 AM** | 40 |
| **Milestone 3** | **Your level.** `MyLevel`, two new functions of your own design, credits. | Mon 11/23, Tue 11/24 | **Wed 12/2, start of class** | 80 |
| **Playtest** | You play someone else's level in your game and write the report. | Tue 12/1 | **Tue 12/1, end of class** | 20 |
| **Demo day** | Two minutes: your level on the projector, and one function you are proud of. | Wed 12/2 | | part of Milestone 3 |

Each milestone has its own sheet in `specs/`. Milestone 2 needs `for` loops, which start on 10/29.

## How it is graded

**Milestone 3 is 80 points:**

| | Points | Full marks means |
|---|---|---|
| The contracts | 20 | `MyLevel` passes every self-check line, is at least 14 wide and 9 tall, has at least 3 monsters and 2 loot, and your partner's playtest report says it loaded in their game untouched. |
| Your own functions | 20 | Two new functions that were not in the template. Each takes at least one parameter, does one clear job, and is called from somewhere. |
| Five tools only | 10 | The self-check's FiveTools line says `none`. |
| Process | 15 | Commits on at least six different class days. One commit per function. The pull request is open, the template is filled in, and `jd12` is the reviewer. |
| It runs | 10 | `dotnet run` builds with no errors, and the game can be played from level 1 to the end. |
| Credits | 5 | `CREDITS.md` names where your art and music came from and any help you used. |

**Art and music are worth zero points.** The credit line in `CREDITS.md` is the only part of
them I grade.

## Help, and what kind

**Your C# is yours.** A chatbot may explain an error message or explain code you already
wrote. It may not write your functions. I will ask you to explain any function with your
name on it, at your desk, without notes.

**Art and music may come from anywhere honest:** the class library, your own drawing, or a
chatbot using the prompts in `asset-library/`. Every source goes in `CREDITS.md`. No
characters from games, films, or shows.

## Where the work happens

In this room, on the class days in the table. If the bell beats you, finish before the next
meeting. Your daily log continues in your log repository exactly as before.

## The three questions for today's log

1. Which function in the game do you understand best right now, and what does each of its parameters do?
2. Which milestone do you expect to be hardest for you, and what makes you think so?
3. Why might a game built only from functions and lists be easier to share with a classmate than one long program?
