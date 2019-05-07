using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SpaceTaxi_2 {
    internal class Program {
        
        public static void Main(string[] args)
        {
            Game newGame = new Game();
            newGame.GameLoop();
        }
    }
}