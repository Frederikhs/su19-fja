using System;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;
using SpaceTaxi.GameStates;
using SpaceTaxi.Taxi;

namespace SpaceTaxi {

    public enum CustomerState {
        ToBeDisplayed, //While the timer runs to display it
        NotPickedUp, // While the customer is on a platform waiting
        InTransit, // While the customer is inside the taxi
        Expired, // While the customers travel timer expired
        Delivered, // While the customer is delivered
        Finished // While the customer is not on screen anymore and has been delivered
    }
    
    public class Customer : IGameEventProcessor<object> {
        public Entity entity { get; private set; }
        public char spawnPlatform; // Determining on which platform the customer should be spawned.
        public string destinationPlatform; // Destination platform of the customer.
        public string PickedUpLevel; // What place to place down the customer.
        public bool WildCardPlatform; //If the customer can be dropped off on any platform.
        public bool DroppedOnSameLevel; //If the customer should be dropped off on the same level.
        public CustomerState CustomerState;

        private int points; // Number of points a correct drop off of the customer is worth.
        private string name; // Name of the customer.
        private int spawnAfter; // Number of seconds that should pass in the level, before it appear.
        private int taxiDuration; // Seconds you have to drop off the customer at the correct platform.
        
        private Image imageStandLeft;
        private Image imageStandRight;

        private Shape shape;

        public Customer(string name, int spawnAfter, char spawnPlatform, string destinationPlatform,
            int taxiDuration, int points) {
            
            this.name = name;
            this.spawnAfter = spawnAfter;
            this.spawnPlatform = spawnPlatform;
            this.destinationPlatform = destinationPlatform;
            this.taxiDuration = taxiDuration;
            this.points = points;
            this.WildCardPlatform = false;
            GenerateImage();
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.TimedEvent, this);
            SwitchState(CustomerState.ToBeDisplayed);
            FindPlatform(this.destinationPlatform);
        }

        /// <summary>
        /// Changes the customers state
        /// </summary>
        ///
        /// <param name="state">
        /// The state to switch to 
        /// </param>
        public void SwitchState(CustomerState state) {
            switch (state) {
                case CustomerState.ToBeDisplayed:
                    ShowAfter();
                    break;
                
                case CustomerState.NotPickedUp:
                    Console.WriteLine("{0} is ready to be picked up",name);
                    DisplayCustomer();
                    break;
                
                case CustomerState.InTransit:
                    Console.WriteLine(
                        "{0} was picked up, you now have {1} seconds to deliver",name,taxiDuration);
                    GotPickedUp();
                    break;
                
                case CustomerState.Expired:
                    Console.WriteLine("{0} expired",name);
                    break;
                
                case CustomerState.Delivered:
                    Console.WriteLine("{0} has been delivered",name);
                    DisplayCustomer();
                    PlaceDownHideAfter();
                    Points.AddPoints(points);
                    break;
                
                case CustomerState.Finished:
                    Console.WriteLine("{0} is no longer in the game",name);
                    EndCustomerLife();
                    break;
            }
            CustomerState = state;
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
        private void FindPlatform(string platformWithHat) {
            if (platformWithHat.Contains("^")) {
                if (platformWithHat.Length > 1) {
                    this.destinationPlatform = (platformWithHat.Split('^'))[1];
                    this.DroppedOnSameLevel = false;
                    Console.WriteLine(
                        "{0} is to be placed down on {1} in the next level",name,destinationPlatform);

                } else {
                    this.WildCardPlatform = true;
                    Console.WriteLine(
                        "{0} is a wildcard, and can be placed anywhere on the next level",name);
                }

                if (GameRunning.CurrentLevel != null) {
                    this.PickedUpLevel = GameRunning.CurrentLevel;
                } else {
                    this.PickedUpLevel = "";
                }
            } else {
                Console.WriteLine("{0} is to be placed down on {1}",name,destinationPlatform);
            }
        }
        
        /// <summary>
        /// Creates a TimedEvent for the customer to spawn. When the timed event is broadcasted
        /// the customer will appear with the DisplayCustomer method.
        /// </summary>
        private void ShowAfter() {
            GameRunning.Instance.CustomerEvents.AddTimedEvent(
                TimeSpanType.Seconds, spawnAfter, "Show", "Customer", name);
        }

        /// <summary>
        /// Generates the necessary images for the customer.
        /// </summary>
        private void GenerateImage() {
            imageStandLeft =
                new Image(Path.Combine("Assets", "Images", "CustomerStandLeft.png"));
            imageStandRight =
                new Image(Path.Combine("Assets", "Images", "CustomerStandRight.png"));
            
            shape = new DynamicShape(new Vec2F(), new Vec2F(0.05f,0.08f));
            
            this.entity = new Entity(shape, imageStandLeft);
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
        /// Hides the customer, this is used when the customer is picked up by a taxi. This method
        /// also starts the timer for the taxiDuration.
        /// </summary>
        public void GotPickedUp() {
            entity.Shape.Extent = new Vec2F(0f, 0f);
            
            //Customer got picked up, starting timer
            GameRunning.Instance.CustomerEvents.AddTimedEvent(
                TimeSpanType.Seconds, taxiDuration, "Travel_Timer", "Customer", name);
        }

        /// <summary>
        /// Shows the customer on the screen.
        /// </summary>
        public void DisplayCustomer() {
            entity.Shape.Extent = new Vec2F(0.05f, 0.08f);
        }

        /// <summary>
        /// Sets the position of the customer.
        /// </summary>
        public void SetPos(Vec2F pos) {
            shape.SetPosition(pos);
        }

        /// <summary>
        /// After the customer has been placed, we hide the customer after 5 seconds
        /// </summary>
        public void PlaceDownHideAfter() {
            GameRunning.Instance.CustomerEvents.AddTimedEvent(
                TimeSpanType.Seconds, 5, "Success_Timer", "Customer", name);
        }

        /// <summary>
        /// Ends the customer, removes it from the customer inside player list, and set extend to 0
        /// </summary>
        private void EndCustomerLife() {
            Player.RemoveCustomerFromList(this);
            entity.Shape.Extent = new Vec2F(0f, 0f);
        }
        
        /// <summary>
        /// Listens for events and invokes methods if message and name matches this customer.
        /// </summary>
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.TimedEvent && gameEvent.Parameter2 == name) {
                switch (gameEvent.Message) {
                    
                    case "Show":
                        SwitchState(CustomerState.NotPickedUp);
                        break;
                    
                    case "Travel_Timer":
                        SwitchState(CustomerState.Expired);
                        break;
                    
                    case "Success_Timer":
                        SwitchState(CustomerState.Finished);
                        break;
                }
            }
        }
        
        /// <summary>
        /// Render the customers entity
        /// </summary>
        public void RenderCustomer() {
            if (CustomerState == CustomerState.NotPickedUp ||
                CustomerState == CustomerState.Delivered) {
                entity.RenderEntity();
            }
            
        }
    }
}