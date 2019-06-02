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
        
        /// <summary>
        /// Adds points to the static variable score
        /// </summary>
        ///
        /// <param name="points">
        /// int amount of points to add
        /// </param>
        ///
        /// <returns>
        /// void
        /// </returns>
        public static void AddPoints(int points) {
            Points.score += points;
        }

        /// <summary>
        /// Renders the score on the screen
        /// </summary>
        ///
        /// <returns>
        /// void
        /// </returns>
        public void RenderScore() {
            display.SetText(string.Format("Score: {0}", Points.score.ToString()));
            display.SetColor(new Vec3I(255, 0, 0));
            display.RenderText();
        }
    }
}