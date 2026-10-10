# Milestone 3: your level

**Built Mon 11/23 and Tue 11/24 · playtest Tue 12/1 · due Wednesday 12/2, start of class · 80 points**

The game's last level is whatever `MyLevel()` returns. You design it, and you add two functions
of your own: Function A and Function B.

## 0. Check your branch

`git status` should say **`On branch milestone-3`**, in this repository and in your log repository; the Milestone 2 sheet had you start both. If it says `main`, stop: `git switch -c milestone-3` and `git push -u origin milestone-3` first. On demo day, when the level is done, merge `milestone-3` into `main` with a pull request in both repositories.

## 1. Design it on paper first

Start from your `MyRoom()` from Quest 14. It is the first draft of this level.

Graph paper, one square per tile. Decide where the player starts, where the door is, and what is
in the way. Give the player **a choice**: a monster to fight or walk around, loot that costs a
detour, a dead end. Name that choice in your pull request. It is 2 of the 20 contract points.

Put walls all the way round the outside. The self-check does not test it.

## 2. Type it into `dungeon/MyLevel.cs`

One `level.Add("...")` per row. The contracts:

- every row the same length
- only the nine tiles: `#` wall · `.` floor · `@` start · `M` monster · `$` loot · `+` door · `N` someone to talk to · `*` a powerup · `B` the boss, one at most
- exactly one `@`, at least one `+`
- **at least 14 wide and 9 tall**, at least **3 monsters** and **2 loot**
- the door, all the loot and all the monsters can be reached on foot. An `N` never moves, so it blocks the way like a wall

`N`, `*` and `B` are optional. The door stays shut until a `B` is beaten.

Water, trees, lava, and ice are all `#`, drawn with a different picture. A tenth tile fails
`CheckLevel`.

## 3. Two new functions of your own

**Function A** is the function for your Personalized Project Reference on Monday 12/7. It

- takes a parameter that changes what it returns or what it does,
- has an `if` and a `for` inside it,
- uses a list,
- is called from the game.

**Function B** takes at least one parameter, does one clear job, and is called from somewhere.

Neither one was in the template. Above each one, write two comment lines: what it does, and what
each parameter is for.

```csharp
// Adds a story to the conversation, one line for each press of Enter.
// story: the lines, in order. reward: what the last line gives, such as "give potion".
```

**A model for Function A.** Your `MyStory()` from Quest 12 is a list of your character's lines.
`AddStory` turns it into a conversation for the `N` on your level:

```csharp
void AddStory(List<string> says, List<string> choiceA, List<int> nextA, List<string> choiceB, List<int> nextB, List<string> gives, List<string> story, string reward)
```

A `for` loop goes over `story`. Every line is added with `AddLine`, with no choice, leading to the
next line. The line being added is number `says.Count`, so the next one is `says.Count + 1`. An
`if` finds the last line: it leads to `-1` and gives `reward`. `MyDialogue` calls it after its
first line:

```csharp
AddLine(says, choiceA, nextA, choiceB, nextB, gives, "You found me. Do you want to hear how I got here?", "Tell me.", 1, "Not now.", -1, "");
AddStory(says, choiceA, nextA, choiceB, nextB, gives, MyStory(), "give potion");
```

Put an `N` on your level, walk into it, choose 1, and your story plays, one line for each press
of Enter. `dotnet run MyDialogue` checks the conversation.

**Where they go.** Your four files are `MyCharacter.cs`, `MyLevel.cs`, `MySpell.cs` and
`MyDialogue.cs`. A suggestion below may say `Program.cs`, `Screens.cs` or `Combat.cs` instead.

- A function of yours that `MyLevel` calls goes in `MyLevel.cs`, and its name starts with your initials,
  such as `JdRowOf`.
- Nothing outside `MyLevel.cs` calls a function in `MyLevel.cs`.

On playtest day your partner's `MyLevel.cs` replaces yours. These two rules are what let it build.

**Fits Function A:**

- `AddStory`, above, in `MyDialogue.cs`
- a new command in `Program.cs`, such as `rest`, that calls a function in `MyCharacter.cs`, such as
  `void JdRest(List<int> health, int amount, int maxHp)`: a `for` over the party, and an `if` so
  only a living hero gets `amount` back

**Fits Function B:**

- `string JdRowOf(char tile, int width)` builds a border row with a `for` loop, and `MyLevel` calls it
- `void JdAddRoomRow(List<string> level, string middle, int width)` pads a row with walls to the full width
- `bool IsHardLevel(List<string> level)`, built on `CountTiles`, in `Screens.cs`, called from
  `PrintLevelBanner` as `IsHardLevel(LoadLevel(levelNumber))`
- a fourth monster kind: change `MonsterName`, `MonsterHealth` and `MonsterAttack` in `Combat.cs`, with a helper of your own
- `void Say(..., string text, int next, string action)` in `MyDialogue.cs`, for the lines with nothing to choose, so `MyDialogue` calls it and not `AddLine`
- a helper that `MySpellDamage` calls, such as `int Falloff(int damage, int distance)`

Write the signature and the two comment lines first. Then the body.

## 4. Check it, often

```
cd dungeon-tests
dotnet run MyLevel
dotnet run
```

Then play it. In `dungeon/Program.cs`, change `int levelNumber = 1;` to `4` to start on your
level, and change it back before you hand in. In the graphical game: `dotnet run -- 4`.

## 5. Extras. Optional, ungraded.

The self-check lists these as `EXTRA`. They never count against you.

- **Your own spell**, in `MySpell.cs`: its name, its cost, and `MySpellDamage(int casterAttack, int distance)`. It is fair when it never does more than `cost * 3`.
- **Someone to talk to.** Put an `N` on your level and write their lines in `MyDialogue.cs`. `dotnet run MyDialogue` checks them.
- **A quest.** `MyQuestNeed` returns `"gold"`, `"boss"` or `"monsters"`, and `MyDialogueStart` picks the opening line for each stage of it.
- **A boss.** One `B`, and `MyBossName` names it.

## 6. Make it look like yours. Optional, ungraded.

Files ending in `4` change only your level: `wall4.png`, `floor4.png`, `door4.png`,
`monster4.png`, `npc4.png`, `boss4.png`, `music4.wav`. Open `asset-library/index.html`. It names the files for you.
**Whatever you use goes in `CREDITS.md`.**

## 7. Hand in

One commit for the level, one for each new function, then push. Messages:
`feat: Add my level`, `feat: Add JdRowOf` (your function's name). Commit on every class day you
work on it.

In the pull request, under **What is in this pull request**, name the choice your level gives the
player, and name Function A and Function B.

### Spec self-check, to copy into the pull request

```
- [ ] MyLevel is at least 14 wide and 9 tall, with 3 or more monsters and 2 or more loot
- [ ] The choice my level gives the player is named in this pull request
- [ ] The self-check ends with 0 failed, 0 still to do
- [ ] The FiveTools line says none
- [ ] Function A has a parameter that changes its result, an if and a for, uses a list, and is called from the game. Its name:
- [ ] Function B has a parameter, does one job, and is called. Its name:
- [ ] Two comment lines above each: what it does, what each parameter is for
- [ ] Every function of mine that MyLevel calls is in MyLevel.cs, and its name starts with my initials
- [ ] Commits on at least six different class days across the project, one commit per function
- [ ] levelNumber in Program.cs is back to 1
- [ ] CREDITS.md is filled in
- [ ] playtest/PLAYTEST.md is filled in (after Tuesday 12/1)
```

## The three questions for the log

**Mon 11/23.** 1. What choice does your level give the player, and which tiles create it? 2. What will most people get wrong when they type a level as strings, and how does the self-check catch it? 3. What is the signature of your first new function, and what is each parameter for?

**Tue 11/24.** 1. Which of your two new functions is called from more than one place, or could be? 2. What did you change after playing your own level, and what made you change it? 3. If a classmate pasted your `MyLevel.cs` into their game right now, what is the one thing that could still go wrong?
