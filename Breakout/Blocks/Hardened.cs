using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;

namespace Breakout.Blocks {
    public class Hardened : Block {

        public Hardened(Vec2F pos, Vec2F extent, string filename) : base(pos, extent, filename) {
            base.health = 2;
            base.value *= 2;
        }

        private void ChangeImage(){
            base.Image = new Image(Path.Combine(
                "Assets", "Images", base.filename.Insert(filename.IndexOf("."), "-damaged")
            ));
        }

        public override void GetHit() {
            if (--health == 0) {
                Status.GetStatus().AddPoints(value);
                base.DeleteEntity();
            } else {
                ChangeImage();
            }
        }
    }
}