using DIKUArcade.EventBus;
using DIKUArcade.State;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace GalagaGame.GalagaState {
    public class StateMachine : IGameEventProcessor<object> {
        public IGameState ActiveState { get; private set; }

        public StateMachine() {
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            ActiveState = MainMenu.GetInstance();
        }

        private void SwitchState(GameStateType stateType) {
            switch (stateType) {
            //... }
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