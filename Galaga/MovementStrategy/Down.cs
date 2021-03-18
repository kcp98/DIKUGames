using DIKUArcade.Entities;

namespace Galaga.MovementStrategy {
    public class Down : IMovementStrategy {
        
        const float MOVEMENT_SPEED = 0.0015f;
        public void MoveEnemy(Enemy enemy) {
            enemy.Shape.Position.Y -= MOVEMENT_SPEED;
        }
        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }
    }
}