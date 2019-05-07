using System;
using System.Collections.Generic;
using System.IO;

namespace SpaceTaxi_1 {
    public class TextLoader {

        private string[] allLevelText;

        /// <summary>
        /// A TextLoader loads an entire .txt file into a string array
        /// </summary>
        public TextLoader(string levelString) {
            Console.WriteLine("Testloader created");
            var path = GetLevelFilePath(levelString+".txt");
            allLevelText = File.ReadAllLines(path);
        }
        
        /// <summary>
        /// Method for retrieving path to Levels
        /// </summary>
        private string GetLevelFilePath(string filename) {
            // Find base path.
            DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location));

            while (dir.Name != "bin") {
                dir = dir.Parent;
            }
            dir = dir.Parent;

            // Find level file.
            string path = Path.Combine(dir.FullName.ToString(), "Levels", filename);

            if (!File.Exists(path)) {
                throw new FileNotFoundException($"Error: The file \"{path}\" does not exist.");
            }

            return path;
        }

        /// <summary>
        /// Method for getting a level map
        /// </summary>
        public List<string> GetLvlStructure() {
            List<string> map = new List<string>(); 
            for (int i = 0; i < 23; i++) {
                map.Add(allLevelText[i]);
            }

            return map;
        }

        /// <summary>
        /// Method for getting level info
        /// </summary>
        public List<string> GetLvlInfo() {
            List<string> levelInfo = new List<string>();
            levelInfo.Add(allLevelText[24]);
            levelInfo.Add(allLevelText[25]);
            
            return levelInfo;
        }
        
        /// <summary>
        /// Method for getting level legends
        /// </summary>
        public List<string> GetLvlLegends() {
            List<string> levelLegends = new List<string>();

            foreach (var line in allLevelText) {
                if (line.Length >= 2) {
                    if (line[1] == ')') {
                        levelLegends.Add(line);
                    }
                }
            }
            
            return levelLegends;
        }
        
        /// <summary>
        /// Method for getting customer info
        /// </summary>
        public List<string> GetCustomerInfo() {
            List<string> custInfo = new List<string>();
            
            foreach (var line in allLevelText) {
                if (line.Contains("Customer")) {
                    custInfo.Add(line);
                }
            }
            
            return custInfo;
        }
        
        
    }
}