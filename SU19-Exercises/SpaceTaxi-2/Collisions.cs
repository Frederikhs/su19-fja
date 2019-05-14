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
        public Player player;

        public Collisions(EntityContainer<pixel> pixels, Player player) {
            this.player = player;
            this.pixels = pixels;
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
                if (collision && pixel.danger) {
                    return true;
                }
                
                //If there is a collision, and the pixel is a portal, we change level
                if (collision && pixel.IsPortal) {
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

                //TODO: Check player speed, and let it sit on a platform is not too fast
                if (collision && pixel.IsPlatform) {
                    if (player.currentSpeed() > 0.005f) {
                        //Player was too fast, Game Over
                        Console.WriteLine("Player was too fast");
                        player.platform = false;
                        CollisionEvents(GameEventType.GameStateEvent, "CHANGE_STATE",
                            "GAME_OVER", "");
                    } else {
                        //Player was not too fast, and can land on platform
                        Console.WriteLine("Player had good speed");
                        var yPos = player.Entity.Shape.Position.Y;
//                        player.SetPosition(pixel.Shape.Position.X, yPos);
                        player.platform = true;
                        return false;
                    }
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