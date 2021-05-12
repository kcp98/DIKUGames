using NUnit.Framework;
using Breakout;
using DIKUArcade.Events;
using System;


namespace BreakoutTests {
    [TestFixture]
    public class ScoreTesting {

        Score score;
        
        [SetUp]
        public void InitiateScore() {
            score = new Score();
        }

        [Test]
        public void TestNonNegative() {
            Assert.Throws<ArgumentException>(
                delegate { score.ProcessEvent(new GameEvent {
                            EventType = GameEventType.GameStateEvent,
                            Message   = "AddPoints",
                            IntArg1   = -1
                        }); });
        }
    }
}