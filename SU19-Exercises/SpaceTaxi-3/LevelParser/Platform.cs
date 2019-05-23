using System;
using System.Collections.Generic;
using DIKUArcade.Entities;

namespace SpaceTaxi_2 {
    public class Platform {

        public static List<List<pixel>> PlatformContainers { get; private set; }

        public static void CreateContainers(List<char> chars) {
            Platform.ResetContainers();
            
            foreach (var someChar in chars) {
                Platform.PlatformContainers.Add(new List<pixel>());
            }
        }
        
        public static void AddPixel(pixel pixel) {
            foreach (var container in Platform.PlatformContainers) {
                if (container.Count > 0) {
                    if (container[0].pixelChar == pixel.pixelChar) {
                        container.Add(pixel);
                    }
                } else {
                    container.Add(pixel);
                }
            }
        }

        private static void ResetContainers() {
            Platform.PlatformContainers = new List<List<pixel>>();
        }

        public static float[] GetWidth(char platformChar) {

            var WorkingContainer = new List<pixel>();
            
            //Searching for working container
            foreach (var container in PlatformContainers) {
                if (container.Count > 0) {
                    if (container[0].pixelChar == platformChar) {
                        WorkingContainer = container;
                        Console.WriteLine("Found working contaner for:"+container[0].pixelChar);
                        Console.WriteLine("Count platofrms:"+PlatformContainers.Count);
                        foreach (var askdnasod in PlatformContainers) {
                            Console.WriteLine("Container has: "+askdnasod[0].pixelChar);
                        }
                    }
                }
            }

            if (WorkingContainer.Count > 0) {
                var start = WorkingContainer[0];
                var end = WorkingContainer[WorkingContainer.Count - 1];

                var startX = start.Shape.Position.X;
                var endX = end.Shape.Position.X + end.Shape.Extent.X;

                return new float[] {
                    startX,
                    endX
                };
            } else {
                return new float[] {
                    0.0f,
                    0.0f
                };
            }
        }
        
    }
}