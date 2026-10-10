# Playtest swap

**Tuesday 12/1 · 50 minutes · 20 points · due at the end of class**

You play someone else's level **in your game**.

## Before you start

Your own work must be committed. `git status` must say **nothing to commit, working tree clean**.
If it does not, commit first.

## 1. Get your partner's level (5 minutes)

Your partner sends you **one file**: their `dungeon/MyLevel.cs`. AirDrop it. Save it over your own
`dungeon/MyLevel.cs`. If their level has its own look, they also send the files in their
`assets` folder that end in `4`. No AirDrop? Swap seats and play on their laptop.

## 2. The contract check (5 minutes)

```
cd ~/version_control/apcsp-2026-2027-crawler-<your-username>/dungeon-tests
dotnet run MyLevel
```

Paste the last line into `playtest/PLAYTEST.md`. **Do not fix their level.** If it fails, that is
what you report.

## 3. Play it (15 minutes)

```
cd ~/version_control/apcsp-2026-2027-crawler-<your-username>/dungeon-graphics
dotnet run -- 4
```

Play to the door. Then play it once more in the text game, where you can see the whole map. In
`dungeon/Program.cs`, change `int levelNumber = 1;` to `int levelNumber = 4;`. Setting it to 4 also
skips your own room from Quest 14. Then:

```
cd ~/version_control/apcsp-2026-2027-crawler-<your-username>/dungeon
dotnet run
```

## 4. Write the report (10 minutes)

Open `playtest/PLAYTEST.md` and answer every question. Say what happened and where, for example: "The ogre at the top right can
be skipped by going under the wall, so the loot behind it is free."

## 5. Put your own level back (2 minutes)

```
cd ~/version_control/apcsp-2026-2027-crawler-<your-username>
git restore dungeon/MyLevel.cs dungeon/Program.cs assets
git status
```

`git restore` puts back your level, your `levelNumber = 1;` and your own art. If `git status` lists
any of their files under **Untracked files**, delete each one, in Finder or with `rm` and the path it
shows. Run `git status` again. It should show only `playtest/PLAYTEST.md` as changed. Then:

```
git add playtest/PLAYTEST.md
git commit
git push
```

Message: `docs: Add playtest report`. Tell your partner what you found. They have until
tomorrow's class to act on it.

## The three questions for today's log

1. Did your partner's level load in your game without any change to your code, and what made that possible?
2. What did you notice about your own level only after watching someone else's?
3. If the class had invented a tenth tile halfway through, what would have happened today?
