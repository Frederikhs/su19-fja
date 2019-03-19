using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_3 {
    public class Score {
        private Text display;
        private int score;

        public Score(Vec2F position, Vec2F extent) {
            score = 0;
            display = new Text(score.ToString(), position, extent);
        }

        //Add 1 point
        public void AddPoint() {
            score += 1;
        }

        //Display the score
        public void RenderScore() {
            display.SetText(string.Format("Score: {0}", score.ToString()));
            display.SetColor(new Vec3I(255, 0, 0));
            display.RenderText();
        }

        //Show if game ended
        public void GameOver() {
            display.SetText("Game over");
            display.SetColor(new Vec3I(0, 255, 0));
            display.RenderText();
        }
    }
}