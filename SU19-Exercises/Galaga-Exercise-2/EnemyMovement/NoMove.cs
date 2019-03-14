using DIKUArcade.Entities;
using Galaga_Exercise_2.GalagaEntities.Enemy;
using Galaga_Exercise_2.MovementStrategy;

namespace Galaga_Exercise_2 {
    public class NoMove : IMovementStrategy {
        public void MoveEnemy(Enemy enemy) {
            //Does nothing
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            //Does nothing
        }
    }
}