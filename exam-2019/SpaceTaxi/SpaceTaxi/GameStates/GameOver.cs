using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using SpaceTaxi.Taxi;

namespace SpaceTaxi.GameStates {
    public class GameOver : IGameState {
        private static GameOver instance;
        private Vec3F active;
        private Entity backGroundImage;
        private Vec3F inactive;
        private Text[] menuButtons;
        private int selectedMenu;
        private Text title;

        /// <summary>
        /// Initialized the game state for game over
        /// </summary>
        public GameOver() {
            InitializeGameState();
        }

        /// <summary>
        /// Render the game over elements. (background and buttons)
        /// </summary>
        public void RenderState() {
            backGroundImage.RenderEntity();
            foreach (var button in menuButtons) {
                button.RenderText();
            }
            title.RenderText();
        }

        
        /// <summary>
        /// Sets the selectedMenu to the active menu,
        /// with color and font size
        /// </summary>
        private void SetActiveMenu() {
            foreach (var button in menuButtons) {
                button.SetColor(inactive);
                button.SetFontSize(50);
            }
            menuButtons[selectedMenu].SetColor(active);
            menuButtons[selectedMenu].SetFontSize(70);
        }

        /// <summary>
        /// Selects the next button up, if we are on top,
        /// select the same again
        /// </summary>
        private void UpMenu() {
            if (selectedMenu + 1 < menuButtons.Length) {
                selectedMenu++;
            } else {
                selectedMenu = menuButtons.Length - 1;
            }

            SetActiveMenu();
        }

        /// <summary>
        /// Selects the next button down, if we are on the button,
        /// select the same again
        /// </summary>
        private void DownMenu() {
            if (selectedMenu - 1 > 0) {
                selectedMenu--;
            } else {
                selectedMenu = 0;
            }

            SetActiveMenu();
        }

        /// <summary>
        /// Handles key events that the user presses
        /// </summary>
        ///
        /// <param name="keyValue">
        /// The key value of the pressed key, i.e. "KEY_UP", "KEY_DOWN"
        /// </param>
        ///
        /// <param name="keyAction">
        /// The key motion, i.e. "KEY_PRESS", "KEY_RELEASE" 
        /// </param>
        ///
        /// <returns>
        /// void
        /// </returns>
        public void HandleKeyEvent(string keyValue, string keyAction) {
            if (keyAction == "KEY_PRESS") {
                switch (keyValue) {
                case "KEY_UP":
                    UpMenu();
                    break;

                case "KEY_DOWN":
                    DownMenu();
                    break;

                case "KEY_ENTER":
                    switch (selectedMenu) {
                    // If Main Menu is chosen we return to main menu
                        case 0: // Main menu
                            SpaceTaxiBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.GameStateEvent,
                                    this,
                                    "CHANGE_STATE",
                                    "MAIN_MENU", "DELETE_GAME"));
                            break;
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Not used
        /// </summary>
        public void GameLoop() { }

        /// <summary>
        /// Initializes the required elements for game over
        /// </summary>
        public void InitializeGameState() {
            //Creating colors
            active = new Vec3F(0.0f, 1.0f, 0.0f); // Green
            inactive = new Vec3F(1.0f, 0.0f, 0.0f); // Red

            //Create background image entity, fills entire screen
            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f),
                    new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));

            //Creating new array and adding buttons to it.
            menuButtons = new[] {
                new Text("Main Menu", new Vec2F(0.2f, 0.1f), new Vec2F(0.3f, 0.3f)),
            };

            //Setting button vars
            selectedMenu = 0;

            //Iterating over buttons and setting their default color and size
            foreach (var button in menuButtons) {
                button.SetColor(inactive);
                button.SetFontSize(50);
            }

            //Setting color and font size of active button
            menuButtons[selectedMenu].SetColor(active);
            menuButtons[selectedMenu].SetFontSize(70);
            
            //Game over display
            title = new Text("Game Over", new Vec2F(0.2f, 0.3f), new Vec2F(0.5f, 0.5f));
            title.SetColor(inactive);
            title.SetFontSize(70);
        }

        /// <summary>
        /// Not used
        /// </summary>
        public void UpdateGameLogic() { }

        /// <summary>
        /// Return static instance, or creates a new one
        /// </summary>
        public static GameOver GetInstance() {
            //Reset the score, since the game is over
            Points.ResetPoints();
            //Also remove all customers who was in the taxi
            Player.ClearCustomers();
            
            return GameOver.instance ?? (GameOver.instance = new GameOver());
        }

    }
}