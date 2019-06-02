using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SpaceTaxi_2 {
    public class LvlCustomer {
      
        private TextLoader myLoader;
        public List<Dictionary<string, string>> AllCustomerDict;  
        public Dictionary<string, string> SomeCustomerDict;

        /// <summary>
        /// Method for getting key/value dictionary for customer info
        /// </summary>
        public LvlCustomer(TextLoader loader) {
            AllCustomerDict = new List<Dictionary<string, string>>();
            
            foreach (var customer in loader.GetCustomerInfo()) {
                Console.WriteLine(":"+customer);
                var CustValues = Regex.Split(customer, "Customer: ")[1];
                
                SomeCustomerDict = new Dictionary<string, string>();
                
                var allInfo = Regex.Split(CustValues, " ");
                SomeCustomerDict.Add("name", allInfo[0]);
                SomeCustomerDict.Add("spawnAfter", allInfo[1]);
                SomeCustomerDict.Add("spawnPlatform", allInfo[2]);
                SomeCustomerDict.Add("destinationPlatform", allInfo[3]);
                SomeCustomerDict.Add("taxiDuration", allInfo[4]);
                SomeCustomerDict.Add("points", allInfo[5]);
                SomeCustomerDict.Add("generated", "false");

                AllCustomerDict.Add(SomeCustomerDict);
            }

        }
    }
}