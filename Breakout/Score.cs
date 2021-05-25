using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;
using System.IO;
using System;

namespace Breakout {
    public class Score : IGameEventProcessor {
       
        public int points { get; private set; }
        private Text text;
        public Score() {
            points = 0;
            text = new Text("Score: 0", new Vec2F(0.0f, -0.25f), new Vec2F(0.3f, 0.3f));
            text.SetColor(System.Drawing.Color.Wheat);
        }

        public void ProcessEvent(GameEvent gameEvent) {
            switch (gameEvent.Message) {
                case "AddPoints":
                    if (gameEvent.IntArg1 < 0) {
                        throw new ArgumentException("Cannot subtract points");
                    }
                    points += gameEvent.IntArg1;
                    text.SetText(string.Format("Score: {0}", points));
                    break;
                default:
                    break;
            }
        }

        public void Render() {
            text.RenderText();
        }
    }
}