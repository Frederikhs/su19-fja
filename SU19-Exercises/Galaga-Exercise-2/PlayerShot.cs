using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_2 {
    public class PlayerShot : Entity {
        private Game game;
        private Shape shape;

        //Setting shape when instantiating class
        public PlayerShot(Game game, DynamicShape shape, IBaseImage image)
            : base(shape, image) {
            this.game = game;
            this.shape = shape;
            //Set default direction
            Direction(new Vec2F(0.0f, 0.01f));
        }

        //Change direction of the shape
        public void Direction(Vec2F directionVector) {
            shape.AsDynamicShape().ChangeDirection(directionVector);
        }
    }
}