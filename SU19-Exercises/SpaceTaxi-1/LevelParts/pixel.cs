using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1
{
    public class pixel : Entity
    {
        private Game game;

        //CREATING ONE IMAGE FOR REFERENCE
        private Image image = new Image(Path.Combine("Assets", "Images", "deep-bronze-square.png"));
        private Shape shape;

        //SETTING SHAPE WHEN INITIATING CLASS
        public pixel(Game game, DynamicShape shape, IBaseImage image, Vec2F startPos) : base(shape, image) {
            this.game = game;
            this.shape = shape;
        }
    }
}