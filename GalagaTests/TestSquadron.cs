using Galaga.GalagaStates;
using Galaga;
using NUnit.Framework;
using DIKUArcade.Events;
using System.Collections.Generic;
using Galaga.Squadron;
using DIKUArcade.Graphics;
using System.IO;

namespace GalagaTests {

    [TestFixture]
    public class SquadronTesting {

        private List<Image> images;
        private List<Image> imagesAlternatives;

        [SetUp]
        public void InitiateSquadrons() {
            DIKUArcade.GUI.Window.CreateOpenGLContext();
            images = ImageStride.CreateStrides(4, Path.Combine("Assets", "Images", "BlueMonster.png"));
            imagesAlternatives = ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "RedMonster.png"));
        }

        [Test]
        public void TestFirstMonsters() {
            ISquadron squadron = new FirstMonsters();

            Assert.AreEqual(8, squadron.MaxEnemies);

            Assert.AreNotEqual(squadron.MaxEnemies, squadron.Enemies.CountEntities());

            squadron.CreateEnemies(images, imagesAlternatives);

            Assert.AreEqual(squadron.MaxEnemies, squadron.Enemies.CountEntities());
        }

        [Test]
        public void TestSecondMonsters() {
            ISquadron squadron = new SecondMonsters();

            Assert.AreEqual(5, squadron.MaxEnemies);

            Assert.AreNotEqual(squadron.MaxEnemies, squadron.Enemies.CountEntities());

            squadron.CreateEnemies(images, imagesAlternatives);

            Assert.AreEqual(squadron.MaxEnemies, squadron.Enemies.CountEntities());   
        }

        [Test]
        public void TestThirdMonsters() {
            ISquadron squadron = new ThirdMonsters();

            Assert.AreEqual(9, squadron.MaxEnemies);

            Assert.AreNotEqual(squadron.MaxEnemies, squadron.Enemies.CountEntities());

            squadron.CreateEnemies(images, imagesAlternatives);

            Assert.AreEqual(squadron.MaxEnemies, squadron.Enemies.CountEntities());   
        }       
    }
}