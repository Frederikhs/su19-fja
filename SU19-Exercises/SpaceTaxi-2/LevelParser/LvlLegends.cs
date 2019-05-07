using System;
using System.Collections.Generic;
using DIKUArcade.Graphics;

namespace SpaceTaxi_2 {
    public class LvlLegends {
        public Dictionary<char, string> LegendsDic { get; }

        /// <summary>
        /// LvlLegends is a dictionary of ASCII chars and their corresponding .png file name
        /// The constructors input determines of which lvl a dictionary is made
        /// </summary>
        public LvlLegends(TextLoader loader) {
            LegendsDic = new Dictionary<char, string>();
            var legendsString = loader.GetLvlLegends();
            foreach (var elem in legendsString) {
                LegendsDic.Add(elem[0], elem.Substring(3));
            }
            
        }
        
    }
}