using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_2
{
    public class pixel : Entity
    {
        private Game game;
        public Shape shape { get; private set; }
        public pixelTypes type;
        public char pixelChar;

        public enum pixelTypes {
            dangerus,
            portal,
            platform,
            neutral
        }

        /// <summary>
        /// Creates what we define as a pixel. The pixel has an image and resides in a game class
        /// </summary>
        public pixel(Game game, Shape shape, IBaseImage image, pixelTypes type, char pixelChar) : base(shape, image) {
            this.game = game;
            this.shape = shape;
            this.type = type;
            this.pixelChar = pixelChar;
        }
        
    }
}