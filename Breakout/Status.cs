using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;
using DIKUArcade.Timers;

namespace Breakout {
    public class Status {

        private static Status instance;

        public int points { get; private set;}
        private int lives;
        private bool timed = false;
        private double time;

        private Text pointsText = new Text("Score: 0", new Vec2F(0.00f, -0.25f), new Vec2F(0.3f, 0.3f));
        private Text livesText  = new Text("Lives: 3", new Vec2F(0.35f, -0.25f), new Vec2F(0.3f, 0.3f));
        private Text timeText   = new Text("Time: 0",  new Vec2F(0.70f, -0.25f), new Vec2F(0.3f, 0.3f));

        private Status() {
            pointsText.SetColor(System.Drawing.Color.Wheat);
            livesText.SetColor(System.Drawing.Color.Wheat);
            timeText.SetColor(System.Drawing.Color.Wheat);
        }

        /// <summary> Get the status bar instance. 
        /// Holds current lives, score and updates remaining time. </summary>
        public static Status GetStatus() {
            return Status.instance ?? (
                Status.instance = new Status()
            );
        }

        public void EndGame() {
            BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                EventType = GameEventType.GameStateEvent,
                Message   = "GameOver",
            });
        }

        public void GetLife() {
            if (--lives < 0) { EndGame(); }
            else { livesText.SetText(string.Format("Lives: {0}", lives)); }
        }

        public void SetTime(bool includeTime, double timeLimit) {
            timed = includeTime; 
            time = timeLimit;
            StaticTimer.RestartTimer();
            StaticTimer.PauseTimer();
        }

        public void Update() {
            if (!timed) { return; }
            double remaining = time - StaticTimer.GetElapsedSeconds();
            if (remaining < 0) { EndGame(); }
            timeText.SetText(string.Format("Time: {0:0.0}", remaining));
            timeText.RenderText();
        }
        
        public void AddPoints(int extra) {
            if (extra > 0) {points += extra;}
            pointsText.SetText(string.Format("Score: {0}", points));
        }

        public void Reset() {
            this.points = 0;
            this.lives  = 3;
            this.timed  = false;
            pointsText.SetText("Score: 0");
            livesText.SetText("Lives: 3");
        }

        public void Render() {
            pointsText.RenderText();
            livesText.RenderText();
            if (timed) { timeText.RenderText(); }
        }

        #region PowerUps
        
        public void ExtraLife() {
            livesText.SetText(string.Format("Lives: {0}", ++lives));
        }

        #endregion
    }
}