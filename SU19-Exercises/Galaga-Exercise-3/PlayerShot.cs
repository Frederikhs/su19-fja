using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Galaga_Exercise_3.GalagaStates;

namespace Galaga_Exercise_3 {
    public class PlayerShot : Entity {
        private GameRunning gameRunning;
        private Shape shape;

        //Setting shape when instantiating class
        public PlayerShot(GameRunning gameRunning, DynamicShape shape, IBaseImage image)
            : base(shape, image) {
            this.gameRunning = gameRunning;
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