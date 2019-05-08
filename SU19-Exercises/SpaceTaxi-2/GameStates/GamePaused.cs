using System;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using SpaceTaxiGame;

namespace SpaceTaxi_2.SpaceTaxiStates {
    public class GamePaused : IGameState {
        private static GamePaused instance;

        //Colors
        private Vec3F active;

        private Entity backGroundImage;
        private Vec3F inactive;
        private int maxMenuButtons;
        private Text[] menuButtons;
        public string PausedLevel;
        private int selectedMenu;

        public GamePaused(string pausedLevel) {
            InitializeGameState();
            this.PausedLevel = pausedLevel;
            GamePaused.instance = this;
        }

        public void RenderState() {
            //Render background image
            backGroundImage.RenderEntity();

            //Render each menu button
            foreach (var aButton in menuButtons) {
                aButton.RenderText();
            }
        }
        
        private void SetActiveMenu() {
            foreach (var button in menuButtons) {
                button.SetColor(inactive);
                button.SetFontSize(50);
            }
            menuButtons[selectedMenu].SetColor(active);
            menuButtons[selectedMenu].SetFontSize(70);
            Console.WriteLine("Active Menu button:"+selectedMenu);
        }

        private void UpMenu() {
            if (selectedMenu + 1 < menuButtons.Length) {
                selectedMenu++;
            } else {
                selectedMenu = menuButtons.Length -1 ;
            }
            SetActiveMenu();
        }

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
                    case 0:
                        SpaceTaxiBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent,
                                this,
                                "CHANGE_STATE",
                                "MAIN_MENU", ""));
                        break;
                    
                    case 1:
                        SpaceTaxiBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent,
                                this,
                                "CHANGE_STATE",
                                "GAME_RUNNING", this.PausedLevel));
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
                new Text("Main Menu", new Vec2F(0.2f, 0.1f), new Vec2F(0.3f, 0.3f)),
                new Text("Continue", new Vec2F(0.2f, 0.2f), new Vec2F(0.5f, 0.3f))
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
        public void UpdateGameLogic() { }

        //Return an instance, or creates a new one
        public static GamePaused GetInstance(string PausedLevel) {
            var running = GamePaused.instance;
            if (running != null) {
                if (running.PausedLevel != PausedLevel) {
                    return new GamePaused(PausedLevel);
                } else {
                    return running;
                }
            } else {
                return new GamePaused(PausedLevel);
            }
        }
    }
}