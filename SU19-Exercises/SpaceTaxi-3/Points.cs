using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_3 {
    public class Points {
        private Text display;
        private static int score;

        public Points(Vec2F position, Vec2F extent) {
            Points.score = 0;
            display = new Text(Points.score.ToString(), position, extent);
        }
        
        public static void AddPoints(int points) {
            Points.score += points;
        }

        //Display the score
        public void RenderScore() {
            display.SetText(string.Format("Score: {0}", Points.score.ToString()));
            display.SetColor(new Vec3I(255, 0, 0));
            display.RenderText();
        }
    }
}