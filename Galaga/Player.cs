using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.EventBus;

namespace Galaga {
    public class Player : IGameEventProcessor<object> {
        private Entity entity;
        private DynamicShape shape;

        private bool moveLeft = false;
        private bool moveRight = false;
        const float MOVEMENT_SPEED = 0.01f;

        private bool moveUp = false;
        private bool moveDown = false;

        public Player(DynamicShape shape, IBaseImage image) {
            entity = new Entity(shape, image);
            this.shape = shape;
        }
        
        public void Render() {
            entity.RenderEntity();
        }

        public void Move() {
            if (moveLeft)  { this.shape.Position.X -= MOVEMENT_SPEED; }
            if (moveRight) { this.shape.Position.X += MOVEMENT_SPEED; }
            if (moveDown)  { this.shape.Position.Y -= MOVEMENT_SPEED; }
            if (moveUp) { this.shape.Position.Y += MOVEMENT_SPEED; }
            if (this.shape.Position.X > 0.9f) { this.shape.Position.X = 0.9f; }
            if (this.shape.Position.X < 0.0f) { this.shape.Position.X = 0.0f; }
            if (this.shape.Position.Y > 0.9f) { this.shape.Position.Y = 0.9f; }
            if (this.shape.Position.Y < 0.0f) { this.shape.Position.Y = 0.0f; }
            this.shape.Move();
        }

        public void SetMoveLeft(bool val) {
            moveLeft = val;
        }

        public void SetMoveRight(bool val) {
            moveRight = val;
        }

        public void SetMoveUp(bool val){
            moveUp = val;
        }
        
        public void SetMoveDown(bool val){
            moveDown = val;
        }

        public Vec2F GetPosition() {
            return this.shape.Position;
        }
                        public void KeyPress(string key) {
            switch (key) {
                case "KEY_LEFT":
                    this.SetMoveLeft(true);
                    break;
                case "KEY_RIGHT":
                    this.SetMoveRight(true);
                    break;
                case "KEY_UP":
                    this.SetMoveUp(true);
                    break;
                case "KEY_DOWN":
                    this.SetMoveDown(true);
                    break;
                default:
                    break;
            }
        }
        public void KeyRelease(string key) {
            switch (key) {
                case "KEY_LEFT":
                    this.SetMoveLeft(false);
                    break;
                case "KEY_RIGHT":
                    this.SetMoveRight(false);
                    break;
                case "KEY_UP":
                    this.SetMoveUp(false);
                    break;
                case "KEY_DOWN":
                    this.SetMoveDown(false);
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
    }
}