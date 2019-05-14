using System;
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
        public static GameRunning instance;
        private Entity backGroundImage;
        private GameTimer gameTimer;
        public Player player;
        private TextLoader loader;
        private GraphicsGenerator grafgen;
        public EntityContainer<pixel> pixel_container;
        private StateMachine stateMachine;
        private Game game;
        public static string CurrentLevel;
        private Collisions collisions;

        public GameRunning(string level) { 
            InitializeGameState();
            PickLevel(level);
            GameRunning.instance = this;
        }

        public void UpdateGameLogic() {
            if (collisions.CollisionCheck()) {
               GameOver();
            } else {
                player.Move();
            }
//            Console.WriteLine("Speed:"+player.Entity.Shape.AsDynamicShape().Direction.Length());
        }

        /// <summary>
        /// Loads the chosen level info pixel_container
        /// </summary>
        private void PickLevel(string level) {
            loader = new TextLoader(level);
            grafgen = new GraphicsGenerator(new LvlLegends(loader),
                new LvlStructures(loader), 500, game, player);
            pixel_container = grafgen.AllGraphics;
            GameRunning.CurrentLevel = level;
            collisions = new Collisions(pixel_container,player);
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

        /// <summary>
        /// Render the background, explosions, background image.
        /// </summary>
        public void RenderState() {
            backGroundImage.RenderEntity();
            pixel_container.RenderEntities();
            player.RenderPlayer();    
            
        }

        /// <summary>
        /// GameRunning return a new instance of null, else a new
        /// depending on a change of level
        /// </summary>
        public static GameRunning GetInstance(string level) {
            var running = GameRunning.instance;
            if (running != null) {
                if (GameRunning.CurrentLevel != level) {
                    Console.WriteLine("GameRunning was not the same, we change level");
                    return new GameRunning(level);
                } else {
                    Console.WriteLine("GameRunning was the same, returning level");
                    return running;
                }
            } else {
                Console.WriteLine("No GameRunning active, creating new");
                return new GameRunning(level);
            }
        }

        /// <summary>
        /// Sends to user to the GameOver state
        /// </summary>
        private void GameOver() {
            SpaceTaxiBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.GameStateEvent,
                        this,
                        "CHANGE_STATE",
                        "GAME_OVER", "DELETE_GAME"));
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
                    case "KEY_UP":
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
                    case "KEY_UP":
                        KeyRelease(keyValue);
                        break;
                }
            }
        }

        public void KeyPress(string key) {
            switch (key) {
                case "KEY_ESCAPE":
                    SpaceTaxiBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent,
                            this,
                            "CHANGE_STATE",
                            "GAME_PAUSED", CurrentLevel));
                    break;
                case "KEY_UP":
                    player.platform = false;
                    SpaceTaxiBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "BOOSTER_UPWARDS", "", ""));
                
                    break;
                case "KEY_LEFT":
                    SpaceTaxiBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "BOOSTER_TO_LEFT", "", ""));
                    break;
                case "KEY_RIGHT":
                    SpaceTaxiBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "BOOSTER_TO_RIGHT", "", ""));
                    break;
                }
        }

        public void KeyRelease(string key) {
            switch (key) {
                case "KEY_LEFT":
                    SpaceTaxiBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "STOP_ACCELERATE_LEFT", "", ""));
                    break;
                case "KEY_RIGHT":
                    SpaceTaxiBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "STOP_ACCELERATE_RIGHT", "", ""));
                    break;
                case "KEY_UP":
                    SpaceTaxiBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "STOP_ACCELERATE_UP", "", ""));
                    break;
            }
        }
    }
}