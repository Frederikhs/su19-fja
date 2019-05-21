using System;
using System.Collections.Generic;

namespace SpaceTaxi_2 {
    public class LvlCustomer {
      
        private TextLoader myLoader;
        public List<string> Customers;

        /// <summary>
        /// Method for getting key/value dictionary for customer info
        /// </summary>
        public LvlCustomer(TextLoader loader) {
            this.Customers = loader.GetCustomerInfo();
            
        }
    }
}