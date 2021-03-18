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
using System;

namespace Galaga {
    // NOTE: We implement the IGameEventProcessor interface!
    public class Game : IGameEventProcessor<object> {
        private Window window;
        private GameTimer gameTimer;
        private Player player;
        private GameEventBus<object> eventBus;

        private ISquadron squadron;
        private Random rand;

        private EntityContainer<PlayerShot> playerShots;
        private IBaseImage playerShotImage;

        private AnimationContainer enemyExplosions;
        private const int EXPLOSION_LENGTH_MS = 500;

        private Score score;

        public Game() {
            window = new Window("Galaga", 500, 500);
            gameTimer = new GameTimer(30, 30);
            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png"))
            );
            eventBus = new GameEventBus<object>();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.InputEvent});

            window.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.InputEvent, player);

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
            squadron.CreateEnemies(images, enemyStridesRed);
        }

        public void ProcessEvent(GameEventType type, GameEvent<object> gameEvent) {
                switch (gameEvent.Message) {
                    case "KEY_SPACE":
                        var leftPosition = player.GetPosition();
                        var correcter = new Vec2F(0.05f, 0.0f);
                        var shotCenterPosition = leftPosition + correcter;
                        playerShots.AddEntity(new PlayerShot(shotCenterPosition, playerShotImage));
                        break; 
                    case "KEY_ESCAPE":
                        window.CloseWindow();
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
                                    down.IncreaseSpeed();
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


        // Instance of down to move enmies
        MovementStrategy.Down down = new MovementStrategy.Down();
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
                    squadron.Enemies.RenderEntities();
                    down.MoveEnemies(squadron.Enemies);
                    playerShots.RenderEntities();
                    enemyExplosions.RenderAnimations();
                    score.RenderScore();
                    window.SwapBuffers();

                    squadron.Enemies.Iterate(enemy => {
                        if(enemy.Shape.Position.Y <= 0){
                            squadron.Enemies.Iterate(enemy => {
                                enemy.DeleteEntity();
                            });
                            player.DeletePlayer();
                        }
                    });
                }
                
                if (gameTimer.ShouldReset()) {
                    // this update happens once every second
                    window.Title = $"Galaga | (UPS,FPS): ({gameTimer.CapturedUpdates},{gameTimer.CapturedFrames})";
                }
            }
        }
    }
}