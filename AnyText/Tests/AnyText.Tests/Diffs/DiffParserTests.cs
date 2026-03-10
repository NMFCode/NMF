using NMF.AnyText;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnyText.Tests.Diffs
{
    [TestFixture]
    public class DiffParserTests
    {
        [Test]
        public void ParsesDiff_Correctly()
        {
            var lines = File.ReadAllLines("TestDocuments/busybox_09.diff");
            var diff = DiffParser.ToTextEdits(lines);

            var expected = new List<(int start, int removed, int added)>
            {
                (5,2,8),
                (15,0,13),
                (29,1,9),
                (40,2,0),
                (41,19,0),
                (42,7,22),
                (65,3,23),
                (89,5,1),
                (91,10,15),
                (107,12,3),
                (112,19,4),
                (117,6,17),
                (135,5,4),
                (141,3,16),
                (158,33,8),
                (168,11,28),
                (197,2,28),
                (227,4,18),
                (246,1,3),
                (250,5,31),
                (283,1,19),
                (303,2,5),
                (310,22,4),
                (315,0,1),
                (317,10,1),
                (319,7,5),
                (325,1,5),
                (332,17,0),
                (333,13,86),
                (420,5,6),
                (427,0,16),
                (444,20,3),
                (448,0,1),
                (451,18,7),
                (460,23,1),
                (462,7,3),
                (466,13,1),
                (468,22,11),
                (480,3,0),
                (481,25,2),
                (484,59,5),
                (490,3,0),
                (491,7,0),
                (493,22,0),
                (494,54,7),
                (502,35,8),
                (511,14,7),
                (519,3,2),
                (522,23,9),
                (532,11,6),
                (539,8,7),
                (547,5,3),
                (551,1,14),
                (566,2,2),
                (569,6,5),
                (575,16,62),
                (638,4,9),
                (648,2,4),
                (653,19,4),
                (658,8,3),
                (662,3,46),
                (709,12,23),
                (733,17,9),
                (743,7,4),
                (748,2,1),
                (750,0,24),
                (775,18,13),
                (789,14,6),
                (796,6,1),
                (798,29,0),
                (799,39,2),
                (802,2,40),
                (843,3,1),
                (845,15,16),
                (862,2,24),
                (887,10,14),
                (902,0,11),
                (914,4,5),
                (920,7,2),
                (923,13,3),
                (927,5,13),
                (941,10,20),
                (962,9,37),
                (1000,4,12),
                (1013,10,1)
            };

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.That(diff[i].Start, Is.EqualTo(new ParsePosition(expected[i].start, 0)));
                Assert.That(diff[i].End, Is.EqualTo(new ParsePosition(expected[i].start + expected[i].removed, 0)));
                Assert.That(diff[i].NewText.Length, Is.EqualTo(expected[i].added + 1));
            }
        }
    }
}
