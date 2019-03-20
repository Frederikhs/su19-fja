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

using DIKUArcade.State;

    namespace GalagaGame.GalagaState {
        public class MainMenu : IGameState {
            private static MainMenu instance;


            private Entity backGroundImage;
            private Text[] menuButtons;
            private int activeMenuButton;
            private int maxMenuButtons;


            public static MainMenu GetInstance() {
                return MainMenu.instance ?? (MainMenu.instance = new MainMenu());
            }

            public MainMenu() {
                InitializeGameState();
            }

            public void RenderState() {


                backGroundImage.RenderEntity();

                foreach (var mb in menuButtons) {
                    mb.RenderText();
                    
                }
            }

            public void HandleKeyEvent(string keyValue, string keyAction) {

                switch (keyAction) {
                case "KEY_PRESS":
                    switch (keyValue) {
                    case "KEY_UP":
                        if (activeMenuButton < maxMenuButtons-1) {
                            menuButtons[1].SetColor(new Vec3F(1.0f, 0.0f, 0.0f));
                            menuButtons[0].SetColor(new Vec3F(0.0f, 1.0f, 0.0f));
                            activeMenuButton = 1;
                        }

                        break;
                    case "KEY_DOWN":
                        if (activeMenuButton > 0) {
                            menuButtons[0].SetColor(new Vec3F(1.0f, 0.0f, 0.0f));
                            menuButtons[1].SetColor(new Vec3F(0.0f, 1.0f, 0.0f));
                            
                            activeMenuButton = 0;
                        }

                        break;
                    case "KEY_ENTER":
                        if (activeMenuButton == 0) {
                            GameRunning.NewInstance();

                            GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.GameStateEvent,
                                    this,
                                    "CHANGE_STATE",
                                    "GAME_RUNNING", ""));

                        } else if (activeMenuButton == 1) {
                            GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.WindowEvent,
                                    this,
                                    "CLOSE_WINDOW",
                                    "", ""));
                        }

                        break;
                    }

                    break;

                }

            }

            public void GameLoop() {
                
            }

           

            public void InitializeGameState() {
                backGroundImage = new Entity(
                    new StationaryShape(new Vec2F(0.0f, 0.0f),
                    new Vec2F(1.0f, 1.0f)), 
                    new Image(Path.Combine("Assets", "Images", "TitleImage.png")));



                activeMenuButton = 0;
                maxMenuButtons = 2;
                menuButtons = new[] {
                    new Text("New Game", new Vec2F(0.1f, 0.0f), new Vec2F(0.6f, 0.3f)),
                    new Text("Quit", new Vec2F(0.1f, -0.1f), new Vec2F(0.6f, 0.3f))

                };
             

                for (int i = 0; i < maxMenuButtons; i++) {
                    menuButtons[i].SetColor(new Vec3F(1.0f, 0.0f, 0.0f));
                    menuButtons[i].SetFontSize(70);


                }

                menuButtons[activeMenuButton].SetColor(new Vec3F(0.0f, 1.0f, 0.0f));
            }
        


        public void UpdateGameLogic() {
                
            }
        }
    }



