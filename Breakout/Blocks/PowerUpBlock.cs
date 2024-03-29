using DIKUArcade.Math;
using DIKUArcade.Events;
using System;

namespace Breakout.Blocks {
    public class PowerUpBlock : Block {

        public PowerUpBlock(Vec2F pos, Vec2F extent, string filename) : base(
            pos, extent, filename
        ) {}

        public override void GetHit() {
            if (--health == 0) {
                Random rand = new Random();
                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.TimedEvent,
                    Message   = "AddPowerUp",
                    ObjectArg1 = new PowerUp(base.Shape.Position.Copy(), rand.Next(5))
                });
                Status.GetStatus().AddPoints(value);
                base.DeleteEntity();
            }
        }
    }
}