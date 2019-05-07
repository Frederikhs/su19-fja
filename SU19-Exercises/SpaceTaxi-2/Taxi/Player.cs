using System;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using SpaceTaxiGame;

namespace SpaceTaxi_2 {
    public class Player : IGameEventProcessor<object> {
        private readonly Image taxiBoosterOffImageLeft;
        private readonly Image taxiBoosterOffImageRight;
        private DynamicShape shape;
        private Orientation taxiOrientation;

        // A Player has a shape
        public Player() {
            shape = new DynamicShape(new Vec2F(), new Vec2F());
            taxiBoosterOffImageLeft =
                new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
            taxiBoosterOffImageRight =
                new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png"));

            Entity = new Entity(shape, taxiBoosterOffImageLeft);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.PlayerEvent, this);
        }

        public Entity Entity { get; }

        public void SetPosition(float x, float y) {
            shape.Position.X = x;
            shape.Position.Y = y;
        }

        public void SetExtent(float width, float height) {
            shape.Extent.X = width;
            shape.Extent.Y = height;
        }

        public void RenderPlayer() {
            //TODO: Next version needs animation. Skipped for clarity.
            Entity.Image = taxiOrientation == Orientation.Left
                ? taxiBoosterOffImageLeft
                : taxiBoosterOffImageRight;
            Entity.RenderEntity();
        }

        public void Direction(Vec2F directionVector) {
            Entity.Shape.Move(directionVector);
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {
                switch (gameEvent.Message) {
                    case "BOOSTER_TO_RIGHT":
                        Console.WriteLine("Moving right");
                        Direction(new Vec2F(0.05f, 0.0f));
                        break;
                    case "BOOSTER_TO_LEFT":
                        Console.WriteLine("Moving left");
                        Direction(new Vec2F(-0.05f, 0.0f));
                        break;

                    //If the player is moving in the same direction as the key pressed, we stop.
//                    case "STOP_ACCELERATE_RIGHT":
//                        if (shape.AsDynamicShape().Direction.X == 0.01f) {
//                            Direction(new Vec2F(0.0f, 0.0f));
//                        }
//
//                        break;
//                    case "STOP_ACCELERATE_LEFT":
//                        if (shape.AsDynamicShape().Direction.X == -0.01f) {
//                            Direction(new Vec2F(0.0f, 0.0f));
//                        }
//
//                        break;
//                    case "shoot":
//                        Shoot();
//                        break;
                }
            }
        }
    }
}