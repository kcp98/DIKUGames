using DIKUArcade;
using DIKUArcade.Timers;
using System.Collections.Generic;
using DIKUArcade.EventBus;
using Galaga.GalagaStates;


namespace Galaga {
    // NOTE: We implement the IGameEventProcessor interface!
    public class Game : IGameEventProcessor<object> {
        private Window window;
        private GameTimer gameTimer;    
        private StateMachine stateMachine;
        public Game() {
            window = new Window("Galaga", 500, 500);
            gameTimer = new GameTimer(30, 30);

            // Initializing the GalagaBus
            GalagaBus.GetBus().InitializeEventBus(new List<GameEventType> { GameEventType.InputEvent, 
                GameEventType.GameStateEvent, GameEventType.PlayerEvent});
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            window.RegisterEventBus(GalagaBus.GetBus());

            // Initializing the StateMachine
            stateMachine = new GalagaStates.StateMachine();
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, stateMachine);
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, stateMachine);
        }
        public void ProcessEvent(GameEventType type, GameEvent<object> gameEvent) {} 
        public void Run() {
            while(window.IsRunning()) {
                gameTimer.MeasureTime();
                
                while (gameTimer.ShouldUpdate()) {
                    window.PollEvents();
                    GalagaBus.GetBus().ProcessEvents();
                    stateMachine.ActiveState.UpdateGameLogic();
                }
            
                if (gameTimer.ShouldRender()) {
                    window.Clear();  
                    stateMachine.ActiveState.RenderState();   
                    window.SwapBuffers();
                }
                
                if (gameTimer.ShouldReset()) {
                    // this update happens once every second
                    window.Title = $"Galaga | (UPS,FPS): ({gameTimer.CapturedUpdates},{gameTimer.CapturedFrames})";
                }
            }
        }
    }
}