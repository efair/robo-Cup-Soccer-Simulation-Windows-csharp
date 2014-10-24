using System.Windows.Forms;
using System;
using System.Drawing;

namespace Robocup
{
    class blueTeam : Soccer
    {
        public blueTeam(Form form)
            : base(form)
        {
        }
        public class Matin
        {
            static int LastBallx, LastBally;
            static int PassedTo = 0;
            static int Defaayi = 0;
            
            static double IsBallAround(int Player)
            {
                double r = Distance(Player, 0);
                r = -r / 60 + 1;
                if (r < 0) r = 0;
                return r;
            }
            static int Distance(int Player, int X, int Y)
            {
                return (int)Math.Sqrt(Math.Pow(x(Player) - X, 2) + Math.Pow(y(Player) - Y, 2));
            }
            static int Distance(int Player, int Enemy)
            {
                return Distance(Player, x(Enemy), y(Enemy));
            }
            static int Angle(int Player, int X, int Y)
            {
                return (int)(-Math.Atan2(Y - y(Player), X - x(Player)) * 180 / Math.PI);
            }
            static int Angle(int Player, int Enemy)
            {
                return Angle(Player, x(Enemy), y(Enemy));
            }
            static bool InDanger(int Player)
            {
                for (int i = -1; i >= -5; i--)
                    if (Distance(Player, x(i), y(i)) < 35)
                        return true;
                return false;
            }
            static bool SecuredPoint(int Player, int X, int Y)
            {
                int d1, d = Distance(Player, X, Y);
                int a1, a = Angle(Player, X, Y);
                for (int i = -1; i >= -5; i--)
                {
                    d1 = Distance(Player, x(i), y(i));
                    if (d1 < d)
                    {
                        a1 = Angle(Player, x(i), y(i));
                        if (Math.Abs(a1 - a) < 20)
                            return false;
                    }
                }
                return true;
            }
            static bool SecuredPoint(int Player, int Enemy)
            {
                return SecuredPoint(Player, x(Enemy), y(Enemy));
            }
            static int SecurestPlayer(int Player)
            {
                int[] p = new int[5];
                for (int i = 0; i < 5; i++)
                    if (SecuredPoint(Player, i + 1) && i + 1 != Player)
                        p[i] = x(i + 1) - Distance(Player, i + 1) / 10;

                int jolotarin = 0;
                int cheghad = 0;
                for (int i = 0; i < 5; i++)
                    if (cheghad < p[i])
                    {
                        cheghad = p[i];
                        jolotarin = i + 1;
                    }

                return jolotarin;
            }
            static int CornerPlace(int y)
            {
                if (y < 190 - 20)
                    return y;
                if (y < 240)
                    return 190 - 20;
                if (y < 290 + 20)
                    return 290 + 20;
                return y;
            }
            static int[] ClosestPlayers(int from)
            {
                int[] d = new int[4];
                int[] p = new int[4];
                int i = 0, j = 1;
                while (i < 4)
                {
                    if (j != from)
                    {
                        p[i] = j;
                        d[i] = Distance(from, x(j), y(j));
                        i++;
                    }
                    j++;
                }
                int p1, d1;
                for (i = 0; i < 3; i++)
                    for (j = i; j < 4; j++)
                        if (d[i] > d[j])
                        {
                            p1 = p[i];
                            d1 = d[i];
                            p[i] = p[j];
                            d[i] = d[j];
                            p[j] = p1;
                            d[j] = d1;
                        }
                return p;
            }
            static int[] ClosestEnemies(int from)
            {
                int[] d = new int[5];
                int[] p = new int[5];
                int i = 0, j = -5;
                while (i < 5)
                {
                    p[i] = j;
                    d[i] = Distance(from, x(j), y(j));
                    i++;
                    j++;
                }
                int p1, d1;
                for (i = 0; i < 4; i++)
                    for (j = i; j < 5; j++)
                        if (d[i] > d[j])
                        {
                            p1 = p[i];
                            d1 = d[i];
                            p[i] = p[j];
                            d[i] = d[j];
                            p[j] = p1;
                            d[j] = d1;
                        }
                return p;
            }
            static void YaarGiri(int Player, int Enemy, int r)
            {
                double angle = -Angle(Enemy, 0);
                angle = angle * Math.PI / 180;
                int px = (int)(x(Enemy) + r * Math.Cos(angle));
                int py = (int)(y(Enemy) + r * Math.Sin(angle));
                run(Player, px, py);
            }
            static void ToopGiri(int Player, int Enemy)
            {
                double angle1 = Angle(Player, Enemy) % 360;
                double angle2 = 180 - Angle(Enemy, 0) % 360;

                angle1 = angle1 * Math.PI / 180;
                angle2 = angle2 * Math.PI / 180;
                double angle3 = 2 * angle1 + angle2;

                if (Math.Abs(angle3 + angle2) < Math.PI)
                {
                    int dx = (int)(10 * Math.Cos(angle3));
                    int dy = (int)(-10 * Math.Sin(angle3));
                    run(Player, x(Player) + dx, y(Player) + dy);
                }
                else
                    run(Player, x(0), y(0));
            }
            static int Shirje()
            {
                if (LastBallx != 0)
                {
                    int dy = LastBally - y(0);
                    int dx = x(0) - LastBallx;
                    if (dx != 0)
                    {
                        int y1 = dy / dx * (x(0) - x(1)) + y(0);
                        return y1;
                    }
                    else
                        return y(0);
                }
                return 240;
            }
            public static void Darvaze()
            {
                run(1, 40, 240);
                if (BallOwner == 1)
                {
                    int i = SecurestPlayer(1);
                    if (i != 0)
                    {
                        pas(i);
                        PassedTo = i;
                    }
                    else
                    {
                        int[] d = ClosestEnemies(1);
                        if (x(d[0]) < 130)
                            shoot(10, CornerPlace(y(0)));
                        else
                        {
                            run(2, 40, y(2));
                            run(3, 40, y(3));
                            if (y(1) > 240)
                                run(1, x(1), y(1) - 10);
                            else
                                run(1, x(1), y(1) + 10);
                        }
                    }
                }
                else
                    if (PassedTo == 1 && x(0) < 120)
                        run(1, x(0), y(0));
                    else
                    {
                        int[] e = ClosestEnemies(1);
                        int[] p = ClosestPlayers(1);

                        if (x(e[0]) < x(p[0]))
                        {
                            if (x(0) < 160)
                                if (x(0) < 120)
                                    run(1, x(0), Shirje());
                                else
                                    run(1, x(0), y(0));
                        }
                        else
                            if (x(0) < 120)
                                run(1, x(0), y(0));
                    }

                LastBallx = x(0);
                LastBally = y(0);
            }
            static void Defa()
            {
                run(2, 220, 340);
                run(3, 220, 140);

                if (BallOwner== 2 || BallOwner== 3)
                {
                    int i = SecurestPlayer(BallOwner);
                    if (i != 0)
                    {
                        pas(i);
                        PassedTo = i;
                    }
                    else
                        if (y(BallOwner) > 240)
                            shoot(x(BallOwner), 450);
                        else
                            shoot(x(BallOwner), 10);
                }
                else if ((BallOwner)== 1)
                {
                    run(2, 120, 290);
                    run(3, 120, 190);
                }
                else
                    if (PassedTo == 2)
                        run(2, x(0), y(0));
                    else if (PassedTo == 3)
                        run(3, x(0), y(0));
                    else
                    {
                        int[] d = ClosestEnemies(1);
                        int p2, p3;
                        if (Distance(2, d[0]) < Distance(3, d[0]))
                        {
                            p2 = d[0];
                            p3 = d[1];
                        }
                        else
                        {
                            p2 = d[1];
                            p3 = d[0];
                        }

                        if (x(0) < 320)
                            if ((BallOwner)== p2)
                            {
                                ToopGiri(2, p2);
                                YaarGiri(3, p3, 30);
                            }
                            else if ((BallOwner)== p3)
                            {
                                ToopGiri(3, p3);
                                YaarGiri(2, p2, 30);
                            }
                            else
                            {
                                if ((BallOwner)== 0)

                                    if (Distance(2, 0) < Distance(3, 0))
                                        run(2, x(0), y(0));
                                    else
                                        run(3, x(0), y(0));
                                else
                                {
                                    YaarGiri(2, p2, 30);
                                    YaarGiri(3, p3, 30);
                                }
                            }
                        else
                            if (x(d[0]) < 320 || Defaayi == 0)
                            {
                                YaarGiri(3, p3, 30);
                                YaarGiri(2, p2, 30);
                            }
                            else
                                if (Distance(2, d[0]) < Distance(3, d[0]))
                                {
                                    run(3, 160, 240);
                                    YaarGiri(2, d[0], 30);
                                }
                                else
                                {
                                    run(2, 160, 240);
                                    YaarGiri(3, d[0], 30);
                                }
                    }
            }
            static void Hamle()
            {
                if ((BallOwner)== 4 || (BallOwner)== 5)
                    if (SecuredPoint(BallOwner, 640, 240) && y(BallOwner) > 100)
                        shoot(640, 240);
                    else if (SecuredPoint(BallOwner, 640, 240 - 25) && y(BallOwner) > 100)
                        shoot(640, 240 - 25);
                    else if (SecuredPoint(BallOwner, 640, 240 + 25) && y(BallOwner) > 100)
                        shoot(640, 240 + 25);
                    else
                    {
                        int[] d = ClosestEnemies(1);
                        if (x(d[3]) < x(BallOwner) && x(BallOwner) < 500)
                        {
                            shoot(500, y(BallOwner));
                            PassedTo = BallOwner;
                            if (BallOwner == 5)
                                run(4, 550, 320);
                            else
                                run(5, 550, 160);
                        }
                        else
                            if (!InDanger(BallOwner) && x(BallOwner) < 540)
                                if ((BallOwner) == 5)
                                {
                                    run(5, 550, 160);
                                    if (y(d[3]) > 240)
                                        YaarGiri(4, d[3], 50);
                                    else
                                        YaarGiri(4, d[2], 50);
                                }
                                else
                                {
                                    run(4, 550, 320);
                                    if (y(d[3]) < 240)
                                        YaarGiri(5, d[3], 50);
                                    else
                                        YaarGiri(5, d[2], 50);
                                }
                            else
                            {
                                int i = SecurestPlayer((BallOwner));
                                if (i != 0)
                                {
                                    pas(i);
                                    PassedTo = i;
                                }
                                else
                                    if (x((BallOwner)) < 640 - 200)
                                        if (y((BallOwner)) > 240)
                                            shoot(x((BallOwner)), 450);
                                        else
                                            shoot(x((BallOwner)), 10);
                                    else
                                        if (y(-1) > 240)
                                            shoot(630, 240 - 25);
                                        else
                                            shoot(630, 240 + 25);
                            }
                    }
                else if (BallOwner == 1)
                {
                    run(4, 320, 290);
                    run(5, 320, 190);
                }
                else if ((BallOwner) == -1 && GradeOfMyTeam <= GradeOfOpponentTeam)
                {
                    int[] d = ClosestEnemies(1);
                    int p2, p3;
                    if (x(5) < x(d[0]) && x(4) < x(d[0]))
                        if (Distance(4, d[0]) < Distance(5, d[0]))
                        {
                            p2 = d[0];
                            p3 = d[1];
                        }
                        else
                        {
                            p2 = d[1];
                            p3 = d[0];
                        }
                    else
                        if (Distance(4, d[2]) < Distance(5, d[2]))
                        {
                            p2 = d[2];
                            p3 = d[3];
                        }
                        else
                        {
                            p2 = d[3];
                            p3 = d[2];
                        }

                    YaarGiri(4, p2, 30);
                    YaarGiri(5, p3, 30);
                }
                else
                    if (PassedTo == 4)
                        run(4, x(0), y(0));
                    else if (PassedTo == 5)
                        run(5, x(0), y(0));
                    else
                    {
                        int[] d = ClosestEnemies(1);
                        int p2, p3;
                        if (x(5) < x(d[0]) && x(4) < x(d[0]))
                            if (Distance(4, d[0]) < Distance(5, d[0]))
                            {
                                p2 = d[0];
                                p3 = d[1];
                            }
                            else
                            {
                                p2 = d[1];
                                p3 = d[0];
                            }
                        else
                            if (Distance(4, d[2]) < Distance(5, d[2]))
                            {
                                p2 = d[2];
                                p3 = d[3];
                            }
                            else
                            {
                                p2 = d[3];
                                p3 = d[2];
                            }

                        if (BallOwner == p2)
                        {
                            ToopGiri(4, p2);
                            YaarGiri(5, p3, 30);
                        }
                        else if (BallOwner == p3)
                        {
                            ToopGiri(5, p3);
                            YaarGiri(4, p2, 30);
                        }
                        else
                        {
                            if (IsBallAround(4) > 0 && BallOwner <= 0 && PassedTo == 0)
                                run(4, x(0), y(0));
                            else
                                run(4, 500, 320);
                            if (IsBallAround(5) > 0 && BallOwner <= 0 && PassedTo == 0)
                                run(5, x(0), y(0));
                            else
                                run(5, 500, 160);
                            //run(4, (int)(IsBallAround(4) * x(0) + (1 - IsBallAround(4)) * 500),(int)(IsBallAround(4) * y(0) + (1 - IsBallAround(4))* 320));
                            //run(5, (int)(IsBallAround(5) * x(0) + (1 - IsBallAround(5)) * 500),(int)(IsBallAround(5) * y(0) + (1 - IsBallAround(5))* 160));
                        }
                    }
            }

            public static void play()
            {
               
                Say(Hero + "a");
                Defaayi = 0;
                if (BallOwner != 0)
                    PassedTo = 0;

                if (CurrentGameStatus == Status.Play)
                {
                    Hamle();
                    Defa();
                    Darvaze();
                }
                else
                    if (BallOwner > 0)
                    {
                        pas(4);
                        PassedTo = 4;
                    }
            }
        }
        public class Mohsen
        {
            static int owner = 0;
            static int NearPlayer(int x, int y)
            {
                int min = 2;
                for (int i = 3; i <= 5; i++)
                {
                    if (Distance(i, x, y) < Distance(min, x, y))
                        min = i;
                }
                return min;
            }

            static int ONearPlayer(int x, int y)
            {
                int min = -2;

                for (int i = -3; i >= -5; i--)
                {
                    if (Distance(i, x, y) < Distance(min, x, y))
                        min = i;
                }
                return min;
            }


            static int SecurePlayer(int player)
            {
                int p = player;

                if (SecuredPoint(player, x(5), y(5)) && player != 5 && !InDanger(5) && Distance(player, x(5), y(5)) < 400)
                    p = 5;
                else if (SecuredPoint(player, x(4), y(4)) && player != 4 && !InDanger(4) && Distance(player, x(4), y(4)) < 400)
                    p = 4;
                else if (SecuredPoint(player, x(3), y(3)) && player != 3 && !InDanger(3) && Distance(player, x(3), y(3)) < 400)
                    p = 3;
                else if (SecuredPoint(player, x(2), y(2)) && player != 2 && !InDanger(2) && Distance(player, x(2), y(2)) < 400)
                    p = 2;
                else if (SecuredPoint(player, x(1), y(1)) && player != 1 && !InDanger(1) && Distance(player, x(1), y(1)) < 400 && x(0) < 320)
                    p = 1;

                return p;
            }

            static double NearGoal()
            {
                if (x(0) <= 150)
                    return 1;
                else if (x(0) > 200)
                    return 0;
                else
                    return (200 - x(0)) / 50;
            }

            static int Distance(int Player, int X, int Y)
            {
                return (int)Math.Sqrt(Math.Pow(x(Player) - X, 2) + Math.Pow(y(Player) - Y, 2));
            }
            static int Angle(int Player, int X, int Y)
            {
                return (int)(-Math.Atan2(Y - y(Player), X - x(Player)) * 180 / Math.PI);
            }

            static bool InDanger(int Player)
            {
                for (int i = -1; i >= -5; i--)
                    if (Distance(Player, x(i), y(i)) < 35)
                        return true;
                return false;
            }

            static bool SecuredPoint(int Player, int X, int Y)
            {
                int d1, d = Distance(Player, X, Y);
                int a1, a = Angle(Player, X, Y);
                for (int i = -1; i >= -5; i--)
                {
                    d1 = Distance(Player, x(i), y(i));
                    if (d1 < d)
                    {
                        a1 = Angle(Player, x(i), y(i));
                        if (Math.Abs(a1 - a) < 25)
                            return false;
                    }
                }
                return true;
            }

            public static void play()
            {
                Say("3 Idiots - MZ Strategy");
                if ((BallOwner) == 1)
                    owner = 1;
                if ((BallOwner) == 2)
                    owner = 2;
                if ((BallOwner) == 3)
                    owner = 3;
                if ((BallOwner) == 4)
                    owner = 4;
                if (BallOwner == 5)
                    owner = 5;

                if ((BallOwner) == -1)
                    owner = -1;
                if ((BallOwner) == -2)
                    owner = -2;
                if ((BallOwner) == -3)
                    owner = -3;
                if ((BallOwner) == -4)
                    owner = -4;
                if ((BallOwner) == -5)
                    owner = -5;

                run(2, x(2), y(2));
                run(3, x(3), y(3));
                run(4, x(4), y(4));
                run(5, x(5), y(5));

                if ((BallOwner) == 0 && NearPlayer(x(0), y(0)) != owner)
                    run(NearPlayer(x(0), y(0)), x(0), y(0));


                run(1, (int)((NearGoal() * x(0)) + ((1 - NearGoal()) * 50)), (240 + y(0)) / 2);
                if ((Distance(1, x(0), y(0)) < 90 && x(0) < 120) || Distance(1, x(0), y(0)) < 30)
                    run(1, x(0), y(0));

                if (BallOwner == 0)
                {
                    if (Distance(NearPlayer(x(0), y(0)), x(0), y(0)) <= Distance(ONearPlayer(x(0), y(0)), x(0), y(0)))
                    {
                        run(2, 320, 320);
                        run(3, 400, 140);
                        run(4, 320, 340);
                        run(5, 500, 160);
                    }
                    else
                    {
                        run(2, 140, 320);
                        run(3, 140, 140);
                        run(4, 300, 340);
                        run(5, 300, 160);
                    }
                }

                if (BallOwner == 0 && NearPlayer(x(0), y(0)) != owner)
                    run(NearPlayer(x(0), y(0)), x(0), y(0));

                if (BallOwner < 0 || (BallOwner == 0 && owner < 0))
                {
                    if (y(0) < 190)
                        run(1, 40, 190);
                    else if (y(0) > 290)
                        run(1, 40, 290);

                    if (Distance(1, x(0), y(0)) < 50)
                        run(1, x(0), y(0));

                    if (x(0) < 300)
                    {
                        run(3, x(-5), y(-5));
                        run(2, x(-4), y(-4));
                        run(5, (x(0) + x(-3)) / 2, (y(0) + y(-3)) / 2);
                        run(4, (x(0) + x(-2)) / 2, (y(0) + y(-2)) / 2);
                    }
                    else
                    {
                        run(3, 100, 200);
                        run(2, 100, 280);
                        run(5, x(-5), y(-5));
                        run(4, x(-4), y(-4));
                    }
                }

                if (BallOwner > 0)
                {
                    if (BallOwner == 1)
                    {
                        if (Distance(ONearPlayer(x(1), y(1)), x(1), y(1)) > 100 && x(1) < 100)
                            run(1, 100, 240);
                        if (!InDanger(1) && SecurePlayer(1) == 1)
                        {
                            if (GradeOfMyTeam >= GradeOfOpponentTeam)
                                run(1, 110, 240);
                            else
                                shoot(2000, 240);
                        }
                        else
                            pas(SecurePlayer(1));

                        run(4, x(2) + 250, 350);
                        run(5, x(3) + 250, 150);
                    }

                    if (BallOwner == 2)
                    {
                        if (!InDanger(BallOwner))
                        {
                            if (y(ONearPlayer(x(2), y(2))) <= y(2) && owner != 1)
                                run(2, 500, y(ONearPlayer(x(2), y(2))) + 20);
                            else
                                run(2, 500, y(ONearPlayer(x(2), y(2))) - 20);

                            run(3, 400, y(2) - 80);

                            if (x(0) >= 500)
                            {
                                if (x(-1) >= 240)
                                    shoot(640, 200);
                                else
                                    shoot(640, 280);
                            }
                        }
                        else
                        {
                            if (x(0) >= 450)
                            {
                                if (y(-1) >= 240)
                                    shoot(640, 205);
                                else
                                    shoot(640, 285);
                            }
                            else
                            {
                                if (SecurePlayer(2) != 2)
                                    pas(SecurePlayer(BallOwner));
                                else
                                {
                                    if (y(-1) >= 240)
                                        shoot(640, 200);
                                    else
                                        shoot(640, 280);
                                }
                            }
                        }

                        if (x(0) > 500)
                            if (SecuredPoint(2, x(5), y(5)))
                                pas(5);
                            else if (SecuredPoint(2, x(4), y(4)))
                                pas(4);
                            else
                            {
                                if (y(-1) < 240)
                                    shoot(640, 280);
                                else
                                    shoot(640, 205);
                            }
                        run(4, x(2) + 150, 320);
                        run(5, x(3) + 150, 150);

                        if (x(0) >= 400)
                        {
                            run(3, 200, 240);
                            run(4, (x(0) + x(-1)) / 2, (y(0) + 190) / 2);
                            run(5, (x(0) + x(-1)) / 2, (y(0) + 290) / 2);
                        }
                    }

                    if (BallOwner == 3)
                    {
                        if (!InDanger(BallOwner))
                        {
                            if (y(ONearPlayer(x(3), y(3))) <= y(3))
                                run(3, 500, y(ONearPlayer(x(3), y(3))) + 20);
                            else
                                run(3, 500, y(ONearPlayer(x(3), y(3))) - 20);

                            run(2, 600, y(3) + 80);

                            if (x(0) >= 500)
                            {
                                if (x(-1) >= 240)
                                    shoot(640, 200);
                                else
                                    shoot(640, 280);
                            }
                        }
                        else
                        {
                            if (x(0) >= 500)
                            {
                                if (y(-1) >= 240)
                                    shoot(640, 210);
                                else
                                    shoot(640, 280);
                            }
                            else
                            {
                                if (SecurePlayer(3) != 3)
                                    pas(SecurePlayer(BallOwner));
                                else
                                {
                                    if (y(-1) >= 240)
                                        shoot(640, 200);
                                    else
                                        shoot(640, 280);
                                }
                            }
                        }

                        if (x(0) > 500)
                            if (SecuredPoint(3, x(5), y(5)))
                                pas(5);
                            else if (SecuredPoint(3, x(4), y(4)))
                                pas(4);
                            else
                            {
                                if (y(-1) < 240)
                                    shoot(640, 280);
                                else
                                    shoot(640, 205);
                            }

                        run(4, x(2) + 150, 320);
                        run(5, x(3) + 150, 150);

                        if (x(0) >= 400)
                        {
                            run(3, 200, 240);
                            run(4, (x(0) + x(-1)) / 2, (y(0) + 190) / 2);
                            run(5, (x(0) + x(-1)) / 2, (y(0) + 290) / 2);
                        }
                    }

                    if (BallOwner == 4)
                    {
                        if (!InDanger(4))
                        {
                            run(4, 500, (y(-2) + y(-3)) / 2);
                            run(5, 500, y(4) - 80);

                            if (x(0) >= 500)
                            {
                                if (y(-1) <= 240)
                                    shoot(650, 275);
                                else
                                    shoot(650, 205);
                            }
                        }
                        else
                        {
                            if (x(0) < 500)
                                pas(SecurePlayer(BallOwner));
                            else if (!SecuredPoint(4, 600, 275))
                                pas(5);
                            else
                                shoot(650, 275);
                        }
                    }

                    if (BallOwner == 5)
                    {
                        if (x(0) == 320 && y(0) == 240)
                            pas(3);
                        else
                        {
                            if (!InDanger(5))
                            {
                                run(5, 600, (y(-2) + y(-3)) / 2);
                                run(4, 600, y(5) + 80);

                                if (x(0) >= 600)
                                {
                                    if (y(-1) <= 240)
                                        shoot(650, 275);
                                    else
                                        shoot(650, 210);
                                }
                            }
                            else
                            {
                                if (x(0) < 500)
                                    pas(SecurePlayer(BallOwner));
                                else if (!SecuredPoint(5, 600, 195))
                                    pas(4);
                                else
                                    shoot(650, 210);
                            }
                        }
                    }
                    if (BallOwner == 2 || BallOwner == 3)
                    {
                        if (x(5) >= 500)
                            run(5, (x(-1) + x(0)) / 2, (y(0) - 100));
                        if (x(4) >= 500)
                            run(4, (x(-1) + x(0)) / 2, y(0) + 100);
                    }
                }

                if (BallOwner == 0 && NearPlayer(x(0), y(0)) != owner)
                    run(NearPlayer(x(0), y(0)), x(0), y(0));

                if (CurrentGameStatus == Status.Corner || CurrentGameStatus == Status.HandOut)
                {
                    if (BallOwner > 1)
                        pas(4);
                }
                Matin.Darvaze();
            }
        }
        class Alireza
        {
            static int LastBallx, LastBally;
            class Queue
            {
                static int[] PlayerCaptured = new int[5];
                static int ToQ = -1;
                public static void Push(int PlayerNum)
                {
                    ToQ++;
                    if (ToQ == PlayerCaptured.Length)
                        ToQ = 0;
                    else
                        PlayerCaptured[ToQ] = PlayerNum;

                }
                public static int Pop()
                {
                    ToQ--;
                    if (ToQ == -1)
                        ToQ = PlayerCaptured.Length - 1;
                    else
                        return PlayerCaptured[ToQ];
                    return PlayerCaptured[ToQ];
                }
                public static bool Check(int PlayerNum)
                {
                    for (int i = 0; i < PlayerCaptured.Length; i++)
                        if (PlayerCaptured[i] == PlayerNum)
                            return false;
                    return true;
                }
                public static void ClearQueue()
                {
                    for (int i = 0; i < PlayerCaptured.Length; i++)
                        PlayerCaptured[i] = 0;
                }

                public static int[] PlayerList()
                {
                    return PlayerCaptured;
                }
            }
            class StatusQueue
            {
                static Status[] a = new Status[5];
                static int ToQ = -1;
                public static void Push(Status NewStatus)
                {
                    if (ToQ == a.Length - 1)
                        ToQ = 0;
                    else
                        ToQ++;
                    a[ToQ] = NewStatus;

                }
                public static Status Pop()
                {
                    if (ToQ == -1)
                        return Status.Play;
                    else if (ToQ == 0)
                    {
                        ToQ = a.Length - 1;
                        return a[0];
                    }
                    else
                    {
                        ToQ--;
                        return a[ToQ + 1];
                    }
                }
            }


            static int Shirje()
            {
                if (LastBallx != 0)
                {
                    int dy = LastBally - y(0);
                    int dx = x(0) - LastBallx;
                    if (dx != 0)
                    {
                        int y1 = dy / dx * (x(0) - x(1)) + y(0);
                        return y1;
                    }
                    else
                        return y(0);
                }
                return 240;
            }
            static int[] ClosestPlayers(int from)
            {
                int[] d = new int[4];
                int[] p = new int[4];
                int i = 0, j = 1;
                while (i < 4)
                {
                    if (j != from)
                    {
                        p[i] = j;
                        d[i] = Distance(from, x(j), y(j));
                        i++;
                    }
                    j++;
                }
                int p1, d1;
                for (i = 0; i < 3; i++)
                    for (j = i; j < 4; j++)
                        if (d[i] > d[j])
                        {
                            p1 = p[i];
                            d1 = d[i];
                            p[i] = p[j];
                            d[i] = d[j];
                            p[j] = p1;
                            d[j] = d1;
                        }
                return p;
            }
            static int[] ClosestEnemies(int from)
            {
                int[] d = new int[5];
                int[] p = new int[5];
                int i = 0, j = -5;
                while (i < 5)
                {
                    p[i] = j;
                    d[i] = Distance(from, x(j), y(j));
                    i++;
                    j++;
                }
                int p1, d1;
                for (i = 0; i < 4; i++)
                    for (j = i; j < 5; j++)
                        if (d[i] > d[j])
                        {
                            p1 = p[i];
                            d1 = d[i];
                            p[i] = p[j];
                            d[i] = d[j];
                            p[j] = p1;
                            d[j] = d1;
                        }
                return p;
            }

            //Hashemian Methods
            static int NearPlayer(int x, int y)
            {
                int min = 2;
                for (int i = 3; i <= 5; i++)
                {
                    if (Distance(i, x, y) < Distance(min, x, y))
                        min = i;
                }
                return min;
            }
            static int ONearPlayer(int x, int y)
            {
                int min = -2;

                for (int i = -3; i >= -5; i--)
                {
                    if (Distance(i, x, y) < Distance(min, x, y))
                        min = i;
                }
                return min;
            }
            static int SecurePlayer(int player)
            {
                int p = player;

                if (SecuredPoint(player, x(5), y(5)) && player != 5 && !InDanger(5) && Distance(player, x(5), y(5)) < 320)
                    p = 5;
                else if (SecuredPoint(player, x(4), y(4)) && player != 4 && !InDanger(4) && Distance(player, x(4), y(4)) < 320)
                    p = 4;
                else if (SecuredPoint(player, x(3), y(3)) && player != 3 && !InDanger(3) && Distance(player, x(3), y(3)) < 320)
                    p = 3;
                else if (SecuredPoint(player, x(2), y(2)) && player != 2 && !InDanger(2) && Distance(player, x(2), y(2)) < 320)
                    p = 2;
                else if (SecuredPoint(player, x(2), y(2)) && player != 1 && !InDanger(1) && Distance(player, x(1), y(1)) < 320)
                    p = 1;

                return p;
            }
            static int Distance(int Player, int X, int Y)
            {
                return (int)Math.Sqrt(Math.Pow(x(Player) - X, 2) + Math.Pow(y(Player) - Y, 2));
            }
            static int Angle(int Player, int X, int Y)
            {
                return (int)(-Math.Atan2(Y - y(Player), X - x(Player)) * 180 / Math.PI);
            }
            static bool InDanger(int Player)
            {
                for (int i = -1; i >= -5; i--)
                    if (Distance(Player, x(i), y(i)) < 35)
                        return true;
                return false;
            }
            static bool SecuredPoint(int Player, int X, int Y)
            {
                int d1, d = Distance(Player, X, Y);
                int a1, a = Angle(Player, X, Y);
                for (int i = -1; i >= -5; i--)
                {
                    d1 = Distance(Player, x(i), y(i));
                    if (d1 < d)
                    {
                        a1 = Angle(Player, x(i), y(i));
                        if (Math.Abs(a1 - a) < 25)
                            return false;
                    }
                }
                return true;
            }
            static double NearGoal()
            {
                if (x(0) <= 150)
                    return 1;
                else if (x(0) > 200)
                    return 0;
                else
                    return (200 - x(0)) / 50;
            }

            //My Self Methods
            static double Distance(Point PlayerNumber1, Point PlayerNumber2)
            {
                int X = PlayerNumber1.X - PlayerNumber2.X;
                int Y = PlayerNumber1.Y - PlayerNumber2.Y;
                return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
            }
            static double Angle(Point Destination1, Point Destination2)
            {
                return Math.Atan(Destination1.Y - Destination2.Y / Destination1.X - Destination2.X);
            }
            static double Angle(int Player1, int Player2)
            {
                Point P1 = new Point(x(Player1), y(Player1));
                Point P2 = new Point(x(Player2), y(Player2));
                return Angle(P1, P2);
            }
            static double NearGate()
            {
                if (x(0) < 150)
                    return 1;
                else if (x(0) > 200)
                    return 0;
                else
                    return (200 - x(0)) / 50;
            }
            static double Near(int PlayerNum)
            {
                for (int i = 1; i < 6; i++)
                {
                    if (Math.Sqrt(Math.Pow(x(PlayerNum) - x(-i), 2) + Math.Pow(y(PlayerNum) - y(-i), 2)) < 150)
                        pas(FindNearFriend());
                }
                return 0;
            }
            static int FindNearFriend()//when ball is our and we want pas other
            {
                double NP = 100;
                int P = -1;
                Point BallP = new Point(x(BallOwner), y(BallOwner));
                for (int i = 1; i < 6; i++)
                {
                    if (i != BallOwner)
                    {
                        Point otherPlayer = new Point(x(i), y(i));
                        if (Distance(BallP, otherPlayer) < NP)
                        {
                            NP = Distance(BallP, otherPlayer);
                            P = i;
                        }
                    }
                }
                return P;
            }

            static int FindNearEnemy(Point fPlayer)//fplayer=the player that is ballowner
            {
                double NP = 700;
                int ePlayer = 1;
                for (int i = -1; i > -6; i--)
                {
                    if (i != BallOwner)
                    {
                        Point otherPlayer = new Point(x(-i), y(-i));
                        if (Distance(fPlayer, otherPlayer) < NP)
                        {
                            NP = Distance(fPlayer, otherPlayer);
                            ePlayer = i;
                        }
                    }
                }
                return ePlayer;
            }
            static int FindNearEnemy(int X, int Y)
            {
                Point NewPoint = new Point(X, Y);
                return FindNearEnemy(NewPoint);
            }
            static int FindNearEnemy(int[] CapedPlayer)//check which player isn't captered!
            {
                double NP = 700;
                int ePlayer = 1;
                Point fPlayer = new Point(x(0), y(0));
                for (int i = -1; i > -6; i--)
                {
                    if (i != BallOwner && Queue.Check(i))
                    {
                        Point otherPlayer = new Point(x(-i), y(-i));
                        if (Distance(fPlayer, otherPlayer) < NP)
                        {
                            NP = Distance(fPlayer, otherPlayer);
                            ePlayer = i;
                        }
                    }
                }
                return ePlayer;
            }

            static Point FindBestPlace(Point OPlayer)//OPlayer = the Nearest Player of ballowner 
            {
                Point BO = new Point(x(BallOwner), y(BallOwner));
                Point Place = new Point();
                Place.X = (OPlayer.X - x(BallOwner)) * (int)Math.Cos(Angle(BO, OPlayer)) + x(0);
                Place.Y = (OPlayer.Y - y(BallOwner)) * (int)Math.Sin(Angle(BO, OPlayer)) + y(0);
                return Place;
            }
            static Point FindBestPlace(int PlayerNum)
            {
                Point Player = new Point(x(PlayerNum), y(PlayerNum));
                return FindBestPlace(Player);
            }
            static Point FindBestPlace(Point GoalKeeper, Point EnemyPlayer)
            {
                Point BP = new Point();
                BP.X = EnemyPlayer.X * (int)Math.Cos(Angle(GoalKeeper, EnemyPlayer)) + x(0);
                BP.Y = EnemyPlayer.Y * (int)Math.Sin(Angle(GoalKeeper, EnemyPlayer)) + y(0);
                return BP;
            }

            static void FindBestPlacePas()//when ball is ours, where should other player go
            {
                if (y(BallOwner) < 240)
                    switch (BallOwner)
                    {
                        case 2:
                            run(3, x(BallOwner), y(BallOwner) + 100);
                            run(4, x(BallOwner) + 100, y(BallOwner));
                            run(5, x(BallOwner) + 100, y(BallOwner) + 100);
                            break;

                        case 3:
                            run(2, x(BallOwner), y(BallOwner) + 100);
                            run(4, x(BallOwner) + 100, y(BallOwner));
                            run(5, x(BallOwner) + 100, y(BallOwner) + 100);
                            break;

                        case 4:
                            run(2, x(BallOwner), y(BallOwner) - 100);
                            run(3, x(BallOwner) - 100, y(BallOwner) - 100);
                            run(5, x(BallOwner), y(BallOwner) + 100);
                            break;

                        case 5:
                            run(2, x(BallOwner) - 100, y(BallOwner) - 100);
                            run(3, x(BallOwner) - 100, y(BallOwner));
                            run(4, x(BallOwner), y(BallOwner) + 100);
                            break;
                    }
                else
                    switch (BallOwner)
                    {
                        case 2:
                            run(3, x(BallOwner), y(BallOwner) - 100);
                            run(4, x(BallOwner) + 100, y(BallOwner));
                            run(5, x(BallOwner) + 100, y(BallOwner) - 100);
                            break;

                        case 3:
                            run(2, x(BallOwner), y(BallOwner) - 100);
                            run(4, x(BallOwner) + 100, y(BallOwner) - 100);
                            run(5, x(BallOwner) + 100, y(BallOwner));
                            break;

                        case 4:
                            run(2, x(BallOwner) - 100, y(BallOwner));
                            run(3, x(BallOwner) - 100, y(BallOwner) - 100);
                            run(5, x(BallOwner), y(BallOwner) - 100);
                            break;

                        case 5:
                            run(2, x(BallOwner) - 100, y(BallOwner) - 100);
                            run(3, x(BallOwner) - 100, y(BallOwner));
                            run(4, x(BallOwner), y(BallOwner) - 100);
                            break;
                    }
            }

            static void Goalkeeper(int LimX)//every thing that goalkeeper must do!
            {
                run(1, 40, 240);
                if (BallOwner != 1)
                {
                    if (x(0) < 120)
                        run(1, x(0), y(0));
                    else
                    {
                        int[] e = ClosestEnemies(1);
                        int[] p = ClosestPlayers(1);

                        if (x(e[0]) < x(p[0]))
                        {
                            if (x(0) < 160)
                                if (x(0) < 120)
                                    run(1, x(0), Shirje());
                                else
                                    run(1, x(0), y(0));
                        }
                        else
                            if (x(0) < 120)
                                run(1, x(0), y(0));
                    }
                }
                else
                {
                    //if (NeedPas() && CanPas() && CheckWay(FindNearFriend()))
                    //    pas(FindNearFriend());
                    //else
                    //    if (y(0) < 240)
                    //        shoot(15, 180);
                    //    else
                    //        shoot(15, 300);
                }

                LastBallx = x(0);
                LastBally = y(0);
            }

            static bool CheckWay(int PN)//PN=Player Number
            {
                for (int i = -50; i < 50; i++)
                    if (!SecuredPoint(PN, x(PN) + i, y(PN)))
                        return false;
                return true;
            }

            static bool NeedPas()//Key pas bede?
            {
                Point P1 = new Point(x(BallOwner), y(BallOwner));
                for (int i = 1; i < 6; i++)
                {
                    Point P2 = new Point(x(-i), y(-i));
                    if (Distance(P1, P2) < 50)
                        return true;
                }
                return false;
            }
            static bool CanPas()//be ki pas bede?
            {
                if (FindNearFriend() != -1)
                    return true;
                else
                    return false;
            }

            static Point ShootPoint()//kojai darvaze bezane?
            {
                Point Goal = new Point(625, 240);
                double V0 = 20;
                double a = -1;
                double deltaX = 0;
                Point P1 = new Point(x(BallOwner), y(BallOwner));
                double Tb, Tgk;

                do
                {
                    if (y(-1) < 240)
                        Goal.Y++;
                    else
                        Goal.Y--;

                    P1 = new Point(x(BallOwner), y(BallOwner));
                    deltaX = Distance(P1, Goal);
                    Tb = -V0 + Math.Sqrt(Math.Pow(V0, 2) - 2 * a * deltaX) / -a;

                    P1.X = x(-1);
                    P1.Y = y(-1);
                    deltaX = Distance(P1, Goal);
                    Tgk = deltaX / V0;
                }
                while (Tb > Tgk && Tb < 2);
                return Goal;
            }

            static Point ConvertIntToPoint(int X, int Y)
            {
                Point S = new Point(X, Y);
                return S;
            }
            static Point ConvertIntToPoint(int PN)
            {
                Point S = new Point(x(PN), y(PN));
                return S;
            }

            //PlayerPoint=Point of the player
            //BallPoint=point of ball
            //MainPoint= the point that player must move around there
            static void PasPlayer(Point PlayerPoint, Point BallPoint, int PlayerNum)
            {
                double r = Distance(PlayerPoint, BallPoint);
                double a = Angle(PlayerPoint, BallPoint) + Math.PI;
                a = Math.Cos(a);
                run(PlayerNum, Convert.ToInt32(x(0) + r * a), Convert.ToInt32(y(0) + r * a));
            }

            public static void Play(int s)
            {
                Say("3 Idiots - Arkantos Strategy", true);
                switch (s)
                {
                    case 0:
                        //shokhmi
                        if (CurrentGameStatus == Status.Goal)
                            Say("Salam!", true);

                        if (BallOwner <= 0)
                        {
                            run(1, x(1), y(0));
                            for (int i = 2; i < 6; i++)
                                run(i, x(0), y(0));
                        }
                        else
                            shoot(621, 285);
                        break;

                    case 1:
                        switch (CurrentGameStatus)
                        {
                            case Status.Play:
                                StatusQueue.Push(Status.Play);
                                //Goalkeeper(200, 100);

                                if (BallOwner < 1)//age daste ma nabood
                                {
                                    run(5, x(0), y(0));
                                    Queue.Push(BallOwner);

                                    Point PPlace4 = FindBestPlace(FindNearEnemy(x(0), y(0)));
                                    Queue.Push(FindNearEnemy(x(0), y(0)));
                                    run(4, PPlace4.X, PPlace4.Y);
                                    Say("PP4X : " + PPlace4.X.ToString() + " PP4Y : " + PPlace4.Y.ToString() + " NEN : " + FindNearEnemy(x(0), y(0)).ToString(), true);

                                    Point PPlace3 = FindBestPlace(FindNearEnemy(Queue.PlayerList()));
                                    Queue.Push(FindNearEnemy(Queue.PlayerList()));
                                    run(3, PPlace3.X, PPlace3.Y);

                                    Point PPlace2 = FindBestPlace(ConvertIntToPoint(x(1), y(1)), ConvertIntToPoint(FindNearEnemy(Queue.PlayerList())));
                                    Queue.Push(FindNearEnemy(Queue.PlayerList()));
                                    run(2, PPlace2.X, PPlace2.Y);

                                    Queue.Pop();
                                    Queue.Pop();

                                }
                                else//age daste ma bood
                                {
                                    Queue.ClearQueue();
                                    FindBestPlacePas();
                                    int GoalDistance = 520;
                                    switch (BallOwner)
                                    {
                                        case 2:
                                            if (NeedPas())
                                            {
                                                if (CheckWay(BallOwner))
                                                    run(BallOwner, x(BallOwner) + 10, y(BallOwner));
                                                else
                                                    if (CanPas() && x(0) < GoalDistance)
                                                        if (SecurePlayer(4) == 4)
                                                            pas(4);
                                                        else
                                                            if (SecurePlayer(3) == 3)
                                                                pas(3);
                                                            else
                                                                pas(1);
                                                    else
                                                    {
                                                        Point BShoot = ShootPoint();
                                                        shoot(BShoot.X, BShoot.Y);
                                                    }
                                            }
                                            else
                                                if (x(BallOwner) < GoalDistance)
                                                    run(BallOwner, x(BallOwner) + 5, y(BallOwner));
                                                else
                                                {
                                                    Point BShoot = ShootPoint();
                                                    shoot(BShoot.X, BShoot.Y);
                                                }
                                            break;

                                        case 3:
                                            if (NeedPas())
                                                if (CheckWay(BallOwner))
                                                    run(BallOwner, x(BallOwner) + 10, y(BallOwner));
                                                else
                                                    if (CanPas() && x(0) < GoalDistance)
                                                        if (SecurePlayer(5) == 5)
                                                            pas(5);
                                                        else
                                                            if (SecurePlayer(2) == 2)
                                                                pas(2);
                                                            else
                                                                pas(1);
                                                    else
                                                    {
                                                        Point BShoot = ShootPoint();
                                                        shoot(BShoot.X, BShoot.Y);
                                                    }
                                            else
                                                if (x(BallOwner) < GoalDistance)
                                                    run(BallOwner, x(BallOwner) + 5, y(BallOwner));
                                                else
                                                {
                                                    Point BShoot = ShootPoint();
                                                    shoot(BShoot.X, BShoot.Y);
                                                }
                                            break;

                                        case 4:
                                            if (NeedPas())
                                                if (CheckWay(BallOwner))
                                                    run(BallOwner, x(BallOwner) + 10, y(BallOwner));
                                                else
                                                    if (CanPas() && x(0) < GoalDistance)
                                                        if (SecurePlayer(5) == 5)
                                                            pas(5);
                                                        else
                                                            if (SecurePlayer(2) == 2)
                                                                pas(2);
                                                            else
                                                            {
                                                                Point BShoot = ShootPoint();
                                                                shoot(BShoot.X, BShoot.Y);
                                                            }
                                                    else
                                                    {
                                                        Point BShoot = ShootPoint();
                                                        shoot(BShoot.X, BShoot.Y);
                                                    }
                                            else
                                                if (x(BallOwner) < GoalDistance)
                                                    run(BallOwner, x(BallOwner) + 5, y(BallOwner));
                                                else
                                                {
                                                    Point BShoot = ShootPoint();
                                                    shoot(BShoot.X, BShoot.Y);
                                                }
                                            break;

                                        case 5:
                                            if (NeedPas())
                                                if (CheckWay(BallOwner))
                                                    run(BallOwner, x(BallOwner) + 10, y(BallOwner));
                                                else
                                                    if (CanPas() && x(0) < GoalDistance)
                                                        if (SecurePlayer(4) == 4)
                                                            pas(4);
                                                        else
                                                            if (SecurePlayer(3) == 3)
                                                                pas(3);
                                                            else
                                                            {
                                                                Point BShoot = ShootPoint();
                                                                shoot(BShoot.X, BShoot.Y);
                                                            }
                                                    else
                                                    {
                                                        Point BShoot = ShootPoint();
                                                        shoot(BShoot.X, BShoot.Y);
                                                    }
                                            else
                                                if (x(BallOwner) < GoalDistance)
                                                    run(BallOwner, x(BallOwner) + 5, y(BallOwner));
                                                else
                                                {
                                                    Point BShoot = ShootPoint();
                                                    shoot(BShoot.X, BShoot.Y);
                                                }
                                            break;
                                    }
                                    if (NeedPas() && CanPas() && x(0) < GoalDistance)
                                        FindNearFriend();
                                    else
                                    {
                                        Point S = ShootPoint();
                                        shoot(S.X, S.Y);
                                    }
                                }
                                break;

                            case Status.Corner:
                                StatusQueue.Push(Status.Corner);
                                if (BallOwner > 0)
                                    if (y(0) < 240)
                                        pas(3);
                                    else
                                        pas(4);
                                break;

                            case Status.HandOut:
                                StatusQueue.Push(Status.HandOut);
                                if (BallOwner > 0)
                                    pas(4);
                                break;

                            case Status.Out:
                                StatusQueue.Push(Status.Out);
                                if (BallOwner > 0)
                                    pas(2);
                                break;

                            case Status.Goal:
                                StatusQueue.Push(Status.Goal);
                                break;
                        }
                        break;

                    case 2://strategy 2

                        if (BallOwner < 1)
                            run(5, x(0), y(0));
                        else
                            shoot(480, 240);
                        break;

                    case 3://strategy 3

                        if (BallOwner > 0)//age toop daste ma bashe
                            switch (BallOwner)
                            {
                                case 1:
                                    if (y(0) < 240)
                                        shoot(10, 150);
                                    else
                                        shoot(10, 340);
                                    break;

                                case 2:
                                    if (SecuredPoint(BallOwner, 320, 450))
                                        shoot(320, 450);
                                    else
                                        if (!InDanger(3))
                                            pas(3);
                                        else
                                            if (y(BallOwner) < 240)
                                                shoot(x(BallOwner), 30);
                                            else
                                                shoot(x(BallOwner), 450);
                                    break;

                                case 3:
                                    if (SecuredPoint(BallOwner, 320, 450))
                                        shoot(320, 450);
                                    else
                                        if (!InDanger(2))
                                            pas(2);
                                        else
                                            if (y(BallOwner) < 240)
                                                shoot(x(BallOwner), 30);
                                            else
                                                shoot(x(BallOwner), 450);
                                    break;

                                case 4:
                                    if (SecuredPoint(BallOwner, 320, 450))
                                        shoot(320, 450);
                                    else
                                        if (!InDanger(2))
                                            pas(2);
                                        else
                                            if (y(BallOwner) < 240)
                                                shoot(x(BallOwner), 30);
                                            else
                                                shoot(x(BallOwner), 450);
                                    break;

                                case 5:
                                    if (SecuredPoint(5, 320, 30))
                                        shoot(320, 30);
                                    else
                                        if (!InDanger(3))
                                            pas(3);
                                        else
                                            if (y(5) < 240)
                                                shoot(x(5), 30);
                                            else
                                                shoot(x(5), 450);
                                    break;
                            }
                        else//age daste ma na bood
                        {
                            Goalkeeper(100);
                            //Matin.Darvaze();
                            if (x(0) < 370)
                            {
                                run(5, x(0), y(0));

                                if (BallOwner == -5)
                                    run(4, x(-4), y(-4));
                                else
                                    run(4, x(-5), y(-5));

                                if (x(-3) < 320)
                                    run(3, x(-3), y(-3));
                                else
                                    run(3, x(-5), y(-5));

                                if (x(0) < 200)
                                    run(2, x(0), y(0));
                                else
                                    run(2, x(-4), y(-4));
                            }
                            else
                            {
                                run(5, 270, y(5));
                                run(4, 270, y(4));
                                run(3, 200, y(3));
                                run(2, 200, y(2));
                            }
                        }
                        break;
                }
            }
            public static void Play()
            {
                switch (CurrentGameStatus)
                {
                    case Status.Play:
                        if (x(0) < 320 || TimeSpent < 50)
                        {
                            if (BallOwner > 0)
                            {
                                Point BallPoint = new Point(x(0), y(0));
                                for (int i = 2; i < 6; i++)
                                {
                                    Point PlayerPoint = new Point(x(i), y(i));
                                    PasPlayer(PlayerPoint, BallPoint, i);
                                }
                            }
                            else
                            {
                                run(5, 220, 110);
                                run(4, 220, 370);
                                run(3, 150, 200);
                                run(2, 150, 280);
                            }
                            if (y(1) > 180 && y(1) < 300)
                                Goalkeeper(100);
                            else
                                run(1, 50, 240);
                        }
                        else
                            if (BallOwner > 0)
                                switch (BallOwner)
                                {
                                    case 2:
                                        pas(4);
                                        break;
                                    case 3:
                                        pas(5);
                                        break;
                                    case 4:
                                        shoot(625, 100);
                                        break;
                                    case 5:
                                        shoot(625, 380);
                                        break;
                                    case 1:
                                        if (y(-5) < 240)
                                            pas(3);
                                        else
                                            pas(2);
                                        break;
                                }
                        break;

                    case Status.Corner:
                        if (BallOwner > 0)
                            if (x(0) < 240)
                                shoot(625, 50);
                            else
                                shoot(625, 400);
                        break;

                    case Status.HandOut:
                        if (BallOwner > 0)
                            if (y(0) < 240)
                                shoot(x(BallOwner), 10);
                            else
                                shoot(x(BallOwner), 460);
                        break;
                }
            }
        }

        static int OG = 0;
        static int strategy = 0;
        static int start = 0;

        public static void BlueTeam()
        {
            BlueTeamName = "3 Idiots";
            if (OG < GradeOfOpponentTeam || (TimeSpent - start >= 200 && OG == GradeOfOpponentTeam && GradeOfMyTeam <= GradeOfOpponentTeam))
            {
                start = TimeSpent;
                strategy++;
                OG =GradeOfOpponentTeam;
            }
            if (strategy > 1)
                strategy = 0;
            if (GradeOfMyTeam > GradeOfOpponentTeam && TimeSpent > 800)
                strategy = 2;
            if (strategy == 0)
                Matin.play();
            else if (strategy == 1)
                Mohsen.play();
            else
                Alireza.Play(3);
        }

    }
}
