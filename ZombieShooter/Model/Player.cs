using System.Drawing;
using System.Windows.Forms;

namespace ZombieShooter
{
    class Player: Character
    {
        public double Direction => Utils.GetDirection(Position,Game.CursorPosition);
        protected bool IsMoving { get; set; }

        public Player(Point position,
            int size = 15,
            double moveDirection = 0,
            double speed = 6,
            int damage = 1,
            int attackSpeed = 20,
            int health = 5) : base(position, size, moveDirection, speed, damage, attackSpeed, health)
        {
            IsMoving = true;
        }

        public Bullet Fire()
        {
            return Health > 0 && CanAttack()
                ? new Bullet(Position, Direction)
                : null;
        }

        public override string ImageFileName => Health > 0 ? "Player.png" : "deadPlayer.png";

        public new void Move()
        {
            if (IsMoving)
                base.Move();
            else
                tickCount++;
        }
        
        public void UpdateMoveDirection()
        {
            IsMoving = false;
            var x = 0;
            var y = 0;
            foreach (var key in Game.PressedKeys)
            {
                switch (key)
                {
                    case Keys.W:
                        y--;
                        break;
                    case Keys.S:
                        y++;
                        break;
                    case Keys.A:
                        x--;
                        break;
                    case Keys.D:
                        x++;
                        break;
                }
            }
            if (x == 0 && y == 0)
                return;
            IsMoving = true;
            var nextPosition = Position + new Size(x, y);
            nextPosition.X = Utils.GetNumberAtInterval(nextPosition.X, 0, Game.MapWidth);
            nextPosition.Y = Utils.GetNumberAtInterval(nextPosition.Y, 0, Game.MapHeight);
            base.UpdateMoveDirection(nextPosition);
        }
    }
}
