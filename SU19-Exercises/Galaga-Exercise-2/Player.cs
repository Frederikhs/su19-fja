using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_2 {
    public class Player : Entity, IGameEventProcessor<object>  {
        public Entity Entity { get; private set; }
        private Game game;

        //CREATING ONE IMAGE FOR REFERENCE
        private Image image = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));
        private PlayerShot playerShot;
        private Shape shape;

        //SETTING SHAPE WHEN INITIATING CLASS
        public Player(Game game, DynamicShape shape, IBaseImage image)
            : base(shape, image) {
            this.game = game;
            this.shape = shape;
        }

        //CHANGE DIRECTION OF SHAPE
        public void Direction(Vec2F directionVector) {
            var test = shape.AsDynamicShape();
            test.ChangeDirection(directionVector);
            shape = test;
        }

        //MOVE FUNCTION WITH BOUNDING BOX
        public void Move() {
            if (shape.Position.X >= 0.0 && shape.Position.X <= 0.9) {
                shape.Move();
            } else if (shape.Position.X < 0.0) {
                shape.Position = new Vec2F(0.0f, 0.1f);
            } else {
                shape.Position = new Vec2F(0.9f, 0.1f);
            }
        }        
        //PLAYER CAN SHOOT, SHOT POSITIONED ON FRONT AND CENTER OF PLAYER
        public void Shoot() {
            playerShot = new PlayerShot(game,
                new DynamicShape(
                    new Vec2F(shape.Position.X + shape.Extent.X / 2 - 0.004f,
                        shape.Position.Y + 0.1f), new Vec2F(0.008f, 0.027f)), image);
            //ADDING SHOT TO LIST OF SHOTS
            game.playerShots.Add(playerShot);
        }
        
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {
                switch (gameEvent.Message) {
                case "move_right":
                    Direction(new Vec2F(0.01f,0.0f));
                    break;
                case "move_left":
                    Direction(new Vec2F(-0.01f, 0.0f));
                    break;
                case "space":
                    Shoot();
                    break;
                }
            } else {
                throw new System.NotImplementedException();
            }
            
        }
    }
}
