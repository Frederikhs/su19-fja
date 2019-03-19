using System.Drawing.Text;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Galaga_Exercise_3;

namespace Galaga_Exercise_3.GalagaEntities.Enemy {
    public class Enemy : Entity {
        private Game game;
        private Shape shape;
        public Vec2F StartPos { get; }

        //Enemy constructor
        public Enemy(Game game, DynamicShape shape, IBaseImage image, Vec2F startPos) : base(shape, image) {
            this.game = game;
            StartPos = startPos;
        }
    }
}