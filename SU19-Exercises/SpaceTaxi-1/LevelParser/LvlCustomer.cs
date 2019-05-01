using System;
using System.Collections.Generic;

namespace SpaceTaxi_1 {
    public class LvlCustomer {
      
        private TextLoader myLoader;
        public List<string> Customers;

        /// <summary>
        /// Method for getting key/value dictionary for customer info
        /// </summary>
        public LvlCustomer(string level) {
            myLoader = new TextLoader(level);
            this.Customers = myLoader.GetCustomerInfo();
        }
    }
}