using System;
using System.Runtime.ConstrainedExecution;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;

namespace Galaga_Exercise_1 {
    public class Player : Entity {
        private Game game;
        private Shape shape;
        private PlayerShot playerShot;
        

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
                
                shape.Position = new Vec2F(0.0f,0.1f);
            } else {
                shape.Position = new Vec2F(0.9f,0.1f);
                
            }
            
        }
        
        private Image image = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));

        public void Shoot() {
            playerShot = new PlayerShot(game,
                new DynamicShape(new Vec2F((shape.Position.X+shape.Extent.X/2)-0.004f, shape.Position.Y+0.1f), new Vec2F(0.008f, 0.027f)),image);
            game.playerShots.Add(playerShot);
        }
        
    }
    
   
}