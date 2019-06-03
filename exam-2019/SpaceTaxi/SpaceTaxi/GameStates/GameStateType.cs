using System;

namespace SpaceTaxi.GameStates {
    public class GameStateType {
        
        /// <summary>
        /// The state the game can be in
        /// </summary>
        public enum EnumGameStateType {
            GameRunning,
            GamePaused,
            GameLevelPicker,
            MainMenu,
            GameOver
        }

        /// <summary>
        /// Converts a string state to a enum state
        /// </summary>
        ///
        /// <param name="state">
        /// String game state type
        /// </param>
        ///
        /// <returns>
        /// EnumGameStateType
        /// </returns>
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

        /// <summary>
        /// Converts a enum state to a string of the state
        /// </summary>
        ///
        /// <param name="state">
        /// EnumGameStateType
        /// </param>
        ///
        /// <returns>
        /// string
        /// </returns>
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