using System;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using SpaceTaxiGame;

namespace SpaceTaxi_2.SpaceTaxiStates {
    public class MainMenu : IGameState {
        private static MainMenu instance;
        private Vec3F active;
        private int selectedMenu;
        private Entity backGroundImage;
        private Vec3F inactive;
        private Text[] menuButtons;

        public MainMenu() {
            InitializeGameState();
        }

        public void RenderState() {
            //Render background image
            backGroundImage.RenderEntity();

            //Render each menu button
            foreach (var aButton in menuButtons) {
                aButton.RenderText();
            }
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
        /// Selects the next button down, if we are on the buttom,
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
                    //If New Game is chosen, we change the state
                    case 0:
                        SpaceTaxiBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent,
                                this,
                                "CHANGE_STATE",
                                "GAME_LEVEL_PICKER", ""));
                        break;
                    //If Quit is chosen we close the window
                    case 1:
                        SpaceTaxiBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.WindowEvent,
                                this,
                                "CLOSE_WINDOW",
                                "", ""));
                        break;
                    }

                    break;
                }
            }
        }
        
        public void GameLoop() { }

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
                new Text("New Game", new Vec2F(0.2f, 0.2f), new Vec2F(0.5f, 0.3f)),
                new Text("Quit", new Vec2F(0.2f, 0.1f), new Vec2F(0.3f, 0.3f))
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
        }

        //Not used
        public void UpdateGameLogic() {
            SpaceTaxiBus.GetBus().ProcessEvents();
        }

        /// <summary>
        /// If Main Menu is called with delete game,
        /// it will delete the running game, else just
        /// return new MainMenu.
        /// </summary>
        public static MainMenu GetInstance(string param2) {
            var running = MainMenu.instance;
            if (param2 == "DELETE_GAME") {
                if (running != null) {
                    GameRunning.instance = null;
                    Console.WriteLine("Player collided with obstacle and died!");
                    return running;
                } else {
                    MainMenu.instance = new MainMenu();
                    return MainMenu.instance;
                }
            }
            MainMenu.instance = new MainMenu();
            return MainMenu.instance;
        }
    }
}