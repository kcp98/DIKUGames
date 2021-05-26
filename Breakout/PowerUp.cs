using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.Physics;
using DIKUArcade.Events;
using System.Collections.Generic;
using System.IO;

namespace Breakout {
    public class PowerUp : Entity {

        private static Vec2F extent = new Vec2F(0.03f,  0.03f);
        private static Vec2F dir    = new Vec2F(0.00f, -0.005f);

        private static List<string> images = new List<string>() {
            "LifePickUp",
            "ExtraBallPowerUp",
            "WidePowerUp", 
            "WallPowerUp",
            "InfinitePowerUp"
        };

        private int index;

        public PowerUp(Vec2F pos, int rand) : base(
            new DynamicShape(pos, extent, dir),
            new Image(Path.Combine("Assets", "Images", images[rand] + ".png"))
        ) { index = rand; }

        public void Move() {
            base.Shape.Move();
            if (base.Shape.Position.Y < 0)
                base.DeleteEntity();
        }

        /// <summary> Checks collision with player. </summary>        
        public void CheckCollision(Entity entity) {
            CollisionData data = CollisionDetection.Aabb(
                base.Shape.AsDynamicShape(), entity.Shape
            );
            if (data.Collision) {
                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.TimedEvent,
                    Message   = images[index]
                });
                base.DeleteEntity();
            }
        }
    }
}