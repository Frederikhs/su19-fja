using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Physics;
using SpaceTaxi_2.SpaceTaxiState;
using SpaceTaxi_2.SpaceTaxiStates;
using SpaceTaxiGame;

namespace SpaceTaxi_2 {
    public class Collisions {
        public EntityContainer<pixel> pixels;
        public List<Customer.Customer> customers;
        public Player player;

        public Collisions(EntityContainer<pixel> pixels, List<Customer.Customer> customers, Player player) {
            this.player = player;
            this.pixels = pixels;
            this.customers = customers;
        }

        /// <summary>
        /// Check if the player shape collides with pixel. If the pixel
        /// is dangerous, we die, else we may change level or sit on the platform
        /// </summary>
        public bool CollisionCheck() {
            
            foreach (pixel pixel in pixels) {
                
                //Bool for if the player collides with an object
                bool collision = CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(),
                    pixel.Shape.AsDynamicShape()).Collision;
                
                //If there is a collision, and the pixel is dangerous, we return true 
                if (collision && pixel.type == pixel.pixelTypes.dangerus) {
                    return true;
                }
                
                //If there is a collision, and the pixel is a portal, we change level
                if (collision && pixel.type == pixel.pixelTypes.portal) {
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
                    
                    //The collision should not end the game, so we return false
                    return false;
                }

                if (collision && pixel.type == pixel.pixelTypes.platform) {
                    if (player.currentSpeed() > 0.005f) {
                        //Player was too fast, Game Over
                        player.platform = false;
                        CollisionEvents(GameEventType.GameStateEvent, "CHANGE_STATE",
                            "GAME_OVER", "DELETE_GAME");
                        break;
                    } else {
                        //Player was not too fast, and can land on platform
                        player.platform = true;

                        foreach (var customer in customers) {
                            if (customer.IsInTransit && customer.destinationPlatform ==
                                pixel.pixelChar.ToString() && !customer.HasTravled) {
                                //Placed down customer
                                customer.SetPos(pixel.Shape.Position+pixel.Shape.Extent);
                                customer.Show();
                                Console.WriteLine("player placed down customer ("+customer.name+")");
                                customer.HasTravled = true;
                            }

                            Console.WriteLine("Customer " + customer.name + " should lan on " +
                                              customer.destinationPlatform + ")");
                            Console.WriteLine("Current pixel is: "+pixel.pixelChar);
                        }
                        
                        return false;
                    }
                }

                // TODO: Work in progress
                foreach (var customer in customers) {
                    if (pixel.Shape.Position.X + pixel.Shape.Extent.Y ==
                        customer.entity.Shape.Position.X) {
                        customer.OnPath = true;
                    }
                }
                
                
                
            }

            foreach (var customer in customers) {
                bool collision = CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(),
                    customer.entity.Shape.AsDynamicShape()).Collision;

                if (collision && !customer.IsInTransit) {
                    customer.Hide();
                    customer.IsInTransit = true;
                    Console.WriteLine("player picked up customer ("+customer.name+")");
                }
            }

            return false;
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