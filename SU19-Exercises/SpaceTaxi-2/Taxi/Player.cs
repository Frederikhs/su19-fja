using System;
using System.Collections.Generic;
using System.Data.Common;
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
        public Entity Entity { get; }
        private DynamicShape shape;
        private Orientation taxiOrientation;
        private Gravity gravity;
        private bool Trusting;
        private bool tSideways;
        private bool leftDir;
        public bool platform;
        public bool tooFast;
        public float maxSpeed;

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
            gravity = new Gravity();
            Trusting = false;
            tSideways = false;
            leftDir = true;
            platform = false;
            tooFast = false;
        }


        /// <summary>
        /// Sets the players position to x,y
        /// </summary>
        public void SetPosition(float x, float y) {
            shape.Position.X = x;
            shape.Position.Y = y;
        }

        /// <summary>
        /// Sets the size of the player image to width,height
        /// </summary>
        public void SetExtent(float width, float height) {
            shape.Extent.X = width;
            shape.Extent.Y = height;
        }
        

        public void RenderPlayer() {
            switch (taxiOrientation) {
                case Orientation.Left when !tSideways:
                    Entity.Image = taxiBoosterOffImageLeft;
                    break;
                case Orientation.Right when !tSideways:
                    Entity.Image = taxiBoosterOffImageRight;
                    break;
                case Orientation.Left when tSideways:
                    Entity.Image = taxiBoosterOffImageUpLeft;
                    break;
                case Orientation.Right when tSideways:
                    Entity.Image = taxiBoosterOffImageUpRight;
                    break;
                case Orientation.LeftT when !tSideways:
                    Entity.Image = taxiBoosterOnImageLeft;
                    break;
                case Orientation.RightT when !tSideways:
                    Entity.Image = taxiBoosterOnImageRight;
                    break;
                case Orientation.LeftT when tSideways:
                    Entity.Image = taxiBoosterOnImageLeft;
                    break;
                case Orientation.RightT when tSideways:
                    Entity.Image = taxiBoosterOnImageUpRight;
                    break;
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
                x = 0.00008f;
                
            } else if (taxiOrientation == Orientation.LeftT) {
                x = -0.00008f;
            }
            
            if (Trusting && !platform) {
                var dir = gravity.NextVel(0.000012f,platform);
                Direction(new Vec2F(x,dir) + Entity.Shape.AsDynamicShape().Direction);
            } else if (!Trusting && !platform){
                var dir = gravity.NextVel(0f,platform);
                Direction(new Vec2F(x,dir) + Entity.Shape.AsDynamicShape().Direction);
               
            } else if (Trusting && platform) {
                var dir = gravity.NextVel(0f,platform);
                Direction(new Vec2F(x, dir));
            } 
            else if (!Trusting && platform) {
                var dir = gravity.NextVel(0f,platform);
                Direction(new Vec2F(x, dir));
            } 
            
            shape.Move();
        }

        public float currentSpeed() {
            return gravity.vCurrent;
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
                        taxiOrientation = Orientation.RightT;
                        leftDir = false;
                        break;
                    
                    case "BOOSTER_TO_LEFT":
                        taxiOrientation = Orientation.LeftT;
                        leftDir = true;
                        break;
                    
                    case "BOOSTER_UPWARDS":
                        if (!Trusting) {
                            Trusting = true;
                            tSideways = true;
                            platform = false;
                        }
                        break;
                    
                    case "STOP_ACCELERATE_UP":
                        if (Trusting) {
                            Trusting = false;
                            tSideways = false;
                        }
                        break;

                    case "STOP_ACCELERATE_RIGHT":
                        if (taxiOrientation == Orientation.RightT) {
                            taxiOrientation = Orientation.Right;
                        }
                        break;
                    case "STOP_ACCELERATE_LEFT":
                        if (taxiOrientation == Orientation.LeftT) {
                            taxiOrientation = Orientation.Left;
                        }
                        break;
                }
            }
        }
    }
}