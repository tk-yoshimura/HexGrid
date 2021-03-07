﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using HexGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexGrid.Tests {
    [TestClass()]
    public class HexaGridTests {
        [TestMethod()]
        public void HexaGridTest() {

            for (int size = 1; size <= 8; size++) {

                Console.WriteLine($"size = {size}");

                HexaGrid grid = new HexaGrid(size);

                Console.WriteLine($"mapsize = {grid.MapWidth}, {grid.MapHeight}");

                Console.WriteLine("map :");

                Console.WriteLine(grid.ToString());
                    
                Console.WriteLine("link :");

                int index = 0;
                foreach (var l in grid.Link) {
                    Console.WriteLine($"{index} : {string.Join(',', l)}");

                    index++;
                }

                Assert.IsTrue(grid.IsValid, $"{size}");
                Assert.AreEqual(1 + size * (size - 1) / 2 * 6, grid.Count, $"{size}");

                Console.WriteLine("---------------------------");
            }
        }
    }
}