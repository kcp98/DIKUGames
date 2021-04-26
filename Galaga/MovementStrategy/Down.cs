using DIKUArcade.Entities;

namespace Galaga.MovementStrategy {
    public class Down : IMovementStrategy {
        
        const float MOVEMENT_SPEED = 0.0015f;
        private static float extraSpeed = 0f;

        public void MoveEnemy(Enemy enemy) {
            var speed = MOVEMENT_SPEED + extraSpeed;
            if (enemy.enraged) { speed *= 3; }

            enemy.Shape.MoveY(-speed);
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }

        public static void IncreaseSpeed(){
            extraSpeed += 0.0005f;
        }

        public static void ResetExtraSpeed(){
            extraSpeed = 0f;
        }
    }
}