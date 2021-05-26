using DIKUArcade.Entities;
using DIKUArcade.Math;
using Breakout.Blocks;
using System.IO;
using System.Collections.Generic;

namespace Breakout.LevelLoading {
    public class ConstructLevel {

        private LoadLevelData levelData;
        public EntityContainer<Block> blocks { get; }
        public double Time { get; }
        public bool Timed { get; }
        public string Title { get; }

        public ConstructLevel(string filename) {
            levelData = new LoadLevelData(Path.Combine("Assets", "Levels", filename));
            blocks    = new EntityContainer<Block>(levelData.mapWidth * levelData.mapHeight);
            PlaceBlocks();
            
            Timed = levelData.meta.ContainsKey("Time");
            if (Timed) {
                try { 
                    Time = double.Parse(levelData.meta["Time"]);
                } catch (System.FormatException exception) {
                    System.Console.WriteLine("Time set to default: {0}", exception.Message);
                    Time = 100.0;
                }
            }
            Title = levelData.meta.GetValueOrDefault("Name");
        }

        public void Render() {
            blocks.RenderEntities();
        }

        public bool IsFinished() {
            foreach (Entity entity in blocks)
                if (!(entity is Unbreakable))
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
                        blocks.AddEntity(ConstructBlock.CreateBlock(
                            new Vec2F(x, y), new Vec2F(dx, dy),
                            levelData.legend[block],
                            levelData.meta.GetValueOrDefault(block)
                        ));
                    }
                    x += dx;
                }
                x  = 0f;
                y -= dy;
            }
        }
    }
}