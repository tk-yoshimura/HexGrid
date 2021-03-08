using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HexGrid.Tests {
    [TestClass()]
    public class TriGridTests {
        [TestMethod()]
        public void TriGridTest() {

            for (int size = 1; size <= 16; size++) {

                Console.WriteLine($"size = {size}");

                TriGrid grid = new TriGrid(size);

                Console.WriteLine($"mapsize = {grid.MapWidth}, {grid.MapHeight}");

                Console.WriteLine("map :");

                Console.WriteLine(grid.ToString());

                Console.WriteLine("link :");

                foreach ((int index, var link) in grid.Link) {
                    Console.WriteLine($"{index} : {string.Join(',', link)}");
                }

                Assert.IsTrue(GridValidationUtil.IsValid(grid), $"{size}");
                Assert.AreEqual(size * (size + 1) / 2, grid.Count, $"{size}");

                Console.WriteLine("---------------------------");
            }
        }
    }
}