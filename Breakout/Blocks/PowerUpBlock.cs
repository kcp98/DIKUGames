using DIKUArcade.Math;
using Breakout.BreakoutStates;
using System;


namespace Breakout.Blocks {
    public class PowerUpBlock : Block {

        public PowerUpBlock(Vec2F pos, Vec2F extent, string filename) : base(pos, extent, filename) {}

        public override void GetHit(){
            if (--health <= 0) {
                Status.GetStatus().AddPoints(value);

                Random rand = new Random();
                GameRunning.GetGameRunning().AddPowerUp(
                    new PowerUp(base.Shape.Position.Copy(), rand.Next(1))
                );
                base.DeleteEntity();
            }
        }
    }
}
