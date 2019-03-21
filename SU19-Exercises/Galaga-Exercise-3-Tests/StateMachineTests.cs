using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.EventBus;
using GalagaGame;
using GalagaGame.GalagaState;
using Galaga_Exercise_3;
using Galaga_Exercise_3.GalagaStates;
using NUnit.Framework;

namespace Galaga_Testing {
    [TestFixture]
    public class StateMachineTesting {
        [SetUp]
        public void InitiateStateMachine() {
            Window.CreateOpenGLContext();
            GalagaBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.GameStateEvent,
                GameEventType.InputEvent
            });

            stateMachine = new StateMachine();
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, stateMachine);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, stateMachine);
        }

        private StateMachine stateMachine;
        private GameEventBus<object> eventBus;
        private Window win;
        private Game game;

        [Test]
        public void TestEventGamePaused() {
            GalagaBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this,
                    "CHANGE_STATE",
                    "GAME_PAUSED", ""));
            GalagaBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());
        }

        [Test]
        public void TestInitialState() {
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
        }
    }
}