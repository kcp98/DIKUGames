using DIKUArcade.State;
using DIKUArcade.Input;
using DIKUArcade.Events;
using System.Collections.Generic;
using DIKUArcade.Graphics;
using System.IO;
using Breakout.LevelLoading;
using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace Breakout.BreakoutStates {
    public class GameRunning : IGameState {

        public static GameRunning instance = null;

        private List<string> levels = new List<string>() {
            "wall.txt",
            "columns.txt",
            "central-mass.txt",
            "level1.txt",
            "level2.txt",
            "level3.txt"
        };
        private Entity background;
        private int currentLevel;
        private Player player;
        private ConstructLevel level;
        private Ball ball;
        private bool ballReleased;

        /// <summary> Get the GameRunning instance.
        /// If null then first instantiates the instance. </summary>
        public static GameRunning GetGameRunning() {
            return GameRunning.instance ?? (
                GameRunning.instance = new GameRunning()
            );
        }

        private GameRunning() {
            background = new Entity(
                new StationaryShape(new Vec2F(0f, 0f), new Vec2F(1f, 1f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );
        }

        /// <summary> Advance to next level or main menu. Resets player and ball. </summary>
        private void NextLevel() {
            if (++currentLevel == levels.Count) {
                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.GameStateEvent,
                    Message   = "MainMenu"
                });
            } else {
                try { level = new ConstructLevel(levels[currentLevel]); }
                catch (FileLoadException exception) {
                    System.Console.WriteLine(
                        "Skipped {0} level due to the exception: {1}",
                        levels[currentLevel],
                        exception.Message
                    );
                    NextLevel();
                }
                player = new Player();
                BreakoutBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);
                ball = new Ball();
                ballReleased = false;
            }
        }


        /// <summary> Resets the level.
        /// TODO reset the score when necessary. </summary>
        public void ResetState() {
            currentLevel = -1;
            NextLevel();
        }

        /// <summary> Moves entities and checks for collissions
        /// and level completetions. </summary>
        public void UpdateState() {
            if (level.IsFinished())
                NextLevel();

            if (!ballReleased) {
                player.Move();
                ball.Move(player);
                return;
            }
            player.Move();
            ball.Move();
            ball.CheckCollision(player);
            level.blocks.Iterate(block => {
                if (ball.CheckCollision(block))
                    block.GetHit("HIT");
            });
        }

        /// <summary> Render the background and entities. </summary>
        public void RenderState() {
            background.RenderEntity();
            player.RenderEntity();
            ball.RenderEntity();
            level.Render();
        }

        /// <summary> Handles key events for moving the player, and pausing the game. </summary>
        public void HandleKeyEvent(KeyboardAction action, KeyboardKey key) {
            switch (key) {
                case KeyboardKey.Escape:
                    BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                        EventType = GameEventType.GameStateEvent,
                        Message   = "GamePaused"
                    });
                    break;
                case KeyboardKey.Space:
                    ballReleased = true;
                    break;
                case KeyboardKey.N:
                    if (action == KeyboardAction.KeyPress)
                        NextLevel();
                    break;
                default:
                    BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                        EventType  = GameEventType.PlayerEvent,
                        StringArg1 = action.ToString(),
                        Message    = key.ToString()
                    });
                    break;
            }
        }
    }
}