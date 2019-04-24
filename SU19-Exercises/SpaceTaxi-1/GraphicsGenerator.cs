using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using OpenTK;

namespace SpaceTaxi_1
{
    public class GraphicsGenerator
    {

        private LvlLegends Legends;
        private LvlStructures Structure;
        public List<Entity> elements;
        
        public EntityContainer<pixel> pixel_container { get; }
        
        private Game game;

        public float width;
        public Player player;

        public GraphicsGenerator(string level, int width, int height, Game game, Player player) {
            //We gather all level info required to produce graphics
            Legends = new LvlLegends(level);
            Structure = new LvlStructures(level);
            GenerateImages(width, height, player);
            
            pixel_container = new EntityContainer<pixel>();

            this.game = game;
            this.player = player;
        }

        public EntityContainer<Entity> GenerateImages(int width, int height, Player player) {
            //We calculate the width and height of each "pixel"
            //We know that the width and height of the level chars is 40x23
            var image_width = ConvertRange((float)width / 40);
            var image_height = ConvertRange((float)height / 23);
            //Each image is to be image_width wide, and have image_height height
            
            //We start at pos -1,1 (top-left), each image is to be placed image_width and image_height apart
            var posX = 0f;
            var posY = 1f-1*image_height;
            this.width = posX;

            EntityContainer<Entity> returner = new EntityContainer<Entity>();
                    
            //We iterate over each line
            foreach (var elem in Structure.structure) {
                //Then we iterate over each char in the line 
                char[] line = new char[elem.Length];
                line = elem.ToCharArray();
                foreach (char someChar in line)
                {
                    if (Legends.LegendsDic.ContainsKey(someChar))
                    {
                        var image = new Image(Path.Combine("Assets", "Images", Legends.LegendsDic[someChar]));
                        returner.AddDynamicEntity(
                            new pixel(game,
                                new DynamicShape(
                                    new Vec2F(posX,posY), new Vec2F(image_width, image_height)), image));
                    }
                    else
                    {
                        switch (someChar)
                        {
                            case '^':
                                //Empty for passage way
                                break;
                            case '>':
                                //This is the player. We set the position
                                player.SetPosition(posX, posY);
                                Console.WriteLine("Sets player pos to"+posX+" & "+posY);
                                break;
                            default:
                                break;
                        }
                    }
                    

                    posX += image_width;
                    
                }

                posX = 0f;
                posY -= image_height;
            }

            return returner;
        }

        public float ConvertRange(float i) {
            var val1 = i;
            var min1 = 0f;
            var max1 = 500f;
            var range1 = max1 - min1;

            var min2 = 0f;
            var max2 = 1f;
            var range2 = max2 - min2;

            return val1*range2/range1 + min2;
        }
    }
}