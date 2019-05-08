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
        private Gravity gravity;

        private bool Trusting;

        // A Player has a shape
        public Player() {
            shape = new DynamicShape(new Vec2F(), new Vec2F());
            taxiBoosterOffImageLeft =
                new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
            taxiBoosterOffImageRight =
                new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png"));

            Entity = new Entity(shape, taxiBoosterOffImageLeft);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.PlayerEvent, this);
            gravity = new Gravity(this);
            Trusting = false;

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
        
        /// <summary>
        /// Moved the player in the direction of the direction vector,
        /// if the player is inside bounding box of map.
        /// </summary>
        public void Move() {
            var x = 0f;
            if (taxiOrientation == Orientation.Right) {
                x = 0.003f;
            } else if (taxiOrientation == Orientation.Left) {
                x = -0.003f;
            }
            
            if (Trusting) {
                var dir = gravity.NextVel(0.0001f);
                Direction(new Vec2F(x,dir));
            } else {
                var dir = gravity.NextVel(0f);
                Direction(new Vec2F(x,dir));
            }
            shape.Move();

        }

        /// <summary>
        /// Changes the Player direction to the supplied directionVector
        /// </summary>
        public void Direction(Vec2F directionVector) {
            Entity.Shape.AsDynamicShape().ChangeDirection(directionVector);
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {
                switch (gameEvent.Message) {
                    case "BOOSTER_TO_RIGHT":
                        Console.WriteLine("Moving right");
                        Direction(new Vec2F(0.01f, 0.0f));
                        taxiOrientation = Orientation.Right;
                        break;
                    
                    case "BOOSTER_TO_LEFT":
                        Console.WriteLine("Moving left");
                        Direction(new Vec2F(-0.01f, 0.0f));
                        taxiOrientation = Orientation.Left;
                        break;
                    
                    case "BOOSTER_UPWARDS":
                        Console.WriteLine("Moving up");
                        if (!Trusting) {
                            Trusting = true;
                        }
                        break;
                    
                    case "STOP_ACCELERATE_UP":
                        Console.WriteLine("stopped moviing up");
                        if (Trusting) {
                            Trusting = false;
                        }
                        break;

                    case "STOP_ACCELERATE_RIGHT":
                        if (taxiOrientation == Orientation.Right) {
                            Direction(new Vec2F(0.0f, 0.0f));
                        }
                        break;
                    case "STOP_ACCELERATE_LEFT":
                        if (taxiOrientation == Orientation.Left) {
                            Direction(new Vec2F(0.0f, 0.0f));
                        }
                        break;

                }
            }
        }
    }
}