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

                    Console.WriteLine($"{width}, {height}");

                    RectGrid grid = new RectGrid(width, height);

                    int?[,] cells = new int?[width, height * 2];

                    for (int y = 0, i = 0; y < height; y++) { 
                        for (int x = 0; x < width; x++, i++) {
                            (_, int px, int py) = grid[x, y];

                            cells[px, py] = i;
                        }
                    }

                    Console.WriteLine("pos :");

                    for (int y = 0; y < height * 2; y++) { 
                        for (int x = 0; x < width; x++) {
                            if (cells[x, y] is null) {
                                Console.Write("    ");
                            }
                            else { 
                                Console.Write($"{cells[x, y].Value,4}");
                            }
                        }

                        Console.Write("\n");
                    }

                    Console.WriteLine("link :");

                    int index = 0;
                    foreach (var l in grid.Link) {
                        Console.WriteLine($"{index} : {string.Join(',', l)}");

                        index++;
                    }

                    Console.WriteLine("plot :");

                    Console.WriteLine(grid.PlotNeighborCells(0));
                    Console.WriteLine(grid.PlotNeighborCells(width - 1));
                    Console.WriteLine(grid.PlotNeighborCells(grid.Count / 2));
                    Console.WriteLine(grid.PlotNeighborCells(width * height - width));
                    Console.WriteLine(grid.PlotNeighborCells(width * height - 1));

                    Assert.IsTrue(grid.IsValid, $"{width}, {height}");
                }
            }
        }
    }
}