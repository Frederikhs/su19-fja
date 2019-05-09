using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Physics;
using SpaceTaxi_2.SpaceTaxiState;
using SpaceTaxi_2.SpaceTaxiStates;

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

        public bool CollisionCheck() {
            foreach (pixel pixel in pixels) {
                if (CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(), pixel.Shape.AsDynamicShape()).Collision && pixel.danger) {
                    
                    hasCollided = true;
                    return true;
                } else if (CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(), pixel.Shape.AsDynamicShape()).Collision && !pixel.danger) {
                    if (player.tooFast) {
                        player.SetPosition(pixel.Shape.Position.X,pixel.Shape.Extent.Y);
                        player.platform = true;
                        return true;

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
        