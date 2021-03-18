using DIKUArcade.Entities;

namespace Galaga.MovementStrategy {
    public class Down : IMovementStrategy {
        
        const float MOVEMENT_SPEED = 0.0015f;
        private static int squadronRefreshes = 1;
        private static float increaseSpeed = 0.0005f;
        public void MoveEnemy(Enemy enemy) {
            if(enemy.EnemyEnraged){
                enemy.Shape.Position.Y -= ((MOVEMENT_SPEED * 3) + increaseSpeed);
            }else{
                enemy.Shape.Position.Y -= (MOVEMENT_SPEED + increaseSpeed);
            }
        }
        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }

        public void IncreaseSpeed(){
            squadronRefreshes++;
            increaseSpeed = increaseSpeed * squadronRefreshes;
        }
    }
}