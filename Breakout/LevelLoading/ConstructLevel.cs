using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;

namespace Breakout.LevelLoading {
    public class ConstructLevel {

        private LoadLevelData levelData;
        public EntityContainer<Block> blocks { get; }

        public ConstructLevel(string filename) {
            levelData = new LoadLevelData(Path.Combine("Assets", "Levels", filename));
            blocks    = new EntityContainer<Block>(levelData.mapWidth * levelData.mapHeight);
            PlaceBlocks();
        }

        public void Render() {
            blocks.RenderEntities();
        }

        public bool IsFinished() {
            foreach (Entity entity in blocks)
                // TODO add isBreakable property to all the coming blocks
                if (entity is Block)
                    return false;
            return true;
        }
        
        /// <summary> Place the blocks in the formation given by the levelData's map.
        /// The maps extent in y is 1.0 -> 0.2 and x is 0 -> 1. </summary>
        private void PlaceBlocks() {
            float dx = 1f   / levelData.mapWidth;
            float dy = 0.8f / levelData.mapHeight;
            float x = 0f;
            float y = 1f - dy;

            foreach (string line in levelData.map) {
                foreach (char c in line) {
                    string block = c.ToString();
                    if (levelData.legend.ContainsKey(block)) {
                        IBaseImage image = new Image(
                            Path.Combine("Assets", "Images", levelData.legend[block])
                        );
                        blocks.AddEntity(
                            new Block(new Vec2F(x, y), new Vec2F(dx, dy), image, 2)
                        );
                    }
                    x += dx;
                }
                x  = 0f;
                y -= dy;
            }
        }
    }
}