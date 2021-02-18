using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ZombieShooter
{
    sealed class GameForm: Form
    {
        private GameState gameState;
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private Timer timer;
        private int playerBeAttackedAnimationTimer;
        private int level = 1;
        private bool _pause = false;
        public int prevLevelsKillsCount { get; private set; }

        public GameForm()
        {
            this.Size = new Size(
                (int)(Game.ElementSize * Game.MapWidth),
                (int)(Game.ElementSize * Game.MapHeight));
            var imagesDirectory = new DirectoryInfo("Images");
            foreach (var e in imagesDirectory.GetFiles("*.png"))
                bitmaps[e.Name] = (Bitmap)Image.FromFile(e.FullName);
            DoubleBuffered = true;
            MouseClick += ClickMouse;
            prevLevelsKillsCount = 0;
            StartLevel();
        }

        public void StartLevel()
        {
            var config = GetLevelsСonfiguration(level);
            playerBeAttackedAnimationTimer = 0;
            gameState = new GameState(config[0], config[1], config[2], config[3], config[4]);
            gameState.Player.Died += PlayerIsDead;
            gameState.Player.WasAttacked += PlayerBeAttacked;
            gameState.Win += Win;
            timer = new Timer { Interval = Game.TimeInterval };
            timer.Tick += TimerTick;
            timer.Start();
            _pause = false;
        }

        private void PlayerIsDead()
        {
            var result = MessageBox.Show("Вы погибли! Нажмите OK чтобы начать заново.", "", MessageBoxButtons.OK);
            if (result != DialogResult.OK) this.Close();
            else
            {
                level = 1;
                timer.Stop();
                timer.Tick -= TimerTick;
                gameState.Player.Died -= PlayerIsDead;
                gameState.Player.WasAttacked -= PlayerBeAttacked;
                gameState.Win -= Win;
                prevLevelsKillsCount = 0;
                StartLevel();
            }
        }

        private void Win()
        {
            level++;
            if (level > Game.LevelsСonfiguration.Length)
            {
                level = 1;
                MessageBox.Show("Вы прошли все уровни. Нажмите окей чтобы начать сначала.", "",
                    MessageBoxButtons.OK);
            }
            else
                MessageBox.Show("Уровень пройден. Нажмите ОК чтобы перейти на следующий уровень.", "", MessageBoxButtons.OK);
            timer.Stop();
            timer.Tick -= TimerTick;
            gameState.Player.Died -= PlayerIsDead;
            gameState.Player.WasAttacked -= PlayerBeAttacked;
            gameState.Win -= Win;
            prevLevelsKillsCount += gameState.KillsCount;
            StartLevel();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            Game.PressedKeys.Add(e.KeyCode);
            if (e.KeyCode == Keys.Space)
                _pause = !_pause;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            Game.PressedKeys.Remove(e.KeyCode);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var loc = e.Location;
            Game.CursorPosition = new Point((int)(loc.X / Game.ElementSize), (int)(loc.Y / Game.ElementSize));
        }

        private void ClickMouse(object sender, EventArgs args)
        {
            gameState.PlayerFire();
        }

        private void FormResize()
        {
            var size = this.Size;
            var newSizeElement1 = (double)size.Height / Game.MapHeight;
            var newSizeElement2 = (double)size.Width / Game.MapWidth;
            Game.ElementSize = newSizeElement2 < newSizeElement1 ? newSizeElement2 : newSizeElement1;
            this.Size = new Size(
                (int)(Game.ElementSize * Game.MapWidth),
                (int)(Game.ElementSize * Game.MapHeight));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(
                Brushes.Black, 0, 0, (int)(Game.ElementSize * Game.MapWidth),
                (int)(Game.ElementSize * Game.MapHeight));
            foreach (var zombie in gameState.Zombies)
                PaintCharacter(e, zombie, zombie.MoveDirection);
            var player = gameState.Player;
            PaintCharacter(e, player, player.Direction);
            foreach (var bullet in gameState.Bullets)
                PaintCharacter(e, bullet, bullet.MoveDirection);
            PlayerBeAttackedAnimationPaint(e);
            PrintGameState(e);
        }

        private void PaintCharacter(PaintEventArgs e, GameObject character, double direction)
        {
            var characterPoints = Utils.GetDestinationPoints(character.Position, character.Size, direction);
            e.Graphics.DrawImage(bitmaps[character.ImageFileName],
                characterPoints.Select(ConvertGamePointToForm).ToArray());
        }

        private void TimerTick(object sender, EventArgs args)
        {
            if (!_pause)
            {
                gameState.OnTick();
                if (playerBeAttackedAnimationTimer > 0)
                    playerBeAttackedAnimationTimer--;
            }
            FormResize();
            Invalidate();
        }

        private void PlayerBeAttackedAnimationPaint(PaintEventArgs e)
        {
            if (!(playerBeAttackedAnimationTimer > 0))
                return;
            var brash = new SolidBrush(Color.FromArgb(playerBeAttackedAnimationTimer, Color.Red));
            e.Graphics.FillRectangle(
                brash, 0, 0, (int)(Game.ElementSize * Game.MapWidth),
                (int)(Game.ElementSize * Game.MapHeight));
        }

        private void PrintGameState(PaintEventArgs e)
        {
            var str = GetGameInfo();
            var point = new PointF((float)Game.ElementSize, (float)Game.ElementSize);
            var font = new Font("Arial", (int)(Game.ElementSize * 20), FontStyle.Bold, GraphicsUnit.Pixel);
            e.Graphics.DrawString(str, font, Brushes.White, point);
        }

        private void PlayerBeAttacked()
        {
            if (playerBeAttackedAnimationTimer > 128)
                return;
            playerBeAttackedAnimationTimer += 60;
        }

        private string GetGameInfo()
        {
            var hp = gameState.Player.Health > -1 ? gameState.Player.Health : 0;
            return "Level " + level + "   HP: " + hp + "  Kill:" + (gameState.KillsCount + prevLevelsKillsCount);
        }

        private int[] GetLevelsСonfiguration(int levelNum)
        {
            if (levelNum > Game.LevelsСonfiguration.Length)
                return null;
            return Game.LevelsСonfiguration[levelNum - 1];
        }

        private Point ConvertGamePointToForm(Point point)
        {
            return new Point((int)(point.X * Game.ElementSize), (int)(point.Y * Game.ElementSize));
        }
    }
}
