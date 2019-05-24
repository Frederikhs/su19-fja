using System;
using System.Collections.Generic;
using DIKUArcade.Math;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SpaceTaxi_1;
using SpaceTaxi_2;
using SpaceTaxi_3;

namespace SpaceTaxi_Tests {
    [TestFixture]
    public class Tests {
        
        /// <summary>
        /// Testing that we get the correct customer(s) & structure for short-n-sweet
        /// </summary>
        [Test]
        public void short_n_sweet_LvlCustomer() {
            SpaceTaxi_1.LvlCustomer lvlcustomer = new SpaceTaxi_1.LvlCustomer(new SpaceTaxi_1.TextLoader("short-n-sweet"));
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
            SpaceTaxi_1.LvlInfo lvlinfo = new SpaceTaxi_1.LvlInfo(new SpaceTaxi_1.TextLoader("short-n-sweet"));
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
            SpaceTaxi_1.LvlLegends lvllegends = new SpaceTaxi_1.LvlLegends(new SpaceTaxi_1.TextLoader("short-n-sweet"));
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
            SpaceTaxi_1.LvlStructures lvlstructures = new SpaceTaxi_1.LvlStructures(new SpaceTaxi_1.TextLoader("short-n-sweet"));
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
            SpaceTaxi_1.LvlCustomer lvlcustomer = new SpaceTaxi_1.LvlCustomer(new SpaceTaxi_1.TextLoader("the-beach"));
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
            SpaceTaxi_1.LvlInfo lvlinfo = new SpaceTaxi_1.LvlInfo(new SpaceTaxi_1.TextLoader("the-beach"));
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
            SpaceTaxi_1.LvlLegends lvllegends = new SpaceTaxi_1.LvlLegends(new SpaceTaxi_1.TextLoader("the-beach"));
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
            SpaceTaxi_1.LvlStructures lvlstructures = new SpaceTaxi_1.LvlStructures(new SpaceTaxi_1.TextLoader("the-beach"));
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

        /// <summary>
        /// TODO: Implement PlayerPixelCollision
        /// Test for checking player collision with an object in the level that is dangerous.
        /// </summary>
        [Test]
        public void PlayerPixelCollision() {
            //Place player beside pixel to check for collision
        }
        
        /// <summary>
        /// TODO: Implement PlayerPlatform
        /// Test for checking player collision with the platform and that the player does not die
        /// </summary>
        [Test]
        public void PlayerPlatform() {
            //Place player beside pixel to check for collision
        }
        
        /// <summary>
        /// TODO: Implement PlayerPortal
        /// Test for checking player collision with the portal and if the level changes
        /// </summary>
        [Test]
        public void PlayerPortal() {
            //Place player beside pixel to check for collision
        }
        
        /// <summary>
        /// TODO: Implement PlayerPickUpCustomer
        /// Test for checking player collision with a customer
        /// </summary>
        [Test]
        public void PlayerPickUpCustomer() {
            //Place player beside a customer to check for collision
        }
        
        /// <summary>
        /// TODO: Implement PlayerPlaceDownCustomer
        /// Test for checking player collision with a the platform while a customer is inside
        /// </summary>
        [Test]
        public void PlayerPlaceDownCustomer() {
            //Place player on the platform to check for collision
        }

        /// <summary>
        /// Test that the customer is show and that is has an extend
        /// TODO: Test need environment with GameRunning for success
        /// </summary>
        [Test]
        public void CustomerShow() {
            Customer cust = new Customer("Test",0,'J',"K",5,1); 
            cust.Show();
            Assert.AreEqual(true, cust.visible);
            Assert.AreEqual(new Vec2F(0.05f, 0.08f), cust.entity.Shape.Extent);
        }
        
        /// <summary>
        /// Test that the customer is hidden and that is has no extend
        /// TODO: Test need environment with GameRunning for success
        /// </summary>
        [Test]
        public void CustomerHide() {
            Customer cust = new Customer("Test",0,'J',"K",5,1); 
            cust.Hide();
            Assert.AreEqual(false, cust.visible);
            Assert.AreEqual(new Vec2F( 0f,0f), cust.entity.Shape.Extent);
        }
    }
}