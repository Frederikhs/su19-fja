using System.Collections.Generic;

namespace SpaceTaxi_1 {
    public class LvlStructures {

        public Dictionary<string, List<string>> StructureDic;
        public List<TextLoader> textLoaderList;

        public LvlStructures(List<string> lvlNames) {
            StructureDic = new Dictionary<string, List<string>>();
            textLoaderList = new List<TextLoader>();
            for (int i = 0; i < lvlNames.Count; i++) {
                textLoaderList.Add(new TextLoader(lvlNames[i]));
            }

            for (int i = 0; i < lvlNames.Count; i++) {
                StructureDic.Add(lvlNames[i], textLoaderList[i].get_lvl_string());
            }
        }
    }
}