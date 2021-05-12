using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;

namespace Breakout {
    public class Block : Entity {

        public int health { get; private set; }
        //private int value = 10;
        public bool isHit { get; private set;} = false;

        public Block(Vec2F pos, Vec2F extent, IBaseImage image, int blockHealthMultiple) : base(
            new DynamicShape(pos, extent), image){
                if((blockHealthMultiple > 0) && (blockHealthMultiple <= 5)){
                    this.health = 1;
                }else {
                    this.health = 1;
                }
        }
    
        public void Damage(){
            if(isHit){
                --this.health;
                if(this.health <= 0){
                    base.DeleteEntity();
                }
                isHit = false;
            }
        }

        public void GetHit(string action){
            if (--health < 0) {
                base.DeleteEntity();
                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                            EventType = GameEventType.GameStateEvent,
                            Message   = "AddPoints",
                            IntArg1   = 1
                        });
            }
        }
    }
}
