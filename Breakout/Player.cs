using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;
using System.IO;

namespace Breakout {
    public class Player : Entity, IGameEventProcessor {
        private bool moveLeft = false;
        private bool moveRight = false;
        private const float speed = 0.012f;
        private const float xtent = 0.15f;

        public Player() : base(
            new DynamicShape(
                new Vec2F(-xtent / 2 + 0.5f, 0.05f),
                new Vec2F( xtent, 0.025f)
            ),
            new Image(Path.Combine("Assets", "Images", "player.png"))
        ) {}
        
        public void Move() {
            if (moveLeft)
                base.Shape.Position.X = System.MathF.Max(
                    base.Shape.Position.X - speed, 0f
                );
            if (moveRight)
                base.Shape.Position.X = System.MathF.Min(
                    base.Shape.Position.X + speed, 1f - xtent
                );
            base.Shape.Move();
        }
        
        private void SetMoveRight(string action) {
            if (action == "KeyRelease")          
                moveRight = false;
            else
                moveRight = true;
        }
        
        private void SetMoveLeft(string action) {
            if (action == "KeyRelease")
                moveLeft = false;
            else
                moveLeft = true;
        }

        public void ProcessEvent(GameEvent gameEvent) {
            switch (gameEvent.Message) {
                case "Left":
                    SetMoveLeft(gameEvent.StringArg1);
                    break;
                case "Right":
                    SetMoveRight(gameEvent.StringArg1);
                    break;
                default:
                    break;
            }
        }
    }
}