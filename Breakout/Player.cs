using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;
using DIKUArcade.Timers;
using System.IO;

namespace Breakout {
    public class Player : Entity, IGameEventProcessor {
        private bool moveLeft = false;
        private bool moveRight = false;
        private const float speed = 0.012f;
        private const float xtent = 0.15f;
        public float mutableXtent { get{return 0.15f;} set{;} }
        private float scalar = 1.5f;

        private double seconds = -1;
        public Player() : base(
            new DynamicShape(
                new Vec2F(-xtent / 2 + 0.5f, 0.05f),
                new Vec2F( xtent, 0.025f)
            ),
            new Image(Path.Combine("Assets", "Images", "player.png"))
        ) {}
        /// <summary> Checks collision with player. </summary>      
        public void Move() {
            if (moveLeft)
                base.Shape.Position.X = System.MathF.Max(
                    base.Shape.Position.X - speed, 0f
                );
            if (moveRight)
                base.Shape.Position.X = System.MathF.Min(
                    base.Shape.Position.X + speed, 1f - mutableXtent
                );
            base.Shape.Move();
            if (seconds != -1) {
                Unwiden();
            }
        }

        private void SetMoveRight(string action) {
            moveRight = "KeyRelease" !=  action;
        }
        
        private void SetMoveLeft(string action) {
            moveLeft = "KeyRelease" !=  action;
        }
        
        public void WidenPlayer() {
            if (seconds == -1) {
                this.Shape.ScaleXFromCenter(scalar);
                this.mutableXtent *= scalar;
                seconds = StaticTimer.GetElapsedSeconds() + 10.0;
            } else {
                seconds += 10;
            }
        }

        private void Unwiden() {
            if (StaticTimer.GetElapsedSeconds() > (seconds)) {
                this.Shape.ScaleXFromCenter(1/scalar);
                this.mutableXtent /= scalar;
                seconds = -1;
            }
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