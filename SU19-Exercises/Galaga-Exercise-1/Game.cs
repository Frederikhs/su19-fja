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


namespace Galaga_Exercise_1 {

    public class Game : IGameEventProcessor<object> {
        private Window win;
        private Player player;
        private DIKUArcade.Timers.GameTimer gameTimer;
        private GameEventBus<object> eventBus;
        private List<Image> enemyStrides;
        private List<Enemy> enemies;
        private Enemy newEnemy;
        private ImageStride enemyAnimation;
        
        public Game() {
            // TODO: Choose some reasonable values for the window and timer constructor.
            // For the window, we recommend a 500x500 resolution (a 1:1 aspect ratio).
            win = new Window("Window", 500, 500);
            gameTimer = new GameTimer(60, 60);
            
            player = new Player(this,
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png")));
            
            //CREATING 4 STRIDES OF 1 IMAGE
            enemyStrides = ImageStride.CreateStrides(4,
                Path.Combine("Assets", "Images", "BlueMonster.png"));

            //CREATING NEW ANIMATION BASED ON IMAGE LIST
            enemyAnimation = new ImageStride(80,enemyStrides);
            
            //CREATING LIST FOR ENEMIES TO BE IN
            enemies = new List<Enemy>();

            //ADDING COUNT ENEMIES
            AddEnemy(4);
            
            eventBus = new GameEventBus<object>();
            eventBus.InitializeEventBus(new List<GameEventType>() {
                GameEventType.InputEvent, // key press / key release
                GameEventType.WindowEvent, // messages to the window
            });
            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);
            
        }

        public void AddEnemy(int count) {
            //FIRST X POS OF FIRST ENEMY
            float xPos = 0.15f;
            
            //CREATING COUNT ENEMIES AND ADDING TO enemies
            for (int i = 0; i < count; i++) {
            newEnemy = new Enemy(this,
                new DynamicShape(new Vec2F(xPos, 0.9f), new Vec2F(0.1f, 0.1f)),
                enemyAnimation);
            enemies.Add(newEnemy);
            //INCREMENTING XPOS WITH 0.2f
            xPos += 0.2f;
            }
        }


        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();

                    // Update game logic here
                    eventBus.ProcessEvents();
                    
                    player.Move();
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    player.RenderEntity();
                    
                    //RENDER EACH ENEMY IN LIST enemies
                    foreach (var anEnemy in enemies) {
                        anEnemy.RenderEntity();
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


        private void KeyPress(string key) {
            switch (key) {
            case "KEY_ESCAPE":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.WindowEvent, this, "CLOSE_WINDOW", "", ""));
                break;
                ;
            case "KEY_LEFT":
                player.Direction(new Vec2F(-0.01f, 0.0f));
                break;
            case "KEY_RIGHT":
                player.Direction(new Vec2F(0.01f, 0.0f));
                break;
            default:
                break;

            }
        }

        public void KeyRelease(string key) {
                    //throw new NotImplementedException();
                }


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
                    player.Direction(new Vec2F(0.0f, 0.0f));
                    KeyRelease(gameEvent.Message);
                    break;
                default:
                    break;
                }
            }
        }

    }
}
    

