using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_2 {
    public class Player : Entity, IGameEventProcessor<object> {
        private Game game;

        //Creating image for reference.
        private Image image = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));
        private PlayerShot playerShot;
        private Shape shape;

        //Setting shape when initiating class.
        public Player(Game game, DynamicShape shape, IBaseImage image)
            : base(shape, image) {
            this.game = game;
            this.shape = shape;
        }

        public Entity Entity { get; private set; }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {
                switch (gameEvent.Message) {
                case "move_right":
                    Direction(new Vec2F(0.01f, 0.0f));
                    break;
                case "move_left":
                    Direction(new Vec2F(-0.01f, 0.0f));
                    break;

                //If the player is moving in the same direction as the key pressed, we stop.
                case "stop_right":
                    if (Shape.AsDynamicShape().Direction.X == 0.01f) {
                        Direction(new Vec2F(0.0f, 0.0f));
                    }

                    break;
                case "stop_left":
                    if (Shape.AsDynamicShape().Direction.X == -0.01f) {
                        Direction(new Vec2F(0.0f, 0.0f));
                    }

                    break;
                case "shoot":
                    Shoot();
                    break;
                }
            }
        }

        //Change direction of the player shape.
        public void Direction(Vec2F directionVector) {
            shape.AsDynamicShape().ChangeDirection(directionVector);
        }

        //Move function with bounding box.
        public void Move() {
            if (shape.Position.X >= 0.0 && shape.Position.X <= 0.9) {
                shape.Move();
            } else if (shape.Position.X < 0.0) {
                shape.Position = new Vec2F(0.0f, 0.1f);
            } else {
                shape.Position = new Vec2F(0.9f, 0.1f);
            }
        }

        //Player can shoot, shot will be positioned on front and center of player.
        private void Shoot() {
            playerShot = new PlayerShot(game,
                new DynamicShape(
                    new Vec2F(shape.Position.X + shape.Extent.X / 2 - 0.004f,
                        shape.Position.Y + 0.1f), new Vec2F(0.008f, 0.027f)), image);
            //Adding shot to list of shots
            game.playerShots.Add(playerShot);
        }
    }
}