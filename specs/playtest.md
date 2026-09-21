# Playtest swap

**Tuesday 11/24 · 50 minutes · 20 points · due at the end of class**

You play someone else's level **in your game**.

## Before you start

Your own work must be committed. `git status` must say **nothing to commit, working tree clean**.
If it does not, commit first.

## 1. Get your partner's level (5 minutes)

Your partner sends you **one file**: their `dungeon/MyLevel.cs`. AirDrop it. Save it over your own
`dungeon/MyLevel.cs`. If their level has its own look, they also send the files in their
`assets` folder that end in `4`.

## 2. The contract check (5 minutes)

```
cd dungeon-tests
dotnet run MyLevel
```

Paste the last line into `playtest/PLAYTEST.md`. **Do not fix their level.** If it fails, that is
what you report.

## 3. Play it (15 minutes)

```
cd ../dungeon-graphics
dotnet run -- 4
```

Play to the door. Then play it once more in the text game, where you can see the whole map.

## 4. Write the report (10 minutes)

Open `playtest/PLAYTEST.md` and answer every question. Say what happened and where, for example: "The ogre at the top right can
be skipped by going under the wall, so the loot behind it is free."

## 5. Put your own level back (2 minutes)

```
cd ~/version_control/apcsp-2026-2027-crawler-<your-username>
git restore dungeon/MyLevel.cs
git status
```

`git status` should show only `playtest/PLAYTEST.md` as changed. Delete any `4` files of theirs
from `assets` that you do not want to keep. Then:

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
3. If the class had invented a seventh tile halfway through, what would have happened today?
