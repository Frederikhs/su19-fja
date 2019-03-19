using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Galaga_Exercise_3.GalagaEntities.Enemy;

namespace Galaga_Exercise_3 {
    public class LineFormation : ISquadron {
        public EntityContainer<Enemy> Enemies { get; }
        public int MaxEnemies { get; }
        private Game Game;
        
        //Constructor
        public LineFormation(int enemyCount) {
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
            
            //Creating x enemies and placing them in Enemies container
            for (var i = 0; i < MaxEnemies; i++) {
                Enemies.AddDynamicEntity(new Enemy(Game,
                    new DynamicShape(new Vec2F(spacePos, 0.9f), new Vec2F(0.1f, 0.1f)),
                    enemyAnimation, new Vec2F(spacePos,0.9f)));
                spacePos += spaceInc;
            }
        }
    }
}