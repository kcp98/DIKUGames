using DIKUArcade.State;
using DIKUArcade.Input;
using DIKUArcade.Events;
using System.Collections.Generic;
using DIKUArcade.Graphics;
using System.IO;
using Breakout.LevelLoading;
using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace Breakout.BreakoutStates {
    public class GameRunning : IGameState {

        public static GameRunning instance = null;

        private List<string> levels = new List<string>() {
            "wall.txt",
            "columns.txt",
            "central-mass.txt",
            "level1.txt",
            "level2.txt",
            "level3.txt"
        };
        private Entity background;

        private int currentLevel = 0;

        private Player player;
        private ConstructLevel level;

        /// <summary> Get the GameRunning instance.
        /// If null then first instantiates the instance. </summary>
        public static GameRunning GetGameRunning() {
            return GameRunning.instance ?? (
                GameRunning.instance = new GameRunning()
            );
        }

        private GameRunning() {
            background = new Entity(
                new StationaryShape(new Vec2F(0f, 0f), new Vec2F(1f, 1f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );
            player = new Player(
                new Image(Path.Combine("Assets", "Images", "player.png"))
            );
            BreakoutBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);

            level = new ConstructLevel(levels[currentLevel]);
        }

        /// <summary> Advance to the next level, or the main menu.
        /// TODO add check for finished levels, when blocks can be destroyed. </summary>
        private void UpdateLevel() {
            if (++currentLevel == levels.Count)
                BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                    EventType  = GameEventType.GameStateEvent,
                    Message    = "MainMenu"
                });
            else
                level = new ConstructLevel(levels[currentLevel]);
        }

        /// <summary> Resets the level.
        /// TODO reset player and ball position, and the score when necessary. </summary>
        public void ResetState() {
            currentLevel = 0;
            level = new ConstructLevel(levels[currentLevel]);
        }

        /// <summary> Moves movable entities.
        /// TODO add ball, and collision check. </summary>
        public void UpdateState() {
            player.Move();
        }

        /// <summary> Render the background and entities.
        /// TODO add ball. </summary>
        public void RenderState() {
            background.RenderEntity();
            player.RenderEntity();
            level.Render();
        }

        /// <summary> Handles key events for moving the player, and pausing the game. </summary>
        public void HandleKeyEvent(KeyboardAction action, KeyboardKey key) {
            string actionString = "PRESS";
            if (action == KeyboardAction.KeyRelease)
                actionString = "RELEASE";
            switch (key) {
                case KeyboardKey.Right:
                    BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                        EventType  = GameEventType.PlayerEvent,
                        StringArg1 = actionString,
                        Message    = "RIGHT"
                    });
                    break;
                case KeyboardKey.Left:
                    BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                        EventType  = GameEventType.PlayerEvent,
                        StringArg1 = actionString,
                        Message    = "LEFT"
                    });
                    break;
                case KeyboardKey.N:
                    if (actionString == "PRESS")
                        UpdateLevel();
                    break;
                case KeyboardKey.P:
                    BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                        EventType = GameEventType.GameStateEvent,
                        Message   = "GamePaused"
                    });
                    break;
                default:
                    break;
            }
        }
    }
}