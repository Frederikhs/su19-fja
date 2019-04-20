using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SpaceTaxi_1 {
    internal class Program {
        
        public static void Main(string[] args) {
            LvlLegends some = new LvlLegends("short-n-sweet");
            Console.WriteLine(
            some.LegendsDic['%']);
        }
    }
}