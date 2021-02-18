using System.Drawing;

namespace ZombieShooter
{
    class Bullet : GameObject
    {
        public override string ImageFileName => "Bullet.png";

        public Bullet(Point position,
            double moveDirection = 0,
            int size = 5,
            double speed = 10, 
            int damage = 1, 
            int attackSpeed = 1) : base(position, size, moveDirection, speed, damage, attackSpeed)
        {
        }
    }
}
