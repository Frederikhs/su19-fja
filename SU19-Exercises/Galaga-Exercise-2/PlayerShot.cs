using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_2 {
    public class PlayerShot : Entity {
        private Game game;
        private Shape shape;

        //SETTING SHAPE WHEN INITIATING CLASS
        public PlayerShot(Game game, DynamicShape shape, IBaseImage image)
            : base(shape, image) {
            this.game = game;
            this.shape = shape;
            //CHANGING DIRECTION WHEN INITIATED
            Direction(new Vec2F(0.0f, 0.01f));
        }

        //CHANGE DIRECTION OF SHAPE
        public void Direction(Vec2F directionVector) {
            var test = shape.AsDynamicShape();
            test.ChangeDirection(directionVector);
            shape = test;
        }
    }
}