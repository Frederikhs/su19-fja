using System;

namespace Galaga_Exercise_3.GalagaStates {
    public class GameStateType {
        public enum EnumGameStateType {
            GameRunning, GamePaused, MainMenu
        }

        public static EnumGameStateType TransformStringToState(string state) {
            switch (state) {
                case "GAME_RUNNING":
                    return EnumGameStateType.GameRunning;
                case "GAME_PAUSED":
                    return EnumGameStateType.GameRunning;
                case "MAIN_MENU":
                    return EnumGameStateType.MainMenu;
                default:
                    throw new ArgumentException("Wrong key you idiot!"); 
            }
        }

        public static string TransformStateToString(EnumGameStateType state) {
            switch (state) {
                case EnumGameStateType.GameRunning:
                    return "GAME_RUNNING";
                case EnumGameStateType.GamePaused:
                    return "GAME_PAUSED";
                case EnumGameStateType.MainMenu:
                    return "MAIN_MENU";
                default:
                    throw new ArgumentException("Wrong key you idiot!");
            }
            
        }
    }
}