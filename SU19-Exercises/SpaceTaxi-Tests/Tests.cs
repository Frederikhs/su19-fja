using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SpaceTaxi_1;

namespace SpaceTaxi_Tests {
    [TestFixture]
    public class Tests {
        // Setting up a TextLoader with the short-n-sweet lvl
        [SetUp] 
        public void InitiateTextLoader() {
            textLoader = new TextLoader("short-n-sweet");
        }
        private TextLoader textLoader;
        
        [Test]
        public void Test1() {
            var expected = new TestData().customers;
            CollectionAssert.AreEquivalent(expected, textLoader.GetCustomerInfo());
        }
    }
}