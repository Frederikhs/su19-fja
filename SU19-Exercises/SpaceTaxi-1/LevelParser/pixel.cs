using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1
{
    public class pixel : Entity
    {
        private Game game;
        private Shape shape;

        /// <summary>
        /// Creates what we define as a pixel. The pixel has an image and resides in a game class
        /// </summary>
        public pixel(Game game, DynamicShape shape, IBaseImage image) : base(shape, image) {
            this.game = game;
            this.shape = shape;
        }
    }
}