using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;
using System.IO;

namespace Breakout.Blocks {
    public class Hardened : Block {
        public Hardened(Vec2F pos, Vec2F extent, string filename) : base(pos, extent, filename) {
            base.health = 2;
            base.value *= 2;
        }
        private void ChangeImage(){
            string newfilename = this.filename.Insert(filename.IndexOf("."), "-damaged");
            this.Image = new Image(Path.Combine("Assets", "Images", newfilename));
        }
        public override void GetHit(){
            if (--health <= 0) {
                base.DeleteEntity();
                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.GameStateEvent,
                    Message   = "AddPoints",
                    IntArg1   = this.value
                });
            }
            else if (health == 1) {
                ChangeImage();
            }
        }
    }
}
