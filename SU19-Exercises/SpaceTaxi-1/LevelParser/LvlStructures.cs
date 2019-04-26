using System;
using System.Collections.Generic;
using DIKUArcade.Graphics;

namespace SpaceTaxi_1 {
    public class LvlStructures {
        private TextLoader myLoader;
        public List<string> Structure;

        public LvlStructures(string level) {
            myLoader = new TextLoader(level);
            Structure = myLoader.GetLvlStructure();
        }
        
    }
}