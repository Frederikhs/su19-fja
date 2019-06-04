using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using SpaceTaxi.GameStates;

namespace SpaceTaxi.Taxi {
    public class Player : IGameEventProcessor<object> {
        public Entity Entity { get; }
        public bool platform;
        public float maxSpeed;
        public static List<Customer> CustomersInsidePlayer;
        
        private Image taxiBoosterOffImageLeft;
        private Image taxiBoosterOffImageRight;
        
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
        
        private DynamicShape shape;
        private Orientation taxiOrientation;
        private Gravity gravity;
        private bool thrusting;
        private bool upThrust;
        private float positiveThrust;
        private float noThrust;
        private float rightMove;
        private float leftMove;

        /// <summary>
        /// Initialized the player images and basic variables that the class uses
        /// </summary>
        public Player() { shape = new DynamicShape(new Vec2F(), new Vec2F());
            InitializeImages();
            Entity = new Entity(shape, taxiBoosterOffImageLeft);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.PlayerEvent, this);
            gravity = new Gravity();
            thrusting = false;
            upThrust = false;
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
        /// Player pick up, set the state to in transit
        /// </summary>
        public static void PickUpCustomer(Customer someCustomer) {
            Player.CreateCustomerList();
            someCustomer.SwitchState(CustomerState.InTransit);
            Player.CustomersInsidePlayer.Add(someCustomer);
        }

        /// <summary>
        /// Place down customer, set the state to delivered
        /// </summary>
        public static void PlaceDownCustomer(Pixel platformPixel, Customer someCustomer) {
            if (someCustomer.CustomerState == CustomerState.InTransit) {
                someCustomer.SetPos(platformPixel.Shape.Position + platformPixel.Shape.Extent);
                someCustomer.SwitchState(CustomerState.Delivered);
            }
        }

        /// <summary>
        /// Removes a customer from the static CustomerInsidePlayer list
        /// </summary>
        /// 
        /// <param name="someCustomer">
        /// The customer that should be removed from the static list of customers inside the player
        /// </param>
        public static void RemoveCustomerFromList(Customer someCustomer) {
            Player.CustomersInsidePlayer = new List<Customer>();
            
            foreach (var customer in Player.CustomersInsidePlayer) {
                if (customer != someCustomer) {
                    Player.CustomersInsidePlayer.Add(customer);
                }
            }
        }

        /// <summary>
        /// Clears the list of customers inside the player
        /// </summary>
        public static void ClearCustomers() {
            Player.CustomersInsidePlayer = new List<Customer>();
        }

        /// <summary>
        /// Sets the players position to x,y
        /// </summary>
        ///
        /// <param name="x">
        /// The x position for the player
        /// </param>
        ///
        /// <param name="y">
        /// The y position for the player
        /// </param>
        public void SetPosition(float x, float y) {
            shape.Position.X = x;
            shape.Position.Y = y;
        }

        /// <summary>
        /// Sets the players position to x,y
        /// </summary>
        ///
        /// <param name="width">
        /// The height for the player
        /// </param>
        ///
        /// <param name="height">
        /// The width for the player
        /// </param>
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
                    case Orientation.Left when !upThrust:
                        Entity.Image = taxiBoosterOffImageLeft;
                        break;
                    case Orientation.Right when !upThrust:
                        Entity.Image = taxiBoosterOffImageRight;
                        break;
                    case Orientation.Left when upThrust:
                        Entity.Image = taxiBoosterOffImageUpLeft;
                        break;
                    case Orientation.Right when upThrust:
                        Entity.Image = taxiBoosterOffImageUpRight;
                        break;
                    case Orientation.LeftT when !upThrust:
                        Entity.Image = taxiBoosterOnImageLeft;
                        break;
                    case Orientation.RightT when !upThrust:
                        Entity.Image = taxiBoosterOnImageRight;
                        break;
                    case Orientation.LeftT when upThrust:
                        Entity.Image = taxiBoosterOnImageUpLeft;
                        break;
                    case Orientation.RightT when upThrust:
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
                ChangeDirection(new Vec2F(x, dir) + Entity.Shape.AsDynamicShape().Direction);
            } else if (!thrusting && !platform) {
                var dir = gravity.GetNextVelocity(noThrust, platform);
                ChangeDirection(new Vec2F(x, dir) + Entity.Shape.AsDynamicShape().Direction);

            } else if (!thrusting && platform) {
                var dir = gravity.GetNextVelocity(noThrust, platform);
                ChangeDirection(new Vec2F(x, dir));
            } else {
                ChangeDirection(new Vec2F(x, noThrust));
            }

            shape.Move();
            
        }

        /// <summary>
        /// Returns the current speed of the player
        /// </summary>
        public double GetPlayerSpeed() {
            return Entity.Shape.AsDynamicShape().Direction.Length();
        }
        
        /// <summary>
        /// Changes the Player direction to the supplied directionVector
        /// </summary>
        ///
        /// <param name="directionVector">
        /// The new direction for the player
        /// </param>
        private void ChangeDirection(Vec2F directionVector) {
            Entity.Shape.AsDynamicShape().ChangeDirection(directionVector);
        }
        
        /// <summary>
        /// Creates all image strides for the player
        /// </summary>
        private void InitializeImages() {
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
            taxiBoosterOffImageUpLeft = new ImageStride(80,OffUpLeftStrides);
            taxiBoosterOffImageUpRight = new ImageStride(80,OffUpRightStrides);
            taxiBoosterOnImageLeft = new ImageStride(80,OnLeftStrides);
            taxiBoosterOnImageRight = new ImageStride(80,OnRightStrides);
            taxiBoosterOnImageUpRight = new ImageStride(80,OnUpRightStrides);
            taxiBoosterOnImageUpLeft = new ImageStride(80,OnUpLeftStrides);
        }
        
        /// <summary>
        /// Handles key events that the user presses
        /// </summary>
        ///
        /// <param name="eventType">
        /// Enum GameEventType
        /// </param>
        ///
        /// <param name="gameEvent">
        /// THe message sent, which contains i.e. "BOOSTER_TO_RIGHT" 
        /// </param>
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {
                switch (gameEvent.Message) {
                    case "BOOSTER_TO_RIGHT":
                        taxiOrientation = Orientation.RightT;
                        break;
                    
                    case "BOOSTER_TO_LEFT":
                        taxiOrientation = Orientation.LeftT;
                        break;
                    
                    case "BOOSTER_UPWARDS":
                        if (!thrusting) {
                            thrusting = true;
                            upThrust = true;
                            platform = false;
                        }
                        break;
                    
                    case "STOP_ACCELERATE_UP":
                        if (thrusting) {
                            thrusting = false;
                            upThrust = false;
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