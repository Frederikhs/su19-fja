using System;
using DIKUArcade.EventBus;

namespace GalagaGame {
    public static class GalagaBus {
        private static GameEventBus<object> eventBus;

        public static GameEventBus<object> GetBus() {
            var bus = GalagaBus.eventBus;
            if (bus != null) {
                Console.WriteLine("Bus was not null");
                return bus;
            }

            Console.WriteLine("Bus was NULL");
            return (GalagaBus.eventBus =
                new GameEventBus<object>());
        }
    }
}