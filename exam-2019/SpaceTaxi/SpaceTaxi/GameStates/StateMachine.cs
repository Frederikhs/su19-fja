using System;
using DIKUArcade.EventBus;
using DIKUArcade.State;

namespace SpaceTaxi.GameStates {
    public class StateMachine : IGameEventProcessor<object> {
        public IGameState ActiveState { get; private set; }

        /// <summary>
        /// Initialized the statemachine and subscribes to events
        /// </summary>
        public StateMachine() {
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            ActiveState = MainMenu.GetInstance("");
        }

        /// <summary>
        /// Processes events for the statemachine
        /// </summary>
        ///
        /// <param name="eventType">
        /// The enum type of event
        /// </param>
        ///
        /// <param name="gameEvent">
        /// The gameEvent object that is the message
        /// </param>
        ///
        /// <returns>
        /// void
        /// </returns>
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

        /// <summary>
        /// Switches between game states
        /// </summary>
        ///
        /// <param name="stateType">
        /// The enum type of state
        /// </param>
        ///
        /// <param name="param2">
        /// Used with resuming a level or going to the next
        /// </param>
        ///
        /// <returns>
        /// void
        /// </returns>
        private void SwitchState(GameStateType.EnumGameStateType stateType, string param2) {
            switch (stateType) {
            case GameStateType.EnumGameStateType.GameRunning:
                Console.WriteLine("State is now GameRunning");
                ActiveState = GameRunning.GetInstance(param2);
                break;
            case GameStateType.EnumGameStateType.GamePaused:
                Console.WriteLine("State is now paused with level "+param2);
                ActiveState = GamePaused.GetInstance(param2);
                break;
            case GameStateType.EnumGameStateType.GameLevelPicker:
                Console.WriteLine("State is now Level Picker");
                ActiveState = GameLevelPicker.GetInstance();
                break;
            case GameStateType.EnumGameStateType.MainMenu:
                Console.WriteLine("State is now Main Menu");
                ActiveState = MainMenu.GetInstance(param2);
                break;
            case GameStateType.EnumGameStateType.GameOver:
                Console.WriteLine("State is now in Game Over");
                ActiveState = GameOver.GetInstance();
                break;
            }
        }
    }
}