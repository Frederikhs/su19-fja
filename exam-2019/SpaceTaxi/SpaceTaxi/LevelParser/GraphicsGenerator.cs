using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using SpaceTaxi.Taxi;

namespace SpaceTaxi.LevelParser
{
    public class GraphicsGenerator {
        public EntityContainer<Pixel> AllGraphics;
        public List<Customer> AllCustomersInGame;
        
        private LvlLegends legends;
        private LvlStructures structure;
        private LvlCustomer customer;
        private LvlInfo info;
        private string[] platforms;
        private List<char> platformsChars;
        private Game game;
        private float width;
        private Player player;

        /// <summary>
        /// Generates a level based on level element classes provided.
        /// </summary>
        public GraphicsGenerator(LvlLegends legends, LvlStructures structures,
            LvlInfo info, LvlCustomer customer, int width, Game game, Player player) {
            this.legends = legends;
            this.structure = structures;
            this.info = info;
            this.customer = customer;
            this.platformsChars = new List<char>();
            
            FindPlatformChars();
            
            this.AllCustomersInGame = new List<Customer>();

            this.game = game;
            this.player = player;
            this.width = width;

            Platform.CreateContainers(this.platformsChars);
            this.AllGraphics = GenerateImages();

        }

        /// <summary>
        /// Finds each platform's char
        /// </summary>
        private void FindPlatformChars() {
            platforms = Regex.Split(info.InfoDic["Platforms"], ", ");
            foreach (var stringChar in platforms) {
                this.platformsChars.Add(Char.Parse(stringChar));
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
        private Pixel CreatePixel(
                float posX, float posY, float imageWidth, float imageHeight,
                string image, Pixel.PixelTypes type, char pixelChar) {
            
            Image img = new Image(Path.Combine("Assets", "Images", image));
            
            return (new Pixel(game,
                new DynamicShape(
                    new Vec2F(posX, posY), new Vec2F(imageWidth, imageHeight)), img,
                type, pixelChar));
        }

        /// <summary>
        /// Method for creating an entity container for a given level based on legends, structures,
        /// width of viewport, game and player.
        /// </summary>
        private EntityContainer<Pixel> GenerateImages() {
            //We know that the width and height of the level chars is 40x23,
            //since we have 40 chars wide and 23 chars long level file
            float imageWidth = ConvertRange(width / 40);
            float imageHeight = ConvertRange(width / 23);
            
            //We start at pos -1,1 (top-left)
            float posX = 0f;
            float posY = 1f-1*imageHeight;

            //Pixel container with all graphical elements inside
            EntityContainer<Pixel> returnContainer = new EntityContainer<Pixel>();
                    
            foreach (var elem in structure.Structure) {
                char[] line = new char[elem.Length];
                line = elem.ToCharArray();
                
                foreach (char someChar in line) {
                    if (legends.LegendsDic.ContainsKey(someChar)) {
                        // Basic pixelType
                        var type = Pixel.PixelTypes.Dangerous;
                        
                        //If the pixel is a platform, we place down a customer
                        if (platforms.Contains(someChar.ToString())) {
                            type = Pixel.PixelTypes.Platform;
                            //Place the customer down
                            PlaceCustomer(someChar, posX, posY, imageHeight);
                        }
                        
                        //Create the pixel
                        var somePixel = CreatePixel(posX, posY, imageWidth, imageHeight,
                            legends.LegendsDic[someChar], type, someChar);
                        
                        //Add the pixel to the return container
                        returnContainer.AddStationaryEntity(somePixel);
                        
                        //If the pixel is a platform, also add to platform pixels list
                        if (platforms.Contains(someChar.ToString())) {
                            Platform.AddPixel(somePixel);
                        }

                    }  else {
                        switch (someChar) {
                            case '^': //Portal
                                returnContainer.AddStationaryEntity(CreatePixel(posX, posY,
                                    imageWidth, imageHeight, "aspargus-passage1.png",
                                    Pixel.PixelTypes.Portal,
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
        ///
        /// <param name="pixelChar">
        /// The char the customer should be placed on
        /// </param>
        ///
        /// <param name="posX">
        /// The x position of the customer
        /// </param>
        ///
        /// <param name="posY">
        /// The y position of the customer
        /// </param>
        ///
        /// <param name="imageHeight">
        /// The height of a pixel. The customers y position will be y+imageHeight 
        /// </param>
        private void PlaceCustomer(char pixelChar, float posX, float posY, float imageHeight) {
            //Placing customer
            foreach (var customerValues in customer.AllCustomerDict) {
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
                    temp.SetPos(new Vec2F(posX,posY+imageHeight-0.000001f));
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