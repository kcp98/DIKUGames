using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;
using DIKUArcade.Input;

namespace Breakout {
    public class Player : Entity, IGameEventProcessor {
        private bool moveLeft = false;
        private bool moveRight = false;
        const float MOVEMENT_SPEED = 0.01f;

        public Player(IBaseImage image) : base(
            new DynamicShape( new Vec2F(0.5f, 0.05f), new Vec2F(0.3f, 0.05f)),
            image
        ) {}
        
        public void Move() {
            if (moveLeft)  { base.Shape.Position.X -= MOVEMENT_SPEED; }
            if (moveRight) { base.Shape.Position.X += MOVEMENT_SPEED; }
            // with the players current extent the x coordinate of the player can't ecxeed 0.7
            // without moving outside the window.
            if (base.Shape.Position.X > 0.7f) { base.Shape.Position.X = 0.7f; }
            if (base.Shape.Position.X < 0.0f) { base.Shape.Position.X = 0.0f; }
            base.Shape.Move();
        }
        
        private void SetMoveRight(string action) {
            if (action == "RELEASE") {                
                moveRight = false;
            } else {
                moveRight = true;
            }
        }
        
        private void SetMoveLeft(string action) {
            if (action == "RELEASE") {
                moveLeft = false;
            } else {
                moveLeft = true;
            }
        }

        public void ProcessEvent(GameEvent gameEvent) {
            switch (gameEvent.Message) {
                case "LEFT":
                    SetMoveLeft(gameEvent.StringArg1);
                    break;
                case "RIGHT":
                    SetMoveRight(gameEvent.StringArg1);
                    break;
                default:
                    break;
            }
        }
    }
}