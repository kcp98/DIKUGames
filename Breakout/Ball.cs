using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using System.IO;
using System;

namespace Breakout{

    public class Ball : Entity {
        
        private const float extent = 0.03f;
        private const float speed  = 0.01f;
        private EntityContainer<Entity> borders = new EntityContainer<Entity>();
        private bool released = false;

        public Ball() : base(
            new DynamicShape(
                new Vec2F(0, 0.075f),
                new Vec2F(extent, extent),
                new Vec2F(0, speed)
            ),
            new Image(Path.Combine("Assets", "Images", "ball.png"))
        ) { SetBorder(); }

        /// <summary> Sets the borders which the ball stays within. </summary>
        private void SetBorder() {
            borders.AddEntity( // LEFT BORDER
                new Entity(new DynamicShape(new Vec2F(0f, 0f), new Vec2F(0f, 1f)), null)
            );
            borders.AddEntity( // Add top border
                new Entity(new DynamicShape(new Vec2F(0f, 1f), new Vec2F(1f, 0f)), null)
            );
            borders.AddEntity( // RIGHT BORDER
                new Entity(new DynamicShape(new Vec2F(1f, 0f), new Vec2F(0f, 1f)), null)
            );
        }

        /// <summary> Calcuates the angle of the direction vector after collision,
        /// relative to the position and extent of the object of collision. </summary>
        /// <return> An angle in radians. </return>
        private float CalculateRelativeAngle(float hitPos, float objPos, float objExtent) {
            // Calculate how far along the object the hit occured, in percent.
            float hitObjExtentRatio = (hitPos - objPos + extent / 2) / objExtent;

            // A collision at the end of an object results in a 45 degree angle in that direction
            float radians = MathF.PI / 4 + hitObjExtentRatio * MathF.PI / 2;

            // Fixes perpendicular angles, prevents the ball getting stuck on a trajectory.
            if (radians == MathF.PI / 2)
                radians += 0.02f;
            return radians;
        }

        private void VerticalCollision(Entity that) {
            Vec2F direction = base.Shape.AsDynamicShape().Direction.Copy();
            float radians   = CalculateRelativeAngle(
                this.Shape.Position.Y, that.Shape.Position.Y, that.Shape.Extent.Y
            );
            direction.Y = - MathF.Cos(radians) * speed;
            direction.X =   MathF.Sin(radians) * speed;

            if (base.Shape.AsDynamicShape().Direction.X > 0f)
                direction.X *= -1f;

            base.Shape.AsDynamicShape().ChangeDirection(direction);
        }

        private void HorizontalCollision(Entity that) {
            Vec2F direction = base.Shape.AsDynamicShape().Direction.Copy();
            float radians   = CalculateRelativeAngle(
                this.Shape.Position.X, that.Shape.Position.X, that.Shape.Extent.X
            );
            direction.X = - MathF.Cos(radians) * speed;
            direction.Y =   MathF.Sin(radians) * speed;
            
            if (base.Shape.AsDynamicShape().Direction.Y > 0f)
                direction.Y *= -1f;

            base.Shape.AsDynamicShape().ChangeDirection(direction);
        }

        /// <summary> Changes the direction of the ball after a collision,
        /// using the Vertial- and HorizontalCollison methods. </summary>
        private void ChangeDirection(Entity entity, CollisionDirection collisionDirection) {
            switch  (collisionDirection) {
                case CollisionDirection.CollisionDirLeft:
                    VerticalCollision(entity);
                    break;
                case CollisionDirection.CollisionDirRight:
                    VerticalCollision(entity);
                    break;
                case CollisionDirection.CollisionDirUp:
                    HorizontalCollision(entity);
                    break;
                case CollisionDirection.CollisionDirDown:
                    HorizontalCollision(entity);
                    break;
                default:
                    break;
            }
        }

        /// <summary> Checks collision with entity. </summary>
        /// <return> Boolean indicating a collision. </return>
        public bool CheckCollision(Entity entity) {
            CollisionData data = CollisionDetection.Aabb(
                base.Shape.AsDynamicShape(), entity.Shape
            );
            if (data.Collision)
                ChangeDirection(entity, data.CollisionDir);

            return data.Collision;
        }
        
        public void Release() {
            released = true;
        }

        /// <summary> Moves the ball and checks collisions with border and player.
        /// If ball not yet released, ball follows the player around.
        /// Deletes ball if out of bounds. </summary>
        public void Move(Player player) {
            if (released) {
                foreach (Entity border in borders) {
                    CheckCollision(border);
                }
                CheckCollision(player);
                base.Shape.Move();

                if (base.Shape.Position.Y < 0)
                    base.DeleteEntity();
            } else {
                float x  = player.Shape.Position.X + player.Shape.Extent.X / 2f;
                float dx = x - base.Shape.Position.X - 0.015f;
                base.Shape.MoveX(dx);
            }
        }
    }
}