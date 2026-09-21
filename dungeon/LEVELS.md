# The levels, drawn as text

Tile alphabet: `#` wall · `.` floor · `@` player start · `M` monster · `$` loot · `+` door · `N` someone to talk to · `*` powerup · `B` boss

Every row in a level is the same length. Counts below were produced from the code by
`tools/make_readme.py`, and `tools/check_levels.py` confirms the door, every `$`, `*`, `M` and `B`
can be reached from `@`, and that no `N` blocks the way.

## `MakeLevel1()`, 10 wide × 7 tall (Levels.cs)

```
##########
#@...#..+#
#.##.#.#.#
#.#..M.#.#
#.#.##.#.#
#..N#..$.#
##########
```

| `#` wall | `.` floor | `@` start | `M` monster | `$` loot | `+` door | `N` talk | `*` powerup | `B` boss | total |
|---|---|---|---|---|---|---|---|---|---|
| 42 | 23 | 1 | 1 | 1 | 1 | 1 | 0 | 0 | 70 |

## `MakeLevel2()`, 16 wide × 9 tall (Levels.cs)

```
################
#@..#.....#...$#
#.#.#.###.#.##.#
#.#..*#$..M....#
#.#####.####.#.#
#...M...#....#.#
#.#####.#.####.#
#N......#....M+#
################
```

| `#` wall | `.` floor | `@` start | `M` monster | `$` loot | `+` door | `N` talk | `*` powerup | `B` boss | total |
|---|---|---|---|---|---|---|---|---|---|
| 81 | 54 | 1 | 3 | 2 | 1 | 1 | 1 | 0 | 144 |

## `MakeLevel3()`, 20 wide × 11 tall (Levels.cs)

```
####################
#@.....#$..M.......#
#.####.#.#########.#
#.#$N#.#.#..*....#.#
#.#..M.#.#.#####.#.#
#.####.#.#.#+B.#.#.#
#......#.#.##M.#.#.#
#.######.#...$.#.#.#
#.#$..M..#######.#.#
#...#............M.#
####################
```

| `#` wall | `.` floor | `@` start | `M` monster | `$` loot | `+` door | `N` talk | `*` powerup | `B` boss | total |
|---|---|---|---|---|---|---|---|---|---|
| 121 | 85 | 1 | 5 | 4 | 1 | 1 | 1 | 1 | 220 |

## `MyLevel()`, 8 wide × 3 tall (MyLevel.cs)

```
########
#@.M.$+#
########
```

| `#` wall | `.` floor | `@` start | `M` monster | `$` loot | `+` door | `N` talk | `*` powerup | `B` boss | total |
|---|---|---|---|---|---|---|---|---|---|
| 18 | 2 | 1 | 1 | 1 | 1 | 0 | 0 | 0 | 24 |
