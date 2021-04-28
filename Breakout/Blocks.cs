using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;
using DIKUArcade.Input;

namespace Breakout{

    public class Blocks : Entity {

        public int health { get; private set; }
        //private int value = 10;
        public bool isHit { get; private set;} = false;

        public Blocks(Vec2F pos, Vec2F extent, IBaseImage image, int blockHealthMultiple) : base(
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
