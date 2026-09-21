// A level with no list: one string variable for each row.

string row0 = "#######";
string row1 = "#@.$.M#";
string row2 = "#.$.$.#";
string row3 = "#######";

int loot = 0;

for (int x = 0; x < row0.Length; x++)
{
    if (row0[x] == '$') loot = loot + 1;
}
for (int x = 0; x < row1.Length; x++)
{
    if (row1[x] == '$') loot = loot + 1;
}
for (int x = 0; x < row2.Length; x++)
{
    if (row2[x] == '$') loot = loot + 1;
}
for (int x = 0; x < row3.Length; x++)
{
    if (row3[x] == '$') loot = loot + 1;
}

Console.WriteLine("Loot: " + loot);
