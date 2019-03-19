using System;
using System.CodeDom;
using System.Drawing;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.State;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using GalagaGame.GalagaState;
using Galaga_Exercise_3;
using Galaga_Exercise_3.GalagaStates;
using Image = DIKUArcade.Graphics.Image;
using DIKUArcade.Window;

namespace Galaga_Exercise_3.GalagaStates {
    public class MainMenu {
        using DIKUArcade.State;

    namespace GalagaGame.GalagaState {
        public class MainMenu : IGameState {
            private static MainMenu instance = null;
            private Window win;

            private Entity backGroundImage;
            private Text[] menuButtons;
            private int activeMenuButton;
            private int maxMenuButtons;


            public static MainMenu GetInstance() {
                return MainMenu.instance ?? (MainMenu.instance = new MainMenu());
            }

            public void RenderState() {
                backGroundImage.Image =
                    new Image(Path.Combine("Assets", "Images", "TitleImage.png"));
                menuButtons[0].SetText("New Game");
                menuButtons[1].SetText("Quit");

                backGroundImage.RenderEntity();
                menuButtons[0].RenderText();
                menuButtons[1].RenderText();
            }

            public void HandleKeyEvent(string keyValue, string keyAction) {
                activeMenuButton = 0;
                maxMenuButtons = 1;
                menuButtons[0].SetColor(Color.Red);
                switch (keyAction) {
                case "KEY_PRESS":
                        switch (keyValue) {
                        case "KEY_UP":
                            if (activeMenuButton < maxMenuButtons) {
                                menuButtons[1].SetColor(Color.Red);
                                activeMenuButton = 1;
                            }   
                            break;
                        case "KEY_DOWN":
                            if (activeMenuButton > 0) {
                                menuButtons[0].SetColor(Color.Red);
                                activeMenuButton = 0;
                            }
                            break;
                        case "ENTER":
                            if (activeMenuButton == 0) {
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.GameStateEvent,
                                    this,
                                    "CHANGE_STATE",
                                    "GAME_RUNNING", "");
                                break;
                            }
    
                            if (activeMenuButton == 1) {
                                win.CloseWindow();                            
                            }
    
                            break;
                        }

                    break;

                }

            }

            public void GameLoop() {
                throw new NotImplementedException();
            }

            public void InitializeGameState() {
                throw new NotImplementedException();
            }


            public void UpdateGameLogic() {
                throw new NotImplementedException();
            }
        }
    }
}