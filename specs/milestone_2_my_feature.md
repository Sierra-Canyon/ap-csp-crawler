# Milestone 2: my feature

**Built Thu 11/12, Mon 11/16, Tue 11/17, Wed 11/18 · due Friday 11/20, 5:00 AM · 40 points**

You add a feature to the game, one part a day. The feature remembers something that happens in the game, in a list, and a command of your own shows it. By Wednesday it has everything the Create Task asks of a program: input, output, a list, a procedure with a parameter, and an algorithm with sequencing, selection and iteration inside it. Your code goes in `dungeon/MyFeature.cs` and in `dungeon/Program.cs`.

## 0. Check your branch, and update from the template

`git status` should say **`On branch milestone-2`**, in this repository and in your log repository. If it says `main`, stop: `git switch -c milestone-2` and `git push -u origin milestone-2` first.

On Thursday, update from the template (the sheet "Updating your repository from the template"). It brings `dungeon/MyFeature.cs` and the self-check rows for it. `cd dungeon-tests` then `dotnet run MyFeature` should list 16 rows that say `TO DO`.

Open the pull request into `main` on Thursday, with `jd12` as reviewer, so every push shows in it. Do not merge it. I grade what is on `milestone-2` at 5:00 AM on Friday 11/20. In class on Friday, after Quiz 4, you merge it yourself, in both repositories, then start `milestone-3` in both.

## 1. Choose your feature

The walkthrough builds a Bestiary: every monster the party beats goes in a list, and `bestiary` shows it. Yours is one of these, or your own idea once I have said yes to it.

| Feature | It remembers | Where `MyRecord` goes in `Program.cs` |
|---|---|---|
| Loot Log | every item picked up from a `$` | under `AddLoot(inventory, item);` |
| Spell Log | every spell cast | under `mana = mana - cost;`, inside `if (cost > 0)` |
| Travel Log | every level reached | under `PrintLevelBanner(levelNumber);` |
| Powerup Log | every powerup picked up | under the line that starts `AddPowerup(` |
| Talk Log | everyone the party talks to | under `talkLine = DialogueStart(levelNumber, state);` |
| Step Log | every command typed | under `string command = ReadCommand();` |
| Your own | ask me first | ask me first |

The Bestiary is the walkthrough's. It is not on the menu.

## 2. The four days

| Day | In `MyFeature.cs` | In `Program.cs` | Commit message |
|---|---|---|---|
| **Thu 11/12** | `MyFeatureWord()`, `MyFeatureTitle()` | an `else if` that prints your title when the player types your word | `feat: Add my command` |
| **Mon 11/16** | `MyRecord(entries, what)` | your list, above the `while`, the `MyRecord` line, and a `for` that prints the list | `feat: Add my list` |
| **Tue 11/17** | `MyCount(entries, word)` | your command learns a second word: `loot gold` prints how many | `feat: Add MyCount` |
| **Wed 11/18** | `MyReport(entries, filter)` | your command prints `MyReport` | `feat: Add MyReport` |

Commit on the day you write it, with that message, and push. The commits are counted by day.

The command works in the text game, `cd dungeon` then `dotnet run`. The graphical game does not read `Program.cs`, so your command is not in it.

## 3. The rules for your functions

- `MyFeatureWord()` is one lowercase word that the game does not already use.
- `MyRecord(entries, what)` adds `what` to the end of `entries`. An empty `what` adds nothing.
- `MyCount(entries, word)` is how many entries are exactly `word`.
- `MyReport(entries, filter)` is everything your command shows, as one string. An empty `filter` shows every entry. Any other `filter` shows only the entries that are exactly `filter`. An empty list says so, in your words. It has a `for` loop and an `if` inside it.
- Do not change a signature. The self-check calls each function the way it is written in the template.
- Above `MyRecord`, `MyCount` and `MyReport`, two comment lines of your own: what the function does, and what each parameter is for.

```csharp
// Adds one item to the loot log.
// entries: the loot log. what: the item picked up, or "" for nothing.
```

## 4. Wednesday: your Personalized Project Reference

On the Create Task, you take four screenshots of your own code into the exam: the Personalized Project Reference. On Wednesday you make one for this feature. In the pull request description, under a heading `Personalized Project Reference`, paste four screenshots:

1. **Procedure (i):** all of `MyReport`, from its first line to its last `}`.
2. **Procedure (ii):** the line in `Program.cs` that calls `MyReport`.
3. **List (i):** the line that adds data to your list. Show the `MyRecord` call in `Program.cs`.
4. **List (ii):** your list being used. Show the loop in `MyReport` that goes over `entries`.

No comments in the screenshots, and no name. Delete the comment lines before you take the screenshot, then undo.

## 5. Check it, often

```
cd dungeon-tests
dotnet run MyFeature
dotnet run
```

Then play it, type your word, and make the thing happen that adds to your list.

## Points

| | |
|---|---|
| The 16 self-check rows for `MyFeature`, one point each | 16 |
| Four commits, one on each day, named as above | 4 |
| In the game: the word prints the title, the list grows when the thing happens, and `word something` shows only that | 3 |
| Two comment lines above `MyRecord`, `MyCount` and `MyReport` | 3 |
| The Personalized Project Reference: four screenshots, each the right code, from your own feature, with no comments | 12 |
| The pull request opened on Thursday, with `jd12` as reviewer and the checklist below | 2 |
| **Total** | **40** |

### Spec self-check, to copy into the pull request on Thursday

```
- [ ] I chose my feature: ____________ and my word is ____________
- [ ] Each day has its own commit, named as on the spec, made on the day I wrote it
- [ ] dotnet run MyFeature ends with 16 passed
- [ ] My command works in the game, with a second word too
- [ ] Two comment lines above MyRecord, MyCount and MyReport
- [ ] No signature was changed
- [ ] The FiveTools line says none
- [ ] Four screenshots under Personalized Project Reference, with no comments
```

## The three questions for each day's log

**Thu 11/12.** 1. In one or two sentences, what is your feature for, and how does it help the player? 2. Describe the input to your command and the output it produces. Which line of `Program.cs` decides whether the player typed your word? 3. What happens if the player types your word with a capital letter, and why?

**Mon 11/16.** 1. What does your list store, and what happens in the game that makes `MyRecord` run? 2. Your list is made above the `while`. What would happen if it were made inside the loop, and why? 3. Suppose you could not use a list. How would your code have to change to remember everything, and what could it no longer do?

**Tue 11/17.** 1. Write two calls to `MyCount` with different arguments that give different results, and give the result of each. 2. Suppose `MyCount` had no `word` parameter. How would your code have to change to count two different things? 3. How many times does the loop in `MyCount` run for `MyCount([gold, arrow, gold], "gold")`, and how many times is the count changed?

**Wed 11/18.** Answer from your Personalized Project Reference. 1. Describe, step by step, what `MyReport` does, in enough detail that someone else could write it. 2. Write two calls to `MyReport` that make different parts of it run, and say which part each one runs. 3. Pick one Boolean expression in `MyReport`. Give a call where it is true and a call where it is false.
