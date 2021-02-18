using System.Linq;
using System.Windows.Forms;
using NUnit.Framework;

namespace ZombieShooter
{
    [TestFixture]
    class GameStateTests
    {
        [Test]
        public static void PlayerDiedInThousandTicks()
        {
            var gameState = new GameState(5);
            var flag = false;
            gameState.Player.Died += () => flag = true;
            var i = 0;
            while (i < 1000)
            {
                i++;
                gameState.OnTick();
            }

            Assert.AreEqual(true, flag);
        }

        [Test]
        public static void CheckPlayerFireRate()
        {
            var gameState = new GameState(0);
            var i = 0;
            var flag = false;
            while (!flag)
            {
                i++;
                gameState.OnTick();
                flag = gameState.Player.CanAttack();
            }
            Assert.AreEqual(i, gameState.Player.AttackSpeed);
        }

        [Test]
        public static void ZombieGoToThePlayer()
        {
            var gameState = new GameState(1);
            var distance = double.MaxValue;
            for (var i = 0; i < 100; i++)
                if (gameState.Zombies.Any())
                    Assert.AreEqual(true,
                        distance > Utils.GetDistance(gameState.Player.Position, gameState.Zombies[i].Position));

        }

        [Test]
        public static void PlayerCanNotGoBeyondLeftBoundaries()
        {
            var gameState = new GameState(1);
            Game.PressedKeys.Add(Keys.A);
            var i = 0;
            while (i < 1000)
            {
                i++;
                gameState.OnTick();
            }
            Assert.AreEqual(true, gameState.Player.Position.X > -5);
        }

        [Test]
        public static void PlayerCanNotGoBeyondRightBoundaries()
        {
            var gameState = new GameState(1);
            Game.PressedKeys.Add(Keys.D);
            var i = 0;
            while (i < 1000)
            {
                i++;
                gameState.OnTick();
            }
            Assert.AreEqual(true, gameState.Player.Position.X < Game.MapWidth + 5);
        }

        [Test]
        public static void PlayerCanNotGoBeyondTopBoundaries()
        {
            var gameState = new GameState(1);
            Game.PressedKeys.Add(Keys.W);
            var i = 0;
            while (i < 1000)
            {
                i++;
                gameState.OnTick();
            }
            Assert.AreEqual(true, gameState.Player.Position.Y > - 5);
        }

        [Test]
        public static void PlayerCanNotGoBeyondBottomBoundaries()
        {
            var gameState = new GameState(1);
            Game.PressedKeys.Add(Keys.S);
            var i = 0;
            while (i < 1000)
            {
                i++;
                gameState.OnTick();
            }
            Assert.AreEqual(true, gameState.Player.Position.Y < Game.MapHeight + 5);
        }
    }
}
