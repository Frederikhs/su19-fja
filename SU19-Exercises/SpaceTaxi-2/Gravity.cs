using System;
using DIKUArcade.Math;

namespace SpaceTaxi_2 {
    public class Gravity {

        private float gravityForce;
        private float Maxgravity;
        private float vCurrent;
        public float MaxSpeed;
        private Player player;
        public bool platform;

        public Gravity(Player player) {
            gravityForce = -0.000001f;
            Maxgravity = -0.01f;
            vCurrent = 0;
            MaxSpeed = 0.001f;
            this.player = player;
        }

        /// <summary>
        /// Calculates next velocity y vector based on the truster value,
        /// and the current velocity.
        /// </summary>
        public float NextVel(float truster, bool platform) {
            if (!platform) {
                var DeltaT = 60.0f;
                var Force = gravityForce + truster;

                if (vCurrent <= Maxgravity) {
                    vCurrent = Maxgravity + truster;
                } else {
                    if (vCurrent >= MaxSpeed) {
                        vCurrent = MaxSpeed + gravityForce;
                    } else {
                        vCurrent += Force * DeltaT;
                    }
                }

                return vCurrent;
            } else {
                vCurrent = 0.0f;
                return vCurrent;
            }


        }

    }
}