using DIKUArcade.Entities;
using DIKUArcade.Math;
using Galaga_Exercise_3.GalagaEntities.Enemy;
using Galaga_Exercise_3.MovementStrategy;

namespace Galaga_Exercise_3 {
    public class Down : IMovementStrategy {
        public void MoveEnemy(Enemy enemy) {
            //Setting downwards movement.
            enemy.Shape.AsDynamicShape().ChangeDirection(new Vec2F(0.0f,-0.001f));
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            //Updating position for all enemies.
            foreach (Enemy anEnemy in enemies) {
                MoveEnemy(anEnemy);
            }
        }
    }
}