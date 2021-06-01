using NUnit.Framework;
using DIKUArcade.Events;
using Breakout.BreakoutStates; 
using System.Collections.Generic;

namespace Breakout {

    public class PowerUpTests {
    private Player player;
    private StateMachine stateMachine;
    private GameEventBus eventBus;

    [SetUp]
    public void InitiateGame() {
        DIKUArcade.GUI.Window.CreateOpenGLContext();   
        player = new Player ();
        stateMachine = new StateMachine();
        eventBus = new GameEventBus();
        eventBus.InitializeEventBus(new List<GameEventType> {
            GameEventType.GameStateEvent, GameEventType.PlayerEvent
        });
        eventBus.Subscribe(GameEventType.GameStateEvent, stateMachine);
        eventBus.Subscribe(GameEventType.PlayerEvent, player);

        eventBus.RegisterEvent(new GameEvent {
                EventType  = GameEventType.GameStateEvent,
                Message    = "GameRunning"
            });
    }       


    [Test]

    public void TestLifePickUp() {
        Status.GetStatus().Reset();
        // Lives should be 3.
        Assert.AreEqual(3, Status.GetStatus().lives);
        Status.GetStatus().ExtraLife();
        // Lives should be 4.
        Assert.AreEqual(4, Status.GetStatus().lives);}

    [Test]
    public void TestWidePowerUp() {
        Assert.AreEqual(0.15, player.mutableXtent, 0.1);
        //Player size after the powerup.
        player.WidenPlayer();
        Assert.AreEqual(0.225,player.mutableXtent, 0.1);

    }

    }


}