using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using NUnit.Framework;
using SpaceTaxi;
using SpaceTaxi.GameStates;
using SpaceTaxi.LevelParser;
using SpaceTaxi.Taxi;

namespace SpaceTaxi_Tests {
    [TestFixture]
    public class Tests {

        [SetUp]
        public void Setup() {
            Window.CreateOpenGLContext();
        }

        /// <summary>
        /// Testing that we get the correct customer(s)
        /// </summary>
        [Test]
        public void short_n_sweet_LvlCustomer() {
            LvlCustomer lvlcustomer = new LvlCustomer(new TextLoader("short-n-sweet"));
            
            List<Dictionary<string, string>> testDict = new List<Dictionary<string, string>>();
            
            Dictionary<string, string> TestCustomer = new Dictionary<string, string>() {
                {"name", "Alice"},
                {"spawnAfter", "10"},
                {"spawnPlatform", "1"},
                {"destinationPlatform", "^J"},
                {"taxiDuration", "10"},
                {"points", "100"},
                {"generated", "false"}
            };
            
            testDict.Add(TestCustomer);
            
            CollectionAssert.AreEqual(testDict,lvlcustomer.AllCustomerDict);
        }

        /// <summary>
        /// Testing that we get the correct lvl info & structure for short-n-sweet
        /// </summary>
        [Test]
        public void short_n_sweet_LvlInfo() {
            LvlInfo lvlinfo = new LvlInfo(new TextLoader("short-n-sweet"));
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
            LvlLegends lvllegends = new LvlLegends(new TextLoader("short-n-sweet"));
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
            LvlStructures lvlstructures = new LvlStructures(new TextLoader("short-n-sweet"));
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
            LvlCustomer lvlcustomer = new LvlCustomer(new TextLoader("the-beach"));
            
            List<Dictionary<string, string>> testDict = new List<Dictionary<string, string>>();
            
            Dictionary<string, string> TestCustomerOne = new Dictionary<string, string>() {
                {"name", "Bob"},
                {"spawnAfter", "10"},
                {"spawnPlatform", "J"},
                {"destinationPlatform", "r"},
                {"taxiDuration", "10"},
                {"points", "100"},
                {"generated", "false"}
            };
            
            Dictionary<string, string> TestCustomerTwo = new Dictionary<string, string>() {
                {"name", "Carol"},
                {"spawnAfter", "30"},
                {"spawnPlatform", "r"},
                {"destinationPlatform", "^"},
                {"taxiDuration", "10"},
                {"points", "100"},
                {"generated", "false"}
            };
            
            testDict.Add(TestCustomerOne);
            testDict.Add(TestCustomerTwo);
            
            CollectionAssert.AreEqual(testDict,lvlcustomer.AllCustomerDict);
        }
        
        /// <summary>
        /// Testing that we get the correct lvl info & structure for the-beach
        /// </summary>
        [Test]
        public void the_beach_LvlInfo() {
            LvlInfo lvlinfo = new LvlInfo(new TextLoader("the-beach"));
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
            LvlLegends lvllegends = new LvlLegends(new TextLoader("the-beach"));
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
            LvlStructures lvlstructures = new LvlStructures(new TextLoader("the-beach"));
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
        /// Testing that we correctly enum state to string
        /// </summary>
        [Test]
        public void TransFormStateToString() {
            Assert.AreEqual("GAME_RUNNING",
                GameStateType.TransformStateToString(GameStateType.EnumGameStateType.GameRunning));
        }
        
        /// <summary>
        /// Testing that we correctly convert string to enum state
        /// </summary>
        [Test]
        public void TransFormStringToState() {
            Assert.AreEqual(GameStateType.EnumGameStateType.GameRunning,
                GameStateType.TransformStringToState("GAME_RUNNING"));
        }

        /// <summary>
        /// Testing we can pick up a customer and see that the customer changes status, and is
        /// inside the static list of customers inside the player
        /// </summary>
        [Test]
        public void PlayerPickUpCustomer() {
            GameRunning.GetInstance("the-beach");
            var testCust = new Customer(
                "", 0, 'c', "", 0, 0);
            Player.PickUpCustomer(testCust);
            var isInside = Player.CustomersInsidePlayer.Contains(testCust);
            
            Assert.AreEqual(CustomerState.InTransit,testCust.CustomerState);
            Assert.AreEqual(true, isInside);
        }
        
        /// <summary>
        /// Testing that we can place down a customer, and that the customer state changes
        /// </summary>
        [Test]
        public void PlayerPlaceDownCustomer() {
            GameRunning.GetInstance("the-beach");
            //Pickup a customer
            var testCust = new Customer(
                "", 0, 'c', "", 0, 0);
            Player.PickUpCustomer(testCust);
            
            //Place down a customer
            Game game = new Game(); //Creating game
            //Image for the pixel to be placed on
            Image img = new Image(Path.Combine("Assets", "Images", "aspargus-edge-bottom.png"));
            
            //Pixel to be placed on
             var pixel = new Pixel(game,
                new DynamicShape(
                    new Vec2F(0f,0f), new Vec2F(1f, 1f)), img,
                Pixel.PixelTypes.Platform, 'x');

            Player.PlaceDownCustomer(pixel,testCust);
            Assert.AreEqual(CustomerState.Delivered,testCust.CustomerState);
        }
        
        /// <summary>
        /// Testing that we can remove a customer from the CustomersInsidePlayer list
        /// </summary>
        [Test]
        public void PlayerRemoveCustomerFromList() {
            GameRunning.GetInstance("the-beach");
            var testCust = new Customer(
                "", 0, 'c', "", 0, 0);
            Player.PickUpCustomer(testCust);
            
            Player.RemoveCustomerFromList(testCust);

            Assert.AreEqual(false, Player.CustomersInsidePlayer.Contains(testCust));
        }
        
        /// <summary>
        /// Testing that we can clear the list of customers inside the player
        /// </summary>
        [Test]
        public void PlayerClearListOfCustomers() {
            List<Customer> empty = new List<Customer>();
            
            Player.ClearCustomers();
            
            Assert.AreEqual(empty, Player.CustomersInsidePlayer);
        }
        
        /// <summary>
        /// Testing that we can set the position of the player
        /// </summary>
        [Test]
        public void PlayerSetPosition() {
            Player player = new Player();
            
            player.SetPosition(1f,2f);
            
            Assert.AreEqual(1f, player.Entity.Shape.Position.X);
            Assert.AreEqual(2f, player.Entity.Shape.Position.Y);
        }
        
        /// <summary>
        /// Testing that we can set the extend of the player
        /// </summary>
        [Test]
        public void PlayerSetExtend() {
            Player player = new Player();
            
            player.SetExtent(1f,2f);
            
            Assert.AreEqual(1f, player.Entity.Shape.Extent.X);
            Assert.AreEqual(2f, player.Entity.Shape.Extent.Y);
        }
        
        /// <summary>
        /// Test of customer switch state
        /// </summary>
        [Test]
        public void CustomerSwitchState() {
            GameRunning.GetInstance("the-beach");
            var testCust = new Customer(
                "", 0, 'c', "", 0, 0);

            testCust.SwitchState(CustomerState.Delivered);
            Assert.AreEqual(CustomerState.Delivered,testCust.CustomerState);
        }
        
        /// <summary>
        /// Test of customer got picked up, and therefore the extend should be 0f,0f
        /// </summary>
        [Test]
        public void CustomerGotPickedUp() {
            GameRunning.GetInstance("the-beach");
            var testCust = new Customer(
                "", 0, 'c', "", 0, 0);

            testCust.GotPickedUp();
            Assert.AreEqual(0f,testCust.entity.Shape.Extent.X);
            Assert.AreEqual(0f,testCust.entity.Shape.Extent.Y);
        }
        
        /// <summary>
        /// Test of customer got displayed
        /// </summary>
        [Test]
        public void CustomerGotDisplayed() {
            GameRunning.GetInstance("the-beach");
            var testCust = new Customer(
                "", 0, 'c', "", 0, 0);

            testCust.DisplayCustomer();
            Assert.AreEqual(0.05f,testCust.entity.Shape.Extent.X);
            Assert.AreEqual(0.08f,testCust.entity.Shape.Extent.Y);
        }
        
        /// <summary>
        /// Testing that we can set the position of a customer
        /// </summary>
        [Test]
        public void CustomerSetPosition() {
            GameRunning.GetInstance("the-beach");
            var testCust = new Customer(
                "", 0, 'c', "", 0, 0);
            
            testCust.SetPos(new Vec2F(1f,2f));
            
            Assert.AreEqual(1f, testCust.entity.Shape.Position.X);
            Assert.AreEqual(2f, testCust.entity.Shape.Position.Y);
        }
        
        /// <summary>
        /// Testing that we calculate velocity correct
        /// </summary>
        [Test]
        public void Gravity() {
            Gravity grav = new Gravity();
            grav.GetNextVelocity(0, false);
        }
    }
}