using System;
using System.Drawing;
using NUnit.Framework;

namespace ZombieShooter
{
    [TestFixture]
    public class UtilsTest
    {
        [TestCase(0, 0, 0,1,Math.PI /2)]
        [TestCase(0, 0, 1, 0, 0)]
        [TestCase(0, 0, 0, -1, -Math.PI / 2)]
        [TestCase(0, 0, -1, 0, Math.PI)]
        public static void GetDirectionTests(int x1, int y1, int x2, int y2, double direction)
        {
            var dir = Utils.GetDirection(new Point(x1, y1), new Point(x2, y2));
            Assert.AreEqual(direction, dir, 1e-5);
        }

        [TestCase(0, 0, 0, 1, 1)]
        [TestCase(0, 0, 1, 0, 1)]
        [TestCase(0, 0, 0, -1, 1)]
        [TestCase(0, 0, -1, 0, 1)]
        [TestCase(0, 0, 0, -2, 2)]
        [TestCase(0, 0, 2,0 , 2)]
        public static void GetDistanceTests(int x1, int y1, int x2, int y2, double distance)
        {
            var dir = Utils.GetDistance(new Point(x1, y1), new Point(x2, y2));
            Assert.AreEqual(distance, dir, 1e-5);
        }

        [TestCase(0, 0, 0, 0)]
        [TestCase(1, 0, 1, 0)]
        [TestCase(2, 0, 2, 0)]
        [TestCase(2, Math.PI/2, 0, 2)]
        public static void GetSizeMovePointTest(double distance, double direction, int x, int y)
        {
            var size = Utils.GetSizeMovePoint(distance, direction);
            Assert.AreEqual(x, size.Width, 1);
            Assert.AreEqual(y, size.Height, 1);
        }
    }
}
