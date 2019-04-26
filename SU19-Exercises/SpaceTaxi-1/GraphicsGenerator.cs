using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using OpenTK;

namespace SpaceTaxi_1
{
    public class GraphicsGenerator {
        private LvlLegends Legends;
        private LvlStructures Structure;
        public List<Entity> elements;
        private Game game;
        public float width;
        public Player player;

        public GraphicsGenerator(string level, int width, int height, Game game, Player player) {
            //We gather all level info required to produce graphics
            Legends = new LvlLegends(level);
            Structure = new LvlStructures(level);
            this.game = game;
            this.player = player;
            this.width = width;
        }

        public EntityContainer<pixel> GenerateImages() {
            var width = (int) this.width;
            var height = (int) this.width;
            var player = this.player;
            
            //We calculate the width and height of each "pixel"
            //We know that the width and height of the level chars is 40x23
            var image_width = ConvertRange((float)width / 40);
            var image_height = ConvertRange((float)height / 23);
            //Each image is to be image_width wide, and have image_height height
            
            //We start at pos -1,1 (top-left), each image is to be placed image_width and image_height apart
            var posX = 0f;
            var posY = 1f-1*image_height;

            EntityContainer<pixel> returnContainer = new EntityContainer<pixel>();
                    
            //We iterate over each line
            foreach (var elem in Structure.Structure) {
                //Then we iterate over each char in the line 
                char[] line = new char[elem.Length];
                line = elem.ToCharArray();
                foreach (char someChar in line) {
                    if (Legends.LegendsDic.ContainsKey(someChar)) {
                        var image = new Image(Path.Combine("Assets", "Images", Legends.LegendsDic[someChar]));
                        returnContainer.AddDynamicEntity(
                            new pixel(game,
                                new DynamicShape(
                                    new Vec2F(posX,posY), new Vec2F(image_width, image_height)), image));
                    }
                    else {
                        switch (someChar)
                        {
                            case '^':
                                //Empty for passage way
                                break;
                            case '>':
                                //This is the player. We set the position
                                player.SetPosition(posX, posY);
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

            return returnContainer;
        }

        //Converts the range of an aspect ratio to another
        public float ConvertRange(float i) {
            return i*1/this.width;
        }
    }
}