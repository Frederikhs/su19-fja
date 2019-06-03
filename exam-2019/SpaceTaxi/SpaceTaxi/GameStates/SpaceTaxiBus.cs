using DIKUArcade.EventBus;

namespace SpaceTaxi.GameStates {
    public static class SpaceTaxiBus {
        public static GameEventBus<object> EventBus;

        /// <summary>
        /// Method for getting the same event bus throughout the game
        /// </summary>
        ///
        /// <returns>
        /// Static GameEventBus
        /// </returns>
        public static GameEventBus<object> GetBus() {
            return SpaceTaxiBus.EventBus ?? (SpaceTaxiBus.EventBus =
                       new GameEventBus<object>());
        }
    }
}