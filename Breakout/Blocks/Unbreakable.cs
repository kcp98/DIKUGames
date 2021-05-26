using DIKUArcade.Math;

namespace Breakout.Blocks {
    public class Unbreakable : Block {

        public Unbreakable(Vec2F pos, Vec2F extent, string filename) : base(
            pos, extent, filename
        ) {}

        public override void GetHit() {}
    }
}