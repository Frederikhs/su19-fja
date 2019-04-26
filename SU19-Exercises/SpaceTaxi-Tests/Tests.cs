using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SpaceTaxi_1;

namespace SpaceTaxi_Tests {
    [TestFixture]
    public class Tests {
        // Setting up a TextLoader with the short-n-sweet lvl and level part classes
        private TextLoader textLoader;
        private LvlCustomer lvlcustomer;
        private LvlInfo lvlinfo;
        private LvlLegends lvllegends;
        private LvlStructures lvlstructures;
        [SetUp] 
        public void InitiateTextLoader() {
            textLoader = new TextLoader("short-n-sweet");
            lvlcustomer = new LvlCustomer("short-n-sweet");
            lvlinfo = new LvlInfo("short-n-sweet");
            lvllegends = new LvlLegends("short-n-sweet");
            lvlstructures = new LvlStructures("short-n-sweet");
        }
        
        //Testing that our textloader and lvlcustomer class finds the correct customer
        //info line in the level short-n-sweet text file.
        [Test]
        public void LvlCustomer() {
            List<String> Customers = new List<string> {
                "Customer: Alice 10 1 ^J 10 100"
            };
            Assert.AreEqual(Customers,lvlcustomer.Customers);
        }

        //Testing that our textloader and lvlinfo class defines the same dictionary
        //key value store for the level short-n-sweet text file
        [Test]
        public void LvlInfo() {
            Dictionary<string, string> infoDic = new Dictionary<string, string> {
                {"Name","SHORT -N- SWEET"},
                {"Platforms","1"}
            };
            CollectionAssert.AreEqual(infoDic,lvlinfo.InfoDic);
        }

        //Testing that our textloader and lvllegends class defines the same dictionary
        //key value store for the level short-n-sweet text file
        [Test]
        public void LvlLegends() {
            Dictionary<char, string> LegendsDic = new Dictionary<char, string> {
                {'%', "white-square.png"},
                {'#', "ironstone-square.png"},
                {'1', "neptune-square.png"},
                {'2', "green-square.png"},
                {'3', "yellow-stick.png"},
                {'o', "purple-circle.png"},
                {'G', "green-upper-left.png"},
                {'H', "green-upper-right.png"},
                {'g', "green-lower-left.png"},
                {'h', "green-lower-right.png"},
                {'I', "ironstone-upper-left.png"},
                {'J', "ironstone-upper-right.png"},
                {'i', "ironstone-lower-left.png"},
                {'j', "ironstone-lower-right.png"},
                {'N', "neptune-upper-left.png"},
                {'M', "neptune-upper-right.png"},
                {'n', "neptune-lower-left.png"},
                {'m', "neptune-lower-right.png"},
                {'W', "white-upper-left.png"},
                {'X', "white-upper-right.png"},
                {'w', "white-lower-left.png"},
                {'x', "white-lower-right.png"}
            };
            CollectionAssert.AreEqual(LegendsDic,lvllegends.LegendsDic);
        }
        
        //Testing that our textloader and lvlstructure class creates the
        //same string list for the level short-n-sweet in the text file.
        [Test]
        public void LvlStructure() {
            List<String> Structure = new List<string> {
                "%#%#%#%#%#%#%#%#%#^^^^^^%#%#%#%#%#%#%#%#",
                "#               JW      JW             %",
                "%      h2g                             #",
                "#      222                     >       %",
                "%      H2G                        o    #",
                "#       3                           o  %",
                "%       3                              #",
                "#       3                           o  %",
                "%       3                       j%i    #",
                "#       3                       W Xi   %",
                "%       3                          %   #",
                "#                                 xI   %",
                "%    o                           xI    #",
                "#                               xI     %",
                "%                              xI      #",
                "#  o   o                      xI       %",
                "%                            xI        #",
                "#    o                      xI         %",
                "%      o                   xI          #",
                "#  o                      xI           %",
                "%       o                 I            #",
                "#         m1111111111111n              %",
                "%#%#%#%#%#%#%#%#%#%#%#%#%#%#%#%#%#%#%#%#"
            };
            Assert.AreEqual(Structure,lvlstructures.Structure);
        }
    }
}