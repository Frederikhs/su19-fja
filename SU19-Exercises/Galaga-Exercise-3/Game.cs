using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using DIKUArcade;
using DIKUArcade.EventBus;
using DIKUArcade.Timers;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using GalagaGame;
using GalagaGame.GalagaState;
using Galaga_Exercise_3.GalagaEntities.Enemy;
using Galaga_Exercise_3.MovementStrategy;


namespace Galaga_Exercise_3 {

    public class Game : IGameEventProcessor<object> {
        private Window win;
        private GameTimer gameTimer;
        private GameEventBus<object> eventBus;
        private StateMachine stateMachine;
        
        public Game() {
            //Creating game window
            win = new Window("Window", 500, 500);
            gameTimer = new GameTimer(60, 60);
          
            eventBus = GalagaBus.GetBus();
            eventBus.InitializeEventBus(new List<GameEventType> {
                GameEventType.PlayerEvent,
                GameEventType.InputEvent, //key press / key release
                GameEventType.WindowEvent,
                GameEventType.GameStateEvent//messages to the window
                
            });
            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.WindowEvent, this);
            
            //Creating state machine
            stateMachine = new StateMachine();
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
    }
}