using System;

namespace SpaceTaxi_1 {
    internal class Program {
        public static void Main(string[] args) {
            var text_loader = new text_loader("short-n-sweet");
//            var game = new Game();
//            game.GameLoop();

            var some = text_loader.get_customer_info();
            foreach (var kasjd in some) {
                Console.WriteLine(kasjd);

            }
            
            var text_loader_ = new text_loader("the-beach");
//            var game = new Game();
//            game.GameLoop();

            var some_ = text_loader_.get_customer_info();
            foreach (var kasjd in some_) {
                Console.WriteLine(kasjd);

            }
        }
    }
}