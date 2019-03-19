using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using DIKUArcade;
using DIKUArcade.EventBus;
using DIKUArcade.Timers;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using GalagaGame;
using Galaga_Exercise_3.GalagaEntities.Enemy;
using Galaga_Exercise_3.MovementStrategy;


namespace Galaga_Exercise_3 {

    public class Game : IGameEventProcessor<object> {
        private Window win;
        private Player player;
        private DIKUArcade.Timers.GameTimer gameTimer;
        private GameEventBus<object> eventBus;
        
        //Enemy vars
        private List<Image> enemyStrides;
        private ImageStride enemyAnimation;
        
        //Enemy formation vars
        private ISquadron enemyFormation;
        private LineFormation Form_LineFormation;
        private PairsFormation Form_PairsFormation;
        private ZigZagFormation Form_ZigZag;
        
        //Enemy movement vars
        private NoMove Move_No = new NoMove();
        private Down Move_Down = new Down();
        private ZigZagDown Move_Zig = new ZigZagDown();
        
        //Playershot var
        public List<PlayerShot> playerShots { get; private set; }
        
        //Explosion vars
        private List<Image> explosionStrides;
        private AnimationContainer explosions;
        private ImageStride explosionStride;
        
        //Score var
        private Score score;
        
        public Game() {
            //Creating game window
            win = new Window("Window", 500, 500);
            gameTimer = new GameTimer(60, 60);
            
            //Create the player
            player = new Player(this,
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png")));
            
            //Create 4 strides of 1 image for monsters
            enemyStrides = ImageStride.CreateStrides(4,
                Path.Combine("Assets", "Images", "BlueMonster.png"));

            //Create new animation based on image list for monsters
            enemyAnimation = new ImageStride(80,enemyStrides);
            
            //Decisions for enemy position:
            //Choose to uncomment your choice of formation (default is ZigZag):
            enemyFormation = new ZigZagFormation(7);
            //enemyFormation = new LineFormation(7);
            //enemyFormation = new PairsFormation(7);
            
            enemyFormation.CreateEnemies(enemyStrides);
            
            //Create eventbus
            eventBus = GalagaBus.GetBus();
            eventBus.InitializeEventBus(new List<GameEventType> {
                GameEventType.PlayerEvent,
                GameEventType.InputEvent, //key press / key release
                GameEventType.WindowEvent //messages to the window
            });
            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.PlayerEvent, player);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);

            //Create list of playershots
            playerShots = new List<PlayerShot>();
            
            //Animation stride for explosions
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(enemyFormation.MaxEnemies);
            explosionStride = new ImageStride(explosionLength / 8, explosionStrides);

            //Create score
            score = new Score(new Vec2F(0.45f,-0.12f), new Vec2F(0.2f,0.2f));
        }

        //For detecting if game is over, if game is over player flies up and game ends
        private bool IsGameOver() {
            if (enemyFormation.Enemies.CountEntities() > 0) {
                return false;
            } else {
                player.Direction(new Vec2F(0.00f, 0.01f));
                return true;
            }    
        }
        
        private void IterateShots() {
            foreach (var shot in playerShots) {
                shot.Shape.Move();
                if (shot.Shape.Position.Y > 1.0f) {
                    shot.DeleteEntity();
                }
                foreach (Enemy enemy in enemyFormation.Enemies) {
                    //Create dynamic shapes
                    var shotDyn = shot.Shape.AsDynamicShape();
                   
                    //Checks if there's a collision
                    if (CollisionDetection.Aabb(shotDyn, enemy.Shape).Collision) {
                        
                        //Delete both enemy and shot if so
                        enemy.DeleteEntity();
                        shot.DeleteEntity();
                        
                        //Create explosion at coordinates
                        AddExplosion(enemy.Shape.Position.X,enemy.Shape.Position.Y,0.1f,0.1f);
                        
                        //Adds a point to the score
                        score.AddPoint();
                    }
                }
            }
            
            //If collision happened add enemies to new enemy container 
            List<Enemy> aliveEnemies = new List<Enemy>();                        
            foreach (Enemy enemy in enemyFormation.Enemies) {
                if (!enemy.IsDeleted()) {
                    aliveEnemies.Add(enemy);
                }
            }
            
            enemyFormation.Enemies.ClearContainer();
            foreach (Enemy enemy in aliveEnemies) {
                if (!enemy.IsDeleted()) {
                    enemyFormation.Enemies.AddDynamicEntity(enemy);
                }
            }
            
            //If collision happened remove playershot from old list and create new list
            List<PlayerShot> newPlayerShots = new List<PlayerShot>();
            foreach (PlayerShot aShot in playerShots) {
                if (!aShot.IsDeleted()) {
                    newPlayerShots.Add(aShot);
                } }
            playerShots = newPlayerShots;
        }
        
        //Function for displaying explosions
        private int explosionLength = 500;
        private void AddExplosion(float posX, float posY,
            float extentX, float extentY) {
            explosions.AddAnimation(
                new StationaryShape(posX, posY, extentX, extentY), explosionLength,
                explosionStride);
        }

        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();

                    //Listen for events
                    eventBus.ProcessEvents();
                    
                    //Move the player in direction is has
                    player.Move();

                    //Animate displayed shots
                    IterateShots();
                    
                    //Decisions for enemy movement:
                    // -- Choose to uncomment your choice of movement (default is ZigZag):
                    Move_Zig.MoveEnemies(enemyFormation.Enemies);
                    //Move_No.MoveEnemies(enemyFormation.Enemies);
                    //Move_Down.MoveEnemies(enemyFormation.Enemies);
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    player.RenderEntity();
                    
                    enemyFormation.Enemies.RenderEntities();

                    //Render each shot
                    foreach (var aShot in playerShots) {
                        aShot.RenderEntity();
                    }
                    //Render explosions
                    explosions.RenderAnimations();

                    //If game is running show score, then display game over
                    if (!IsGameOver()) {
                        score.RenderScore();                        
                    } else {
                        score.GameOver();
                    }
                    
                    win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()) {
                    // 1 second has passed - display last captured ups and fps
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                                ", FPS: " + gameTimer.CapturedFrames;
                }
            }
        }

        // User input events
        private void KeyPress(string key) {
            switch (key) {
            case "KEY_ESCAPE":
                if (!IsGameOver()) {
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.WindowEvent, this, "CLOSE_WINDOW", "", ""));
                }
                break;
            case "KEY_LEFT":
                if (!IsGameOver()) {
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "move_left", "", ""));
                }

                break;
            case "KEY_RIGHT":
                if (!IsGameOver()) {
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "move_right", "", ""));
                }

                break;
            case "KEY_SPACE":
                if (!IsGameOver()) {
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "shoot", "", ""));
                }
                break;
            default:
                break;
            }
        }

        private void KeyRelease(string key) {
            switch (key) {
            case "KEY_LEFT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "stop_left", "", ""));
                break;
                
            case "KEY_RIGHT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "stop_right", "", ""));
                break;
            default:
                break;
            }
        }


        // Process events
        public void ProcessEvent(GameEventType eventType,
            GameEvent<object> gameEvent) {
            if (eventType == GameEventType.WindowEvent) {
                switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    win.CloseWindow();
                    break;
                default:
                    break;
                }
            } else if (eventType == GameEventType.InputEvent) {
                switch (gameEvent.Parameter1) {
                case "KEY_PRESS":
                    KeyPress(gameEvent.Message);
                    break;
                case "KEY_RELEASE":
                    KeyRelease(gameEvent.Message);
                    break;
                }
            }
        }
    }
}