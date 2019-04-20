using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SpaceTaxi_1 {
    public class LvlInfo {
        public Dictionary<string, string> InfoDic { get; }

        // A LvlInfo is a dictionary of various info about a lvl
        // The constructors input determines of which lvl a dictionary is made
        public LvlInfo(string levelString) {
            InfoDic = new Dictionary<string, string>();
            var infoString = new TextLoader(levelString).get_lvl_info();
            foreach (var elem in infoString) {
                string[] split = Regex.Split(elem, ": "); // Added Regex split, so we don't get a space in the string
                
                InfoDic.Add(split[0], split[1]);
            }
            
        }
    }
}