using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using DIKUArcade.Timers;
using SpaceTaxi.LevelParser;
using SpaceTaxi.Taxi;

namespace SpaceTaxi.GameStates {
    public class GameRunning : IGameState {
        public static GameRunning Instance;
        public EntityContainer<Pixel> PixelContainer;
        public List<Customer> CustomerContainer;
        public static string CurrentLevel;
        public TimedEventContainer CustomerEvents;
        
        private Player player;
        private Entity backGroundImage;
        private TextLoader loader;
        private GraphicsGenerator graphGenerator;
        private Game game;
        private Collisions collisions;
        private Points points;

        /// <summary>
        /// Initialized the game state for game running based on the level picked
        /// </summary>
        public GameRunning(string level) {
            GameRunning.CurrentLevel = level;
            GameRunning.Instance = this;
            InitializeGameState();
            CustomerEvents = new TimedEventContainer(10);
            CustomerEvents.AttachEventBus(SpaceTaxiBus.GetBus());
            PickLevel(level);
            
            //Create score
            points = new Points(
                new Vec2F(0.45f, -0.12f), new Vec2F(0.2f, 0.2f));
            
        }

        /// <summary>
        /// Updates logic for game running (collision detection, player movement and timed events)
        /// </summary>
        public void UpdateGameLogic() {
            if (collisions.CollisionCheck()) {
               GameOver();
            } else {
                player.Move();
            }
            CustomerEvents.ProcessTimedEvents();
        }

        /// <summary>
        /// Loads the chosen level info pixel_container
        /// </summary>
        private void PickLevel(string level) {
            loader = new TextLoader(level);
            graphGenerator = new GraphicsGenerator(new LvlLegends(loader),
                new LvlStructures(loader), new LvlInfo(loader), new LvlCustomer(loader), 500, game, player);
            PixelContainer = graphGenerator.AllGraphics;
            CustomerContainer = graphGenerator.AllCustomersInGame;
            collisions = new Collisions(PixelContainer,CustomerContainer,player);
        }

        /// <summary>
        /// Initializes the required elements for game running
        /// </summary>
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

        /// <summary>
        /// Not used
        /// </summary>
        public void GameLoop() { }

        /// <summary>
        /// Render the background, explosions, background image.
        /// </summary>
        public void RenderState() {
            backGroundImage.RenderEntity();
            PixelContainer.RenderEntities();
            player.RenderPlayer();

            foreach (var someCustomer in CustomerContainer) {
                someCustomer.RenderCustomer();
            }

            if (Player.CustomersInsidePlayer != null) {
                foreach (var someCustomer in Player.CustomersInsidePlayer) {
                    someCustomer.RenderCustomer();
                }
            }
            points.RenderScore();

        }

        /// <summary>
        /// GameRunning return a new instance of null, else a new
        /// depending on a change of level
        /// </summary>
        ///
        /// <param name="level">
        /// Is a string of the level name. e.i. "the-beach" 
        /// </param>
        ///
        /// <returns>
        /// A static GameRunning instance
        /// </returns>
        public static GameRunning GetInstance(string level) {
            var running = GameRunning.Instance;
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

        /// <summary>
        /// Handles key pressed events
        /// </summary>
        ///
        /// <param name="key">
        /// The key value of the pressed key, i.e. "KEY_UP", "KEY_DOWN"
        /// </param>
        ///
        /// <returns>
        /// void
        /// </returns>
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

        /// <summary>
        /// Handles key released events
        /// </summary>
        ///
        /// <param name="key">
        /// The key value of the pressed key, i.e. "KEY_UP", "KEY_DOWN"
        /// </param>
        ///
        /// <returns>
        /// void
        /// </returns>
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