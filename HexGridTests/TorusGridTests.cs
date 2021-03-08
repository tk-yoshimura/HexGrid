using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HexGrid.Tests {
    [TestClass()]
    public class TorusGridTests {
        [TestMethod()]
        public void TorusGridTest() {

            for (int height = 2; height <= 8; height += 2) {
                for (int width = 2; width <= 8; width++) {

                    Console.WriteLine($"size = {width}, {height}");

                    TorusGrid grid = new TorusGrid(width, height);

                    Console.WriteLine($"mapsize = {grid.MapWidth}, {grid.MapHeight}");

                    Console.WriteLine("map :");

                    Console.WriteLine(grid.ToString());
                    
                    Console.WriteLine("link :");

                    foreach ((int index, var link) in grid.Link) {
                        Console.WriteLine($"{index} : {string.Join(',', link)}");
                    }

                    Assert.IsTrue(grid.IsValid, $"{width}, {height}");
                    Assert.AreEqual(width * height, grid.Count, $"count {width}, {height}");

                    Console.WriteLine("---------------------------");
                }
            }
        }
    }
}