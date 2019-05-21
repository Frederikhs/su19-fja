using System;
using DIKUArcade.EventBus;

namespace SpaceTaxiGame {
    public static class SpaceTaxiBus {
        public static GameEventBus<object> eventBus;

        public static GameEventBus<object> GetBus() {
            return SpaceTaxiBus.eventBus ?? (SpaceTaxiBus.eventBus =
                       new GameEventBus<object>());
        }
    }
}