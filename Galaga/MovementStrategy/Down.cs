using DIKUArcade.Entities;

namespace Galaga.MovementStrategy {
    public class Down : IMovementStrategy {
        
        const float MOVEMENT_SPEED = 0.0015f;
        private int squadronRefreshes = 1;
        private float increaseSpeed = 0;
        public void MoveEnemy(Enemy enemy) {
            enemy.Shape.Position.Y -= MOVEMENT_SPEED + increaseSpeed;
        }
        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }

        public void IncreaseSpeed(){
            squadronRefreshes++;
            increaseSpeed = 0.0005f * squadronRefreshes;
        }
    }
}