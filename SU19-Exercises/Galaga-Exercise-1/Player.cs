using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_1 {
    public class Player : Entity {
        private Game game;
        private Shape shape;
        

        public Player(Game game, DynamicShape shape, IBaseImage image)
            : base(shape, image) {
            this.game = game;
            this.shape = shape;
        }

        public void Direction(Vec2F directionVector) {
            var test = shape.AsDynamicShape();
            test.ChangeDirection(directionVector);
            shape = test;

        }

        public void Move() {
            
            if (shape.Position.X >= 0.0 && shape.Position.X <= 0.9) {
                shape.Move();    
            } else if (shape.Position.X < 0.0) {
                
                shape.Position = new Vec2F(0.0f,0.0f);
            } else {
                shape.Position = new Vec2F(0.9f,0.0f);
                
            }
            
        }
    }
    
   
}