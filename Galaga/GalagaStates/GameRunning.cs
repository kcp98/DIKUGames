using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using System.IO;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using Galaga.Squadron;
using Galaga.MovementStrategy;
using System;
using DIKUArcade.EventBus;

namespace Galaga.GalagaStates {
    public class GameRunning : IGameState {
        private Player player;
        private ISquadron squadron;
        private IMovementStrategy movement;
        private Random rand;
        private EntityContainer<PlayerShot> playerShots;
        private IBaseImage playerShotImage;
        private AnimationContainer enemyExplosions;
        private const int EXPLOSION_LENGTH_MS = 500;
        private Score score;
        private Entity background;
        private bool gameOver = false;
        private static GameRunning instance = null;

        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }
        public GameRunning () {
            InitializeGameState();
        }
        public void newGame() {
            GameRunning.instance = null;
        }
        public void GameLoop() {}


        public void InitializeGameState() {
            //The player
            player = new Player(
            new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
            new Image(Path.Combine("Assets", "Images", "Player.png")));

            GalagaBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);

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


        public void UpdateGameLogic() {
            player.Move();
            IterateShot();
        }


        public void RenderState() {
            background.RenderEntity();   
            if (!gameOver) {
                player.Render();
                squadron.Enemies.RenderEntities();
                playerShots.RenderEntities();
                enemyExplosions.RenderAnimations();
            }
            CheckGameEnded();
            score.RenderScore(gameOver);
            movement.MoveEnemies(squadron.Enemies);
        }


        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyAction) {
                case "KEY_PRESS":
                    KeyPress(keyValue);
                    break;
                case "KEY_RELEASE":
                    KeyRelease(keyValue);
                    break;
                default:
                    break;
            }
        }


        private void RefreshSquadron() {
            var images = ImageStride.CreateStrides(4, Path.Combine("Assets", "Images", "BlueMonster.png"));
            var enemyStridesRed = ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "RedMonster.png"));
            switch (rand.Next(3)) {
                case 0:
                    squadron = new FirstMonsters();
                    break;
                case 1:
                    squadron = new SecondMonsters();
                    break;
                case 2:
                    squadron = new ThirdMonsters();
                    break;
            }
            switch (rand.Next(3)) {
                case 0:
                    movement = new NoMove();
                    break;
                case 1:
                    movement = new Down();
                    break;
                case 2:
                    movement = new ZigZagDown();
                    break;
            }
            squadron.CreateEnemies(images, enemyStridesRed);
        }

        private void IterateShot() {
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
                                    Down.IncreaseSpeed();
                                    ZigZagDown.IncreaseSpeed();
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
                if (enemy.Shape.Position.Y <= 0f) {
                    gameOver = true;
                }
            });
        }
        public void KeyRelease(string key) {
             switch (key) {
                 case "KEY_ESCAPE":
                     GalagaBus.GetBus().RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors( 
                                GameEventType.GameStateEvent, this, "CHANGE_STATE", "GAME_PAUSED", ""));
                    break;
                 case "KEY_SPACE":
                    var shotPosition = new Vec2F(0.05f, 0f) + player.GetPosition();                  
                    playerShots.AddEntity(new PlayerShot(shotPosition, playerShotImage));
                    break;
                 default:
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, key, "KEY_RELEASE", ""));
                    break;
            }
        }

        public void KeyPress(string key) {
             switch (key) {
                default:
                    // If the game is over any key pressed will transfer to main menu
                    if (gameOver==true){ 
                        GalagaBus.GetBus().RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors( 
                                GameEventType.GameStateEvent, this, "CHANGE_STATE", "MAIN_MENU", ""));
                    }
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, key, "KEY_PRESS", ""));
                    break;
            }
        }

    }
}