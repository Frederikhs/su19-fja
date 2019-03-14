using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_2 {
    public class Score {
        private Text display;
        private int score;

        public Score(Vec2F position, Vec2F extent) {
            score = 0;
            display = new Text(score.ToString(), position, extent);
        }

        //ADD 1 POINT TO SCORE EACH TIME CALLED
        public void AddPoint() {
            score += 1;
        }

        //SHOW THE SCORE
        public void RenderScore() {
            display.SetText(string.Format("Score: {0}", score.ToString()));
            display.SetColor(new Vec3I(255, 0, 0));
            display.RenderText();
        }

        //SHOW WHEN GAME HAS ENDED
        public void GameOver() {
            display.SetText("Game over");
            display.SetColor(new Vec3I(0, 255, 0));
            display.RenderText();
        }
    }
}