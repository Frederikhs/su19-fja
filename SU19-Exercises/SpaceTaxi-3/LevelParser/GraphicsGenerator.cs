using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_2
{
    public class GraphicsGenerator {
        private LvlLegends Legends;
        private LvlStructures Structure;
        private LvlCustomer Customer;
        private LvlInfo lvlinfo;
        private string[] lvlPlatforms;
        private List<char> lvlPlatformsChars;
        private Game game;
        private float width;
        private Player player;
        
        public EntityContainer<pixel> AllGraphics;
        public List<Customer> AllCustomersInGame;

        public GraphicsGenerator(LvlLegends legends, LvlStructures structures,
            LvlInfo lvlinfo, LvlCustomer lvlcustomer, int width, Game game, Player player) {
            this.Legends = legends;
            this.Structure = structures;
            this.lvlinfo = lvlinfo;
            this.Customer = lvlcustomer;
            this.lvlPlatformsChars = new List<char>();
            FindPlatformChars();
            
            this.AllCustomersInGame = new List<Customer>();

            this.game = game;
            this.player = player;
            this.width = width;

            Platform.CreateContainers(this.lvlPlatformsChars);
            this.AllGraphics = GenerateImages();

        }

        /// <summary>
        /// Finds each platform's char
        /// </summary>
        private void FindPlatformChars() {
            lvlPlatforms = Regex.Split(lvlinfo.InfoDic["Platforms"], ", ");
            foreach (var stringChar in lvlPlatforms) {
                this.lvlPlatformsChars.Add(Char.Parse(stringChar));
            }
            
        }

        /// <summary>
        /// Creates a pixel
        /// </summary>
        ///
        /// <param name="posX">
        /// The x position for the pixel
        /// </param>
        ///
        /// <param name="posY">
        /// The y position for the pixel
        /// </param>
        ///
        /// <param name="imageHeight">
        /// The height of the pixel
        /// </param>
        ///
        /// <param name="imageWidth">
        /// The width of the pixel
        /// </param>
        ///
        /// <param name="image">
        /// The image string name of the png file, without the ".png"
        /// </param>
        ///
        /// <param name="type">
        /// The type of pixel, needs to be one of the pixel.pixelTypes
        /// </param>
        ///
        /// <param name="pixelChar">
        /// The char of the pixel
        /// </param>
        ///
        /// <returns>
        /// A pixel object
        /// </returns>
        private pixel CreatePixel(
                float posX, float posY, float imageWidth, float imageHeight,
                string image, pixel.pixelTypes type, char pixelChar) {
            
            Image img = new Image(Path.Combine("Assets", "Images", image));
            
            return (new pixel(game,
                new DynamicShape(
                    new Vec2F(posX, posY), new Vec2F(imageWidth, imageHeight)), img,
                type, pixelChar));
        }

        /// <summary>
        /// Method for creating an entity container for a given level based on legends, structures, width of viewport, game and player.
        /// </summary>
        private EntityContainer<pixel> GenerateImages() {
            //We know that the width and height of the level chars is 40x23,
            //since we have 40 chars wide and 23 chars long level file
            float imageWidth = ConvertRange(width / 40);
            float imageHeight = ConvertRange(width / 23);
            
            //We start at pos -1,1 (top-left)
            float posX = 0f;
            float posY = 1f-1*imageHeight;

            //Pixel container with all graphical elements inside
            EntityContainer<pixel> returnContainer = new EntityContainer<pixel>();
                    
            foreach (var elem in Structure.Structure) {
                char[] line = new char[elem.Length];
                line = elem.ToCharArray();
                
                foreach (char someChar in line) {
                    if (Legends.LegendsDic.ContainsKey(someChar)) {
                        // Basic pixelType
                        var type = pixel.pixelTypes.dangerus;
                        
                        //If the pixel is a platform, we place down a customer
                        if (lvlPlatforms.Contains(someChar.ToString())) {
                            type = pixel.pixelTypes.platform;
                            //Place the customer down
                            PlaceCustomer(someChar, posX, posY, imageHeight);
                        }
                        
                        //Create the pixel
                        var somePixel = CreatePixel(posX, posY, imageWidth, imageHeight, Legends.LegendsDic[someChar],
                            type, someChar);
                        
                        //Add the pixel to the return container
                        returnContainer.AddStationaryEntity(somePixel);
                        
                        //If the pixel is a platform, also add to platform pixels list
                        if (lvlPlatforms.Contains(someChar.ToString())) {
                            Platform.AddPixel(somePixel);
                        }

                    }  else {
                        switch (someChar) {
                            case '^': //Portal
                                returnContainer.AddStationaryEntity(CreatePixel(posX, posY,
                                    imageWidth, imageHeight, "aspargus-passage1.png",
                                    pixel.pixelTypes.portal,
                                    someChar));
                                break;
                            case '>': //Player
                                player.SetPosition(posX, posY);
                                break;
                        }
                    }

                    //We are iterating the horizontal line, thus incrementing the x position
                    posX += imageWidth;
                }

                //We are iterating the vertical line, thus decrementing the y position,
                //and setting x position to 0
                posX = 0f;
                posY -= imageHeight;
            }

            //Finally return the container with all the pixels, player, and customers inside inside
            return returnContainer;
        }

        /// <summary>
        /// Places down a customer on the screen, and changes its generated key to true,
        /// so it cannot be displayed more than once
        /// </summary>
        private void PlaceCustomer(char pixelChar, float posX, float posY, float imageHeight) {
            //Placing customer
            foreach (var customerValues in Customer.AllCustomerDict) {
                if (customerValues["spawnPlatform"] == pixelChar.ToString() &&
                    customerValues["generated"] == "false") {
                    Customer temp = new Customer(
                        customerValues["name"],
                        Int32.Parse(customerValues["spawnAfter"]),
                        Char.Parse(customerValues["spawnPlatform"]),
                        customerValues["destinationPlatform"],
                        Int32.Parse(customerValues["taxiDuration"]),
                        Int32.Parse(customerValues["points"])
                    );

                    customerValues["generated"] = "true";

                    temp.visible = true;
                    temp.SetPos(new Vec2F(posX,posY+imageHeight-0.000001f));

                    Console.WriteLine("Spawned customer");
                    AllCustomersInGame.Add(temp);
                }
            }
        }
        
        /// <summary>
        /// Convert the range of a float to the viewpoint
        /// </summary>
        /// 
        /// <param name="i">
        /// The width of viewport/40
        /// </param>
        ///
        /// <returns>
        /// Converted pixel width or height based on viewport
        /// </returns>
        private float ConvertRange(float i) {
            return i*1/width;
        }
    }
}