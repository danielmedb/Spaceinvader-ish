using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;


namespace spaceinvader_ish
{

    public class Bullet
    {
        public int BulletY;
        public int BulletX;
        public string pewpew = "\u2022";

        public Bullet(int x, int y)
        {
            BulletY = y;
            BulletX = x;
        }
    }

    public class Invader
    {
        public int InvaderY;
        public int InvaderX;
        public string vader;
        public string mMonster;
        public int mHP;


        public Invader(int x, int y, string Monster, int HP)
        {
            InvaderY = y;
            InvaderX = x;
            mMonster = Monster;
            mHP = HP;

            if (mMonster == "Invader")
            {
                vader = "▼";
            }
            else
            {
                vader = "\u03A8";
            }
        }
    }
    
    class Game
    {

        public int x;
        public int y = 20;
        public int Frame = 0;
        public int Score = 0;
        public int EscapedInvaders = 0;
        public int TotalInvaders = 0;
        public int Bosses = 0;

        private List<Bullet> Bullets = new List<Bullet>();

        public List<Invader> Invaders = new List<Invader>();
        
       
        
        public void MoveLeft()
        {
            if (x >= 1)
            {
                Console.SetCursorPosition(x,y);
                Console.Write(" ");
                x--;
               Render();
            }
        }

        public void MoveRight()
        {
            Console.SetCursorPosition(x, y);
            Console.Write(" ");
            x++;
            Render();
        }

        public void RenderBullet()
        {
           
            Console.SetCursorPosition(x , y);
            Bullets.Add(new Bullet(x, y-1));
        }

      
        public int GetNapTime(DateTime initialTime)
        {
            var finalTimeStamp = DateTime.Now;
            var timeDifference = (finalTimeStamp - initialTime).TotalMilliseconds;
            int naptime = 0;
            if (timeDifference < 33.3)
            {
                naptime = (int)(33.3 - timeDifference);
            }
            return naptime > 0 ? naptime : 0;
        }
        
        public void UpdateBullets()
        {
            var ActiveBullets = new List<Bullet>();
            
            if (Bullets != null)
            {
                foreach (var bullet in Bullets)
                {
                   if(!checkHit(bullet))
                   {
                        if (bullet.BulletY != 0)
                        {
                            Console.SetCursorPosition(bullet.BulletX, bullet.BulletY);
                            Console.Write(' ');

                            bullet.BulletY -= 1;
                            
                            ActiveBullets.Add(bullet);
                            Console.SetCursorPosition(bullet.BulletX, bullet.BulletY);
                            Console.Write(bullet.pewpew);
                            
                        }

                        if (bullet.BulletY == 0)
                        {
                            Console.SetCursorPosition(bullet.BulletX, bullet.BulletY);
                            Console.Write(' ');
                        }
                   }
                }
            }
            Bullets = ActiveBullets;
        }
        
        public void GenerateInvader()
        {
            Random random = new Random();
            int x = random.Next(0, 30);
            if (TotalInvaders == 10)
            {
                var invader = new Invader(x, 0, "Boss", 5);
                Invaders.Add(invader);
            }
            
            else
            {
                var invader = new Invader(x, 0, "Invader", 1);
                Invaders.Add(invader);
            }

            TotalInvaders++;
           
        }
        
        
        public void UpdateInvader()
        {
            var activeInvaders = new List<Invader>();

            if (Invaders != null)
            {
                foreach (var invader in Invaders)
                {
                    Console.SetCursorPosition(invader.InvaderX, invader.InvaderY);
                    Console.Write(' ');

                    if (invader.InvaderY != 20)
                    {
                        invader.InvaderY += 1;
                        Console.SetCursorPosition(invader.InvaderX, invader.InvaderY);
                        Console.Write(invader.vader);
                        activeInvaders.Add(invader);
                    }
                    else if (invader.InvaderY == 20)
                    {
                        EscapedInvaders++;
                    }
                }
            }
            Invaders = activeInvaders;
        }

        private bool checkHit(Bullet bullet)
        {
            if (bullet != null)
            {
                var killedInvader = Invaders.Find(invader => invader.InvaderX == bullet.BulletX && invader.InvaderY == bullet.BulletY);
                
                if (killedInvader != null)
                {
                    if (killedInvader.mHP >= 1)
                    {
                        killedInvader.mHP--;
                    }
                    if (killedInvader.mHP == 0)
                    {
                        Console.SetCursorPosition(bullet.BulletX, bullet.BulletY);
                        Console.Write(' ');
                        Invaders.Remove(killedInvader);
                        Score++;
                        return true;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }

        
        public void GetKeyStrokes()
        {
            if (Console.KeyAvailable)
            {
                var input = Console.ReadKey(true);
                switch (input.Key)
                {
                    case ConsoleKey.A:
                        MoveLeft();
                        break;
                    case ConsoleKey.D:
                        MoveRight();
                        break;
                    case ConsoleKey.Spacebar:
                        RenderBullet();
                        break;
                }
            }
        }
        
        public void Render()
        {
            
            Console.SetCursorPosition(x,y);
            Console.Write('$');
        }

        public void stats()
        {
            Console.SetCursorPosition(0,y+1);  
            Console.Write($"Score: {Score} | " +
                          $"Escaped: {EscapedInvaders} | " +
                          $"Invaders: {TotalInvaders} | " +
                          $"Bosses: {Bosses}");
        }
        
        public void boss()
        {
            Random bossHealth = new Random();
            bossHealth.Next(10, 20);
        }
        
    }
    class Program
    {
        static void Main(string[] args)
        {
            

            Console.Clear();
            Console.SetCursorPosition(5,5);
            Console.WriteLine("Willkommen!");
            Console.SetCursorPosition(5,6);
            Console.WriteLine("Move left with A and move right with D");
            Console.SetCursorPosition(5,7);
            Console.WriteLine("Fire bullets with spacebar");
            Console.SetCursorPosition(5,8);
            Console.SetCursorPosition(5,10);
            Console.WriteLine("Bosses spawns randomly with various HP.");
            Console.SetCursorPosition(5,11);
            Console.WriteLine("Finaly Boss? Kill the Psyduck!");
            Console.SetCursorPosition(5,12);
            Console.WriteLine("Press any key to Enter.");
            
            string gameoverText1 = "Your score is: ";
            string gameoverText2 = "Press any key to play again!";
            var pressedKey = Console.ReadKey();
            Game game = new Game();
            if (pressedKey.Key == ConsoleKey.Enter)
            {
                
                Console.Clear();

                while (true)
                {
                    var initialTimeStamp = DateTime.Now;

                    int napTime = game.GetNapTime(initialTimeStamp);

                    Thread.Sleep(napTime);
                    game.Frame++;

                    if (game.Frame % 10 == 0)
                    {
                        game.UpdateInvader();
                    }

                    if (game.Frame % 60 == 0)
                    {
                        game.GenerateInvader();
                    }

                    if (game.EscapedInvaders == 5)
                    {
                        break;
                    }
                    
                    game.GetKeyStrokes();
                    game.UpdateBullets();
                    game.stats();

                }
            }

            // Game over
            Console.Clear();
            Console.SetCursorPosition(10,5);
            Console.WriteLine("Game over!");
            Console.SetCursorPosition(10,6);
            for (int i = 0; i < gameoverText1.Length; i++)
            {
                Console.Write(gameoverText1[i]);
                Thread.Sleep(60);
            }
            Console.Write(game.Score);
        }
    }
}