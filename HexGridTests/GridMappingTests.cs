using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HexGrid.Tests {
    [TestClass()]
    public class GridMappingTests {
        [TestMethod()]
        public void GridMappingTest() {
            foreach (Grid grid in new Grid[] { 
                new RectGrid(2, 2), new RectGrid(3, 2), new RectGrid(2, 3), new RectGrid(3, 3), 
                new TriGrid(2), new TriGrid(3), new HexaGrid(2), new HexaGrid(3) }) {
                
                GridMapping mapping = new GridMapping(grid);

                Console.WriteLine(grid.ToString());

                for (int y = 0; y < mapping.Height; y++) { 
                    for (int x = 0; x <= mapping.Width; x++) {
                        Console.WriteLine($"{x},{y} : {mapping[x, y]}");
                    }

                    Console.WriteLine("");
                }

                Console.WriteLine("---------------------------");
            }
        }
    }
}