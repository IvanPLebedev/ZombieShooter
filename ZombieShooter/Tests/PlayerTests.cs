using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NUnit.Framework;

namespace ZombieShooter
{
    [TestFixture]
    class PlayerTests
    {
        [Test]
        public static void FireTest()
        {
            var player = new Player(new Point(0,0));
            for ( var i = 0; i <= player.AttackSpeed; i++)
                player.Move();
            var bullet = player.Fire();
            Assert.AreEqual(player.Direction, bullet.MoveDirection, 1e-5);
        }

        [TestCase(new Keys[] { Keys.D }, 0)]
        [TestCase(new Keys[] { Keys.D, Keys.S },  Math.PI/4)]
        [TestCase(new Keys[] { Keys.A, },  0)]
        [TestCase(new Keys[] { Keys.S }, Math.PI/2)]
        public static void MovingTest(Keys[] keys, double expectedMoveDirection)
        {
            var firstPoint = new Point(0, 0);
            var player = new Player(firstPoint);
            foreach (var key in keys)
                Game.PressedKeys.Add(key);
            player.UpdateMoveDirection();
            Assert.AreEqual(expectedMoveDirection, player.MoveDirection, 1e-3);
            player.Move();
            var distance = Utils.GetDistance(firstPoint, player.Position);
            Assert.AreEqual((double)player.Speed, distance, 0.5);
            Game.PressedKeys.Clear();
        }

        [TestCase(1,0,0)]
        [TestCase(0, 1, Math.PI/2)]
        [TestCase(-1, 0, Math.PI)]
        [TestCase(1, 1, Math.PI/4)]
        public static void DirectionTest(int x, int y, double direction)
        {
            var player = new Player(new Point(0,0));
            Game.CursorPosition = new Point(x,y);
            Assert.AreEqual(direction, player.Direction, 1e-1);
        }
    }
}
