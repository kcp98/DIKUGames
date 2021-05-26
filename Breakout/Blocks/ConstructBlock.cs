using DIKUArcade.Math;

namespace Breakout.Blocks {
    public static class ConstructBlock {

        public static Block CreateBlock(Vec2F pos, Vec2F extent, string filename, string special) {
            switch (special) {
                case "Unbreakable":
                    return new Unbreakable(pos, extent, filename);
                case "Hardened":
                    return new Hardened(pos, extent, filename);
                case "PowerUp":
                    return new PowerUpBlock(pos, extent, filename);
                default:
                    return new Block(pos, extent, filename);
            }
        }
    }
}