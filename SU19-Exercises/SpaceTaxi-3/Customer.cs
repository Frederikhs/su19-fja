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

namespace SpaceTaxi_2.Customer {
    public class Customer : IGameEventProcessor<object> {

        public string name; // Name of the costumer
        public int spawnAfter; // Number of seconds that should pass in the level, before the customer appears (is spawned).
        public char spawnPlatform; // Determining on which platform the customer should be spawned.
        public string destinationPlatform; // Destination platform of the customer
        public string PickedUpLevel; // What place to place down the customer
        public int taxiDuration; // Number of seconds you have to drop off the customer at the correct platform
        public int points; // Number of points a correct drop off of the customer is worth.
        public bool WildCardPlatform;
        public bool DroppedOnSameLevel;
        public bool expiredCustomer;

        public bool visible;

        public bool IsInTransit;
        public bool HasTravled;

        private Image imageStandLeft;
        private Image imageStandRight;

        private float Start;
        private float End;

        public bool OnPath;
        private Direction walkingDirection;

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
            

            shape.AsDynamicShape().ChangeDirection(new Vec2F(0.0005f, 0.0f));
            
            
            Hide();
            ShowAfter();
            FindPlatform(this.destinationPlatform);

            Console.WriteLine("");
            Console.WriteLine("Customer: " + name);
            Console.WriteLine("Destplatform: " + this.destinationPlatform);
            Console.WriteLine("WildCardPlatform: " + WildCardPlatform);
            Console.WriteLine("PickedUpLevel: " + PickedUpLevel);
            Console.WriteLine("");
        }

        private void FindPlatform(string platformWithHat) {
            if (platformWithHat.Contains("^")) {
                if (platformWithHat.Length > 1) {
                    this.destinationPlatform = (platformWithHat.Split('^'))[1];
                    this.PickedUpLevel = GameRunning.CurrentLevel;
                    this.DroppedOnSameLevel = false;

                } else {
                    this.WildCardPlatform = true;
                }
            }
        }

        private void ShowAfter() {
            GameRunning.instance.customerEvents.AddTimedEvent(
                TimeSpanType.Seconds, 1, "Show", "Customer", name);
        }

        private void GenerateImage() {
            imageStandLeft =
                new Image(Path.Combine("Assets", "Images", "CustomerStandLeft.png"));
            imageStandRight =
                new Image(Path.Combine("Assets", "Images", "CustomerStandRight.png"));
            
            shape = new DynamicShape(new Vec2F(), new Vec2F(0.05f,0.08f));
            
            this.entity = new Entity(shape, imageStandLeft);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.TimedEvent, this);
        }

        private float GetPosX() {
            return this.entity.Shape.Position.X;
        }
        
        
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

//            Console.WriteLine("-----\nCustomer: "+name);
//            Console.WriteLine("WalkingDirection: "+walkingDirection);
//            Console.WriteLine("GetPos: "+GetPosX());
//            Console.WriteLine("Start: "+Platform.GetWidth(this.spawnPlatform)[0]);
//            Console.WriteLine("End: "+Platform.GetWidth(this.spawnPlatform)[1]);
//            Console.WriteLine("––––––––––––");
//            shape.Move();
        }
            
            
        
        
        public void Hide() {
            this.visible = false;
            entity.Shape.Extent = new Vec2F(0f, 0f);
            
            //Customer got picked up, starting timer
            GameRunning.instance.customerEvents.AddTimedEvent(
                TimeSpanType.Seconds, taxiDuration, "Travel_Timer", "Customer", name);
        }

        public void Show() {
            this.visible = true;
            entity.Shape.Extent = new Vec2F(0.05f, 0.08f);
        }

        public void SetPos(Vec2F pos) {
            shape.SetPosition(pos);
        }
        
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
                }
            }
        }
        
        public void RenderCustomer() {
            if (!this.IsInTransit && !this.HasTravled) {
                entity.RenderEntity();
            }
        }
    }
}