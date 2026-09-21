// ============================================================
//  MyLevel.cs: THIS FILE IS YOURS.
//
//  The game calls MyLevel() for its final level. Obey the contracts and
//  your level drops into ANYBODY's game:
//    - every row the same length
//    - only these nine tiles:  # wall   . floor   @ start   M monster   $ loot   + door
//                              N someone to talk to   * a powerup   B the boss (one at most)
//      What the N says, and what the B is called, are in MyDialogue.cs.
//    - exactly one @, and at least one +
// ============================================================

List<string> MyLevel()
{
    List<string> level = new List<string>();
    level.Add("########");
    level.Add("#@.M.$+#");
    level.Add("########");
    return level;
}
