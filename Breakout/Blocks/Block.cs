using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;

namespace Breakout.Blocks {
    public class Block : Entity {

        protected int health = 1; 
        protected int value = 10;
        protected string filename;

        public Block(Vec2F pos, Vec2F extent, string filename) : base(
            new DynamicShape(pos, extent),
            new Image(Path.Combine("Assets", "Images", filename))
        ) { this.filename = filename; }

        /// <summary> If the block is hit the method decrements the health, deletes the
        /// block and add points</summary>  
        public virtual void GetHit() {
            if (--health == 0) {
                Status.GetStatus().AddPoints(value);
                base.DeleteEntity();
            }
        }
    }
}