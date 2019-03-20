using System;
using DIKUArcade.EventBus;
using DIKUArcade.State;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using GalagaGame.GalagaState;
using Galaga_Exercise_3;
using Galaga_Exercise_3.GalagaStates;


namespace GalagaGame.GalagaState {
    public class StateMachine : IGameEventProcessor<object> {
        public IGameState ActiveState { get; private set; }

        public StateMachine() {
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            ActiveState = MainMenu.GetInstance();
        }

        private void SwitchState(GameStateType.EnumGameStateType stateType) {
            switch (stateType) {
                case GameStateType.EnumGameStateType.GameRunning:
                    ActiveState = GameRunning.GetInstance();
                    Console.WriteLine("Change active state to running");
                    break;
                case GameStateType.EnumGameStateType.GamePaused:
                    ActiveState = GamePaused.GetInstance();
                    Console.WriteLine("Change active state to pause");
                    break;
                case GameStateType.EnumGameStateType.MainMenu:
                    ActiveState = MainMenu.GetInstance();
                    Console.WriteLine("Change active state to main menu");
                    break;
            }
            
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.GameStateEvent) {
                switch (gameEvent.Message) {
                case "CHANGE_STATE":
                    SwitchState(GameStateType.TransformStringToState(gameEvent.Parameter1));
                    Console.WriteLine("Changing state:"+gameEvent.Parameter1);
                    break;

                }
            } else if (eventType == GameEventType.InputEvent) {
                ActiveState.HandleKeyEvent(gameEvent.Message,gameEvent.Parameter1);
                }
            }
        }
    }

