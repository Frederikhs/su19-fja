using System;

namespace SpaceTaxi_2.SpaceTaxiStates {
    public class GameStateType {
        public enum EnumGameStateType {
            GameRunning,
            GamePaused,
            GameLevelPicker,
            MainMenu,
            GameOver
        }

        public static EnumGameStateType TransformStringToState(string state) {
            switch (state) {
            case "GAME_RUNNING":
                return EnumGameStateType.GameRunning;
            case "GAME_PAUSED":
                return EnumGameStateType.GamePaused;
            case "GAME_LEVEL_PICKER":
                return EnumGameStateType.GameLevelPicker;
            case "MAIN_MENU":
                return EnumGameStateType.MainMenu;
            case "GAME_OVER":
                return EnumGameStateType.GameOver;
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
            case EnumGameStateType.GameLevelPicker:
                return "GAME_LEVEL_PICKER";
            case EnumGameStateType.MainMenu:
                return "MAIN_MENU";
            case EnumGameStateType.GameOver:
                return "GAME_OVER";
            default:
                throw new ArgumentException("Wrong key you idiot!");
            }
        }
    }
}