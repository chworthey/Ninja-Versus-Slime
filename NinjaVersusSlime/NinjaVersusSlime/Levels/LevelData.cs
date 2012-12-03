using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NinjaVersusSlime.Levels
{
    static class LevelData
    {
        public static List<string> LevelText = new List<string>();

        public static void LoadLevels()
        {
            LevelText.Add(
    @"
T.................................T
T.................................T
T.................................T
T..............................x..T
T1...................e........TTTTT
T.......TTT...............TTTTTTTTT
TTTTTTTTTTTTTTTTTTTTTTTTT.TTTTTTTTT
...................................
...................................
");
            LevelText.Add(
    @"
...................................
T.................................T
T.................TTTT............T
T..1..........TTTwwwwT....e.......T
T........TTTTTTTTssssTTT........x.T
TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT
...................................
...................................
");

            LevelText.Add(
@"
...............................................
..............................................T
..............................................T
...................e..........................T
TT..........................................x.T
TT.1...........TTTTTTTT......e.............TTTT
TT............T.......T..................TTTTTT
TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT
...............................................
...............................................
");
        }
    }
}
