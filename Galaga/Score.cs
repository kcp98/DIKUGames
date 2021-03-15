using DIKUArcade.Math;
using DIKUArcade.Graphics;


namespace Galaga {
    public class Score {
        private int score;
        private Text display;

        public Score(Vec2F position, Vec2F extent) {
            score = 0;
            display = new Text(score.ToString(), position, extent);
            display.SetColor(new Vec3I(255, 255, 255));
            display.SetFontSize(100);
            display.SetFont("Phosphate");
        }

        public void AddPoint() {
            display.SetText(string.Format("{0}", ++score));
        }
        public void RenderScore() {
            display.RenderText();
        }
    }
}