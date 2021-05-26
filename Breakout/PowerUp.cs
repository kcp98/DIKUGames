using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;
using DIKUArcade.Entities;
using System.Collections.Generic;
using DIKUArcade.Physics;
using Breakout.BreakoutStates;
using DIKUArcade.Events;

namespace Breakout {
    public class PowerUp : Entity {

        private static Vec2F extent = new Vec2F(0.03f,  0.03f);
        private static Vec2F dir    = new Vec2F(0.00f, -0.005f);

        private static List<string> images = new List<string>() {
            "LifePickUp.png",
        };

        private int index;
        
        /// TODO create gameEvents for the power ups instead maybe :)
        private void ActivatePower() {
            switch (index) {
                case 0:
                    Status.GetStatus().ExtraLife();
                    break;
                default:
                    System.Console.WriteLine("PowerUp for index {0}, not yet implemented", index);
                    break;
            }
        }

        public PowerUp(Vec2F pos, int rand) : base(
            new DynamicShape(pos, extent, dir),
            new Image(Path.Combine("Assets", "Images", images[rand]))
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
                ActivatePower();
                base.DeleteEntity();
            }
        }
    }
}
