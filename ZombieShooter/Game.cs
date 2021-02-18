using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ZombieShooter
{
    public static class Game
    {
        public static double ElementSize = 2.5;
        public const int MapHeight = 360;
        public const int MapWidth = 560; 
        public const int TimeInterval = 30;
        public static Point CursorPosition = new Point(MapHeight, MapWidth);
        public static HashSet<Keys> PressedKeys = new HashSet<Keys>();
        public static readonly int[][] LevelsСonfiguration = new[]
        {
            new[] {40, 2, 1, 1, 40},
            new[] {40, 4, 1, 1, 40},
            new[] {40, 2, 2, 2, 40},
            new[] {60, 2, 1, 1, 15}
        };
    }
}
