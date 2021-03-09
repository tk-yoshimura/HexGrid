using Microsoft.VisualStudio.TestTools.UnitTesting;
using HexGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                for (double v = 0; v <= mapping.Height; v += 0.5) { 
                    for (double u = 0; u <= mapping.Width; u += 0.5) {
                        Console.WriteLine($"{u:F1},{v:F1} : {mapping[u, v]}");
                    }

                    Console.WriteLine("");
                }

                Console.WriteLine("---------------------------");
            }
        }
    }
}