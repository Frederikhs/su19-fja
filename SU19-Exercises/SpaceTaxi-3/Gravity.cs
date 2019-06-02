using System;
using DIKUArcade.Math;

namespace SpaceTaxi_2 {
    public class Gravity {

        private float gravityForce;
        private float Maxgravity;
        public float vCurrent { get; private set; }
        public float MaxSpeed;
        public bool platform;

        public Gravity() {
            gravityForce = -0.000004f;
            Maxgravity = -0.00015f;
            vCurrent = 0;
            MaxSpeed = 0.00002f;
        }
        
        /// <summary>
        /// Calculates next velocity y vector based on the thruster value,
        /// and the current velocity.
        /// </summary>
        ///
        /// <param name="thruster">
        /// float value for the thruster
        /// </param>
        ///
        /// <param name="platform">
        /// bool if the player is on a platform it will not change velocity
        /// </param>
        ///
        /// <returns>
        /// float next velocity
        /// </returns>
        public float NextVel(float thruster, bool platform) {
            if (!platform) {
                var DeltaT = 60.0f;
                var Force = gravityForce + thruster;

                if (vCurrent <= Maxgravity) {
                    vCurrent = Maxgravity + thruster;
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