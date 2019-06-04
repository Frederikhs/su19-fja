using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi {
    public class Points {
        private static int score;
        private Text display;

        public Points(Vec2F position, Vec2F extent) {
            Points.score = 0;
            display = new Text(Points.score.ToString(), position, extent);
            display.SetColor(new Vec3I(255, 0, 0));
        }
        
        /// <summary>
        /// Adds points to the static variable score
        /// </summary>
        ///
        /// <param name="points">
        /// int amount of points to add
        /// </param>
        public static void AddPoints(int points) {
            Points.score += points;
        }

        /// <summary>
        /// Resets the score to 0
        /// </summary>
        public static void ResetPoints() {
            Points.score = 0;
        }

        /// <summary>
        /// Returns the current score
        /// </summary>
        public static int GetPoints() {
            return Points.score;
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
            display.RenderText();
        }
    }
}