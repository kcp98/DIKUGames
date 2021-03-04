using DIKUArcade;
using DIKUArcade.Timers;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.Collections.Generic;
using DIKUArcade.EventBus;

namespace Galaga {
    // NOTE: We implement the IGameEventProcessor interface!
    public class Game : IGameEventProcessor<object> {
        private Window window;
        private GameTimer gameTimer;
        private Player player;
        private GameEventBus<object> eventBus;

        private EntityContainer<Enemy> enemies;

        /*private EntityContainer<PlayerShot> playerShots;
        private IBaseImage playerShotImage;*/

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
            /*playerShots = new EntityContainer<PlayerShot>();
            playerShotImage = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));*/
        }

        public void KeyPress(string key) {
            // TODO: switch on key string and set the player's move direction
            switch (key) {
                case "KEY_LEFT":
                    player.SetMoveLeft(true);
                    break;
                case "KEY_RIGHT":
                    player.SetMoveRight(true);
                    break;
                case "KEY_ESCAPE":
                    window.CloseWindow();
                    break;
                default:
                    break;
            }
        }
        public void KeyRelease(string key) {
            // TODO: switch on key string and disable the player's move direction
            // TODO: Close window if escape is pressed
            switch (key) {
                case "KEY_LEFT":
                    player.SetMoveLeft(false);
                    break;
                case "KEY_RIGHT":
                    player.SetMoveRight(false);
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

        /*private void IterateShot(){
            playerShots.Iterate(shot => {
                if(){

                }else{
                    enemies.Iterate(enemy =>{

                    });
                }
            });
        }*/


        public void Run() {
            while(window.IsRunning()) {
                gameTimer.MeasureTime();
                
                while (gameTimer.ShouldUpdate()) {
                    window.PollEvents();

                    // update game logic here...
                    eventBus.ProcessEvents();
                    player.Move();
                
                }
            
                if (gameTimer.ShouldRender()) {
                    window.Clear();

                    // render game entities here...
                    this.player.Render();

                    enemies.RenderEntities();

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