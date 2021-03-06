///اصلاحات این موتور به شرح زیر می باشد:
///1. تابع "گفتن" درست کار نمیکرد که درستش کردم
///2. تابع اوت دست کار نمیکرد که تصحیح شد
///3. کلاس فرم اولیه را حذف کرده و تنها نیاز برنامه تنظیم تابع اصلی برنامه است.
///4. افزودن بازیکن قهرمان به هر تیم
/// 
/// <summary>
/// ایده پردازی و پیاده سازی: مهندس سید ابراهیم هاشمیان
/// توسعه دهنده، بهینه ساز و پشتیبان : احسان احسانی مقدم
/// </summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;


namespace Robocup
{
    public partial class SoccerForm : Form
    {
    
        public SoccerForm()
        {
            this.Controls.Clear();
            new Robocup.Soccer(this);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SoccerForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "SoccerForm";
            this.ResumeLayout(false);

        }
    }
    public class POINT { public double x, y;}
    public class COMMAND { public int Name; public double x, y;}
    public class PLAYER
    {
        public double x, y; public COMMAND Command; public PLAYER() { Command = new COMMAND(); }
    }
    public class BallPlay
    {
        public double x, y; public COMMAND Command; public BallPlay() { Command = new COMMAND(); }
    }
    public class OPLAYER
    {
        public double x, y;
    }
    /// <summary>
    ///  وضعیت بازی
    /// </summary>
    public enum Status { Play, Goal, Corner, Out, HandOut };
    public class DATABASE
    {
        public Status GameStatus;
        public int Time, BallOwnerChange, redGrade, blueGrade, BallLooserCounter;
        public int LastBallOwner, Turn;
        public BallPlay Ball = new BallPlay();
        public PLAYER[] red = new PLAYER[5];
        public PLAYER[] blue = new PLAYER[5];
        public double BallSpeed, BallA = 1, BallMaxSpeed = 20;
        public bool Shoot = false;
        public DATABASE()
        {
            for (int i = 0; i < 5; i++)
            {
                red[i] = new PLAYER();
                blue[i] = new PLAYER();
            }
        }
    }
    public class Soccer
    {
        private static int Redhero;
        private static int Bluehero;
        private static int GameSpeed = 5; //به منظور تغییر سرعت بازی این عدد را تغییر دهید. متناسب با کامپیوتر خود بهترین حالت را پیدا کنید.
        private static int timeOut=500; // به منظور تغییر زمان هر نیمه، این عدد را تغییر دهید.
        private static Graphics g;
        private static POINT Ball = new POINT();
        private static Random rnd = new Random();
        private static double[] xdata = new double[11];
        private static double[] ydata = new double[11];
        private static DATABASE Game = new DATABASE();
        private static PLAYER[] Player = new PLAYER[6];
        private static OPLAYER[] OPlayer = new OPLAYER[6];
        private static Pen pen = new Pen(Color.Black, 1);
        private static int Time, Grade, OGrade, BallOwnerChange, o;
        private static bool HalfTime = false;
        private static Status GameStatus;
        private static int heroMan;
        /// <summary>
        ///  شماره بازیکنی را که قدرت فوق العاده دارد را برمی گرداند.
        /// </summary>
        public static int Hero
        {
            get
            {
                return heroMan;
            }
        }
        /// <summary>
        ///  یک نقطه که مختصات وسط محوطه ی جریمه را نشان می دهد.
        /// </summary>
        public static Point OpponentGround18Middle
        {
            get
            {
                Point a = new Point();
                a.X = 520;
                a.Y = 240;
                return a;
            }
        }
        /// <summary>
        ///  یک نقطه که مختصات بالای محوطه ی جریمه را نشان می دهد.
        /// </summary>
        public static Point OpponentGround18Top
        {
            get
            {
                Point a = new Point();
                a.X = 520;
                a.Y = 140;
                return a;
            }
        }
        /// <summary>
        ///  یک نقطه که مختصات پایین محوطه ی جریمه را نشان می دهد.
        /// </summary>
        public static Point OpponentGround18Botton
        {
            get
            {
                Point a = new Point();
                a.X = 520;
                a.Y = 340;
                return a;
            }
        }
        /// <summary>
        ///  یک نقطه که پایین خط وسط زمین را نشان می دهد.
        /// </summary>
        public static Point MiddleLineBotton
        {
            get
            {
                Point a = new Point();
                a.X = 320;
                a.Y = 440;
                return a;
            }
        }
        /// <summary>
        ///  یک نقطه که بالای خط وسط زمین را نشان می دهد.
        /// </summary>
        public static Point MiddleLineTop
        {
            get
            {
                Point a = new Point();
                a.X = 320;
                a.Y = 40;
                return a;
            }
        }
        /// <summary>
        ///  یک نقطه که مختصات وسط محوطه ی جریمه را نشان می دهد.
        /// </summary>
        public static Point MyGround18Middle
        {
            get
            {
                Point a = new Point();
                a.X = 120;
                a.Y = 240;
                return a;
            }
        }
        /// <summary>
        ///  یک نقطه که مختصات پایین محوطه ی جریمه را نشان می دهد.
        /// </summary>
        public static Point MyGround18Botton
        {
            get
            {
                Point a = new Point();
                a.X = 120;
                a.Y = 340;
                return a;
            }
        }
        /// <summary>
        ///  یک نقطه که مختصات بالای محوطه ی جریمه را نشان می دهد.
        /// </summary>
        public static Point MyGround18Top
        {
            get
            {
                Point a = new Point();
                a.X = 120;
                a.Y = 140;
                return a;
            }
        }
        /// <summary>
        /// یک نقطه که مختصات نقطه وسط زمین در آن قرار دارد.
        /// </summary>
        public static Point GroundMeddle 
        {
            get
            {
                Point a = new Point();
                a.X = 320;
                a.Y = 240;
                return a;
            }
        }
        /// <summary>
        /// یک نقطه که مختصات نقطه پایین دروازه حریف در آن قرار دارد.
        /// </summary>
        public static Point OpponentGoalBottom
        {
            get
            {
                Point a = new Point();
                a.X = 620;
                a.Y = 290;
                return a;
            }
        }
        /// <summary>
        /// یک نقطه که مختصات نقطه وسط دروازه حریف در آن قرار دارد.
        /// </summary>
        public static Point OpponentGoalMeddle
        {
            get
            {
                Point a = new Point();
                a.X = 620;
                a.Y = 240;
                return a;
            }
        }
        /// <summary>
        /// یک نقطه که مختصات نقطه بالای دروازه حریف در آن قرار دارد.
        /// </summary>
        public static Point OpponentGoalTop
        {
            get
            {
                Point a = new Point();
                a.X = 620;
                a.Y = 190;
                return a;
            }
        }
        /// <summary>
        /// یک نقطه که مختصات نقطه پایین دروازه خودی در آن قرار دارد.
        /// </summary>
        public static Point MyGoalBottom
        {
            get
            {
                Point a = new Point();
                a.X = 20;
                a.Y = 290;
                return a;
            }
        }
        /// <summary>
        /// یک نقطه که مختصات نقطه وسط دروازه خودی در آن قرار دارد.
        /// </summary>
        public static Point MyGoalMeddle
        {
            get
            {
                Point a = new Point();
                a.X = 20;
                a.Y = 240;
                return a;
            }
        }
        /// <summary>
        /// یک نقطه که مختصات نقطه بالای دروازه خودی در آن قرار دارد.
        /// </summary>
        public static Point MyGoalTop 
        {
            get
            {
                Point a = new Point();
                a.X = 20;
                a.Y = 190;
                return a;
            }
        }
        /// <summary>
        /// وضعیت توپ
        /// </summary>
        public static int BallOwner
        {
            get
            {
                return o;
            }
        }
        /// <summary>
        /// بازیکن صاحب توپ
        /// </summary>
        public static int NumberOfBallOwner
        {
            get
            {
                return BallOwnerChange;
            }
        }
        /// <summary>
        /// امتیاز تیم حریف
        /// </summary>
        public static int GradeOfOpponentTeam
        {
            get
            {
                return OGrade;
            }
        }
        /// <summary>
        /// امتیاز تیم من
        /// </summary>
        public static int GradeOfMyTeam
        {
            get
            {
                return Grade;
            }
        }
        /// <summary>
        /// وضعیت کنونی بازی
        /// </summary>
        public static Status CurrentGameStatus
        {
            get
            {
                return GameStatus;
            }
        }
        /// <summary>
        /// زمان سپری شده
        /// </summary>
        public static int TimeSpent
        {
            get
            {
                return Time;
            }
        }
        /// <summary>
        /// یک نقطه که مختصات توپ در آن قرار دارد.
        /// </summary>
        public static Point ball
        {
            get
            {
                Point a = new Point();
                a.X = x(0);
                a.Y = y(0);
                return a;
            }
        }
        /// <summary>
        /// یک نقطه که مختصات نقطه کرنر سمت چپ بالا در آن قرار دارد.
        /// </summary>
        public static Point CornerLeftTop
        {
            get
            {
                Point a = new Point();
                a.X = 20;
                a.Y = 40;
                return a;
            }
        }
        /// <summary>
        /// یک نقطه که مختصات نقطه کرنر سمت چپ پایین در آن قرار دارد.
        /// </summary>
        public static Point CornerLeftButtom
        {
            get
            {
                Point a = new Point();
                a.X = 20;
                a.Y = 440;
                return a;
            }
        }
        /// <summary>
        /// یک نقطه که مختصات نقطه کرنر سمت راست بالا در آن قرار دارد.
        /// </summary>
        public static Point CornerRightTop
        {
            get
            {
                Point a = new Point();
                a.X = 620;
                a.Y = 40;
                return a;
            }
        }
        /// <summary>
        /// یک نقطه که مختصات نقطه کرنر سمت راست پایین در آن قرار دارد.
        /// </summary>
        public static Point CornerRightBottom
        {
            get
            {
                Point a = new Point();
                a.X = 620;
                a.Y = 440;
                return a;
            }
        }
        /// <summary>
        /// مختصات طولی را برمیگرداند
        /// </summary>
        protected static int x(int p)
        {
            if (p > 5 || p < -5)
            {
                MessageBox.Show(_PrintTurn() + "side fault! x[i] and y[i] index must be in range: [-5 .. 5]");
                return -1;
            }
            else
                if (p >= 0) return (Convert.ToInt32(xdata[p]));
                else return (Convert.ToInt32(xdata[5 - p]));
        }
        /// <summary>
        /// مختصات عرضی را برمیگرداند
        /// </summary>
        protected static int y(int p)
        {
            if (p > 5 || p < -5)
            {
                MessageBox.Show(_PrintTurn() + "side fault! x[i] and y[i] index must be in range: [-5 .. 5]");
                return -1;
            }
            else
                if (p >= 0) return (Convert.ToInt32(ydata[p]));
                else return (Convert.ToInt32(ydata[5 - p]));
        }
        static void _readx(double d0, double d1, double d2, double d3, double d4, double d5, double d6, double d7, double d8, double d9, double d10)
        {
            xdata[0] = d0;
            xdata[1] = d1;
            xdata[2] = d2;
            xdata[3] = d3;
            xdata[4] = d4;
            xdata[5] = d5;
            xdata[6] = d6;
            xdata[7] = d7;
            xdata[8] = d8;
            xdata[9] = d9;
            xdata[10] = d10;
        }
        static void _ready(double d0, double d1, double d2, double d3, double d4, double d5, double d6, double d7, double d8, double d9, double d10)
        {
            ydata[0] = d0;
            ydata[1] = d1;
            ydata[2] = d2;
            ydata[3] = d3;
            ydata[4] = d4;
            ydata[5] = d5;
            ydata[6] = d6;
            ydata[7] = d7;
            ydata[8] = d8;
            ydata[9] = d9;
            ydata[10] = d10;
        }
        static void _InitializeParameter(int Turn, int Case)
        {
            if (Case == 1) // Goal Arrangement
            {
                if (Turn == 1) // Red Team must start the game
                {
                    Game.BallOwnerChange = 5;
                    Game.LastBallOwner = 1;
                    Game.Ball.x = 320;
                    Game.Ball.y = 240;
                    Game.Ball.Command.Name = 1;
                    Game.red[3].x = 310;
                    Game.red[3].y = 345;
                    Game.red[4].x = 310;
                    Game.red[4].y = 240;
                    Game.blue[3].x = 400;
                    Game.blue[3].y = 320;
                    Game.blue[4].x = 400;
                    Game.blue[4].y = 160;
                }
                else // Blue Team must start the Game
                {
                    Game.BallOwnerChange = -5;
                    Game.LastBallOwner = -1;
                    Game.Ball.x = 320;
                    Game.Ball.y = 240;
                    Game.Ball.Command.Name = 1;
                    Game.red[3].x = 240;
                    Game.red[3].y = 320;
                    Game.red[4].x = 240;
                    Game.red[4].y = 160;
                    Game.blue[3].x = 330;
                    Game.blue[3].y = 345;
                    Game.blue[4].x = 330;
                    Game.blue[4].y = 240;
                }
                Game.red[0].x = 40;
                Game.red[0].y = 240;
                Game.red[1].x = 140;
                Game.red[1].y = 380;
                Game.red[2].x = 140;
                Game.red[2].y = 100;
                Game.blue[0].x = 600;
                Game.blue[0].y = 240;
                Game.blue[1].x = 500;
                Game.blue[1].y = 380;
                Game.blue[2].x = 500;
                Game.blue[2].y = 100;
            }
            if (Case == 4) // OutHand Arrangement
            {
                if (Turn == 1) // Red Team must start the game
                {
                    Game.BallOwnerChange = 1 * 5;
                    Game.LastBallOwner = 1;
                    if (Game.Ball.y < 40) // From the upper Out line
                    {
                        Game.Ball.y = 50;
                        Game.red[1].x = (Game.Ball.x * 2 + 20) / 3;
                        Game.red[1].y = 240;
                        Game.red[2].x = (Game.Ball.x * 2 + 20) / 3;
                        Game.red[2].y = (Game.Ball.y * 2 + 240) / 3;
                        Game.red[3].x = (Game.Ball.x * 2 + 620) / 3;
                        Game.red[3].y = (Game.Ball.y * 2 + 240) / 3;
                        Game.red[4].x = Game.Ball.x;
                        Game.red[4].y = Game.Ball.y - 10;
                        Game.blue[1].x = (Game.Ball.x + 620 * 2) / 3;
                        Game.blue[1].y = 240;
                        Game.blue[2].x = (Game.Ball.x + 620 * 2) / 3;
                        Game.blue[2].y = (Game.Ball.y * 2 + 240) / 3;
                        Game.blue[3].x = (Game.red[3].x * 2 + 620) / 3;
                        Game.blue[3].y = (Game.red[3].y * 2 + 240) / 3;
                        Game.blue[4].x = (Game.red[1].x + Game.red[2].x + Game.red[3].x) / 3;
                        Game.blue[4].y = (Game.red[1].y + Game.red[2].y + Game.red[3].y) / 3;
                    }
                    else // From the lower Out line
                    {
                        Game.Ball.y = 430;
                        Game.red[1].x = (Game.Ball.x * 2 + 20) / 3;
                        Game.red[1].y = 240;
                        Game.red[2].x = (Game.Ball.x * 2 + 20) / 3;
                        Game.red[2].y = (Game.Ball.y * 2 + 240) / 3;
                        Game.red[3].x = (Game.Ball.x * 2 + 620) / 3;
                        Game.red[3].y = (Game.Ball.y * 2 + 240) / 3;
                        Game.red[4].x = Game.Ball.x;
                        Game.red[4].y = Game.Ball.y + 10;
                        Game.blue[1].x = (Game.Ball.x + 620 * 2) / 3;
                        Game.blue[1].y = 240;
                        Game.blue[2].x = (Game.Ball.x + 620 * 2) / 3;
                        Game.blue[2].y = (Game.Ball.y * 2 + 240) / 3;
                        Game.blue[3].x = (Game.red[3].x * 2 + 620) / 3;
                        Game.blue[3].y = (Game.red[3].y * 2 + 240) / 3;
                        Game.blue[4].x = (Game.red[1].x + Game.red[2].x + Game.red[3].x) / 3;
                        Game.blue[4].y = (Game.red[1].y + Game.red[2].y + Game.red[3].y) / 3;
                    }
                }
                else // Blue Team must start the game
                {
                    Game.BallOwnerChange = -1 * 5;
                    Game.LastBallOwner = -1;
                    if (Game.Ball.y < 40) // from the upper out line
                    {
                        Game.Ball.y = 50;
                        Game.blue[1].x = (Game.Ball.x * 2 + 620) / 3;
                        Game.blue[1].y = 240;
                        Game.blue[2].x = (Game.Ball.x * 2 + 620) / 3;
                        Game.blue[2].y = (Game.Ball.y * 2 + 240) / 3;
                        Game.blue[3].x = (Game.Ball.x * 2 + 20) / 3;
                        Game.blue[3].y = (Game.Ball.y * 2 + 240) / 3;
                        Game.blue[4].x = Game.Ball.x;
                        Game.blue[4].y = Game.Ball.y - 10;
                        Game.red[1].x = (Game.Ball.x + 20 * 2) / 3;
                        Game.red[1].y = 240;
                        Game.red[2].x = (Game.Ball.x + 20 * 2) / 3;
                        Game.red[2].y = (Game.Ball.y * 2 + 240) / 3;
                        Game.red[3].x = (Game.blue[3].x * 2 + 20) / 3;
                        Game.red[3].y = (Game.blue[3].y * 2 + 240) / 3;
                        Game.red[4].x = (Game.blue[1].x + Game.blue[2].x + Game.blue[3].x) / 3;
                        Game.red[4].y = (Game.blue[1].y + Game.blue[2].y + Game.blue[3].y) / 3;
                    }
                    else // From the lower out line
                    {
                        Game.Ball.y = 430;
                        Game.blue[1].x = (Game.Ball.x * 2 + 620) / 3;
                        Game.blue[1].y = 240;
                        Game.blue[2].x = (Game.Ball.x * 2 + 620) / 3;
                        Game.blue[2].y = (Game.Ball.y * 2 + 240) / 3;
                        Game.blue[3].x = (Game.Ball.x * 2 + 20) / 3;
                        Game.blue[3].y = (Game.Ball.y * 2 + 240) / 3;
                        Game.blue[4].x = Game.Ball.x;
                        Game.blue[4].y = Game.Ball.y + 10;
                        Game.red[1].x = (Game.Ball.x + 20 * 2) / 3;
                        Game.red[1].y = 240;
                        Game.red[2].x = (Game.Ball.x + 20 * 2) / 3;
                        Game.red[2].y = (Game.Ball.y * 2 + 240) / 3;
                        Game.red[3].x = (Game.blue[3].x * 2 + 20) / 3;
                        Game.red[3].y = (Game.blue[3].y * 2 + 240) / 3;
                        Game.red[4].x = (Game.blue[1].x + Game.blue[2].x + Game.blue[3].x) / 3;
                        Game.red[4].y = (Game.blue[1].y + Game.blue[2].y + Game.blue[3].y) / 3;
                    }
                }
                Game.Ball.Command.Name = 1;
                Game.red[0].x = 40;
                Game.red[0].y = 240;
                Game.blue[0].x = 600;
                Game.blue[0].y = 240;
            }
            if (Case == 3) // Out Arrangement
            {
                if (Turn == 1) // Red Team Must start the game
                {
                    Game.BallOwnerChange = 1;
                    Game.LastBallOwner = 1;
                    Game.Ball.x = 50;
                    Game.Ball.y = 240;
                    Game.Ball.Command.Name = 1;
                    Game.red[1].x = 180;
                    Game.red[1].y = 340;
                    Game.red[2].x = 180;
                    Game.red[2].y = 140;
                    Game.red[3].x = 360;
                    Game.red[3].y = 400;
                    Game.red[4].x = 360;
                    Game.red[4].y = 80;
                    Game.blue[1].x = 440;
                    Game.blue[1].y = 380;
                    Game.blue[2].x = 440;
                    Game.blue[2].y = 100;
                    Game.blue[3].x = 320;
                    Game.blue[3].y = 340;
                    Game.blue[4].x = 320;
                    Game.blue[4].y = 140;
                }
                else // Blue team must start the game
                {
                    Game.BallOwnerChange = -1;
                    Game.LastBallOwner = -1;
                    Game.Ball.x = 590;
                    Game.Ball.y = 240;
                    Game.Ball.Command.Name = 1;
                    Game.red[1].x = 200;
                    Game.red[1].y = 380;
                    Game.red[2].x = 200;
                    Game.red[2].y = 100;
                    Game.red[3].x = 320;
                    Game.red[3].y = 340;
                    Game.red[4].x = 320;
                    Game.red[4].y = 140;
                    Game.blue[1].x = 460;
                    Game.blue[1].y = 340;
                    Game.blue[2].x = 460;
                    Game.blue[2].y = 140;
                    Game.blue[3].x = 280;
                    Game.blue[3].y = 400;
                    Game.blue[4].x = 280;
                    Game.blue[4].y = 80;
                }
                Game.red[0].x = 40;
                Game.red[0].y = 240;
                Game.blue[0].x = 600;
                Game.blue[0].y = 240;
            }
            if (Case == 2) // Corner Arrangement
            {
                if (Turn == 1) // Red Team must start the game
                {
                    Game.BallOwnerChange = 5;
                    Game.LastBallOwner = 1;
                    Game.red[1].x = 200;
                    Game.red[1].y = 240;
                    Game.red[2].x = 540;
                    Game.red[2].y = 320;
                    Game.red[3].x = 540;
                    Game.red[3].y = 160;
                    Game.blue[1].x = 540;
                    Game.blue[1].y = 240;
                    Game.blue[4].x = 280;
                    Game.blue[4].y = 240;
                    Game.Ball.x = 620;
                    if (Game.Ball.y < 240) // From the Left-Top
                    {
                        Game.Ball.y = 50;
                        Game.red[4].x = Game.Ball.x;
                        Game.red[4].y = Game.Ball.y - 10;
                        Game.blue[2].x = 600;
                        Game.blue[2].y = 160;
                        Game.blue[3].x = 480;
                        Game.blue[3].y = 160;
                    }
                    else // From the Left-Bottom
                    {
                        Game.Ball.y = 430;
                        Game.red[4].x = Game.Ball.x;
                        Game.red[4].y = Game.Ball.y + 10;
                        Game.blue[2].x = 600;
                        Game.blue[2].y = 320;
                        Game.blue[3].x = 480;
                        Game.blue[3].y = 320;
                    }
                }
                else // Blue Team must start the game
                {
                    Game.BallOwnerChange = -5;
                    Game.LastBallOwner = -1;
                    Game.blue[1].x = 420;
                    Game.blue[1].y = 240;
                    Game.blue[2].x = 100;
                    Game.blue[2].y = 320;
                    Game.blue[3].x = 100;
                    Game.blue[3].y = 160;
                    Game.red[3].x = 100;
                    Game.red[3].y = 240;
                    Game.red[4].x = 340;
                    Game.red[4].y = 240;
                    Game.Ball.x = 20;
                    if (Game.Ball.y < 240) // From the Left-Top
                    {
                        Game.Ball.y = 50;
                        Game.blue[4].x = Game.Ball.x;
                        Game.blue[4].y = Game.Ball.y - 10;
                        Game.red[2].x = 40;
                        Game.red[2].y = 160;
                        Game.red[3].x = 160;
                        Game.red[3].y = 160;
                    }
                    else // From the Left-Bottom
                    {
                        Game.Ball.y = 430;
                        Game.blue[4].x = Game.Ball.x;
                        Game.blue[4].y = Game.Ball.y + 10;
                        Game.red[2].x = 40;
                        Game.red[2].y = 320;
                        Game.red[3].x = 160;
                        Game.red[3].y = 320;
                    }
                }
                Game.Ball.Command.Name = 1;
                Game.red[0].x = 40;
                Game.red[0].y = 240;
                Game.blue[0].x = 600;
                Game.blue[0].y = 240;
            }
            for (int i = 0; i < 5; i++)
            {
                Game.red[i].Command.Name = 0;
                Game.blue[i].Command.Name = 0;
            }
        }
        static void _Initialize()
        {
            Game.Time = 0;
            Game.BallSpeed = 0;
        }
        static void _StartGame()
        {
            g.Clear(Color.Black);
            pen.Color = (Color.White);
            g.FillRectangle(Brushes.Green, 0, 0, 640, 480);
            Game.GameStatus = 0;
            _InitializeParameter(1, 1);
        }
        static bool _TimeUp()
        {
            if (Game.Time < timeOut)
            {
                if (Game.GameStatus != Status.HandOut && Game.GameStatus != Status.Corner) Game.Time++;
                return false;
            }
            else return true;
        }
        static void _SendInformation(int Side)
        {
            Time = Game.Time;
            GameStatus = Game.GameStatus;
            Ball.y = Game.Ball.y;
            Game.Turn = Side;
            if (Side == 1)
            {
                heroMan = Redhero;
                Ball.x = Game.Ball.x;
                Grade = Game.redGrade;
                OGrade = Game.blueGrade;
                BallOwnerChange = Game.BallOwnerChange;
                for (int i = 0; i < 5; i++)
                {
                    Player[i + 1].x = Game.red[i].x;
                    Player[i + 1].y = Game.red[i].y;
                    Player[i + 1].Command.Name = -1;
                    Player[i + 1].Command.x = -1;
                    Player[i + 1].Command.y = -1;
                    OPlayer[i + 1].x = Game.blue[i].x;
                    OPlayer[i + 1].y = Game.blue[i].y;
                }
            }
            else
            {
                heroMan  = Bluehero;
                Ball.x = 640 - Game.Ball.x;
                BallOwnerChange = -Game.BallOwnerChange;
                Grade = Game.blueGrade;
                OGrade = Game.redGrade;
                for (int i = 0; i < 5; i++)
                {
                    Player[i + 1].x = 640 - Game.blue[i].x;
                    Player[i + 1].y = Game.blue[i].y;
                    Player[i + 1].Command.Name = -1;
                    Player[i + 1].Command.x = -1;
                    Player[i + 1].Command.y = -1;
                    OPlayer[i + 1].x = 640 - Game.red[i].x;
                    OPlayer[i + 1].y = Game.red[i].y;
                }
            }
            _readx(Ball.x, Player[1].x, Player[2].x, Player[3].x, Player[4].x, Player[5].x, OPlayer[1].x, OPlayer[2].x, OPlayer[3].x, OPlayer[4].x, OPlayer[5].x);
            _ready(Ball.y, Player[1].y, Player[2].y, Player[3].y, Player[4].y, Player[5].y, OPlayer[1].y, OPlayer[2].y, OPlayer[3].y, OPlayer[4].y, OPlayer[5].y);
            o = BallOwnerChange;
        }
        
        static string _PrintSide(int Side)
        {
            if (Side == 1) return "Left";
            else return "Right";
        }
        static string _PrintCommandName(int name)
        {
            switch (name)
            {
                case -1: return "CONTINUE";
                case 0: return "NOACTION";
                case 1: return "MOVE";
                case 3: return "SHOOT";
                default: return Convert.ToString(name);
            }
        }
        static void _ReceiveCommands(int Side)
        {
            if (Side == 1)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Player[i + 1].Command.Name < -1 || Player[i + 1].Command.Name > 2)
                    {
                        MessageBox.Show(_PrintSide(Side) + " Side Fault! Player[" + (i + 1) + "].Command.Name = " + _PrintCommandName(Player[i + 1].Command.Name));
                    }
                     
                    if (Player[i + 1].Command.Name > 1 && Game.BallOwnerChange != Side * (i + 1))
                        if (Game.BallOwnerChange == 0)
                            MessageBox.Show(_PrintSide(Side) + " Side Fault! Player[" + (i + 1) + "].Command.Name = " + _PrintCommandName(Player[i + 1].Command.Name) + "But no one has Ball!");
                        else
                            MessageBox.Show(_PrintSide(Side) + " Side Fault! Player[" + (i + 1) + "].Command.Name = " + _PrintCommandName(Player[i + 1].Command.Name) + "But Player[" + Math.Abs(Game.BallOwnerChange) + "] from " + _PrintSide(Side) + " side has Ball!");
                          
                    if (Game.red[i].Command.Name == -2)
                        if (Game.Time - Game.BallLooserCounter > 6)
                            Game.red[i].Command.Name = 0;
                    if (Player[i + 1].Command.Name != -1 && Game.red[i].Command.Name != -2)
                    {
                        Game.red[i].Command.Name = Player[i + 1].Command.Name;
                        if (Player[i + 1].Command.Name != 0)
                        {
                            Game.red[i].Command.x = Player[i + 1].Command.x;
                            Game.red[i].Command.y = Player[i + 1].Command.y;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Player[i + 1].Command.Name < -1 || Player[i + 1].Command.Name > 2)
                    {

                        MessageBox.Show(_PrintSide(Side) + " Side Fault! Player[" + i + 1 + "].Command.Name = " + _PrintCommandName(Player[i + 1].Command.Name));
                    }
                    if (Player[i + 1].Command.Name > 1 && Game.BallOwnerChange != Side * (i + 1))
                    {
                        if (Game.BallOwnerChange == 0)
                            MessageBox.Show(_PrintSide(Side) + " Side Fault! Player[" + i + 1 + "].Command.Name = " + _PrintCommandName(Player[i + 1].Command.Name) + " But no one has Ball!");
                        else
                            MessageBox.Show(_PrintSide(Side) + " Side Fault! Player[" + i + 1 + "].Command.Name = " + _PrintCommandName(Player[i + 1].Command.Name) + " But Player[" + Math.Abs(Game.BallOwnerChange) + "] from " + _PrintSide(Side) + " side has Ball!");
                    }
                    if (Game.blue[i].Command.Name == -2)
                        if (Game.Time - Game.BallLooserCounter > 6)
                            Game.blue[i].Command.Name = 0;
                    if (Player[i + 1].Command.Name != -1 && Game.blue[i].Command.Name != -2)
                    {
                        Game.blue[i].Command.Name = Player[i + 1].Command.Name;
                        if (Player[i + 1].Command.Name != 0)
                        {
                            Game.blue[i].Command.x = 640 - Player[i + 1].Command.x;
                            Game.blue[i].Command.y = Player[i + 1].Command.y;
                        }
                    }
                }
            }
        }
        static bool _NearBall(int Who)
        {
            double dx, dy, d;
            if (Who > 0)
            {
                dx = Game.red[Who - 1].x - Game.Ball.x;
                dy = Game.red[Who - 1].y - Game.Ball.y;
            }
            else
                if (Who < 0)
                {
                    dx = Game.blue[-Who - 1].x - Game.Ball.x;
                    dy = Game.blue[-Who - 1].y - Game.Ball.y;
                }
                else
                    return false;
            d = dx * dx + dy * dy;
            if (d < 13 * 13)
                return true;
            else
                return false;
        }
        static void _NextPosition(int Who, double CurX, double CurY, double LastX, double LastY, double Speed, out double x, out double y)
        {
            double Alpha;
            if (CurX == LastX && CurY == LastY)
            {
                x = LastX;
                y = LastY;
                return;
            }
            Alpha = Math.Atan2(LastY - CurY, LastX - CurX);
            x = CurX + Speed * Math.Cos(Alpha);
            y = CurY + Speed * Math.Sin(Alpha);
            double dif1 = (x - CurX) * (x - CurX) + (y - CurY) * (y - CurY);
            double dif2 = (LastX - CurX);
            dif2 = dif2 * (LastX - CurX);
            double d3 = (LastY - CurY);
            d3 = d3 * (LastY - CurY);
            dif2 = dif2 + d3;
            if (dif1 >= dif2)
                if (Who > 0)
                {
                    x = LastX;
                    y = LastY;
                    Game.red[Who - 1].Command.Name = 0;
                }
                else
                    if (Who == 0)
                    {
                        x = LastX;
                        y = LastY;
                        Game.Ball.Command.Name = 0;
                    }
                    else
                    {
                        x = LastX;
                        y = LastY;
                        Game.blue[-Who - 1].Command.Name = 0;
                    }
            if (Who != 0 && Who == Game.BallOwnerChange)
            {
                Game.Ball.x = CurX + (Speed + 10) * Math.Cos(Alpha);
                Game.Ball.y = CurY + (Speed + 10) * Math.Sin(Alpha);
            }
        }
        static void _Move(int Who)
        {
            double x, y, CurX, CurY, LastX, LastY, Speed;
            if (Who == Game.BallOwnerChange)
                Speed = 4;
            else
                Speed = 5;
            if (Who > 0)
            {
                if (Who == Redhero) Speed = 7;
                CurX = Game.red[Who - 1].x;
                CurY = Game.red[Who - 1].y;
                LastX = Game.red[Who - 1].Command.x;
                LastY = Game.red[Who - 1].Command.y;
                _NextPosition(Who, CurX, CurY, LastX, LastY, Speed, out x, out y);
                Game.red[Who - 1].x = x;
                Game.red[Who - 1].y = y;
            }
            else
            {
                if (Who == -Bluehero) Speed = 7;
                CurX = Game.blue[-Who - 1].x;
                CurY = Game.blue[-Who - 1].y;
                LastX = Game.blue[-Who - 1].Command.x;
                LastY = Game.blue[-Who - 1].Command.y;
                _NextPosition(Who, CurX, CurY, LastX, LastY, Speed, out x, out y);
                Game.blue[-Who - 1].x = x;
                Game.blue[-Who - 1].y = y;
            }
        }
        static void _NewBallSpeed(double LastX, double LastY)
        {
            double d = Game.Ball.x - LastX;
            d *= (Game.Ball.x - LastX);
            double dTemp = Game.Ball.y - LastY;
            dTemp *= (Game.Ball.y - LastY);
            d += dTemp;
            d = Math.Sqrt(d);
            double s = Game.BallSpeed * Game.BallSpeed - d * 2 * Game.BallA;
            if (s > 0)
                Game.BallSpeed = Math.Sqrt(s);
            else
            {
                Game.BallSpeed = 0;
                Game.Ball.Command.Name = 0;
            }
        }
        static void _SendBall(int Who, int command)
        {
            bool isHero=false;
            double x, y, CurX, CurY, LastX, LastY, Speed;
            Game.Ball.Command.Name = command;
            if (Who > 0)
            {
                if (Who == Redhero)
                    isHero = true;
                Game.Ball.Command.x = Game.red[Who - 1].Command.x;
                Game.Ball.Command.y = Game.red[Who - 1].Command.y;
                CurX = Game.red[Who - 1].x;
                CurY = Game.red[Who - 1].y;
            }
            else
            {
                if (Who == -Bluehero)
                    isHero = true;
                Game.Ball.Command.x = Game.blue[-Who - 1].Command.x;
                Game.Ball.Command.y = Game.blue[-Who - 1].Command.y;
                CurX = Game.blue[-Who - 1].x;
                CurY = Game.blue[-Who - 1].y;
            }
            LastX = Game.Ball.Command.x;
            LastY = Game.Ball.Command.y;
            if (Game.Shoot)
                if (LastX > 620 || LastX < 20 || LastY < 40 || LastY > 440)
                    Speed = Game.BallMaxSpeed;
                else
                {
                    double V0 = LastX - CurX;
                    V0 *= (LastX - CurX);
                    double V0Temp = LastY - CurY;
                    V0Temp *= (LastY - CurY);
                    V0 += V0Temp;
                    Speed = Math.Sqrt(2 * Game.BallA * V0);
                    
                    if (Speed > Game.BallMaxSpeed)
                        Speed = Game.BallMaxSpeed;
                    if (isHero)
                    {
                        Speed = Game.BallMaxSpeed + 20;
                    }
                    Game.Shoot = false;

                }
            else
                Speed = Game.BallMaxSpeed;
            _NextPosition(0, CurX, CurY, LastX, LastY, Speed + 10, out x, out y);
            Game.BallSpeed = Speed;
            _NewBallSpeed(CurX, CurY);
            Game.Ball.x = x;
            Game.Ball.y = y;
            Game.BallOwnerChange = 0;
        }
        static void _BallGoAlone()
        {
            double CurX, CurY, LastX, LastY, x, y;
            CurX = Game.Ball.x;
            CurY = Game.Ball.y;
            LastX = Game.Ball.Command.x;
            LastY = Game.Ball.Command.y;
            _NextPosition(0, CurX, CurY, LastX, LastY, Game.BallSpeed, out x, out y);
            Game.Ball.x = x;
            Game.Ball.y = y;
            _NewBallSpeed(CurX, CurY);
        }
        static void _CheckCatchBall(int Who)
        {
            if (Who == 0)
                return;
            if (Who > 0)
            {
                if (Game.red[Who - 1].Command.Name == 2)
                {
                    Game.red[Who - 1].Command.Name = 0;
                    return;
                }
                if (Game.red[Who - 1].Command.Name == -2)
                    return;
            }
            else
            {
                if (Game.blue[-Who - 1].Command.Name == 2)
                {
                    Game.blue[-Who - 1].Command.Name = 0;
                    return;
                }
                if (Game.blue[-Who - 1].Command.Name == -2)
                    return;
            }
            if (Game.BallOwnerChange * Who <= 0 && _NearBall(Who))
            {
                if (Game.BallOwnerChange != 0)
                {
                    if (Game.BallOwnerChange > 0)
                    {
                        for (int i = 0; i < 5; i++)
                            if (Game.blue[i].Command.Name == -2)
                                Game.blue[i].Command.Name = 0;
                        Game.red[Game.BallOwnerChange - 1].Command.Name = -2;
                        Game.BallLooserCounter = Game.Time;
                    }
                    else
                    {
                        for (int i = 0; i < 5; i++)
                            if (Game.red[i].Command.Name == -2)
                                Game.red[i].Command.Name = 0;
                        Game.blue[-Game.BallOwnerChange - 1].Command.Name = -2;
                        Game.BallLooserCounter = Game.Time;
                    }
                }
                Game.BallOwnerChange = Who;
                if (Who > 0)
                    Game.LastBallOwner = 1;
                else
                    Game.LastBallOwner = -1;
                Game.Ball.Command.Name = 1;
                if (Who > 0)
                {
                    Game.Ball.x = Game.red[Who - 1].x + 10;
                    Game.Ball.y = Game.red[Who - 1].y;
                }
                else
                {
                    Game.Ball.x = Game.blue[-Who - 1].x - 10;
                    Game.Ball.y = Game.blue[-Who - 1].y;
                }
                return;
            }
        }
        static void _CheckBallSwap()
        {
            int Who;
            if (Game.BallOwnerChange > 0)
                for (Who = -5; Who <= 5; Who++)
                    _CheckCatchBall(Who);
            else
                for (Who = 5; Who >= -5; Who--)
                    _CheckCatchBall(Who);
            return;
        }
        static void _DoCommands()
        {
            if (Game.GameStatus == Status.HandOut || Game.GameStatus == Status.Corner)
                if (Game.BallOwnerChange > 0)
                    if (Game.red[Game.BallOwnerChange - 1].Command.Name != 2) return;
                    else Game.GameStatus = 0;
                else
                    if (Game.blue[-Game.BallOwnerChange - 1].Command.Name != 2) return;
                    else Game.GameStatus = 0;
            else
                if (Game.GameStatus == Status.Goal) Game.GameStatus = Status.Play;
                else
                    if (Game.GameStatus == Status.Out) Game.GameStatus = Status.Play;
            for (int i = 0; i < 5; i++)
            {
                switch (Game.red[i].Command.Name)
                {
                    case 1: _Move(1 * (i + 1)); break;
                    case 2: _SendBall(1 * (i + 1), 2); break;
                }
                switch (Game.blue[i].Command.Name)
                {
                    case 1: _Move(-1 * (i + 1)); break;
                    case 2: _SendBall(-1 * (i + 1), 2); break;
                }
            }
            _CheckBallSwap();
            switch (Game.Ball.Command.Name)
            {
                case 2: _BallGoAlone(); break;
            }
        }
        static bool _CheckGoal()
        {
            if (Game.Ball.x >= 20 && Game.Ball.x <= 620 && Game.Ball.y >= 40 && Game.Ball.y <= 440)
                return false;
            if (Game.Ball.x > 5 && Game.Ball.x < 20 && Game.Ball.y > 240 - 100 / 2 && Game.Ball.y < 240 + 100 / 2)
            {
                Game.blueGrade++;
                Game.GameStatus = Status.Goal;
                _InitializeParameter(1, 1);
                return true;
            }
            if (Game.Ball.x > 620 && Game.Ball.x < 635 && Game.Ball.y > 240 - 100 / 2 && Game.Ball.y < 240 + 100 / 2)
            {
                Game.redGrade++;
                Game.GameStatus = Status.Goal;
                _InitializeParameter(-1, 1);
                return true;
            }
            if (Game.Ball.y < 40 || Game.Ball.y > 440)
            {
                Game.GameStatus = Status.HandOut;
                if (Game.LastBallOwner == 1)
                    _InitializeParameter(-1, 4);
                else
                    _InitializeParameter(1, 4);
                return true;
            }
            if (Game.Ball.x < 20)
                if (Game.LastBallOwner == 1)
                {
                    Game.GameStatus = Status.Corner;
                    _InitializeParameter(-1, 2);
                    return true;
                }
                else
                {
                    Game.GameStatus = Status.Out;
                    _InitializeParameter(1, 3);
                    return true;
                }
            if (Game.Ball.x > 620)
                if (Game.LastBallOwner == -1)
                {
                    Game.GameStatus = Status.Corner;
                    _InitializeParameter(1, 2);
                    return true;
                }
                else
                {
                    Game.GameStatus = Status.Out;
                    _InitializeParameter(-1, 3);
                    return true;
                }

            MessageBox.Show(" ERROR! Ball is not in the Ground... But there is no Goal or Corner or OutHand Status!\n Ball.x = " + Game.Ball.x + " - Ball.y = " + Game.Ball.y);
            return true;
        }
        static string _PrintTurn()
        {
            if (HalfTime == false)
            {
                if (Game.Turn == 1)
                    return "Red";
                else
                    return "Blue";
            }
            else
            {
                if (Game.Turn == 1)
                    return "Blue";
                else
                    return "Red";
            }
        }
        public static bool isHalftime
        {
            get
            {
                if (HalfTime)
                    return true;
                else
                    return false;
            }
        }
        static void _ClearGround()
        {
            clearSay();
            for (int i = 0; i < 5; i++)
            {
                g.FillEllipse(Brushes.Green, Convert.ToInt32(Game.red[i].x - 6), Convert.ToInt32(Game.red[i].y - 6), 12, 12);
                g.FillEllipse(Brushes.Green, Convert.ToInt32(Game.blue[i].x - 6), Convert.ToInt32(Game.blue[i].y - 6), 12, 12);
            }
            g.FillEllipse(Brushes.Green, Convert.ToInt32(Game.Ball.x - 7), Convert.ToInt32(Game.Ball.y - 7), 13, 13);
        }
        static void _DrawGround()
        {
            pen.Color = (Color.White);
            // Ground Lines
            g.DrawLine(pen, 320, 40, 320, 440);
            g.DrawEllipse(pen, 220, 140, 200, 200);
            g.DrawRectangle(pen, 20, 240 - 100 / 2, 50, 100);
            g.DrawRectangle(pen, 570, 240 - 100 / 2, 50, 100);
            g.DrawRectangle(pen, 20, 240 - 100, 100, 200);
            g.DrawRectangle(pen, 520, 240 - 100, 100, 200);
            // Gates
            pen.Color = (Color.White);
            // Left Gate
            g.FillRectangle(Brushes.RoyalBlue, 15, 240 - 100 / 2, 5, 100);
            g.DrawRectangle(pen, 15, 240 - 100 / 2, 5, 100);
            // Right Gate
            g.FillRectangle(Brushes.RoyalBlue, 620, 240 - 100 / 2, 5, 100);
            g.DrawRectangle(pen, 620, 240 - 100 / 2, 5, 100);
            // Ground
            g.DrawRectangle(pen, 320 - 600 / 2, 240 - 400 / 2, 600, 400);
            showSay();
            showMeSay();
        }
        static bool isBallPictureLoaded = true;
        static void _ShowTeams()
        {
            
            for (int i = 0; i < 5; i++)
            {
                if (!Play.HalfTime)
                    g.FillEllipse(Brushes.Red, Convert.ToInt32(Game.red[i].x - 5), Convert.ToInt32(Game.red[i].y - 5), 10, 10);
                else
                    g.FillEllipse(Brushes.Blue, Convert.ToInt32(Game.red[i].x - 5), Convert.ToInt32(Game.red[i].y - 5), 10, 10);
                g.DrawString("" + (i + 1), new Font("tahoma", 7), Brushes.White, Convert.ToInt32(Game.red[i].x - 5), Convert.ToInt32(Game.red[i].y - 5));
                if (!Play.HalfTime)
                g.FillEllipse(Brushes.Blue, Convert.ToInt32(Game.blue[i].x - 5), Convert.ToInt32(Game.blue[i].y - 5), 10, 10);
                else
                    g.FillEllipse(Brushes.Red, Convert.ToInt32(Game.blue[i].x - 5), Convert.ToInt32(Game.blue[i].y - 5), 10, 10);
                g.DrawString("" + (i + 1), new Font("tahoma", 7), Brushes.White, Convert.ToInt32(Game.blue[i].x - 5), Convert.ToInt32(Game.blue[i].y - 5));
            }
            if (isBallPictureLoaded)
                g.DrawImage(Image.FromFile("Ball.png"), Convert.ToInt32(Game.Ball.x - 5), Convert.ToInt32(Game.Ball.y - 5));
            else
                g.FillEllipse(Brushes.White, Convert.ToInt32(Game.Ball.x - 5), Convert.ToInt32(Game.Ball.y - 5), 10, 10);

            g.FillRectangle(Brushes.Green, 2, 2, 62, 32);
            g.DrawString(Convert.ToString(Game.Time), new Font("tahoma", 12), Brushes.Orange, 0, 0);
            g.DrawString(Convert.ToString(Game.GameStatus), new Font("tahoma", 8), Brushes.Orange, 0, 20);
            g.FillRectangle(Brushes.Green, 300, 2, 80, 20);
            g.DrawString(Game.redGrade + " - " + Game.blueGrade, new Font("tahoma", 12), Brushes.White, 300, 0);
        }
        bool active = true;
        void ShowGameTick(object sender, EventArgs e)
        {
            
            if (active)
            {
                
                if (!HalfTime)
                {
                    if (_TimeUp())
                    {
                        _Initialize();
                        _StartGame();
                        Play.HalfTime = !Play.HalfTime;
                        int temp = Game.redGrade;
                        Game.redGrade = Game.blueGrade;
                        Game.blueGrade = temp;
                        active = false;
                        MessageBox.Show("شروع نیمه ی دوم");
                        active = true;
                    }
                    if (!_CheckGoal())
                    {
                        _DrawGround();
                        _ShowTeams();
                        GameBoard.Image = bmp;
                        GameBoard.Refresh();
                        _ClearGround();
                        _SendInformation(1);
                        Play.RedTeam();
                        _ReceiveCommands(1);
                        _SendInformation(-1);
                        Play.BlueTeam();
                        _ReceiveCommands(-1);
                        _DoCommands();
                    }
                }
                else{
                    if (_TimeUp())
                    {
                        ShowTimer.Stop();
                        MessageBox.Show(RedTeamName + "   " + Game.blueGrade + "   -   " + Game.redGrade + "   " + BlueTeamName);
                    }
                    if (!_CheckGoal())
                    {
                        _DrawGround();
                        _ShowTeams();
                        GameBoard.Image = bmp;
                        GameBoard.Refresh();
                        _ClearGround();
                        _SendInformation(1);
                        Play.RedTeam();
                        _ReceiveCommands(1);
                        _SendInformation(-1);
                        Play.BlueTeam();
                        _ReceiveCommands(-1);
                        _DoCommands();
                    }
                }
            }
        }
        System.Windows.Forms.Timer ShowTimer;
        void btnClick(object sender, EventArgs e)
        {
            ((Button)sender).Visible = false;
            ShowTimer = new System.Windows.Forms.Timer();
            ShowTimer.Interval = GameSpeed;
            ShowTimer.Tick += new System.EventHandler(ShowGameTick);
            Game.redGrade = 0;
            Game.blueGrade = 0;
            for (int i = 0; i < 6; i++)
            {
                Player[i] = new PLAYER();
                OPlayer[i] = new OPLAYER();
            }
            Random r = new Random();
            Redhero = r.Next(1, 6);
            Bluehero = r.Next(1, 6);
            _Initialize();
            _StartGame();
            ShowTimer.Start();
        }
        static Bitmap bmp = new Bitmap(640, 480);
        static System.Windows.Forms.PictureBox GameBoard;
        public Soccer(Form form)
        {
            
            form.Text = "RoboCup Soccer Simolation";
            try
            {
                form.Icon = new Icon("ball.ico");
            }
            catch
            {
            }
            form.ClientSize = new System.Drawing.Size(690, 502);
            startGame(form);
            GameBoard.Image = bmp;
            form.Show();
        }
        void startGame(Form form)
        {
            form.WindowState = FormWindowState.Maximized;
            form.Controls.Clear();
            Button StartButton=new Button();
            StartButton.BackColor = System.Drawing.Color.Chartreuse;
            StartButton.Dock = System.Windows.Forms.DockStyle.Fill;
            StartButton.Font = new System.Drawing.Font("Tahoma", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            StartButton.Location = new System.Drawing.Point(0, 0);
            StartButton.Name = "StartButton";
            StartButton.Size = new System.Drawing.Size(398, 301);
            StartButton.TabIndex = 0;
            StartButton.Text = "شروع رقابت";
            StartButton.UseVisualStyleBackColor = false;
            StartButton.Click += new System.EventHandler(btnClick);
            GameBoard = new System.Windows.Forms.PictureBox();
            GameBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            GameBoard.Location = new System.Drawing.Point(0, 0);
            GameBoard.Name = "Game Board";
            GameBoard.Size = new System.Drawing.Size(690, 502);
            GameBoard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            form.Controls.Add(StartButton);
            form.Controls.Add(GameBoard);
            g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            try
            {
                g.DrawImage(Image.FromFile("Ball.png"), Convert.ToInt32(Game.Ball.x - 5), Convert.ToInt32(Game.Ball.y - 5));
                isBallPictureLoaded = true;
            }
            catch
            {
                isBallPictureLoaded = false;
            }
        }

        static int BlueCount;
        static int RedCount;
        static string RedText = "";
        static string BlueText = "";
        /// <summary>
        /// فریاد زدن - متن مورد نظر را به این تابع ارسال کنید تا در 50 دور بازی و تا زمانی که متن دیگری فریاد زده شود آن را نمایش دهد
        /// </summary>
        public static void Say(string Text) { Say(Text, false); }
        /// <summary>
        ///  فریاد زدن مطالب مهم - متن مورد نظر و متغیر دو مقداری مهم بودن را به این تابع ارسال کنید. اگر مقدار صحیح را برای اهمیت ارسال کرده باشید، این تابع حتما 50 دور بازی متن شما را فریاد می زند، حتی اگه متن دیگری فریاد زده شود.
        ///  اگر مقدار نادرست را برای اهمیت ارسال کرده باشید متن شما 50 دور بازی و تا زمانی که متن دیگری فریاد زده شود نمایش داده می شود
        /// </summary>
        public static void Say(string Text, bool isImportant)
        {
            if (Text.Length > 40)
                return;
            if (Game.Turn == 1)
            {
                if (RedText == "" || isImportant)
                {
                    RedText = Text;
                    RedCount = 0;
                }
            }
            else
            {
                if (BlueText == "" || isImportant)
                {
                    BlueText = Text;
                    BlueCount = 0;
                }
            }
        }
        /// <summary>
        ///  اگر کدهای تیم قرمز را وارد می کنید، با مقدار دهی این متغیر نام تیم خود را مشخص کنید
        /// </summary>
        public static string RedTeamName = "";
        /// <summary>
        ///  اگر کدهای تیم آبی را وارد می کنید، با مقدار دهی این متغیر نام تیم خود را مشخص کنید
        /// </summary>
        public static string BlueTeamName = "";
        static void showMeSay()
        { 
            string left="";
            string right="";
            if (!Play.HalfTime)
            {
                left = RedTeamName;
                right = BlueTeamName;
            }
            else
            {
                right = RedTeamName;
                left = BlueTeamName;
            }
            g.DrawString(left, new Font("tahoma", 8), Brushes.WhiteSmoke, 60, 10);
            g.DrawString(right, new Font("tahoma", 8), Brushes.WhiteSmoke, 480, 10);
        }
        static void showSay()
        {
            RedCount++;
            BlueCount++;
            if (!HalfTime)
            {
                g.DrawString(RedText, new Font("tahoma", 10), Brushes.DarkRed, 20, 450);
                g.DrawString(BlueText, new Font("tahoma", 10), Brushes.DarkBlue, 330, 450);
            }
            else
            {
                g.DrawString(RedText, new Font("tahoma", 10), Brushes.DarkBlue, 20, 450);
                g.DrawString(BlueText, new Font("tahoma", 10), Brushes.DarkRed, 330, 450);
            }
        }
        static void clearSay()
        {
            g.FillRectangle(Brushes.Green, 20, 442, 300, 450);
            g.FillRectangle(Brushes.Green, 330, 442, 300, 450);
            if (RedCount > 20)
            {
                RedText = "";
                RedCount = 0;
            }
            if (BlueCount > 20)
            {
                BlueText = "";
                BlueCount = 0;
            }
        }

        /// <summary>
        /// دستور شوت - با ارسال طول و عرض مورد نظر، بازیکن صاحب توپ، توپ را به آن نقطه شوت می کند.
        /// توجه شود که به دلیل اصطکاک، توپ بیش از نصف زمین را طی نخواهد کرد.  
        /// همچنین به خاطر وجود خطای جزئی، ممکن است توپ دقیقا در مختصات خواسته شد توقف ننماید. 
        /// اما مهمترین نکته آنکه شما نمی توانید زمانی که توپ را در اختیار ندارد اقدام به شوت زدن کنید. این امر باعث خواهد شد تا تیم حریف برنده اعلام شود
        /// </summary>
        protected static void shoot(int x, int y)
        {
            if (BallOwnerChange <= 0)
                MessageBox.Show(_PrintTurn() + "side Fault! Your command is shoot but BallOwner = " + BallOwnerChange);
            else
            {
                Game.Shoot = true; // for calculate the Ball Speed when it go alone
                Player[BallOwnerChange].Command.Name = 2;
                Random p = new Random();
                int r = p.Next(11) + p.Next(11) + p.Next(11) + p.Next(11) - 20;
                double alpha = p.Next(180) * Math.PI / 180;
                Player[BallOwnerChange].Command.x = (int)(x + r * Math.Cos(alpha));
                Player[BallOwnerChange].Command.y = (int)(y + r * Math.Sin(alpha));
            }
        }
        /// <summary>
        /// دستور پاس - با ارسال شماره بازیکن مورد نظر، بازیکن صاحب توپ، توپ را به او پاس می دهد.
        /// توجه شود که به دلیل اصطکاک، توپ بیش از نصف زمین را طی نخواهد کرد.  
        /// همچنین به خاطر وجود خطای جزئی، ممکن است توپ دقیقا در مختصات خواسته شد توقف ننماید. 
        /// اما مهمترین نکته آنکه شما نمی توانید زمانی که توپ را در اختیار ندارد اقدام به پاس دادن کنید. این امر باعث خواهد شد تا تیم حریف برنده اعلام شود
        /// </summary>
        protected static void pas(int b)
        {
            if (BallOwnerChange <= 0)
            {
                MessageBox.Show(_PrintTurn() + "side Fault! Your command is pas but BallOwner = " + BallOwnerChange);
               
            }
            else
                if (Math.Abs(b) < 1 || Math.Abs(b) > 5)
                    MessageBox.Show(_PrintTurn() + "side Fault! Your Command is pas to player " + b + "!");
                else
                {
                    Player[BallOwnerChange].Command.Name = 2;
                    Random p = new Random();
                    int r = (p.Next(11) + p.Next(11) + p.Next(11) + p.Next(11)) - 20;
                    double alpha = p.Next(180) * Math.PI / 180;
                    Player[BallOwnerChange].Command.x = (int)(Player[b].x + r * Math.Cos(alpha));
                    Player[BallOwnerChange].Command.y = (int)(Player[b].y + r * Math.Sin(alpha));
                }
        }
        /// <summary>
        /// دستور دویدن - با ارسال شماره بازیکن مورد نظر، و مختصات طولی و عرضی مقصد، بازیکن به آن نقطه می دود.
        /// اگر توپ دراختیار این بازیکن باشد، با توپ و اگر در اختیارش نباشد بدون توپ به آن نقطه خواهد رفت.
        /// توجه شود بازیکن صاحب توپ با سرعت کمتری قادر به دویدن است.
        /// </summary>
        protected static void run(int a, int x, int y)
        {
            if (a < 1 || a > 5)
                MessageBox.Show(_PrintTurn() + "side Fault! Your Command is run for player " + a + "!");
            else
            {
                Player[a].Command.Name = 1;
                Player[a].Command.x = x;
                Player[a].Command.y = y;
            }
        }
    }
    class Play : Soccer
    {
        public Play(Form form)
            : base(form) { }
        public static void RedTeam()
        {
            if (!Soccer.isHalftime)
                redTeam.RedTeam();
            else
                blueTeam.BlueTeam();
        }
        public static void BlueTeam()
        {
            if (Soccer.isHalftime)
                redTeam.RedTeam();
            else
                blueTeam.BlueTeam();
        }
    }
}