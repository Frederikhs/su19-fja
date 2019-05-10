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

        //Colors
        private Vec3F active;
        private int activeMenuButton;

        private Entity backGroundImage;
        private Vec3F inactive;
        private int maxMenuButtons;
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

        public void HandleKeyEvent(string keyValue, string keyAction) {
            if (keyAction == "KEY_PRESS") {
                switch (keyValue) {
                case "KEY_UP":
                    //Setting inactive
                    menuButtons[1].SetColor(inactive);
                    menuButtons[1].SetFontSize(50);

                    //setting active
                    menuButtons[0].SetColor(active);
                    menuButtons[0].SetFontSize(70);
                    activeMenuButton = 0;
                    break;

                case "KEY_DOWN":
                    //Setting inactive
                    menuButtons[0].SetColor(inactive);
                    menuButtons[0].SetFontSize(50);

                    //setting active
                    menuButtons[1].SetColor(active);
                    menuButtons[1].SetFontSize(70);
                    activeMenuButton = 1;
                    break;

                case "KEY_ENTER":
                    switch (activeMenuButton) {
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

        //Not used
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
            maxMenuButtons = menuButtons.Length;
            activeMenuButton = 1;

            //Iterating over buttons and setting their default color and size
            foreach (var button in menuButtons) {
                button.SetColor(inactive);
                button.SetFontSize(50);
            }

            //Setting color and font size of active button
            menuButtons[activeMenuButton].SetColor(active);
            menuButtons[activeMenuButton].SetFontSize(70);
        }

        //Not used
        public void UpdateGameLogic() {
            SpaceTaxiBus.GetBus().ProcessEvents();
        }

        //Return an instance, or creates a new one
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