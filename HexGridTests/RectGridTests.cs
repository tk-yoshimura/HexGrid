using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace HexGrid.Tests {
    [TestClass()]
    public class RectGridTests {
        [TestMethod()]
        public void RectGridTest() {

            for (int height = 1; height <= 8; height++) {
                for (int width = 1; width <= 8; width++) {

                    Console.WriteLine($"size = {width}, {height}");

                    RectGrid grid = new RectGrid(width, height);

                    Console.WriteLine($"mapsize = {grid.MapWidth}, {grid.MapHeight}");

                    Console.WriteLine("map :");

                    Console.WriteLine(grid.ToString());

                    Console.WriteLine("link :");

                    foreach ((int index, var link) in grid.Link) {
                        Console.WriteLine($"{index} : {string.Join(',', link)}");
                    }

                    Assert.IsTrue(GridValidationUtil.IsValid(grid), $"{width}, {height}");
                    Assert.AreEqual(width * height, grid.Count, $"count {width}, {height}");

                    if (width >= 2 && height >= 2) {
                        Assert.AreEqual(2, grid[0].Links);
                        Assert.AreEqual(3, grid[width - 1].Links);

                        if (height % 2 == 0) {
                            Assert.AreEqual(3, grid[width * height - width].Links);
                            Assert.AreEqual(2, grid[width * height - 1].Links);
                        }
                        else{
                            Assert.AreEqual(2, grid[width * height - width].Links);
                            Assert.AreEqual(3, grid[width * height - 1].Links);
                        }

                        Assert.AreEqual(2, grid.Cells.Where((cell) => cell.Links == 2).Count());
                    }

                    Console.WriteLine("---------------------------");
                }
            }
        }
    }
}