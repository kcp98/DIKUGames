using NUnit.Framework;
using Breakout;
using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Math;
using System.Collections.Generic;

namespace BreakoutTests {
    [TestFixture]
    public class PlayerTesting {
        private Player player;
        private GameEventBus eventBus;

        [SetUp]
        public void InitiatePlayer() {
            player   = new Player(null);
            eventBus = new GameEventBus();
        }
    }
}