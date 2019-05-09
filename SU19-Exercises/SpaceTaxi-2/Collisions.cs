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
        

        public Collisions(EntityContainer<pixel> pixels, Player player) {
            this.player = player;
            this.pixels = pixels;
            
            this.playerDeadStrides = ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png" ));
            this.playerDead = new ImageStride(500/8, playerDeadStrides);
        }

        public bool CollisionCheck() {
            foreach (pixel pixel in pixels) {
                if (CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(), pixel.Shape.AsDynamicShape()).Collision && pixel.danger) {
                    Console.WriteLine("hej");
                    
                    return true;
                }
            }

            return false;
        }
    }
}
        