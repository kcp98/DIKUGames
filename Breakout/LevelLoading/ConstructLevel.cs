using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;

namespace Breakout.LevelLoading {
    public class ConstructLevel {

        private LoadLevelData levelData;
        public EntityContainer<Blocks> theBlocks { get; }

        public ConstructLevel(string filename, string prefix = "") {
            levelData = new LoadLevelData(filename, prefix);
            theBlocks = new EntityContainer<Blocks>(levelData.mapWidth * levelData.mapHeight);
            PlaceBlocks();
        }

        public void Render() {
            theBlocks.RenderEntities();
        }
        
        private void PlaceBlocks() {
            float y = 1f;
            float x = 0f;
            // Set the border of the map to 0.2f y, so there is room for the player
            float dy = 0.8f / levelData.mapHeight;
            float dx = 1f   / levelData.mapWidth;

            foreach (string line in levelData.map) {
                foreach (char c in line) {
                    string block = c.ToString();
                    if (levelData.legend.ContainsKey(block)) {
                        IBaseImage image = new Image(
                            Path.Combine("Assets", "Images", levelData.legend[block])
                        );
                        Vec2F extent = new Vec2F(0.0833f, 0.04167f);
                        theBlocks.AddEntity(
                            new Blocks(new Vec2F(x, y), new Vec2F(dx, dy), image, 2)
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