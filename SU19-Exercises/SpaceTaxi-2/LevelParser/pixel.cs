using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_2
{
    public class pixel : Entity
    {
        private Game game;
        private Shape shape;
        public bool danger;
        public bool IsPortal;

        /// <summary>
        /// Creates what we define as a pixel. The pixel has an image and resides in a game class
        /// </summary>
        public pixel(Game game, Shape shape, IBaseImage image, bool danger, bool isPortal) : base(shape, image) {
            this.game = game;
            this.shape = shape;
            this.danger = danger;
            this.IsPortal = isPortal;
        }
    }
}