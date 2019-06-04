using System;
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Physics;
using OpenTK;
using SpaceTaxi.GameStates;
using SpaceTaxi.Taxi;

namespace SpaceTaxi {
    public class Collisions {
        public EntityContainer<Pixel> Pixels;
        public List<Customer> Customers;
        public Player Player;
        
        private float speedLimit;
        
        public Collisions(EntityContainer<Pixel> pixels, List<Customer> customers, Player player) {
            this.Player = player;
            this.Pixels = pixels;
            this.Customers = customers;
            speedLimit = 0.005f;
        }

        private bool PixelPlayerCollision(Pixel pixel) {
            return CollisionDetection.Aabb(Player.Entity.Shape.AsDynamicShape(),
                pixel.Shape.AsDynamicShape()).Collision;
        }

        /// <summary>
        /// Check if the player shape collides with pixel. If the pixel
        /// is dangerous, we die, else we may change level or sit on the platform
        /// </summary>
        public bool CollisionCheck() {
            foreach (Pixel pixel in Pixels) {
                
                //Bool for if the player collides with an object
                bool collision = PixelPlayerCollision(pixel);
                
                //If there is a collision, and the pixel is dangerous, we return true 
                if (collision && pixel.Type == Pixel.PixelTypes.Dangerous) {
                    return true;
                }
                
                //If there is a collision, and the pixel is a portal, we change level
                if (collision && pixel.Type == Pixel.PixelTypes.Portal) {
                    CollisionPortal();
                    
                    //The collision should not end the game, so we return false
                    return false;
                }

                if (collision && pixel.Type == Pixel.PixelTypes.Platform) {
                    if (Player.GetPlayerSpeed() > speedLimit) {
                        //Player was too fast, Game Over
                        Player.platform = false;
                        CollisionEvents(GameEventType.GameStateEvent, "CHANGE_STATE",
                            "GAME_OVER", "DELETE_GAME");
                        break;
                    } else {
                        //Player was not too fast, and can land on platform
                        Player.platform = true;
                        CheckLandCustomers(pixel);
                        return false;
                    }
                }

            }

            ShouldPickUp();

            return false;
        }

        /// <summary>
        /// Checks if customer is successfully landed on correct platform based on platform pixel
        /// </summary>
        /// 
        /// <param name="pixel">
        /// pixel for some platform the player has landed on
        /// </param>
        private void CheckLandCustomers(Pixel pixel) {
            if (Player.CustomersInsidePlayer != null) {
                foreach (var customer in Player.CustomersInsidePlayer) {

                    if (customer.CustomerState != CustomerState.Expired) {

                        if (customer.destinationPlatform == pixel.PixelChar.ToString()) {

                            if (customer.DroppedOnSameLevel && customer.PickedUpLevel ==
                                GameRunning.CurrentLevel) {

                                Player.PlaceDownCustomer(pixel, customer);

                            } else if (!customer.DroppedOnSameLevel &&
                                       customer.PickedUpLevel != GameRunning.CurrentLevel &&
                                       customer.CustomerState != CustomerState.Expired) {

                                Player.PlaceDownCustomer(pixel, customer);

                            }

                        } else if (customer.WildCardPlatform &&
                                   customer.CustomerState != CustomerState.Expired &&
                                   customer.PickedUpLevel != GameRunning.CurrentLevel) {

                            Player.PlaceDownCustomer(pixel, customer);

                        }
                    }

                }
            }
        }

        /// <summary>
        /// Switches level from current to the opposite level as we only have two levels.
        /// </summary>
        private void CollisionPortal() {
            switch (GameRunning.CurrentLevel) {
            case "the-beach":
                CollisionEvents(GameEventType.GameStateEvent, "CHANGE_STATE",
                    "GAME_RUNNING", "short-n-sweet");
                break;
            case "short-n-sweet":
                CollisionEvents(GameEventType.GameStateEvent, "CHANGE_STATE",
                    "GAME_RUNNING", "the-beach");
                break;
            }
        }

        /// <summary>
        /// Checks for collision between player and customer.
        /// Calls PickUpCustomer if customer not already in transit
        /// </summary>
        private void ShouldPickUp() {
            foreach (var customer in Customers) {
                bool collision = CollisionDetection.Aabb(Player.Entity.Shape.AsDynamicShape(),
                    customer.entity.Shape.AsDynamicShape()).Collision;

                if (collision && customer.CustomerState == CustomerState.NotPickedUp) {
                    Player.PickUpCustomer(customer);
                }
            }
        }
        
        /// <summary>
        /// Creates events
        /// </summary>
        /// 
        /// <param name="eventType">
        /// The type of event to be broadcasted
        /// </param>
        ///
        /// <param name="message">
        /// The message to be broadcasted
        /// </param>
        ///
        /// <param name="param1">
        /// The 1st parameter
        /// </param>
        ///
        /// <param name="param2">
        /// The 2nd parameter
        /// </param>
        public void CollisionEvents(GameEventType eventType, string message,
            string param1, string param2) {
            
            SpaceTaxiBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    eventType,
                    this, message,
                    param1, param2));
        }
    }
}