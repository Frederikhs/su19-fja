using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Physics;

namespace SpaceTaxi_2 {
    public class Collisions {
        private bool collisionHappened;

        public Collisions(DynamicShape actor, GraphicsGenerator gen) {
            collisionHappened = false;
            var pixelContainer = gen.AllGraphics;
            //foreach (var pixel in pixelContainer) {
            //    if (CollisionDetection.Aabb(actor, pixel.Shape).Collision) {
            //        collisionHappened = true;
            //    }
            //}
        }
    }
}
        