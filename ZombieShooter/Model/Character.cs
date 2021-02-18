using System;
using System.Drawing;

namespace ZombieShooter
{
    abstract class Character : GameObject
    {
        public int Health { get; private set;}

        public event Action Died;
        public event Action WasAttacked;


        protected Character(Point position, 
            int size, 
            double moveDirection, 
            double speed, 
            int damage, 
            int attackSpeed,
            int health) : base(position, size, moveDirection, speed, damage, attackSpeed)
        {
            Health = health;
        }

        public void UpdateMoveDirection(Point directionPoint) =>
            MoveDirection = Health > 0? Utils.GetDirection(Position, directionPoint): MoveDirection;

        public new void Move()
        {
            if (Health > 0)
                base.Move();
            else
                tickCount++;
        }

        public void BeAttacked(int damage)
        {
            while (damage > 0)
            {
                Health--;
                damage--;
                WasAttacked?.Invoke();
                if (Health == 0) Died?.Invoke();
            }
        }
    }
}
