using System;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;
using SpaceTaxi_2.SpaceTaxiState;
using SpaceTaxi_2.SpaceTaxiStates;
using SpaceTaxiGame;

namespace SpaceTaxi_2 {
    public class Customer : IGameEventProcessor<object> {

        public string name; // Name of the customer.
        public int spawnAfter; // Number of seconds that should pass in the level, before the customer appears (is spawned).
        public char spawnPlatform; // Determining on which platform the customer should be spawned.
        public string destinationPlatform; // Destination platform of the customer.
        public string PickedUpLevel; // What place to place down the customer.
        public int taxiDuration; // Number of seconds you have to drop off the customer at the correct platform.
        public int points; // Number of points a correct drop off of the customer is worth.
        public bool WildCardPlatform; //If the customer can be dropped off on any platform.
        public bool DroppedOnSameLevel; //If the customer should be dropped off on the same level.
        public bool visible; //If the customer is visible on screen.
        
        //TODO: Refractor into enum CustState
        public bool expiredCustomer; //If the customer is expired. e.g. the taxiDuration is expired.
        public bool IsInTransit; //If the customer is in transit
        public bool HasTravled; //If the customer has travelled to his/her destination

        private Image imageStandLeft;
        private Image imageStandRight;

        private float Start; //The start x position for the platform which the customer can walk.
        private float End; //The end x position.

        public bool OnPath; //If the customer is on the right path (platform)
        private Direction walkingDirection; //What direction the customer is facing

        private enum Direction {
            StandLeft,
            StandRight,
            WalkLeft,
            WalkRight
        }

        public Entity entity { get; private set; }
        private Shape shape;

        public Customer(string name, int spawnAfter, char spawnPlatform, string destinationPlatform,
            int taxiDuration, int points) {
            this.name = name;
            this.spawnAfter = spawnAfter;
            this.spawnPlatform = spawnPlatform;
            this.destinationPlatform = destinationPlatform;
            this.taxiDuration = taxiDuration;
            this.points = points;
            this.walkingDirection = Direction.WalkRight;
            this.visible = false;
            this.HasTravled = false;
            this.WildCardPlatform = false;
            this.expiredCustomer = false;
            GenerateImage();
            Hide();
            ShowAfter();
            FindPlatform(this.destinationPlatform);
        }

        /// <summary>
        /// Sets the class variables depending on what platform the customer should land on. If it
        /// containers a ^ we do something, else we do not change anything, as destination platform
        /// is the char that was in the constructor
        /// </summary>
        ///
        /// <param name="platformWithHat">
        /// platform 
        /// </param>
        ///
        /// <returns>
        /// void
        /// </returns>
        private void FindPlatform(string platformWithHat) {
            if (platformWithHat.Contains("^")) {
                if (platformWithHat.Length > 1) {
                    this.destinationPlatform = (platformWithHat.Split('^'))[1];
                    this.DroppedOnSameLevel = false;
                    Console.WriteLine("Customer ("+name+") is to be placed down on "+destinationPlatform+" in the next level");

                } else {
                    this.WildCardPlatform = true;
                    Console.WriteLine("Customer "+name+" is a wildcard");
                }
                
                this.PickedUpLevel = GameRunning.CurrentLevel;
            }
        }
        
        /// <summary>
        /// Creates a TimedEvent for the customer to spawn. When the timed event is broadcasted
        /// the customer will appear with the Show method.
        /// </summary>
        ///
        /// <returns>
        /// void
        /// </returns>
        private void ShowAfter() {
            GameRunning.instance.customerEvents.AddTimedEvent(
                TimeSpanType.Seconds, spawnAfter, "Show", "Customer", name);
        }

        /// <summary>
        /// Generates the necessary images for the customer.
        /// </summary>
        ///
        /// <returns>
        /// void
        /// </returns>
        private void GenerateImage() {
            imageStandLeft =
                new Image(Path.Combine("Assets", "Images", "CustomerStandLeft.png"));
            imageStandRight =
                new Image(Path.Combine("Assets", "Images", "CustomerStandRight.png"));
            
            shape = new DynamicShape(new Vec2F(), new Vec2F(0.05f,0.08f));
            
            this.entity = new Entity(shape, imageStandLeft);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.TimedEvent, this);
        }

        /// <summary>
        /// Returns the x position in the game.
        /// </summary>
        /// ///
        /// <returns>
        /// void
        /// </returns>
        private float GetPosX() {
            return this.entity.Shape.Position.X;
        }
        
        
        /// <summary>
        /// Moves the customer in the direction its going, if it reached the end of the platform
        /// its walking on, we change direction.
        /// </summary>
        public void WalkCustomer() {
            if (walkingDirection == Direction.WalkRight &&
                GetPosX() >= Platform.GetWidth(this.spawnPlatform)[1]) {
                Console.WriteLine("CHanging direction");
                shape.AsDynamicShape().ChangeDirection(new Vec2F(-0.0005f, 0.0f));
                this.walkingDirection = Direction.WalkLeft;
            } else if (walkingDirection == Direction.WalkLeft &&
                       GetPosX() <= Platform.GetWidth(this.spawnPlatform)[0]) {
                shape.AsDynamicShape().ChangeDirection(new Vec2F(0.0005f, 0.0f));
                this.walkingDirection = Direction.WalkRight;
            }
            shape.Move();
        }

        /// <summary>
        /// Hides the customer, this is used when the customer is picked up by a taxi. This method
        /// also starts the timer for the taxiDuration.
        /// </summary>
        public void Hide() {
            this.visible = false;
            entity.Shape.Extent = new Vec2F(0f, 0f);
            
            //Customer got picked up, starting timer
            GameRunning.instance.customerEvents.AddTimedEvent(
                TimeSpanType.Seconds, taxiDuration, "Travel_Timer", "Customer", name);
        }

        /// <summary>
        /// Shows the customer on the screen.
        /// </summary>
        public void Show() {
            this.visible = true;
            entity.Shape.Extent = new Vec2F(0.05f, 0.08f);
        }

        /// <summary>
        /// Sets the position of the customer.
        /// </summary>
        public void SetPos(Vec2F pos) {
            shape.SetPosition(pos);
        }

        public void HideAfterSuccess() {
            GameRunning.instance.customerEvents.AddTimedEvent(
                TimeSpanType.Seconds, 5, "Success_Timer", "Customer", name);
            this.HasTravled = true;
            this.IsInTransit = false;
        }

        private void EndCustomerLife() {
            entity.Shape.Extent = new Vec2F(0f, 0f);
            this.visible = false;
        }
        
        /// <summary>
        /// Listens for events and invokes methods if message and name matches this customer.
        /// </summary>
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.TimedEvent) {
                switch (gameEvent.Message) {
                    case "Show":
                        if (gameEvent.Parameter2 == name) {
                            this.Show();
                        }
                        break;
                    
                    case "Hide":
                        if (gameEvent.Parameter2 == name) {
                            this.Hide();
                        }
                        break;
                    
                    case "Travel_Timer":
                        if (gameEvent.Parameter2 == name) {
                            this.expiredCustomer = true;
                        }
                        break;
                    case "Success_Timer":
                        if (gameEvent.Parameter2 == name) {
                            this.EndCustomerLife();
                        }
                        break;
                }
            }
        }
        
        /// <summary>
        /// Render the customers entity if it is not in transit and hos not yet travelled.
        /// </summary>
        public void RenderCustomer() {
            if (!this.IsInTransit) {
                entity.RenderEntity();
            }
            
        }
    }
}