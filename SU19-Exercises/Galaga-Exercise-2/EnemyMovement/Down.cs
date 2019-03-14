using DIKUArcade.Entities;
using DIKUArcade.Math;
using Galaga_Exercise_2.GalagaEntities.Enemy;
using Galaga_Exercise_2.MovementStrategy;

namespace Galaga_Exercise_2 {
    public class Down : IMovementStrategy {
        public void MoveEnemy(Enemy enemy) {
            enemy.Shape.AsDynamicShape().ChangeDirection(new Vec2F(0.0f,-0.001f));
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy anEnemey in enemies) {
                MoveEnemy(anEnemey);
            }
        }
    }
}