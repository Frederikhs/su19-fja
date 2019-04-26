using System;
using System.Collections.Generic;

namespace SpaceTaxi_1 {
    public class LvlCustomer {
      
        private TextLoader myLoader;
        public List<string> Customers;

        public LvlCustomer(string level) {
            myLoader = new TextLoader(level);
            this.Customers = myLoader.GetCustomerInfo();
        }
    }
}