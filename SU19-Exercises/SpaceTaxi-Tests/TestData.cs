using System.Collections.Generic;

namespace SpaceTaxi_Tests {
    public class TestData {
        private List<string> structure;
        private List<string> info;
        private List<string> legends;
        private List<string> customers;

        public TestData() {
            structure = new List<string>(new string[] { "Customer: Alice 10 1 ^J 10 100" });
            
        }
    }
}