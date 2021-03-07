using Microsoft.VisualStudio.TestTools.UnitTesting;
using HexGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexGrid.Tests {
    [TestClass()]
    public class RectGridTests {
        [TestMethod()]
        public void RectGridTest() {

            for (int height = 1; height <= 8; height++) {
                for (int width = 1; width <= 8; width++) {

                    Console.WriteLine($"size = {width}, {height}");

                    RectGrid grid = new RectGrid(width, height);

                    Console.WriteLine("coord :");

                    Console.WriteLine(grid.ToMap());

                    Console.WriteLine("link :");

                    int index = 0;
                    foreach (var l in grid.Link) {
                        Console.WriteLine($"{index} : {string.Join(',', l)}");

                        index++;
                    }

                    Assert.IsTrue(grid.IsValid, $"{width}, {height}");

                    Console.WriteLine("---------------------------");
                }
            }
        }
    }
}