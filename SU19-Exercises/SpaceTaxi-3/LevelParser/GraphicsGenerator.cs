using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SpaceTaxi_2
{
    public class GraphicsGenerator {
        private LvlLegends Legends;
        private LvlStructures Structure;
        public List<Entity> elements;
        private LvlInfo lvlinfo;
        private string[] lvlPlatforms;
        private Game game;
        public float width;
        public Player player;
        private bool isDangerous;

        public EntityContainer<pixel> AllGraphics;

        public GraphicsGenerator(LvlLegends legends, LvlStructures structures, LvlInfo lvlinfo, int width, Game game, Player player) {
            //We gather all level info required to produce graphics
            this.Legends = legends;
            this.Structure = structures;
            this.lvlinfo = lvlinfo;
            FindPlatformChars();

            this.game = game;
            this.player = player;
            this.width = width;
            this.AllGraphics = GenerateImages();
            this.isDangerous = true;
        }

        private void FindPlatformChars() {
            lvlPlatforms = Regex.Split(lvlinfo.InfoDic["Platforms"], ", ");
        }

        /// <summary>
        /// Method for creating an entity container for a given level based on legends, structures, width of viewport, game and player.
        /// </summary>
        private EntityContainer<pixel> GenerateImages() {
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

                        var isPlatform = false;
                        if (lvlPlatforms.Contains(someChar.ToString())) {
                            isDangerous = false;
                            isPlatform = true;
                        }
                        var image = new Image(Path.Combine("Assets", "Images", Legends.LegendsDic[someChar]));
                        returnContainer.AddStationaryEntity(
                            new pixel(game,
                                new DynamicShape(
                                    new Vec2F(posX,posY), new Vec2F(image_width, image_height)), image,isDangerous, false, isPlatform));
                    }
                    else {
                        switch (someChar)
                        {
                            case '^':
                                var image = new Image(Path.Combine("Assets", "Images", "aspargus-passage1.png"));
                                returnContainer.AddStationaryEntity(
                                    new pixel(game,
                                        new DynamicShape(
                                            new Vec2F(posX,posY), new Vec2F(image_width, image_height)), image,false, true,  false));
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
                    isDangerous = true;
                    
                }

                posX = 0f;
                posY -= image_height;
                isDangerous = true;
            }

            return returnContainer;
        }

        /// <summary>
        /// Convert the range of a float to the viewpoint
        /// </summary>
        private float ConvertRange(float i) {
            return i*1/this.width;
        }
    }
}