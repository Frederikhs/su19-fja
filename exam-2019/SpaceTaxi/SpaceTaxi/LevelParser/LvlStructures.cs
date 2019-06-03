using System.Collections.Generic;

namespace SpaceTaxi.LevelParser {
    public class LvlStructures {
        public List<string> Structure;

        /// <summary>
        /// Retrieves the list of string of level structure for a given level
        /// </summary>
        public LvlStructures(TextLoader loader) {
            Structure = loader.GetLvlStructure();
        }
        
    }
}