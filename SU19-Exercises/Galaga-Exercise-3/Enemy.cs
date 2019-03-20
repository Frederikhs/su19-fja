using System.Drawing.Text;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Galaga_Exercise_3;
using Galaga_Exercise_3.GalagaStates;

namespace Galaga_Exercise_3.GalagaEntities.Enemy {
    public class Enemy : Entity {
        private GameRunning gameRunning;
        private Shape shape;
        public Vec2F StartPos { get; }

        //Enemy constructor
        public Enemy(GameRunning gameRunning, DynamicShape shape, IBaseImage image, Vec2F startPos) : base(shape, image) {
            this.gameRunning = gameRunning;
            StartPos = startPos;
        }
    }
}