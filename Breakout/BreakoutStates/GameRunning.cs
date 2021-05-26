using DIKUArcade.State;
using DIKUArcade.Input;
using DIKUArcade.Events;
using System.Collections.Generic;
using DIKUArcade.Graphics;
using System.IO;
using Breakout.LevelLoading;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Timers;

namespace Breakout.BreakoutStates {
    public class GameRunning : IGameState {

        public static GameRunning instance = null;

        private List<string> levels = new List<string>() {
            "level4.txt", "wall.txt",   "columns.txt", "central-mass.txt",
            "level1.txt", "level2.txt",  "level3.txt", "level4.txt"
        };
        private Entity background;
        private int currentLevel;
        private Player player;
        private ConstructLevel level;
        private EntityContainer<Ball> balls = new EntityContainer<Ball>();
        private EntityContainer<PowerUp> powerUps = new EntityContainer<PowerUp>();

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

        public void AddPowerUp(PowerUp powerUp) {
            powerUps.AddEntity(powerUp);
        }

        public void AddPowerUpBall() {
            balls.AddEntity(new Ball());
        }


        /// <summary> Advance to next level or main menu.
        /// Resets player, ball and static timer. </summary>
        private void NextLevel() {
            if (++currentLevel == levels.Count) {
                Status.GetStatus().EndGame();
                return;
            }
            try { level = new ConstructLevel(levels[currentLevel]); }
            catch (FileLoadException exception) {
                System.Console.WriteLine(
                    "Skipped {0} level due to the exception: {1}",
                    levels[currentLevel], exception.Message
                );
                NextLevel();
            }
            player = new Player();
            BreakoutBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);
            powerUps.ClearContainer();
            balls.ClearContainer();
            balls.AddEntity(new Ball());
            Status.GetStatus().SetTime(level.Timed, level.Time);
        }


        /// <summary> Resets the level and status bar. </summary>
        public void ResetState() {
            Status.GetStatus().Reset();
            currentLevel = -1;
            NextLevel();
        }

        /// <summary> Moves entities and checks for collissions
        /// and level completetions. Updates time in status bar. </summary>
        public void UpdateState() {
            if (level.IsFinished()) { NextLevel(); }
            Status.GetStatus().Update();

            player.Move();
            balls.Iterate(ball => {
                ball.Move(player);
                level.blocks.Iterate(block => {
                    if (ball.CheckCollision(block))
                        block.GetHit();
                });
            });
            powerUps.Iterate(powerUp => {
                powerUp.Move();
                powerUp.CheckCollision(player);
            });
            if (balls.CountEntities() == 0) {
                Status.GetStatus().GetLife();
                balls.AddEntity(new Ball());
            }
        }

        public override string ToString() {
            return level.Title;
        }

        /// <summary> Render the background, entities and status bar. </summary>
        public void RenderState() {
            background.RenderEntity();
            player.RenderEntity();
            balls.RenderEntities();
            level.Render();
            powerUps.RenderEntities();
            Status.GetStatus().Render();
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
                    StaticTimer.ResumeTimer();
                    balls.Iterate(ball => {
                        ball.Release();
                    });
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