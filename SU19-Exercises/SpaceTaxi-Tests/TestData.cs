using System.Collections.Generic;
using System.Runtime.Remoting;

namespace SpaceTaxi_Tests {
    public class TestData {
        private List<string> structure;
        private List<string> info;
        private List<string> legends;
        public Dictionary<string, string> customers;

        public TestData() {
            customers = new Dictionary<string, string>();
            customers.Add("Customer:", " Alice 10 1 ^J 10 100");
            
        }
    }
}