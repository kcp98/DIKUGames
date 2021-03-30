using NUnit.Framework;
using Galaga.GalagaStates;

namespace GalagaTests {

    [TestFixture]
    public class GameStateTypeTesting {

        [SetUp]
        public void init() {
        
        }

        [TestCase("GAME_RUNNING", GameStateType.GameRunning)]
        [TestCase("GAME_PAUSED", GameStateType.GamePaused)]
        [TestCase("MAIN_MENU", GameStateType.MainMenu)]
        // Testing TransformStateToString
        public void TransformStateToStringTest(string state, GameStateType gameStateType) {
            Assert.AreEqual(StateTransformer.TransformStringToState(state), gameStateType);
        }
        
        [TestCase(GameStateType.GameRunning, "GAME_RUNNING")]
        [TestCase(GameStateType.GamePaused, "GAME_PAUSED")]
        [TestCase(GameStateType.MainMenu, "MAIN_MENU")]
        // Testing TransformStringToState
        public void TransformStringToStateTest(GameStateType gameStateType, string state) {
            Assert.AreEqual(StateTransformer.TransformStateToString(gameStateType), state);
        }   
    }
}