using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SpaceTaxi_1;

namespace SpaceTaxi_Tests {
    [TestFixture]
    public class Tests {
        
        /// <summary>
        /// Testing that we get the correct customer(s) & structure for short-n-sweet
        /// </summary>
        [Test]
        public void short_n_sweet_LvlCustomer() {
            LvlCustomer lvlcustomer = new LvlCustomer("short-n-sweet");
            List<String> Customers = new List<string> {
                "Customer: Alice 10 1 ^J 10 100"
            };
            Assert.AreEqual(Customers,lvlcustomer.Customers);
        }

        /// <summary>
        /// Testing that we get the correct lvl info & structure for short-n-sweet
        /// </summary>
        [Test]
        public void short_n_sweet_LvlInfo() {
            LvlInfo lvlinfo = new LvlInfo("short-n-sweet");
            Dictionary<string, string> infoDic = new Dictionary<string, string> {
                {"Name","SHORT -N- SWEET"},
                {"Platforms","1"}
            };
            CollectionAssert.AreEqual(infoDic,lvlinfo.InfoDic);
        }

        /// <summary>
        /// Testing that we get the correct lvl legends & structure for short-n-sweet
        /// </summary>
        [Test]
        public void short_n_sweet_LvlLegends() {
            LvlLegends lvllegends = new LvlLegends("short-n-sweet");
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
        
        /// <summary>
        /// Testing that we get the correct lvl structure for short-n-sweet
        /// </summary>
        [Test]
        public void short_n_sweet_LvlStructure() {
            LvlStructures lvlstructures = new LvlStructures("short-n-sweet");
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
        
        /// <summary>
        /// Testing that we get the correct customer(s) & structure for the-beach
        /// </summary>
        [Test]
        public void the_beach_LvlCustomer() {
            LvlCustomer lvlcustomer = new LvlCustomer("the-beach");
            List<String> Customers = new List<string> {
                "Customer: Bob 10 J r 10 100",
                "Customer: Carol 30 r ^ 10 100"
            };
            Assert.AreEqual(Customers,lvlcustomer.Customers);
        }
        
        /// <summary>
        /// Testing that we get the correct lvl info & structure for the-beach
        /// </summary>
        [Test]
        public void the_beach_LvlInfo() {
            LvlInfo lvlinfo = new LvlInfo("the-beach");
            Dictionary<string, string> infoDic = new Dictionary<string, string> {
                {"Name","THE BEACH"},
                {"Platforms","J, i, r"}
            };
            CollectionAssert.AreEqual(infoDic,lvlinfo.InfoDic);
        }
        
        /// <summary>
        /// Testing that we get the correct lvl legends & structure for the-beach
        /// </summary>
        [Test]
        public void the_beach_LvlLegends() {
            LvlLegends lvllegends = new LvlLegends("the-beach");
            Dictionary<char, string> LegendsDic = new Dictionary<char, string> {
                {'A', "aspargus-edge-left.png"},
                {'B', "aspargus-edge-right.png"},
                {'T', "aspargus-edge-top.png"},
                {'C', "aspargus-edge-top-left.png"},
                {'D', "aspargus-edge-top-right.png"},
                {'G', "white-left-half-circle.png"},
                {'H', "white-right-half-circle.png"},
                {'I', "white-square.png"},
                {'J', "white-square.png"},
                {'O', "olive-green-square.png"},
                {'S', "salt-box-square.png"},
                {'M', "minsk-square.png"},
                {'a', "emperor-square.png"},
                {'b', "emperor-lower-right.png"},
                {'c', "emperor-lower-left.png"},
                {'d', "emperor-upper-left.png"},
                {'e', "deep-bronze-square.png"},
                {'f', "deep-bronze-left-half-circle.png"},
                {'g', "deep-bronze-right-half-circle.png"},
                {'i', "ironstone-square.png"},
                {'j', "ironstone-square.png"},
                {'l', "ironstone-lower-left.png"},
                {'u', "ironstone-upper-right.png"},
                {'o', "studio-square.png"},
                {'p', "studio-upper-right.png"},
                {'q', "studio-lower-left.png"},
                {'r', "studio-square.png"},
                {'t', "tacha-square.png"},
                {'s', "tacha-upper-right.png"}
            };
            CollectionAssert.AreEqual(LegendsDic,lvllegends.LegendsDic);
        }
        
        /// <summary>
        /// Testing that we get the correct lvl structure for the-beach
        /// </summary>
        [Test]
        public void the_beach_LvlStructure() {
            LvlStructures lvlstructures = new LvlStructures("the-beach");
            List<String> Structure = new List<string> {
                "CTTTTTTTTTTTTTTTTD^^^^^^CTTTTTTTTTTTTttt",
                "A                                    stt",
                "A                                      B",
                "A HJJJJJJJJG                           B",
                "HIIIIIIIIIG                            B",
                "A  HIIIG                               B",
                "A                     prrrrrq          B",
                "A                      poooooqbc       B",
                "A                       poooooad       B",
                "A                        poooooq       B",
                "A                         poooooq      B",
                "A                          pooooo      B",
                "A                           aoooo      B",
                "A                           apooo      B",
                "A              >            a poo      B",
                "A                           a  po      B",
                "A ujl                       a   p      B",
                "A  ujl                      a          B",
                "A   ujl                     a          B",
                "A    ujiiiiiiiiiiiiiij      a          B",
                "A     gef         gef       a          B",
                "MMMMMMMeMMMMMMMMMMMeMMMMMMMMaMMMMMMMMMMM",
                "OOOOSSSeSSSSSSSSSSSeSSSSSSSOaOOOOOOOOOOO"
            };
            Assert.AreEqual(Structure,lvlstructures.Structure);
        }
        
    }
}