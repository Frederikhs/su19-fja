using System;
using System.Runtime.ConstrainedExecution;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_1 {
    public class PlayerShot : Entity {
        private Game game;
        private Shape shape;

        public PlayerShot(Game game, DynamicShape shape, IBaseImage image)
            : base(shape, image) {
            this.game = game;
            this.shape = shape;
            Direction(new Vec2F(0.0f,0.01f));
        }
        public void Direction(Vec2F directionVector) {
            var test = shape.AsDynamicShape();
            test.ChangeDirection(directionVector);
            shape = test;
        }
    }
    
   
}