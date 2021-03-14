using DIKUArcade.Entities;
using System.Collections.Generic;
using DIKUArcade.Graphics;
using DIKUArcade.Math;


namespace Galaga.Squadron {
    public class MobMonsters : ISquadron {

        public EntityContainer<Enemy> Enemies { get; }

        public int MaxEnemies { get; } = 5;

        public MobMonsters() {
            Enemies = new EntityContainer<Enemy>(MaxEnemies);
        }

        public void CreateEnemies(List<Image> enemyStrides, List<Image> alternativeEnemyStrides) {
            for (int i = 0; i < MaxEnemies; i++) {
                Enemies.AddEntity(
                    new Enemy(
                        new DynamicShape(new Vec2F(0.1f + (float)i * 0.2f, 0.9f), new Vec2F(0.1f, 0.1f)),
                        new ImageStride(80, enemyStrides),
                        new ImageStride(80, alternativeEnemyStrides)
                    )
                );
            }
        }
    }
}