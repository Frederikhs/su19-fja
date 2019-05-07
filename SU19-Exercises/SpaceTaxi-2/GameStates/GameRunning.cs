using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.State;
using DIKUArcade.Timers;
using SpaceTaxi_2.SpaceTaxiState;
using SpaceTaxiGame;

namespace SpaceTaxi_2.SpaceTaxiStates {
    public class GameRunning : IGameState {
        private static GameRunning instance;
        private Entity backGroundImage;
        private GameTimer gameTimer;
        public Player player;
        private TextLoader loader;
        private GraphicsGenerator grafgen;
        public EntityContainer<pixel> pixel_container;
        private StateMachine stateMachine;
        private Game game;
        

        public GameRunning() { 
            InitializeGameState();
        }

        

        public void UpdateGameLogic() {
            
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            if (keyAction == "KEY_PRESS") {
                switch (keyValue) {
                case "KEY_RIGHT":
                    KeyPress(keyValue);
                    break;
                case "KEY_LEFT":
                    KeyPress(keyValue);
                    break;
                case "KEY_SPACE":
                    KeyPress(keyValue);
                    break;
                case "KEY_ESCAPE":
                    KeyPress(keyValue);
                    break;
                }
            } else if (keyAction == "KEY_RELEASE") {
                switch (keyValue) {
                case "KEY_RIGHT":
                    KeyRelease(keyValue);
                    break;
                case "KEY_LEFT":
                    KeyRelease(keyValue);
                    break;
                }
            }
        }

        public void InitializeGameState() {
            // game assets
            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );
            backGroundImage.RenderEntity();

            // game entities
            player = new Player();
            player.SetExtent(0.1f, 0.1f);

            //Change the level based on what level name the loader is constructed with
            loader = new TextLoader("the-beach");
            grafgen = new GraphicsGenerator(new LvlLegends(loader),
                new LvlStructures(loader), 500, game, player);
            pixel_container = grafgen.AllGraphics;
        }

        public void GameLoop() { }

        public void RenderState() {
            backGroundImage.RenderEntity();
            pixel_container.RenderEntities();
            player.RenderPlayer();
            
        }

        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }
        

        private void KeyPress(string key) {
            switch (key) {
            case "KEY_ESCAPE":
                SpaceTaxiBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.GameStateEvent,
                        this,
                        "CHANGE_STATE",
                        "GAME_PAUSED", ""));
                break;
            case "KEY_LEFT":
                SpaceTaxiBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "move_left", "", ""));
                break;
            case "KEY_RIGHT":
                SpaceTaxiBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "move_right", "", ""));
                break;
            case "KEY_SPACE":
                SpaceTaxiBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "shoot", "", ""));
                break;
            }
        }

        private void KeyRelease(string key) {
            switch (key) {
            case "KEY_LEFT":
                SpaceTaxiBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "stop_left", "", ""));
                break;

            case "KEY_RIGHT":
                SpaceTaxiBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "stop_right", "", ""));
                break;
            }
        }
    }
}