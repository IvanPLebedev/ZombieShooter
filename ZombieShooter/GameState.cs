using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ZombieShooter
{
    class GameState
    {
        public Player Player { get; private set; }
        public List<Zombie> Zombies { get; private set; }
        public List<Bullet> Bullets { get; private set; }
        public int KillsCount { get; private set; }

        private int zombiesCount;
        private int time;
        private readonly int zombieCreationsInterval;
        private readonly int zombiesSpeed;
        private readonly int zobiesDamage;
        private readonly int zombiesHealth;
        public event Action Win;

        private bool _notWin = true;
        private bool _pause = false;
        private readonly Random random = new Random();

        public GameState(int zombiesCount, int zombiesSpeed = 2, int zombiesDamage = 1, int zombiesHealth = 1, int zombieCreationsInterval = 40)
        {
            Player  = new Player(new Point(Game.MapWidth / 2, Game.MapHeight / 2));
            Zombies = new List<Zombie>();
            Bullets = new List<Bullet>();
            time = 0;
            this.zombiesCount = zombiesCount;
            this.zombieCreationsInterval = zombieCreationsInterval;
            this.zombiesSpeed = zombiesSpeed;
            this.zobiesDamage = zombiesDamage;
            this.zombiesHealth = zombiesHealth;
            KillsCount = 0;
        }

        public void PlayerFire()
        {
            var bullet = Player.Fire();
            if (bullet != null)
                Bullets.Add(bullet);
        }

        public void OnTick()
        {
            time++;
            if (time == zombieCreationsInterval)
            {
                CreateZombie();
                time = 0;
            }
            Player.UpdateMoveDirection();
            Player.Move();
            ZombiesTurns();
            BulletsTurn();
            if (!_notWin) return;
            if (zombiesCount <= 0 && !Zombies.Any(z => z.Health > 0))
            {
                _notWin = false;
                Win?.Invoke();
            }
        }

        private void ZombiesTurns()
        {
            for (var i = 0; i < Zombies.Count; i++)
            {
                var zombie = Zombies[i];
                if (zombie.Health <= 0) continue;
                zombie.UpdateMoveDirection(Player.Position);
                zombie.Move();
                if (zombie.CanAttack())
                    Conflict(zombie, Player);
            }
        }

        private void BulletsTurn()
        {
            for (var i = 0; i < Bullets.Count; i++)
            {
                var bullet = Bullets[i];
                bullet.Move();
                if (bullet.Position.X < 0 || bullet.Position.X > Game.MapWidth ||
                    bullet.Position.Y < 0 || bullet.Position.Y > Game.MapHeight)
                {
                    Bullets.Remove(bullet);
                    i--;
                    continue;
                }
                if (!bullet.CanAttack()) continue;
                if (!Zombies.Any(zombie => Conflict(bullet, zombie))) continue;
                Bullets.Remove(bullet);
                i--;
                KillsCount++;
            }
        }

        private void CreateZombie()
        {
            if (zombiesCount <= 0) return;
            zombiesCount--;
            Zombies.Add(new Zombie(GenerateZombiePosition(), speed: zombiesSpeed, damage:zobiesDamage, health:zombiesHealth));
        }

        private Point GenerateZombiePosition()
        {
            var p = random.Next(4);
            const int edgeIndent = 20;
            switch (p)
            {
                case 0:
                    return new Point(random.Next(Game.MapWidth), -edgeIndent);
                case 1:
                    return new Point(random.Next(Game.MapWidth), Game.MapHeight + edgeIndent);
                case 2:
                    return new Point(-edgeIndent, random.Next(Game.MapHeight));
                default:
                    return new Point(Game.MapWidth + edgeIndent, random.Next(Game.MapHeight));
            }
        }

        private static bool Conflict(GameObject attacker, Character victim)
        {
            if ((Utils.GetDistance(attacker.Position, victim.Position) < (attacker.Size + victim.Size) * 0.75)
                && victim.Health > 0)
            {
                victim.BeAttacked(attacker.Damage);
                return true;
            }
            return false;
        }
    }
}
