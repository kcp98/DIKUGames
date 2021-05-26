using DIKUArcade.State;
using DIKUArcade.Input;
using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Timers;
using Breakout.LevelLoading;
using System.Collections.Generic;
using System.IO;

namespace Breakout.BreakoutStates {
    public class GameRunning : IGameState, IGameEventProcessor {

        public static GameRunning instance;

        private Entity background;

        private ConstructLevel level;
        private List<string> levels = new List<string>() {
            "level4.txt", "wall.txt",   "columns.txt", "central-mass.txt",
            "level1.txt", "level2.txt",  "level3.txt", "level4.txt"
        };
        private int currentLevel;

        private Player player;
        private EntityContainer<Ball> balls = new EntityContainer<Ball>();
        
        private EntityContainer<PowerUp> powerUps = new EntityContainer<PowerUp>();
        
        private Entity wall = new Entity(new DynamicShape(new Vec2F(0f, 0f), new Vec2F(1f, 0f)), null);
        private Entity wallOveraly;
        private double wallSeconds = -1;
        
        private double infiniteSeconds = -1;
        private bool infiniteOccupied = false;

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
            wallOveraly = new Entity(
                new StationaryShape(new Vec2F(0f, 0f), new Vec2F(1f, 1f)),
                new Image(Path.Combine("Assets", "Images", "Overlay.png"))
            );
            BreakoutBus.GetBus().Subscribe(GameEventType.TimedEvent, this);
        }

        #region PowerUps and IGameEventProcessor

        public void AddPowerUp(PowerUp powerUp) {
            powerUps.AddEntity(powerUp);
        }

        private void powerUpWall() {
            wallSeconds = StaticTimer.GetElapsedSeconds() + 10.0;
        }

        private void PowerUpWallCollision() {
            balls.Iterate(ball => {
                ball.CheckCollision(wall);
            });
            if (StaticTimer.GetElapsedSeconds() > wallSeconds) {
                wallSeconds = -1;
            }
        }

        private void PowerUpInfinite() {
            infiniteSeconds = StaticTimer.GetElapsedSeconds() + 10.0;
            AddInfiniteBalls();
        }

        private void AddInfiniteBalls() {
            if (!infiniteOccupied && infiniteSeconds != -1) {
                balls.AddEntity(new Ball());
                infiniteOccupied = true;
            }
            if (StaticTimer.GetElapsedSeconds() > infiniteSeconds) {
                infiniteSeconds = -1;
            }
        }

        public void ProcessEvent(GameEvent gameEvent) {
            switch (gameEvent.Message) {
                case "LifePickUp":
                    Status.GetStatus().ExtraLife();
                    break;
                case "ExtraBallPowerUp":
                    balls.AddEntity(new Ball());
                    break;
                case "WidePowerUp":
                    player.WidenPlayer();
                    break;
                case "WallPowerUp":
                    powerUpWall();
                    break;
                case "InfinitePowerUp":
                    PowerUpInfinite();
                    break;
            }
        }
        #endregion

        /// <summary> Advance to next level or main menu.
        /// Resets player, ball and static timer. </summary>
        private void NextLevel() {
            if (++currentLevel == levels.Count) {
                Status.GetStatus().EndGame();
                return;
            }
            try {
                level = new ConstructLevel(levels[currentLevel]);
            } catch (FileLoadException exception) {
                System.Console.WriteLine("Skipped level due to exception: {0}", exception.Message);
                NextLevel();
            }
            player = new Player();
            BreakoutBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);
            wallSeconds = -1;
            infiniteSeconds = -1;
            powerUps.ClearContainer();
            balls.ClearContainer();
            balls.AddEntity(new Ball());
            Status.GetStatus().SetTime(level.Timed, level.Time);
        }

        #region IGameState

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

            AddInfiniteBalls();
            player.Move();
            balls.Iterate(ball => {
                ball.Move(player);
                level.blocks.Iterate(block => {
                    if (ball.CheckCollision(block))
                        block.GetHit();
                    if (wallSeconds != -1) {
                        PowerUpWallCollision();
                    }
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

        /// <summary> Render the background, entities and status bar. </summary>
        public void RenderState() {
            background.RenderEntity();
            player.RenderEntity();
            balls.RenderEntities();
            level.Render();
            powerUps.RenderEntities();
            Status.GetStatus().Render();
            if (wallSeconds != -1)
                wallOveraly.RenderEntity();
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
                    infiniteOccupied = false;
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
        
        #endregion

        /// <summary> Overridden to use for window titles. </summary>
        public override string ToString() {
            return level.Title;
        }
    }
}