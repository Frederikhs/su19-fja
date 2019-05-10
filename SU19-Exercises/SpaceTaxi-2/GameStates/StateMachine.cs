using System;
using DIKUArcade.EventBus;
using DIKUArcade.State;
using SpaceTaxi_2.SpaceTaxiStates;
using SpaceTaxiGame;

namespace SpaceTaxi_2.SpaceTaxiState {
    public class StateMachine : IGameEventProcessor<object> {
        public IGameState ActiveState { get; private set; }

        public StateMachine() {
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            ActiveState = MainMenu.GetInstance("");
            Console.WriteLine("Statemachine created, we are now in Main Menu");
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.GameStateEvent) {
                switch (gameEvent.Message) {
                case "CHANGE_STATE":
                    SwitchState(GameStateType.TransformStringToState(gameEvent.Parameter1),gameEvent.Parameter2);
                    break;
                }
            } else if (eventType == GameEventType.InputEvent) {
                ActiveState.HandleKeyEvent(gameEvent.Message, gameEvent.Parameter1);
            }
        }

        private void SwitchState(GameStateType.EnumGameStateType stateType, string param2) {
            switch (stateType) {
            case GameStateType.EnumGameStateType.GameRunning:
                Console.WriteLine("Game is now running");
                ActiveState = GameRunning.GetInstance(param2);
                break;
            case GameStateType.EnumGameStateType.GamePaused:
                Console.WriteLine("STATEMACHINE: Game is now paused ("+param2+")");
                ActiveState = GamePaused.GetInstance(param2);
                break;
            case GameStateType.EnumGameStateType.GameLevelPicker:
                Console.WriteLine("Game is now in Level Picker");
                ActiveState = GameLevelPicker.GetInstance();
                break;
            case GameStateType.EnumGameStateType.MainMenu:
                Console.WriteLine("Game is now in Main Menu");
                ActiveState = MainMenu.GetInstance(param2);
                break;
            }
        }
    }
}