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

        public GraphicsGenerator(string level, int width, int height, Game game) {
            //We gather all level info required to produce graphics
            Legends = new LvlLegends(level);
            Structure = new LvlStructures(level);
            GenerateImages(width, height);
            
            pixel_container = new EntityContainer<pixel>();

            this.game = game;

        }

        public EntityContainer<Entity> GenerateImages(int width, int height) {
            //We calculate the width and height of each "pixel"
            //We know that the width and height of the level chars is 40x23
            var image_width = ConvertRange((float)width / 40);
            var image_height = ConvertRange((float)height / 23);
            //Each image is to be image_width wide, and have image_height height
            
            //We start at pos -1,1 (top-left), each image is to be placed image_width and image_height apart
            var posX = -1f;
            var posY = 1f;

            EntityContainer<Entity> returner = new EntityContainer<Entity>();
                    
            //We iterate over each line
            foreach (var elem in Structure.structure) {
                //Then we iterate over each char in the line 
                char[] line = new char[elem.Length];
                line = elem.ToCharArray();
                foreach (var someChar in line)
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
                        var image = new Image(Path.Combine("Assets", "Images", "deep-bronze-square.png"));
                        returner.AddDynamicEntity(
                            new pixel(game,
                                new DynamicShape(
                                    new Vec2F(posX,posY), new Vec2F(image_width, image_height)), image));
                    }

                    posX += image_width;
                    Console.WriteLine("PosX:"+posX+" PosY:"+posY);
                    if (Legends.LegendsDic.ContainsKey(someChar))
                    {
                        Console.WriteLine(Legends.LegendsDic[someChar]);
                    }
                }

                posX = 1f;
                posY += -image_height;
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