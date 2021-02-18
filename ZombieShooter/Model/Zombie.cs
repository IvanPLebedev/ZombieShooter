using System.Drawing;

namespace ZombieShooter
{
    class Zombie : Character
    {
        public Zombie(Point position,
            int size = 15,
            double moveDirection = 0,
            double speed = 2,
            int damage = 1,
            int attackSpeed = 30,
            int health = 1) : base(position, size, moveDirection, speed, damage, attackSpeed, health)
        {
        }

        public override string ImageFileName => Health > 0 ? "Zombie.png" : "deadZombie.png";
    }
}
