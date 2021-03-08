using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HexGrid.Tests {
    [TestClass()]
    public class HexaGridTests {
        [TestMethod()]
        public void HexaGridTest() {

            for (int size = 1; size <= 16; size++) {

                Console.WriteLine($"size = {size}");

                HexaGrid grid = new HexaGrid(size);

                Console.WriteLine($"mapsize = {grid.MapWidth}, {grid.MapHeight}");

                Console.WriteLine("map :");

                Console.WriteLine(grid.ToString());
                    
                Console.WriteLine("link :");

                foreach ((int index, var link) in grid.Link) {
                    Console.WriteLine($"{index} : {string.Join(',', link)}");
                }

                Assert.IsTrue(grid.IsValid, $"{size}");
                Assert.AreEqual(1 + size * (size - 1) / 2 * 6, grid.Count, $"{size}");

                Console.WriteLine("---------------------------");
            }
        }
    }
}