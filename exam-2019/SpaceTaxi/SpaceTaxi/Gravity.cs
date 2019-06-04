using System;
using DIKUArcade.Math;

namespace SpaceTaxi {
    public class Gravity {
        public float CurrentVelocity { get; private set; }
        private float maxSpeed;
        
        private float gravityForce;
        private float maxGravity;

        /// <summary>
        /// Sets velocity and speed values
        /// </summary>
        public Gravity() {
            gravityForce = -0.000004f;
            maxGravity = -0.00015f;
            CurrentVelocity = 0;
            maxSpeed = 0.00002f;
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
        public float GetNextVelocity(float thruster, bool platform) {
            if (!platform) {
                var deltaT = 60.0f;
                var force = gravityForce + thruster;

                if (CurrentVelocity <= maxGravity) {
                    CurrentVelocity = maxGravity + thruster;
                } else {
                    if (CurrentVelocity >= maxSpeed) {
                        CurrentVelocity = maxSpeed + gravityForce;
                    } else {
                        CurrentVelocity += force * deltaT;
                    }
                }

                return CurrentVelocity;
            } else {
                CurrentVelocity = 0.0f;
                return CurrentVelocity;
            }

        }

    }
}