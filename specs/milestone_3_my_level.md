# Milestone 3: your level

**Built Fri 11/20 and Mon 11/23 · playtest Tue 11/24 · due Tuesday 12/1, start of class · 80 points**

The game's last level is whatever `MyLevel()` returns. You design it, and you add two functions
of your own.

## 1. Design it on paper first

Graph paper, one square per tile. Decide where the player starts, where the door is, and what is
in the way. Give the player **a choice**: a monster to fight or walk around, loot that costs a
detour, a dead end.

## 2. Type it into `dungeon/MyLevel.cs`

One `level.Add("...")` per row. The contracts:

- every row the same length
- only the nine tiles: `#` wall · `.` floor · `@` start · `M` monster · `$` loot · `+` door · `N` someone to talk to · `*` a powerup · `B` the boss, one at most
- exactly one `@`, at least one `+`
- walls all the way round the outside
- **at least 14 wide and 9 tall**, at least **3 monsters** and **2 loot**
- the door, all the loot and all the monsters can be reached on foot. An `N` never moves, so it blocks the way like a wall

`N`, `*` and `B` are optional. The door stays shut until a `B` is beaten.

Water, trees, lava, and ice are all `#`, drawn with a different picture. A tenth tile fails
`CheckLevel`.

## 3. Two new functions of your own

Each takes **at least one parameter**, does **one clear job**, and is **called from somewhere**.
Put them in any of your four files. Some that fit:

- `string RowOf(char tile, int width)` builds a border row with a `for` loop, and `MyLevel` calls it
- `void AddRoomRow(List<string> level, string middle, int width)` pads a row with walls to the full width
- `int LootCount(List<string> level)` or `bool IsHardLevel(List<string> level)`, built on `CountTiles`, called from `PrintLevelBanner` or the game loop
- a fourth monster kind: change `MonsterName`, `MonsterHealth` and `MonsterAttack`, with a helper of your own
- a new command in `Program.cs`, such as `rest`, that calls a function you wrote
- `void Say(..., string text, int next, string action)` in `MyDialogue.cs`, for the lines with nothing to choose, so `MyDialogue` calls it and not `AddLine`
- a helper that `MySpellDamage` calls, such as `int Falloff(int damage, int distance)`

Write the signature first. Say what each parameter is for before you write the body.

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
`feat: Add my level`, `feat: Add RowOf` (your function's name).

### Spec self-check, to copy into the pull request

```
- [ ] MyLevel is at least 14 wide and 9 tall, with 3 or more monsters and 2 or more loot
- [ ] The self-check ends with 0 failed, 0 still to do
- [ ] The FiveTools line says none
- [ ] Two new functions, each with a parameter, each called from somewhere. Their names:
- [ ] levelNumber in Program.cs is back to 1
- [ ] CREDITS.md is filled in
- [ ] playtest/PLAYTEST.md is filled in (after Tuesday 11/24)
```

## The three questions for the log

**Fri 11/20.** 1. What choice does your level give the player, and which tiles create it? 2. What will most people get wrong when they type a level as strings, and how does the self-check catch it? 3. What is the signature of your first new function, and what is each parameter for?

**Mon 11/23.** 1. Which of your two new functions is called from more than one place, or could be? 2. What did you change after playing your own level, and what made you change it? 3. If a classmate pasted your `MyLevel.cs` into their game right now, what is the one thing that could still go wrong?
