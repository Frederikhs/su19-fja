using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.EventBus;
using DIKUArcade.Timers;
using GalagaGame;
using GalagaGame.GalagaState;

namespace Galaga_Exercise_3 {
    public class Game : IGameEventProcessor<object> {
        private GameEventBus<object> eventBus;
        private GameTimer gameTimer;
        private StateMachine stateMachine;
        private Window win;

        public Game() {
            //Creating game window
            win = new Window("Window", 500, 500);
            gameTimer = new GameTimer(60, 60);

            eventBus = GalagaBus.GetBus();
            eventBus.InitializeEventBus(new List<GameEventType> {
                GameEventType.PlayerEvent,
                GameEventType.InputEvent, //key press / key release
                GameEventType.WindowEvent,
                GameEventType.GameStateEvent //messages to the window
            });
            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.WindowEvent, this);

            //Creating state machine
            stateMachine = new StateMachine();
        }

        // Process events
        public void ProcessEvent(GameEventType eventType,
            GameEvent<object> gameEvent) {
            if (eventType == GameEventType.WindowEvent) {
                switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    win.CloseWindow();
                    break;
                }
            }
        }

        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    GalagaBus.GetBus().ProcessEvents();
                    stateMachine.ActiveState.UpdateGameLogic();
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    stateMachine.ActiveState.RenderState();
                    win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()) {
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates + ", FPS: " +
                                gameTimer.CapturedFrames;
                    //1 second passed
                }
            }
        }
    }
}