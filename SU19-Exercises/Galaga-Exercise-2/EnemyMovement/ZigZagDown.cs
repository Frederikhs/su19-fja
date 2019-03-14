using System;
using DIKUArcade.Entities;
using Galaga_Exercise_2.GalagaEntities.Enemy;
using Galaga_Exercise_2.MovementStrategy;

namespace Galaga_Exercise_2 {
    public class ZigZagDown : IMovementStrategy {
        public void MoveEnemy(Enemy enemy) {
            //Defining static values.
            float s = 0.0003f;
            float p = 0.045f;
            float a = 0.05f;
            
            //Getting the start position
            float startPosX = enemy.StartPos.X;
            float startPosY = enemy.StartPos.Y;
            
            //Moving enemy s points down.
            enemy.Shape.Position.Y -= s;
            
            //Setting downwards wave movement.
            enemy.Shape.Position.X =
                (startPosX + a * (float) Math.Sin(
                     (2.0 * Math.PI * (startPosY - enemy.Shape.Position.Y)
                                    / p
                    )));
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            //Updating position for all enemies.
            foreach (Enemy anEnemy in enemies) {
                MoveEnemy(anEnemy);
            }
        }
    }
}