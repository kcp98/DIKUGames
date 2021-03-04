using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga {
    public class Player {
        private Entity entity;
        private DynamicShape shape;

        private bool moveLeft = false;
        private bool moveRight = false;
        const float MOVEMENT_SPEED = 0.01f;

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
            if (this.shape.Position.X > 0.9f) { this.shape.Position.X = 0.9f; }
            if (this.shape.Position.X < 0.0f) { this.shape.Position.X = 0.0f; }
            this.shape.Move();
        }

        public void SetMoveLeft(bool val) {
            moveLeft = val;
        }

        public void SetMoveRight(bool val) {
            moveRight = val;
        }
        
        public Vec2F GetPosition() {
            return this.shape.Position;
        }
    }
}