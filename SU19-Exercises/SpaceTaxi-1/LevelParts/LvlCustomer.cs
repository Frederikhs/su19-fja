using System;
using System.Collections.Generic;

namespace SpaceTaxi_1 {
    public class LvlCustomer {

        public Dictionary<string, string> CustomerDic;
        
        // A LvlCustomer is a dictionary of the customers of a level. Key=name, value=other info
        // The constructors input determines of which lvl a dictionary is made
        // TODO: split at name, not ':'
        public LvlCustomer(string levelString) {
            CustomerDic = new Dictionary<string, string>();
            var customerString = new TextLoader(levelString).get_customer_info();
            foreach (var elem in customerString) {
                string[] split = elem.Split(':');
                Console.WriteLine(split[0]);
                Console.WriteLine(split[1]);
                
                CustomerDic.Add(split[0], split[1]);
            }
            
        }
        // TODO: Maybe consider using list of customers or arrays. Looking up in dictionary can be non-scalable
    }
}