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
        //public DynamicShape actor;
        public EntityContainer<pixel> pixels;
        public readonly ImageStride playerDead;
        public List<Image> playerDeadStrides;
        public Player player;
        public bool hasCollided;
        public bool platform;
        

        public Collisions(EntityContainer<pixel> pixels, Player player) {
            this.player = player;
            this.pixels = pixels;
            
            this.playerDeadStrides = ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png" ));
            this.playerDead = new ImageStride(5000, playerDeadStrides);
            hasCollided = false;
            platform = false;
        }

        /// <summary>
        /// Check if the player shape collides with pixel. If the pixel
        /// is dangerous, we die, else we may change level or sit on the platform
        /// </summary>
        public bool CollisionCheck() {
            foreach (pixel pixel in pixels) {
                bool isCollision = CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(),
                    pixel.Shape.AsDynamicShape()).Collision;
                
                if (isCollision && pixel.danger) {
                    hasCollided = true;
                    return true;
                } else if (isCollision && pixel.portal) {
                    if (GameRunning.CurrentLevel == "the-beach") {
                        SpaceTaxiBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent,
                                this,
                                "CHANGE_STATE",
                                "GAME_RUNNING", "short-n-sweet"));
                    } else if (GameRunning.CurrentLevel == "short-n-sweet") {
                        SpaceTaxiBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent,
                                this,
                                "CHANGE_STATE",
                                "GAME_RUNNING", "the-beach"));
                    }
                    return false;
                    
                } else if (isCollision && !pixel.danger) {
                    if (player.tooFast) {
                        player.SetPosition(pixel.Shape.Position.X,pixel.Shape.Extent.Y);
                        player.platform = true;
                        return false;

                    } else {
                        player.SetPosition(pixel.Shape.Position.X,pixel.Shape.Extent.Y);
                        player.platform = true;
                        return false;
                    }
                }
            }

            return false;
        }
    }
}
        