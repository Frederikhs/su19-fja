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
using OpenTK;
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
        public string CurrentLevel;

        public GameRunning(string level) { 
            InitializeGameState();
            PickLevel(level);
        }

        public void UpdateGameLogic() { }

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

        private void PickLevel(string level) {
            loader = new TextLoader(level);
            grafgen = new GraphicsGenerator(new LvlLegends(loader),
                new LvlStructures(loader), 500, game, player);
            pixel_container = grafgen.AllGraphics;
            CurrentLevel = level;
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
        }

        public void GameLoop() { }

        public void RenderState() {
            backGroundImage.RenderEntity();
            pixel_container.RenderEntities();
            player.RenderPlayer();
            
        }

        public static GameRunning GetInstance(string level) {
            var running = GameRunning.instance;
            if (running != null) {
                if (running.CurrentLevel != level) {
                    return new GameRunning(level);
                } else {
                    return running;
                }
            } else {
                return new GameRunning(level);
            }
        }
        

        private void KeyPress(string key) {
            switch (key) {
            case "KEY_ESCAPE":
                SpaceTaxiBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.GameStateEvent,
                        this,
                        "CHANGE_STATE",
                        "GAME_PAUSED", this.CurrentLevel));
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