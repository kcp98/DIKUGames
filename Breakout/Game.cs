using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Input;
using System.Collections.Generic;
using DIKUArcade.Graphics;
using System.IO;

namespace Breakout {
    public class Game : DIKUGame {
        private Player player;
        private GameEventBus eventBus;

        public Game(WindowArgs winArgs) : base(winArgs) {
            window.SetKeyEventHandler(KeyHandler);
            window.SetClearColor(0.1f, 0.1f, 0.8f);

            player = new Player(
                new Image(Path.Combine("Assets", "Images", "player.png"))
            );

            eventBus = new GameEventBus();
            eventBus.InitializeEventBus(
                new List<GameEventType> {
                    GameEventType.PlayerEvent
                }
            );

            eventBus.Subscribe(GameEventType.PlayerEvent, player);

        }

        private void KeyHandler(KeyboardAction action, KeyboardKey key) {
            var actionString = "PRESS";
            if (action == KeyboardAction.KeyRelease) {
                actionString = "RELEASE";
            }
            switch (key) {
                case KeyboardKey.Escape:
                    window.CloseWindow();
                    break;
                case KeyboardKey.Right:
                    eventBus.RegisterEvent(new GameEvent {
                        EventType  = GameEventType.PlayerEvent,
                        StringArg1 = actionString,
                        Message    = "RIGHT"
                    });
                    break;
                case KeyboardKey.Left:
                    eventBus.RegisterEvent(new GameEvent {
                        EventType  = GameEventType.PlayerEvent,
                        StringArg1 = actionString,
                        Message    = "LEFT"
                    });
                    break;
                default:
                    break;
            }
        }

        public override void Update() {
            eventBus.ProcessEvents();
            player.Move();
        }

        public override void Render() {
            player.RenderEntity();
        }
    }
}