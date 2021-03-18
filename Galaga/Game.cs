using DIKUArcade;
using DIKUArcade.Timers;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.Collections.Generic;
using DIKUArcade.EventBus;
using DIKUArcade.Physics;
using Galaga.Squadron;
using Galaga.MovementStrategy;
using System;

namespace Galaga {
    // NOTE: We implement the IGameEventProcessor interface!
    public class Game : IGameEventProcessor<object> {
        private Window window;
        private GameTimer gameTimer;
        private Player player;
        private GameEventBus<object> eventBus;

        private ISquadron squadron;
        private IMovementStrategy movement;
        private Random rand;

        private EntityContainer<PlayerShot> playerShots;
        private IBaseImage playerShotImage;

        private AnimationContainer enemyExplosions;
        private const int EXPLOSION_LENGTH_MS = 500;

        private Score score;

        private Entity background;
        private bool GameOver = false;

        public Game() {
            window = new Window("Galaga", 500, 500);
            gameTimer = new GameTimer(30, 30);
            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png"))
            );
            eventBus = new GameEventBus<object>();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.InputEvent, GameEventType.PlayerEvent});

            window.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.PlayerEvent, player);

            // Squadrons of enemies
            rand = new Random();
            RefreshSquadron();

            //PlayerShot
            playerShots = new EntityContainer<PlayerShot>();
            playerShotImage = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));
        
            //Explosions
            enemyExplosions = new AnimationContainer(8);

            // Score
            score = new Score(new Vec2F(0.1f, 0.1f), new Vec2F(0.4f, 0.4f));

            // Background
            StationaryShape bgShape = new StationaryShape(new Vec2F(0f, 0f), new Vec2F(1f, 1f));
            IBaseImage bgImage = new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"));
            background = new Entity(bgShape, bgImage);
            
        }

        private void RefreshSquadron() {
            var images = ImageStride.CreateStrides(4, Path.Combine("Assets", "Images", "BlueMonster.png"));
            var enemyStridesRed = ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "RedMonster.png"));
            switch (rand.Next(3)) {
                case 0:
                    squadron = new FirstMonsters(); break;
                case 1:
                    squadron = new SecondMonsters(); break;
                case 2:
                    squadron = new ThirdMonsters(); break;
            }
            switch (rand.Next(3)) {
                case 0:
                    movement = new NoMove(); break;
                case 1:
                    movement = new Down(); break;
                case 2:
                    movement = new ZigZagDown(); break;
            }
            squadron.CreateEnemies(images, enemyStridesRed);
        }

        public void KeyRelease(string key) {
             switch (key) {
                 case "KEY_ESCAPE":
                    window.CloseWindow();
                    break;
                 case "KEY_SPACE":
                    var leftPosition = player.GetPosition();
                    var correcter = new Vec2F(0.05f, 0.0f);
                    var shotCenterPosition = leftPosition + correcter;
                    playerShots.AddEntity(new PlayerShot(shotCenterPosition, playerShotImage));
                    break;
                 default:
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, key, "KEY_RELEASE", ""));
                    break;
             }
         }

        public void ProcessEvent(GameEventType type, GameEvent<object> gameEvent) {
            switch (gameEvent.Parameter1) {
                case "KEY_PRESS":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, gameEvent.Message, "KEY_PRESS", ""));
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
                    squadron.Enemies.Iterate(enemy => {
                        CollisionData data = CollisionDetection.Aabb(
                            shot.Shape.AsDynamicShape(), enemy.Shape
                        );
                        if (data.Collision) {
                            shot.DeleteEntity();
                            if (enemy.Hit(true)) {
                                AddExplosion(enemy.Shape.Position, enemy.Shape.Extent);
                                enemy.DeleteEntity();
                                score.AddPoint();
                                if (squadron.Enemies.CountEntities() <= 1) {
                                    // movement.IncreaseSpeed();
                                    RefreshSquadron();
                                }
                            }
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

        public void CheckGameEnded(){
            squadron.Enemies.Iterate(enemy => {
                if(enemy.Shape.Position.Y <= 0.0f){
                    GameOver = true;
                    eventBus.Unsubscribe(GameEventType.PlayerEvent, player);
                    player.DeletePlayer();
                    }
            });
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
                    background.RenderEntity();   
                    if(!GameOver){
                        player.Render();
                        squadron.Enemies.RenderEntities();
                        movement.MoveEnemies(squadron.Enemies);
                        playerShots.RenderEntities();
                        enemyExplosions.RenderAnimations();
                    }

                    CheckGameEnded();
                    
                    score.RenderScore(GameOver);
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