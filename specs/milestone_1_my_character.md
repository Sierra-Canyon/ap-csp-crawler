# Milestone 1: your character

**Built Friday 10/9 · due Monday 10/12, end of class · 20 points**

Two functions in one file, `dungeon/MyCharacter.cs`.

## 0. Check your branch

The functions unit is on `crawler`, merged into `main` by your pull request on Day 7. Milestone 1 goes on its own branch, `milestone-1`, which that pull request's sheet had you create.

```
cd ~/version_control/apcsp-2026-2027-crawler-<your-username>
git status
```

`git status` should say **`On branch milestone-1`**. If it says `main`, stop: `git switch -c milestone-1` and `git push -u origin milestone-1` first, or the work lands somewhere I do not read. When the milestone is done, merge it into `main` with a pull request, the same way as the unit.

## 1. Write `IsFairCharacter`

A character costs `hp + atk * 2`. A character is **fair** when the cost is 18 or less.

```csharp
bool IsFairCharacter(int hp, int atk)
```

Right now it returns `true` for everything. Replace the body so it answers the question.
Two parameters in, one `bool` out. **The order of the parameters matters**:
`IsFairCharacter(10, 4)` and `IsFairCharacter(4, 10)` are different questions.

| Call | Should return | Because |
|---|---|---|
| `IsFairCharacter(10, 4)` | `true` | 10 + 8 is 18 |
| `IsFairCharacter(10, 5)` | `false` | 10 + 10 is 20 |
| `IsFairCharacter(9, 5)` | `false` | 9 + 10 is 19 |
| `IsFairCharacter(1, 1)` | `true` | 1 + 2 is 3 |

## 2. Rewrite `MyCharacter` so it uses it

Pick a name of 8 letters or fewer, a health from 1 to 10 and an attack from 1 to 5.
Put the health and attack in two `int` variables. Then:

- **if** your character is fair, call `AddCharacter` with your name and your numbers
- **else** call `AddCharacter` with your name, `5` and `2`

`AddCharacter` has six parameters. Read its signature in `dungeon/README.txt` before you
call it. Pass the three lists straight through, in the same order.

## 3. Check it

```
cd ~/version_control/apcsp-2026-2027-crawler-<your-username>/dungeon-tests
dotnet run IsFair
dotnet run
```

The last line must say **`0 failed`**, and no line that starts `TO DO Milestone 1` may be left.
(The `Milestone 3` lines stay until December. That is expected.) Then play it: `cd ~/version_control/apcsp-2026-2027-crawler-<your-username>/dungeon`, `dotnet run`,
and find your name in the party list.

## 4. Commit, push, open the pull request

From the **top of the repository**:

```
git status
git add dungeon/MyCharacter.cs CREDITS.md
git commit
```

`git commit` opens the message template. Your message goes on line 3, and it is exactly:

```
feat: Add my character
```

Then `git push`. On **github.com**, in this repository: **Pull requests** → **New pull request** →
base **`main`** ← compare **`crawler`** → **Create pull request**. Fill in the template. In the
right-hand sidebar click the gear beside **Reviewers** and choose `jd12`. **Do not merge it.**
It stays open until 12/1, and every push you make from now on appears in it.

### Spec self-check, to copy into the pull request

```
- [ ] IsFairCharacter returns true at a cost of 18 and false at 19
- [ ] MyCharacter uses if / else and calls IsFairCharacter
- [ ] My character has my own name, not Hero
- [ ] The self-check says 0 failed and has no TO DO Milestone 1 line
- [ ] My name is in CREDITS.md
```

## The three questions for today's log

1. What would go wrong, and would the compiler notice, if you called `AddCharacter` with the health and the attack swapped?
2. `IsFairCharacter` returns a `bool`. What did your code do with that returned value, and what would happen if you called the function and ignored the answer?
3. Where else in this game would a function that answers a yes or no question be useful?
