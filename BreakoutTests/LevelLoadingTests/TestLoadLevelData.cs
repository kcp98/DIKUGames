using NUnit.Framework;
using Breakout;
using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Math;
using System.Collections.Generic;
using Breakout.LevelLoading;

namespace BreakoutTests {
    [TestFixture]
    public class TestLoadLevelData {

        private LoadLevelData levelData;

        [SetUp]
        public void InitiateLoadLevelData() {
            
        }


        // R.1 - It can handle differences in the metadata.
        // I.e. not every file has the same metadata fields.


        // R.2 - The data read from the file is stored as
        // expected in data structures. I.e. create the data
        // structure as you would expect it to be and prove
        // that it is as such when read from the file.



        // R.3 - Empty or invalid files are handled without
        // crashing the program.
        [Test]
        public void TestErrorHandling() {
            Assert.Throws<System.IO.FileLoadException>(
                delegate { levelData = new LoadLevelData("invalid.txt", "../../../"); }
            );
        }



    }
}