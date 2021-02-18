using System;
using System.Drawing;
using System.Linq;


namespace ZombieShooter
{
    public static class Utils
    {
        public static double GetDirection(Point position, Point directionPoint)
        {
            return Math.Atan2(directionPoint.Y - position.Y, directionPoint.X - position.X );
        }

        public static double GetDistance(Point first, Point second)
        {
            return Math.Sqrt((second.X - first.X) * (second.X - first.X) + (second.Y - first.Y) * (second.Y - first.Y));
        }

        public static Point[] GetDestinationPoints(Point position, double size, double direction)
        {
            var angles = new[] { Math.PI * 3 / 4, Math.PI / 4, -Math.PI * 3 / 4 };
            return angles
                .Select(a => position + GetSizeMovePoint(size, a + direction))
                .ToArray();
        }

        public static Size GetSizeMovePoint(double distance, double direction)
        {
            return new Size((int)(distance * Math.Cos(direction)), (int)(distance * Math.Sin(direction)));
        }

        public static int GetNumberAtInterval(int num, int min, int max)
        {
            if (num < min)
                return min;
            return num > max ? max : num;
        }
    }
}
