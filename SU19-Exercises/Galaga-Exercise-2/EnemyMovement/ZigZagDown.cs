using System;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using Galaga_Exercise_2.GalagaEntities.Enemy;
using Galaga_Exercise_2.MovementStrategy;

namespace Galaga_Exercise_2 {
    public class ZigZagDown : IMovementStrategy {
        public void MoveEnemy(Enemy enemy) {
            var currentPosY = enemy.StartPos.X;
//            var currentPosY = enemy.Shape.Position.Y;
            var nextPosY = currentPosY + 0.0003f;

//            var currentPosX = enemy.Shape.Position.X;
            var currentPosX = enemy.StartPos.Y;
            var nextPosX = currentPosX +
                           0.05f * Math.Sin(
                               (
                                   2 * Math.PI * (currentPosY - nextPosY)
                                            /
                                            0.045f));
            
//            enemy.Shape.AsDynamicShape().ChangeDirection(new Vec2F(0.0f,-0.001f));
            //enemy.Position(new Vec2F(nextPosY, ((float)nextPosX)));
            
            
            enemy.Shape.Move(new Vec2F(nextPosY-currentPosY, ((float)nextPosX-(float)currentPosX)));
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy anEnemey in enemies) {
                MoveEnemy(anEnemey);
            }
        }
    }
}