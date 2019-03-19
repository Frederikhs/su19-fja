using DIKUArcade.EventBus;
using DIKUArcade.State;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using GalagaGame.GalagaState;
using Galaga_Exercise_3;
using Galaga_Exercise_3.GalagaStates;


namespace GalagaGame.GalagaState {
    public class StateMachine : IGameEventProcessor<object> {
        public GameStateType.EnumGameStateType ActiveState { get; private set; }

        public StateMachine() {
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            ActiveState = MainMenu.GetInstance();
        }

        private void SwitchState(GameStateType.EnumGameStateType stateType) {
            switch (stateType) {
                case GameStateType.EnumGameStateType.GameRunning:
                    ActiveState = GameStateType.EnumGameStateType.GameRunning;
                    break;
                case GameStateType.EnumGameStateType.GamePaused:
                    ActiveState = GameStateType.EnumGameStateType.GamePaused;
                    break;
                case GameStateType.EnumGameStateType.MainMenu:
                    ActiveState = GameStateType.EnumGameStateType.MainMenu;
                    break;
                    
            }

            // vores kode her
            //
            //
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            throw new System.NotImplementedException();
        }
    }
}