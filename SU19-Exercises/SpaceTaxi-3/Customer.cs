using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using SpaceTaxiGame;

namespace SpaceTaxi_2.Customer {
    public class Customer : IGameEventProcessor<object> {

        public string name; // Name of the costumer
        public int spawnAfter; // Number of seconds that should pass in the level, before the customer appears (is spawned).
        public char spawnPlatform; // Determining on which platform the customer should be spawned.
        public string destinationPlatform; // Destination platform of the customer
        public int taxiDuration; // Number of seconds you have to drop off the customer at the correct platform
        public int points; // Number of points a correct drop off of the customer is worth.

        public bool visible;

        private Image imageStandLeft;
        private Image imageStandRight;

        private enum Direction {
            StandLeft,
            StandRight
        }

        private Entity entity;
        private Shape shape;

        public Customer(string name, int spawnAfter, char spawnPlatform, string destinationPlatform,
            int taxiDuration, int points) {
            this.name = name;
            this.spawnAfter = spawnAfter;
            this.spawnPlatform = spawnPlatform;
            this.destinationPlatform = destinationPlatform;
            this.taxiDuration = taxiDuration;
            this.points = points;

            this.visible = false;

            GenerateImage();
        }

        private void GenerateImage() {
            imageStandLeft =
                new Image(Path.Combine("Assets", "Images", "CustomerStandLeft.png"));
            imageStandRight =
                new Image(Path.Combine("Assets", "Images", "CustomerStandRight.png"));
            
            shape = new DynamicShape(new Vec2F(), new Vec2F());
            
            this.entity = new Entity(shape, imageStandLeft);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.TimedEvent, this);
        }
        
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.TimedEvent) {
                switch (gameEvent.Message) {
                case "Appear":
                    // TODO: Implement
                    break;
                }
            }
        }
    }
}