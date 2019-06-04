using DIKUArcade.EventBus;
using SpaceTaxi.GameStates;

namespace SpaceTaxi {
    internal class Program {
        public static void Main(string[] args) {
            //Creates the game and starts the gameloop
            Game newGame = new Game();
            newGame.GameLoop();
        }
    }
}