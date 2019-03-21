using DIKUArcade.EventBus;
using DIKUArcade.State;
using Galaga_Exercise_3.GalagaStates;

namespace GalagaGame.GalagaState {
    public class StateMachine : IGameEventProcessor<object> {
        public StateMachine() {
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            ActiveState = MainMenu.GetInstance();
        }

        public IGameState ActiveState { get; private set; }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.GameStateEvent) {
                switch (gameEvent.Message) {
                case "CHANGE_STATE":
                    SwitchState(GameStateType.TransformStringToState(gameEvent.Parameter1));
                    break;
                }
            } else if (eventType == GameEventType.InputEvent) {
                ActiveState.HandleKeyEvent(gameEvent.Message, gameEvent.Parameter1);
            }
        }

        private void SwitchState(GameStateType.EnumGameStateType stateType) {
            switch (stateType) {
            case GameStateType.EnumGameStateType.GameRunning:
                ActiveState = GameRunning.GetInstance();
                break;
            case GameStateType.EnumGameStateType.GamePaused:
                ActiveState = GamePaused.GetInstance();
                break;
            case GameStateType.EnumGameStateType.MainMenu:
                ActiveState = MainMenu.GetInstance();
                break;
            }
        }
    }
}