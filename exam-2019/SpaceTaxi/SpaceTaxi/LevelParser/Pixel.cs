using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace SpaceTaxi
{
    public class Pixel : Entity
    {
        public Shape shape { get; private set; }
        public PixelTypes Type;
        public char PixelChar;
        
        private Game game;

        /// <summary>
        /// Pixels all have a type, and they must be of enum type PixelTypes
        /// </summary>
        public enum PixelTypes {
            Dangerous,
            Portal,
            Platform,
            Neutral
        }

        /// <summary>
        /// Creates what we define as a pixel. The pixel has an image and resides in a game class
        /// </summary>
        public Pixel(Game game, Shape shape, IBaseImage image, PixelTypes type,
            char pixelChar) : base(shape, image) {
            this.game = game;
            this.shape = shape;
            this.Type = type;
            this.PixelChar = pixelChar;
        }
        
    }
}