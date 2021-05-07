using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;

namespace Breakout{

    public class Ball : Entity{
        
        private static Vec2F extent = new Vec2F(0.03f, 0.03f);
        private static Vec2F startDirection = new Vec2F(0.0f, 0.005f); 

        private Vec2F speed;
        private Vec2F direction;

        public Ball(Vec2F position, IBaseImage image) : 
            base(new DynamicShape(position, extent, startDirection), image){

        }

        public void Move(){
            base.Shape.Move(startDirection.X, startDirection.Y);
        }
    }
}