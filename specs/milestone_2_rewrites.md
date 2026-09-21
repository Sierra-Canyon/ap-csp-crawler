# Milestone 2: eight rewrites

**Built Tue 11/10, Thu 11/12, Mon 11/16, Tue 11/17 · due Wednesday 11/18, 5:00 AM · 40 points**

Each day you choose **two** functions from that day's menu, delete what is inside them, and
write them again yourself. Eight by Friday. The game must still work after each one.

## The menu

| Day | File | Choose two |
|---|---|---|
| **Tue 11/10** Crawler 2 | `Levels.cs` | `TileAt` · `IsWall` · `CountTiles` · `FindTileX` · `FindTileY` |
| **Thu 11/12** Crawler 3 | `Characters.cs`, `Combat.cs` | `FindCharacter` · `TotalHealth` · `StrongestIndex` · `AnyAlive` · `NextAliveIndex` · `MonsterAt` · or `FindSpell` (`Magic.cs`) · `HasFlag` · `AllDefeated` (`Dialogue.cs`) |
| **Mon 11/16** Crawler 4 | `Combat.cs` | `Damage` · `Clamp` · `WinnerName` · `HeroAttacks` · or `BossPhase` (`Boss.cs`) · `TargetInLine` (`Ranged.cs`) |
| **Tue 11/17** Crawler 5 | `Loot.cs`, `Characters.cs` | `CountItem` · `RemoveItem` · `DrinkPotion` · `HealthBar` · or `TickPowerups` (`Powerups.cs`) · `AddMany` (`Dialogue.cs`) |

## How to rewrite a function

1. **Read it once**, and read its line in `dungeon/README.txt`. Say out loud what each parameter is for.
2. **Delete everything between its `{` and `}`.** Keep the signature. Do not change the signature,
   because the rest of the game calls it.
3. **Write the body again** from the signature and the one-line description.
4. **Check it:** `cd dungeon-tests` then `dotnet run TileAt` (use your function's name).
   A FAIL line shows the call, the right answer and what yours gave. Trace that call by hand.
5. **Put two comment lines above it:**
   ```csharp
   // Rewritten by me on 11/10.
   // What I got wrong first: I compared x with level.Count instead of y.
   ```
   If nothing went wrong, say what you checked to be sure.
6. **Commit that one function:** `git add dungeon/Levels.cs`, `git commit`, message exactly
   `feat: Rewrite TileAt`. Then `git push`.

**Stuck?** Your hints, in this order: the README line, the FAIL message, your notes, me.
Looking at the old version on github.com is the last resort. If you do it, say so in your
comment. Saying so costs no points.

## Tuesday 11/10 only: the graphical game

Before you start the menu:

```
cd dungeon-graphics
dotnet run
```

The first run downloads the drawing library, so it takes a minute. Arrow keys move. Walk into a
monster to swing. This window runs the functions in `dungeon/`, including the ones you rewrite.

### Spec self-check, to copy into the pull request on Friday

```
- [ ] Eight functions rewritten, two from each day's menu
- [ ] Each has my two comment lines above it
- [ ] Each has its own commit, named feat: Rewrite <FunctionName>
- [ ] No signature was changed
- [ ] The self-check ends with 0 failed
- [ ] The FiveTools line says none
```

## The three questions for each day's log

**Tue 11/10.** 1. In `level[y][x]`, which index picks the row, and how did you check? 2. What will most people get wrong when they rewrite `TileAt`, and why is it easy to miss? 3. Which of your two functions calls another function, and what would break if that one were wrong?

**Thu 11/12.** 1. Why do these functions return an index and not a name? 2. What does your loop do when the list is empty, and how do you know? 3. Where in the game would the wrong answer from `AnyAlive` first show up?

**Mon 11/16.** 1. `Damage(4, 1)` and `Damage(1, 4)` both compile. How would a player notice the wrong one? 2. Which parameter of `HeroAttacks` is a list that the function changes, and how is that different from what `Heal` can do? 3. What did the self-check tell you today that reading your own code did not?

**Tue 11/17.** 1. `RemoveItem` removes one potion, not all of them. Which line of yours makes that true? 2. What happens in `DrinkPotion` if the answer from `Heal` is not stored anywhere? 3. Of your eight rewrites, which one would you now be able to write on paper, and which one not yet?
