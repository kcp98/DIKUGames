using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Galaga {
    public class Enemy : Entity {

        private int hitpoints = 3;
        private IBaseImage enragedImage;
        public StationaryShape startPos { get; }
        
        public bool enraged { get; private set; } = false;

        public Enemy(DynamicShape shape, IBaseImage image, IBaseImage enraged)
            : base(shape, image) {
                enragedImage = enraged;
                startPos = new StationaryShape(shape.Position, shape.Extent);
        }
        
        /// <summary>
        /// Reduces hitpoints and returns a bool indicating whether or not enemy has died.
        /// </summary>
        public bool Hit(bool damage) {
            if (damage && --hitpoints < 2) {
                this.Image = enragedImage;
                enraged = true;
            }
            return hitpoints <= 0;
        }
    }
}