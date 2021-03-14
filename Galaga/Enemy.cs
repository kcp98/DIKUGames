using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Galaga {
    public class Enemy : Entity {

        private int hitpoints = 4;

        private IBaseImage enragedImage;

        public Enemy(DynamicShape shape, IBaseImage image, IBaseImage enraged)
            : base(shape, image) {
                enragedImage = enraged;
        }
        
        /// <summary>
        /// Reduces hitpoints and returns a bool indicating whether or not enemy has died.
        /// </summary>
        public bool Hit(bool damage) {
            if (damage && --hitpoints <= 2) {
                this.Image = enragedImage;
            }
            return hitpoints <= 0;
        }
        
        
    }
    
}