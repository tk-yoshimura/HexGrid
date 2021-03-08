using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace HexGrid.Tests {
    [TestClass()]
    public class RingGridTests {
        [TestMethod()]
        public void RingGridTest() {

            for (int height = 1; height <= 8; height++) {
                for (int width = 2; width <= 8; width++) {

                    Console.WriteLine($"size = {width}, {height}");

                    RingGrid grid = new RingGrid(width, height);

                    Console.WriteLine($"mapsize = {grid.MapWidth}, {grid.MapHeight}");

                    Console.WriteLine("map :");

                    Console.WriteLine(grid.ToString());

                    Console.WriteLine("link :");

                    foreach ((int index, var link) in grid.Link) {
                        Console.WriteLine($"{index} : {string.Join(',', link)}");
                    }

                    Assert.IsTrue(GridValidationUtil.IsValid(grid), $"{width}, {height}");
                    Assert.AreEqual(width * height, grid.Count, $"count {width}, {height}");

                    if (height >= 2) {
                        Assert.AreEqual(4, grid[0].Links);
                        Assert.AreEqual(4, grid[width - 1].Links);
                        Assert.AreEqual(4, grid[width * height - width].Links);
                        Assert.AreEqual(4, grid[width * height - 1].Links);

                        Assert.AreEqual(0, grid.Cells.Where((cell) => cell.Links < 4).Count());
                    }

                    Console.WriteLine("---------------------------");
                }
            }
        }
    }
}