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
            "wall.txt",   "columns.txt", "central-mass.txt",
            "level1.txt", "level2.txt",  "level3.txt", "level4.txt"
        };
        private Entity background;
        private int currentLevel;
        private Player player;
        private ConstructLevel level;
        private EntityContainer<Ball> balls = new EntityContainer<Ball>();
        private int lives = 3;
        private Text life = new Text("Lives: 3", new Vec2F(0.35f, -0.25f), new Vec2F(0.3f, 0.3f));
        private Text time = new Text("Time:  0", new Vec2F(0.70f, -0.25f), new Vec2F(0.3f, 0.3f));
        private Score score;

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
            life.SetColor(System.Drawing.Color.Wheat);
            time.SetColor(System.Drawing.Color.Wheat);
        }

        /// <summary> Renders the remaining time of the level if necessary. </summary>
        private void UpdateRenderTime() {
            if (!level.Timed) { return; }

            double remaining = level.Time - StaticTimer.GetElapsedSeconds();
    
            if (remaining < 0) {
                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.GameStateEvent,
                    Message   = "GameOver",
                    IntArg1   = score.points
                });
            }
            time.SetText(string.Format("Time: {0:0.0}", remaining));
            time.RenderText();
        }

        /// <summary> Advance to next level or main menu.
        /// Resets player, ball and static timer. </summary>
        private void NextLevel() {
            if (++currentLevel == levels.Count) {
                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.GameStateEvent,
                    Message = "GameOver",
                    IntArg1 = score.points
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
                balls.ClearContainer();
                balls.AddEntity(new Ball());
                if (StaticTimer.GetElapsedSeconds() > 0) { // Necessary for testing
                    StaticTimer.RestartTimer();
                    StaticTimer.PauseTimer();
                }
            }
        }


        /// <summary> Resets the level and score. </summary>
        public void ResetState() {
            currentLevel = -1;
            NextLevel();
            score = new Score();
            BreakoutBus.GetBus().Subscribe(GameEventType.GameStateEvent, score);
        }

        /// <summary> Moves entities and checks for collissions
        /// and level completetions. </summary>
        public void UpdateState() {
            if (level.IsFinished())
                NextLevel();

            player.Move();
            balls.Iterate(ball => {
                ball.Move(player);
                level.blocks.Iterate(block => {
                    if (ball.CheckCollision(block))
                        block.GetHit();
                });
            });
            
            if (balls.CountEntities() > 0) {
                return;
            } else if (--lives >= 0) {
                balls.AddEntity(new Ball());
                life.SetText(string.Format("Lives: {0}", lives));
            } else {
                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.GameStateEvent,
                    Message   = "GameOver",
                    IntArg1   = score.points
                });
            }
        }

        public override string ToString() {
            return level.Title;
        }

        /// <summary> Render the background and entities. </summary>
        public void RenderState() {
            background.RenderEntity();
            player.RenderEntity();
            balls.RenderEntities();
            level.Render();
            score.Render();
            life.RenderText();
            UpdateRenderTime();
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