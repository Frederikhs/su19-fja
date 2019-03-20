using System;
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Galaga_Exercise_3.GalagaEntities.Enemy;
using Galaga_Exercise_3.GalagaStates;

namespace Galaga_Exercise_3 {
    public class ZigZagFormation : ISquadron {
        public EntityContainer<Enemy> Enemies { get; }
        public int MaxEnemies { get; }
        private GameRunning gameRunning;
        
        //Constructor
        public ZigZagFormation(int enemyCount) {
            Console.WriteLine("Called ZigZagFormation");
            //Adding x enemies
            MaxEnemies = enemyCount;
            
            //Creating new container for enemies to reside
            Enemies = new EntityContainer<Enemy>();
        }
        
        public void CreateEnemies(List<Image> enemyStrides) {
            Console.WriteLine("Called CreateEnemies");
            //Creating image strides for the enemies
            ImageStride enemyAnimation = new ImageStride(80,enemyStrides);
            
            //Setting enemy space and position incrementer
            float spaceInc = 0.9f / MaxEnemies;
            float spacePos = spaceInc / 2;
            float yPos = 0.9f;
            
            //For checking if the enemy is up or down
            bool up = true;
            
            //Creating x enemies and placing them in Enemies container
            for (var i = 0; i < MaxEnemies; i++) {
                Enemies.AddDynamicEntity(new Enemy(gameRunning,
                    new DynamicShape(new Vec2F(spacePos, yPos), new Vec2F(0.1f, 0.1f)),
                    enemyAnimation, new Vec2F(spacePos,yPos)));
                spacePos += spaceInc;
                
                //Setting yPos depending on the last pos
                if (up) {
                    yPos -= 0.1f;
                    up = false;
                } else {
                    yPos += 0.1f;
                    up = true;
                }
            }
        }
    }
}