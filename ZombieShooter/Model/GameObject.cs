using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieShooter
{
    abstract class GameObject
    {
        public Point Position { get; protected set; }
        public readonly int Size;
        public readonly int Damage;
        public readonly int AttackSpeed;
        public double Speed { get; protected set; }
        public double MoveDirection { get; protected set; }
        public abstract string ImageFileName { get; }
        protected int tickCount;

        protected GameObject(Point position,
            int size,
            double moveDirection,
            double speed,
            int damage, 
            int attackSpeed)
        {
            Position = position;
            Size = size;
            MoveDirection = moveDirection;
            Speed = speed;
            Damage = damage;
            AttackSpeed = attackSpeed;
            tickCount = 0;
        }

        public void Move()
        {
            tickCount++;
            Position += Utils.GetSizeMovePoint(Speed, MoveDirection);
        }

        public bool CanAttack()
        {
            if (tickCount < AttackSpeed)
                return false;
            tickCount = 0;
            return true;
        }
    }
}
