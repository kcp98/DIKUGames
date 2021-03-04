using DIKUArcade;
using DIKUArcade.Timers;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.Collections.Generic;
using DIKUArcade.EventBus;
using DIKUArcade.Physics;

namespace Galaga {
    // NOTE: We implement the IGameEventProcessor interface!
    public class Game : IGameEventProcessor<object> {
        private Window window;
        private GameTimer gameTimer;
        private Player player;
        private GameEventBus<object> eventBus;

        private EntityContainer<Enemy> enemies;

        private EntityContainer<PlayerShot> playerShots;
        private IBaseImage playerShotImage;

        private AnimationContainer enemyExplosions;
        private List<Image> explosionStrides;
        private const int EXPLOSION_LENGTH_MS = 500;

        private Text pointsDisplay;
        private int pointsCounter = 0;

        public Game() {
            window = new Window("Galaga", 500, 500);
            gameTimer = new GameTimer(30, 30);
            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png"))
            );
            eventBus = new GameEventBus<object>();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.InputEvent });
            window.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, this);

            //Enemies
            var images = ImageStride.CreateStrides(4, Path.Combine("Assets", "Images", "BlueMonster.png"));
            const int numEnemies = 8;
            enemies = new EntityContainer<Enemy>(numEnemies);
            for (int i = 0; i < numEnemies; i++) {
                enemies.AddEntity(new Enemy(
                    new DynamicShape(new Vec2F(0.1f + (float)i * 0.1f, 0.9f), new Vec2F(0.1f, 0.1f)),
                        new ImageStride(80, images)));
            }

            //PlayerShot
            playerShots = new EntityContainer<PlayerShot>();
            playerShotImage = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));
        
            //Explosions
            enemyExplosions = new AnimationContainer(numEnemies);
            explosionStrides = ImageStride.CreateStrides(8,Path.Combine("Assets", "Images", "Explosion.png"));

            // Points display
            pointsDisplay = new Text("0 Points!", new Vec2F(0f, 0f), new Vec2F(0.2f, 0.2f));
            pointsDisplay.SetColor(new Vec3I(255, 255, 255));
        }

        public void KeyPress(string key) {
            switch (key) {
                case "KEY_LEFT":
                    player.SetMoveLeft(true);
                    break;
                case "KEY_RIGHT":
                    player.SetMoveRight(true);
                    break;
                default:
                    break;
            }
        }
        
        public void KeyRelease(string key) {
            switch (key) {
                case "KEY_LEFT":
                    player.SetMoveLeft(false);
                    break;
                case "KEY_RIGHT":
                    player.SetMoveRight(false);
                    break;
                case "KEY_ESCAPE":
                    window.CloseWindow();
                    break;
                case "KEY_SPACE":
                    playerShots.AddEntity(new PlayerShot(player.GetPosition(), playerShotImage));
                    break;
                default:
                    break;
            }
        }

        public void ProcessEvent(GameEventType type, GameEvent<object> gameEvent) {
            switch (gameEvent.Parameter1) {
                case "KEY_PRESS":
                    KeyPress(gameEvent.Message);
                    break;
                case "KEY_RELEASE":
                    KeyRelease(gameEvent.Message);
                    break;
                default:
                    break;
            }
        } 

        private void IterateShot(){
            playerShots.Iterate(shot => {
                shot.Move();
                if (shot.Shape.Position.Y >= 1f) {
                    shot.DeleteEntity();
                } else {
                    enemies.Iterate(enemy => {
                        CollisionData data = CollisionDetection.Aabb(
                            shot.Shape.AsDynamicShape(), enemy.Shape
                        );
                        if (data.Collision) {
                            AddExplosion(enemy.Shape.Position, enemy.Shape.Extent);
                            shot.DeleteEntity();
                            enemy.DeleteEntity();
                            pointsDisplay.SetText(string.Format("{0} Points!", ++pointsCounter));
                        }
                    });
                }
            });
        }

        public void AddExplosion(Vec2F position, Vec2F extent) {
            ImageStride explosion = new ImageStride(EXPLOSION_LENGTH_MS / 8,
                ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png"))
            );
            StationaryShape explosionPosition = new StationaryShape(position, extent);
            enemyExplosions.AddAnimation(explosionPosition, EXPLOSION_LENGTH_MS, explosion);
        }


        public void Run() {
            while(window.IsRunning()) {
                gameTimer.MeasureTime();
                
                while (gameTimer.ShouldUpdate()) {
                    window.PollEvents();
                    eventBus.ProcessEvents();
                    player.Move();
                    IterateShot();
                }
            
                if (gameTimer.ShouldRender()) {
                    window.Clear();
                    player.Render();
                    enemies.RenderEntities();
                    playerShots.RenderEntities();
                    enemyExplosions.RenderAnimations();
                    pointsDisplay.RenderText();
                    window.SwapBuffers();
                }
                
                if (gameTimer.ShouldReset()) {
                    // this update happens once every second
                    window.Title = $"Galaga | (UPS,FPS): ({gameTimer.CapturedUpdates},{gameTimer.CapturedFrames})";
                }
            }
        }
    }
}