using System;

namespace SpaceTaxi_2.SpaceTaxiStates {
    public class GameStateType {
        public enum EnumGameStateType {
            GameRunning,
            GamePaused,
            GameLevelPicker,
            MainMenu
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
            default:
                throw new ArgumentException("Wrong key you idiot!");
            }
        }
    }
}