using System;
using System.Collections.Generic;
using DIKUArcade.Graphics;

namespace SpaceTaxi_2 {
    public class LvlStructures {
        private TextLoader myLoader;
        public List<string> Structure;

        /// <summary>
        /// Retrieves the list of string of level structure for a given level
        /// </summary>
        public LvlStructures(TextLoader loader) {
            Structure = loader.GetLvlStructure();
        }
        
    }
}