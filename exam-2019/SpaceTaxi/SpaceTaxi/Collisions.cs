using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Text;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using SpaceTaxi.GameStates;

namespace SpaceTaxi {
    public class Collisions {
        public EntityContainer<Pixel> pixels;
        public List<Customer> customers;
        public Player player;
        private float speedLimit;
        

        public Collisions(EntityContainer<Pixel> pixels, List<Customer> customers, Player player) {
            this.player = player;
            this.pixels = pixels;
            this.customers = customers;
            speedLimit = 0.005f;
        }

        /// <summary>
        /// Check if the player shape collides with pixel. If the pixel
        /// is dangerous, we die, else we may change level or sit on the platform
        /// </summary>
        public bool CollisionCheck() {
            
            foreach (Pixel pixel in pixels) {
                
                //Bool for if the player collides with an object
                bool collision = CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(),
                    pixel.Shape.AsDynamicShape()).Collision;
                
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
                    if (player.currentSpeed() > speedLimit) {
                        //Player was too fast, Game Over
                        player.platform = false;
                        CollisionEvents(GameEventType.GameStateEvent, "CHANGE_STATE",
                            "GAME_OVER", "DELETE_GAME");
                        break;
                    } else {
                        //Player was not too fast, and can land on platform
                        player.platform = true;
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
        /// <param name="pixel pixel">
        /// pixel for some platform the player has landed on
        /// 
        /// </summary>
        private void CheckLandCustomers(Pixel pixel) {
            foreach (var customer in Player.CustomersInsidePlayer) {
                if (customer.IsInTransit && customer.destinationPlatform ==
                    pixel.PixelChar.ToString() && !customer.HasTravled) {

                    if (customer.DroppedOnSameLevel && customer.PickedUpLevel ==
                        GameRunning.CurrentLevel && !customer.expiredCustomer) {
                        Console.WriteLine("Played down customer on the same level it got picked up");
                        player.PlaceDownCustomer(pixel, customer);
                        customer.SetPos(pixel.shape.Position);
                        customer.Show();
                        
                    } else if (!customer.DroppedOnSameLevel &&
                               customer.PickedUpLevel != GameRunning.CurrentLevel &&
                               !customer.expiredCustomer) {
                        Console.WriteLine("Played down customer on another level");
                        player.PlaceDownCustomer(pixel, customer);
                        
                        
                        customer.SetPos(pixel.shape.Position);
                        customer.Show();
                    } else {
                        Console.WriteLine("Customer expired "+customer.name);
                    }

                } else if (customer.WildCardPlatform && !customer.expiredCustomer && customer.PickedUpLevel != GameRunning.CurrentLevel) {
                    player.PlaceDownCustomer(pixel, customer);
                    
                    customer.SetPos(pixel.shape.Position);
                    customer.Show();
                    Console.WriteLine("Placed down "+customer.name+" on "+pixel.PixelChar);
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
            foreach (var customer in customers) {
                bool collision = CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(),
                    customer.entity.Shape.AsDynamicShape()).Collision;

                if (collision && !customer.IsInTransit) {
                    player.PickUpCustomer(customer);
                }
            }
        }


        public void CollisionEvents(GameEventType eventType, string message, string param1, string param2) {
            SpaceTaxiBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    eventType,
                    this, message,
                    param1, param2));
        }
    }
}