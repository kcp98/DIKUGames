using NUnit.Framework;
using System.Collections.Generic;
using Breakout.LevelLoading;
using System.IO;

namespace BreakoutTests {
    [TestFixture]
    public class TestLevelLoader {

        [Test] // R.1
        public void TestDifferentMetaData() {
            LevelLoader columns = new LevelLoader(
                Path.Combine( "../../../" + "Assets", "Levels", "columns.txt")
            );
            LevelLoader level1 = new LevelLoader(
                 Path.Combine( "../../../" + "Assets", "Levels", "level1.txt")
            );

            Assert.AreEqual(false, columns.meta.ContainsKey("Time"));
            Assert.AreEqual(true,   level1.meta.ContainsKey("Time"));
        }

        [Test] // R.2
        public void TestDataStructures() {
            LevelLoader wall = new LevelLoader(
                Path.Combine( "../../../" + "Assets", "Levels", "wall.txt")
            );
            List<string> expectedMap = new List<string>() {
                "------------", "------------", "------------", "------------", "------------",
                "------------", "#%#%#%#%#%#%", "#%#%#%#%#%#%", "#%#%#%#%#%#%", "#%#%#%#%#%#%",
                "#%#%#%#%#%#%", "#%#%#%#%#%#%", "#%#%#%#%#%#%", "#%#%#%#%#%#%", "#%#%#%#%#%#%",
                "#%#%#%#%#%#%", "#%#%#%#%#%#%", "#%#%#%#%#%#%", "------------", "------------",
                "------------", "------------", "------------", "------------",
            };
            Assert.AreEqual(expectedMap, wall.map);

            Dictionary<string, string> expectedMeta = new Dictionary<string, string>() {
                {"Name", "Wall"}
            };
            Assert.AreEqual(expectedMeta, wall.meta);

            Dictionary<string, string> expectedLegend = new Dictionary<string, string>() {
                {"#", "red-block.png"},
                {"%", "orange-block.png"}
            };
            Assert.AreEqual(expectedLegend, wall.legend);

            Assert.AreEqual(24, wall.mapHeight);
            Assert.AreEqual(12, wall.mapWidth);
        }

        [Test] // R.3
        public void TestErrorHandling() {
            Assert.Throws<System.IO.FileLoadException>(
                delegate { new LevelLoader(
                    Path.Combine( "../../../" + "Assets", "Levels", "invalid.txt")
                ); }
            );
        }
    }
}