using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

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

                Assert.IsTrue(GridValidationUtil.IsValid(grid), $"{size}");
                Assert.AreEqual(1 + size * (size - 1) / 2 * 6, grid.Count, $"{size}");

                if (size >= 2) {
                    Assert.AreEqual(3, grid[0].Links);
                    Assert.AreEqual(3, grid[size - 1].Links);
                    Assert.AreEqual(3, grid[grid.Count / 2 - size + 1].Links);
                    Assert.AreEqual(3, grid[grid.Count / 2 + size - 1].Links);                    
                    Assert.AreEqual(3, grid[grid.Count - size].Links);
                    Assert.AreEqual(3, grid[grid.Count - 1].Links);

                    Assert.AreEqual(0, grid.Cells.Where((cell) => cell.Links < 3 || cell.Links == 5).Count());
                    Assert.AreEqual(6, grid.Cells.Where((cell) => cell.Links == 3).Count());
                    Assert.AreEqual((size - 2) * 6, grid.Cells.Where((cell) => cell.Links == 4).Count());
                }

                Console.WriteLine("---------------------------");
            }
        }
    }
}