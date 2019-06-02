using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;
using SpaceTaxi_2.SpaceTaxiState;
using SpaceTaxi_2.SpaceTaxiStates;
using SpaceTaxiGame;

namespace SpaceTaxi_2 {
    public class Game : IGameEventProcessor<object> {
        private GameEventBus<object> eventBus;
        private GameTimer gameTimer;
        private Window win;
        private StateMachine stateMachine;

        public Game() {
            // window
            win = new Window("Space Taxi Game v0.1", 500, AspectRatio.R1X1);

            // event bus
            eventBus = SpaceTaxiBus.GetBus();
            eventBus.InitializeEventBus(new List<GameEventType> {
                GameEventType.InputEvent, // key press / key release
                GameEventType.WindowEvent, // messages to the window, e.g. CloseWindow()
                GameEventType.PlayerEvent, // commands issued to the player object, e.g. move,
                GameEventType.GameStateEvent, // game state events
                GameEventType.TimedEvent // timed events
            });
            win.RegisterEventBus(eventBus);

            // game timer
            gameTimer = new GameTimer(60); // 60 UPS, no FPS limit
            
            // event delegation
            eventBus.Subscribe(GameEventType.WindowEvent, this);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            stateMachine = new StateMachine();
        }

        /// <summary>
        /// Updates game elements, renders object, and keeps track of time
        /// </summary>
        ///
        /// <returns>
        /// void
        /// </returns>
        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();

                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    eventBus.ProcessEvents();
                    SpaceTaxiBus.GetBus().ProcessEvents();
                    stateMachine.ActiveState.UpdateGameLogic();
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    stateMachine.ActiveState.RenderState();
                    win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()) {
                    // 1 second has passed - display last captured ups and fps from the timer
                    win.Title = "Space Taxi | UPS: " + gameTimer.CapturedUpdates + ", FPS: " +
                                gameTimer.CapturedFrames;
                }
            }
        }
        
        /// <summary>
        /// Handles events that are assigned to the game object.
        /// </summary>
        ///
        /// <param name="eventType">
        /// GameEventType to handle
        /// </param>
        ///
        /// <param name="gameEvent">
        /// A GameEvent object of same type, as eventType which information
        /// </param>
        ///
        /// <returns>
        /// void
        /// </returns>
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
    }
}