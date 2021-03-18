using DIKUArcade.Entities;
using System;
using DIKUArcade.Math;

namespace Galaga.MovementStrategy {
    public class ZigZagDown : IMovementStrategy {
        const float amplitude = 0.05f;
        const float period = 0.045f;
        const float MOVEMENT_SPEED = 0.0003f;
        public void MoveEnemy(Enemy enemy) {
                float x_0 = enemy.startPos.Position.X;
                float y_0 = enemy.startPos.Position.Y;
                float y_i = enemy.Shape.Position.Y - MOVEMENT_SPEED;
                float x_i = x_0 + amplitude * MathF.Sin((2*MathF.PI*(y_0-y_i)/period));
                enemy.Shape.SetPosition(new Vec2F(x_i,y_i));
        }
        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }
    }
}