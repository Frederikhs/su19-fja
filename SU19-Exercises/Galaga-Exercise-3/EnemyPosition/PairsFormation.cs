using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Galaga_Exercise_3.GalagaEntities.Enemy;
using Galaga_Exercise_3.GalagaStates;

namespace Galaga_Exercise_3 {
    public class PairsFormation : ISquadron {
        public EntityContainer<Enemy> Enemies { get; }
        public int MaxEnemies { get; }
        private GameRunning gameRunning;
        
        //Constructor
        public PairsFormation(int enemyCount) {
            //Adding x enemies
            MaxEnemies = enemyCount;
            
            //Creating new container for enemies to reside
            Enemies = new EntityContainer<Enemy>();
        }
        
        public void CreateEnemies(List<Image> enemyStrides) {
            //Creating image strides for the enemies
            ImageStride enemyAnimation = new ImageStride(80,enemyStrides);
            
            //Setting enemy space and position incrementer
            float spaceInc = 0.9f / MaxEnemies;
            float spacePos = spaceInc / 2;
            float yPos = 0.0f;
            
            //Need neighbor
            bool hasBuddy = false;
            
            //Creating x enemies and placing them in Enemies container
            for (var i = 0; i < MaxEnemies; i++) {
                if (hasBuddy) {
                    yPos = 0.7f;
                    hasBuddy = false;
                } else {
                    yPos = 0.9f;
                    spacePos += spaceInc;
                    hasBuddy = true;
                }
                
                //Placing enemy at a position
                Enemies.AddDynamicEntity(new Enemy(gameRunning,
                    new DynamicShape(new Vec2F(spacePos, yPos), new Vec2F(0.1f, 0.1f)),
                    enemyAnimation, new Vec2F(spacePos,yPos)));
            }
        }
    }
}