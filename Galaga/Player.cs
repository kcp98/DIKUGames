using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga {
    public class Player {
        private Entity entity;
        private DynamicShape shape;

        private float moveLeft = 0.0f;
        private float moveRight = 0.0f;
        const float MOVEMENT_SPEED = 0.01f;
        
        public Player(DynamicShape shape, IBaseImage image) {
            entity = new Entity(shape, image);
            this.shape = shape;
        }
        
        public void Render() {
            entity.RenderEntity();
        }

        public void Move() {
            if (this.shape.Position.X > 0.9f) { this.shape.Position.X = 0.9f; }
            if (this.shape.Position.X < 0.0f) { this.shape.Position.X = 0.0f; }
            this.shape.Move();
        }

        public void SetMoveLeft(bool val) {
            if (val) { this.moveLeft -= MOVEMENT_SPEED; }
            UpdateDirection();
        }

        public void SetMoveRight(bool val) {
            if (val) { this.moveRight += MOVEMENT_SPEED; }
            UpdateDirection();
        }
        
        private void UpdateDirection() {
            this.shape.MoveX(moveRight + moveLeft);
            this.moveLeft = 0.0f;
            this.moveRight = 0.0f;
        }
        
        public Vec2F GetPosition() {
            return this.shape.Position;
        }
    }
}