using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Galaga {
    public class Enemy : Entity {

        private int hitpoints = 4;
        public StationaryShape startPos { get; }
        private IBaseImage enragedImage;
        public bool EnemyEnraged { get; private set; } = false ;
        public Enemy(DynamicShape shape, IBaseImage image, IBaseImage enraged)
            : base(shape, image) {
                enragedImage = enraged;
                startPos = new StationaryShape(shape.Position, shape.Extent);
        }
        
        /// <summary>
        /// Reduces hitpoints and returns a bool indicating whether or not enemy has died.
        /// </summary>
        public bool Hit(bool damage) {
            if (damage && --hitpoints <= 2) {
                this.Image = enragedImage;
                EnragedEnemy();
            }
            return hitpoints <= 0;
        }
        
        public void EnragedEnemy(){
            EnemyEnraged = true;
        }
    }
    
}