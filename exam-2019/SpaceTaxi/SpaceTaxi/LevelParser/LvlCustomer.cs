using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SpaceTaxi.LevelParser {
    public class LvlCustomer {
        public List<Dictionary<string, string>> AllCustomerDict;  

        /// <summary>
        /// Method for getting key/value dictionary for customer info
        /// </summary>
        public LvlCustomer(TextLoader loader) {
            AllCustomerDict = new List<Dictionary<string, string>>();
            
            foreach (var customer in loader.GetCustomerInfo()) {
                var customerValues = Regex.Split(customer, "Customer: ")[1];
                var allInfo = Regex.Split(customerValues, " ");
                
                Dictionary<string, string> someCustomerDict = new Dictionary<string, string>() {
                    {"name", allInfo[0]},
                    {"spawnAfter", allInfo[1]},
                    {"spawnPlatform", allInfo[2]},
                    {"destinationPlatform", allInfo[3]},
                    {"taxiDuration", allInfo[4]},
                    {"points", allInfo[5]},
                    {"generated", "false"}
                };
                AllCustomerDict.Add(someCustomerDict);
            }

        }
    }
}