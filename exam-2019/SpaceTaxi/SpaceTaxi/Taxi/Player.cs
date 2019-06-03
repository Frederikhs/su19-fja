using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using SpaceTaxi;
using SpaceTaxi.GameStates;

namespace SpaceTaxi {
    public class Player : IGameEventProcessor<object> {
        private Image taxiBoosterOffImageLeft;
        private Image taxiBoosterOffImageRight;
        private readonly Image taxiBoosterOffImageUp;
        private ImageStride taxiBoosterOnImageLeft;
        private ImageStride taxiBoosterOnImageRight;
        private ImageStride taxiBoosterOnImageUpRight;
        private ImageStride taxiBoosterOnImageUpLeft;
        private ImageStride taxiBoosterOffImageUpLeft;
        private ImageStride taxiBoosterOffImageUpRight;
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
        private bool thrusting;
        private bool tSideways;
        private bool leftDir;
        public bool platform;
        public float maxSpeed;
        public static List<Customer> CustomersInsidePlayer;
        private float positiveThrust;
        private float noThrust;
        private float rightMove;
        private float leftMove;

        public Player() { shape = new DynamicShape(new Vec2F(), new Vec2F());
            InitializeImages();
            Entity = new Entity(shape, taxiBoosterOffImageLeft);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.PlayerEvent, this);
            gravity = new Gravity();
            thrusting = false;
            tSideways = false;
            leftDir = true;
            platform = false;
            positiveThrust = 0.000012f;
            noThrust = 0.0f;
            rightMove = 0.00008f;
            leftMove = -0.00008f;

            Player.CreateCustomerList();
        }

        /// <summary>
        /// Creates a new static list of customers that resides in the player class, if there is
        /// not one yet
        /// </summary>
        private static void CreateCustomerList() {
            if (Player.CustomersInsidePlayer == null) {
                Player.CustomersInsidePlayer = new List<Customer>();
            }
        }
        
        
        /// <summary>
        /// Player pick up, hides the customer and set it in transit. Adds customer to static list
        /// </summary>
        public void PickUpCustomer(Customer someCustomer) {
            if (Player.CustomersInsidePlayer == null) {
                Player.CustomersInsidePlayer = new List<Customer>();
            }
            someCustomer.Hide();
            someCustomer.IsInTransit = true;
            Console.WriteLine("Player picked up customer ("+someCustomer.name+")");
            Player.CustomersInsidePlayer.Add(someCustomer);
        }

        /// <summary>
        /// Places down customer, and sets its position, also awards points if the timer has
        /// not yet expired
        /// </summary>
        public void PlaceDownCustomer(Pixel platformPixel,Customer someCustomer) {
            //Place down customer

            Console.WriteLine("hastravled; "+someCustomer.HasTravled);
            Console.WriteLine("intransit; "+someCustomer.IsInTransit);

            if (!someCustomer.HasTravled && someCustomer.IsInTransit) {
                someCustomer.SetPos(platformPixel.Shape.Position + platformPixel.Shape.Extent);
                someCustomer.Show();
                Console.WriteLine("Player placed down customer (" + someCustomer.name + ")");
                Console.WriteLine("at: " + someCustomer.entity.Shape.Position.X);
                someCustomer.HasTravled = true;
                someCustomer.IsInTransit = false;
                Points.AddPoints(someCustomer.points);
                someCustomer.HideAfterSuccess();
            }
        }

        /// <summary>
        /// Removes a customer from the static CustomerInsidePlayer list
        /// </summary>
        private void RemoveCustomerFromList(Customer someCustomer) {
            Player.CustomersInsidePlayer = new List<Customer>();
            
            foreach (var customer in Player.CustomersInsidePlayer) {
                if (customer != someCustomer) {
                    Player.CustomersInsidePlayer.Add(customer);
                }
            }
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
        
        /// <summary>
        /// Render the players entity with the correct entity image
        /// </summary>
        public void RenderPlayer() {
            switch (platform) {
                case true:
                    Entity.Image = Entity.Image;
                    break;
                case false:
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
            switch (taxiOrientation) {
            case Orientation.RightT:
                x = rightMove;
                break;
            case Orientation.LeftT:
                x = leftMove;
                break;
            }

            if (thrusting && !platform) {
                var dir = gravity.GetNextVelocity(positiveThrust, platform);
                Direction(new Vec2F(x, dir) + Entity.Shape.AsDynamicShape().Direction);
            } else if (!thrusting && !platform) {
                var dir = gravity.GetNextVelocity(noThrust, platform);
                Direction(new Vec2F(x, dir) + Entity.Shape.AsDynamicShape().Direction);

            } else if (!thrusting && platform) {
                var dir = gravity.GetNextVelocity(noThrust, platform);
                Direction(new Vec2F(x, dir));
            } else {
                Direction(new Vec2F(x, noThrust));
            }

            shape.Move();
            
        }

        
        /// <summary>
        /// Returns the current speed of the player
        /// </summary>
        public double currentSpeed() {
            return Entity.Shape.AsDynamicShape().Direction.Length();
        }
        

        /// <summary>
        /// Changes the Player direction to the supplied directionVector
        /// </summary>
        public void Direction(Vec2F directionVector) {
            Entity.Shape.AsDynamicShape().ChangeDirection(directionVector);
        }

        /// <summary>
        /// Handles events that are assigned to the game object.
        /// </summary>
        ///
        /// <param name="eventType">
        /// GameEventType to handle
        /// </param>
        ///
        /// <param name="gameEvent">
        /// A GameEvent object of same type, as eventType which information
        /// </param>
        private void InitializeImages() {
            taxiBoosterOffImageLeft = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
            taxiBoosterOffImageRight = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png"));
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
            taxiBoosterOffImageUpLeft = new ImageStride(80,OffUpLeftStrides);
            taxiBoosterOffImageUpRight = new ImageStride(80,OffUpRightStrides);
            taxiBoosterOnImageLeft = new ImageStride(80,OnLeftStrides);
            taxiBoosterOnImageRight = new ImageStride(80,OnRightStrides);
            taxiBoosterOnImageUpRight = new ImageStride(80,OnUpRightStrides);
            taxiBoosterOnImageUpLeft = new ImageStride(80,OnUpLeftStrides);
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
                        if (!thrusting) {
                            thrusting = true;
                            tSideways = true;
                            platform = false;
                        }
                        break;
                    
                    case "STOP_ACCELERATE_UP":
                        if (thrusting) {
                            thrusting = false;
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