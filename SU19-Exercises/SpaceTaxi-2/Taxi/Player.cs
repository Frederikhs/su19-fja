using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using SpaceTaxi_2.SpaceTaxiState;
using SpaceTaxiGame;

namespace SpaceTaxi_2 {
    public class Player : IGameEventProcessor<object> {
        private readonly Image taxiBoosterOffImageLeft;
        private readonly Image taxiBoosterOffImageRight;
        private readonly Image taxiBoosterOffImageUp;
        private readonly ImageStride taxiBoosterOnImageLeft;
        private readonly ImageStride taxiBoosterOnImageRight;
        private readonly ImageStride taxiBoosterOnImageUpRight;
        private readonly ImageStride taxiBoosterOnImageUpLeft;
        private readonly ImageStride taxiBoosterOffImageUpLeft;
        private readonly ImageStride taxiBoosterOffImageUpRight;
        
        
        private List<Image> OnRightStrides;
        private List<Image> OnUpRightStrides;
        private List<Image> OnLeftStrides;
        private List<Image> OnUpLeftStrides;
        private List<Image> OffUpLeftStrides;
        private List<Image> OffUpRightStrides;
        


        private DynamicShape shape;
        private Orientation taxiOrientation;
        private Gravity gravity;

        private bool Trusting;
        private bool tSideways;
        private bool leftDir;

        // A Player has a shape
        public Player() {
            shape = new DynamicShape(new Vec2F(), new Vec2F());
            taxiBoosterOffImageLeft =
                new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
            taxiBoosterOffImageRight =
                new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png"));
            
            OnLeftStrides = ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "Taxi_Thrust_Back.png"));
            OnRightStrides = ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "Taxi_Thrust_Back_Right.png"));
            OnUpLeftStrides = ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Back.png"));
            OnUpRightStrides = ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Back_Right.png"));
            OffUpLeftStrides = ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom.png"));
            OffUpRightStrides = ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Right.png"));
            taxiBoosterOffImageUpLeft =
                new ImageStride(80,OffUpLeftStrides);
            taxiBoosterOffImageUpRight =
                new ImageStride(80,OffUpRightStrides);
            taxiBoosterOnImageLeft =
                new ImageStride(80,OnLeftStrides);
            taxiBoosterOnImageRight =
                new ImageStride(80,OnRightStrides);
            taxiBoosterOnImageUpRight =
                new ImageStride(80,OnUpRightStrides);
            taxiBoosterOnImageUpLeft =
                new ImageStride(80,OnUpLeftStrides);
           

            Entity = new Entity(shape, taxiBoosterOffImageLeft);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.PlayerEvent, this);
            gravity = new Gravity(this);
            Trusting = false;
            tSideways = false;
            leftDir = true;

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
            //Entity.Image = taxiOrientation == Orientation.Left
            //    ? taxiBoosterOffImageLeft
            //    : taxiBoosterOffImageRight; 
            if (taxiOrientation == Orientation.Left && !tSideways) {
                Entity.Image = taxiBoosterOffImageLeft;

            } else if (taxiOrientation == Orientation.Right && !tSideways) {
                Entity.Image = taxiBoosterOffImageLeft;
            }
            
            else if (taxiOrientation == Orientation.LeftT && !tSideways) {
                Entity.Image = taxiBoosterOnImageLeft;
            }
            else if (taxiOrientation == Orientation.RightT && !tSideways) {
                Entity.Image = taxiBoosterOnImageRight;
            }
            else if (taxiOrientation == Orientation.LeftT && tSideways) {
                Entity.Image = taxiBoosterOnImageUpLeft;
            }
            else if (taxiOrientation == Orientation.RightT && tSideways) {
                Entity.Image = taxiBoosterOnImageUpRight;
            }
            else if (taxiOrientation == Orientation.Left && leftDir && tSideways) {
                Entity.Image = taxiBoosterOffImageUpLeft;
            }
            else if (taxiOrientation == Orientation.Right && !leftDir && tSideways) {
                Entity.Image = taxiBoosterOffImageUpRight;
            }

            
            Entity.RenderEntity();
        }
        
        /// <summary>
        /// Moved the player in the direction of the direction vector,
        /// if the player is inside bounding box of map.
        /// </summary>
        public void Move() {
            var x = 0f;
            if (taxiOrientation == Orientation.RightT) {
                x = 0.003f;
            } else if (taxiOrientation == Orientation.LeftT) {
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
                        taxiOrientation = Orientation.RightT;
                        leftDir = false;
                        
                        break;
                    
                    case "BOOSTER_TO_LEFT":
                        Console.WriteLine("Moving left");
                        Direction(new Vec2F(-0.01f, 0.0f));
                        taxiOrientation = Orientation.LeftT;
                        leftDir = true;
                        
                        break;
                    
                    case "BOOSTER_UPWARDS":
                        Console.WriteLine("Moving up");
                        if (!Trusting) {
                            Trusting = true;
                            tSideways = true;
                            //taxiOrientation = Orientation.Up;
                            
                        }
                        
                        break;
                    
                    case "STOP_ACCELERATE_UP":
                        Console.WriteLine("stopped moving up");
                        if (Trusting) {
                            Trusting = false;
                            tSideways = false;
                            
                        }
                        break;

                    case "STOP_ACCELERATE_RIGHT":
                        if (taxiOrientation == Orientation.RightT) {
                            Direction(new Vec2F(0.0f, 0.0f));
                            taxiOrientation = Orientation.Right;
                            tSideways = false;
                        }
                        break;
                    case "STOP_ACCELERATE_LEFT":
                        if (taxiOrientation == Orientation.LeftT) {
                            Direction(new Vec2F(0.0f, 0.0f));
                            taxiOrientation = Orientation.Left;
                            tSideways = false;
                        }
                        break;

                }
            }
        }
    }
}