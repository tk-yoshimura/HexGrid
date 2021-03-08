using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

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

                    Assert.IsTrue(GridValidationUtil.IsValid(grid), $"{width}, {height}");
                    Assert.AreEqual(width * height, grid.Count, $"count {width}, {height}");

                    Assert.AreEqual(6, grid[0].Links);
                    Assert.AreEqual(6, grid[width - 1].Links);
                    Assert.AreEqual(6, grid[width * height - width].Links);
                    Assert.AreEqual(6, grid[width * height - 1].Links);

                    Assert.AreEqual(0, grid.Cells.Where((cell) => cell.Links != 6).Count());

                    Console.WriteLine("---------------------------");
                }
            }
        }
    }
}