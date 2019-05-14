using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SpaceTaxi_2 {
    public class LvlInfo {
        public Dictionary<string, string> InfoDic { get; }

        /// <summary>
        /// A LvlInfo is a dictionary of various info about a lvl
        /// The constructors input determines of which lvl a dictionary is made
        /// </summary>
        public LvlInfo(TextLoader loader) {
            InfoDic = new Dictionary<string, string>();
            var infoString = loader.GetLvlInfo();
            foreach (var elem in infoString) {
                string[] split = Regex.Split(elem, ": ");
                InfoDic.Add(split[0], split[1]);
                
            }
        }
        
    }
}