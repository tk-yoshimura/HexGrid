﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HexGrid.Tests {
    [TestClass()]
    public class RingGridTests {
        [TestMethod()]
        public void RingGridTest() {

            for (int height = 1; height <= 8; height++) {
                for (int width = 2; width <= 8; width += 1) {

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

                    Console.WriteLine("---------------------------");
                }
            }
        }
    }
}