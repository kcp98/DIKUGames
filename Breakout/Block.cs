using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Breakout {
    public class Block : Entity {

        public int health { get; private set; }
        //private int value = 10;
        public bool isHit { get; private set;} = false;

        public Block(Vec2F pos, Vec2F extent, IBaseImage image, int blockHealthMultiple) : base(
            new DynamicShape(pos, extent), image){
                if((blockHealthMultiple > 0) && (blockHealthMultiple <= 5)){
                    this.health = 5 * blockHealthMultiple;
                }else {
                    this.health = 5;
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
            if(action == "HIT"){
                isHit = true;
            }else {
                isHit = false;
            }
        }
    }
}
