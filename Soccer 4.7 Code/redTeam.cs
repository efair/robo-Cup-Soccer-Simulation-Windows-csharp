using System;
using System.Windows.Forms;

namespace Robocup
{
    class redTeam : Soccer
    {
        public redTeam(Form form)
            : base(form)
        {
        }


        /*_____________________SHOOT_____________________*/
        static bool shoot(int ply)
        {
            for (int i = 180; i <= 230; i += 10)
            {
                if (SecuredPoint(ply, 620, i, 20) && Distance(ply, 620, i) <= 250)
                {
                    shoot(625, i);
                    return true;
                }
            }
            return false;
        }
        static bool dribble(int player, int ang, int yd)
        {
            ang = 100;
            yd = 240;
            if (x(NrOppToBall()) < x(player) - 5)
            {
                shoot(x(player) + 25, (y(player) + 4 * yd) / 5);
                return true;
            }
            if (x(NrOppToBall()) < x(player) - 20 && Math.Abs(y(NrOppToBall()) - y(player)) > 30)
            {
                shoot(x(player) + 25, (y(player) + 4 * yd) / 5);
                return true;
            }

            if (x(player) > 450)
                return false;

            if (SecuredPoint(player, x(player) + 40, (y(player) + 4 * yd) / 5, ang))
            {
                shoot(x(player) + 30, (y(player) + 4 * yd) / 5);
                return true;
            }
            return false;
        }
        static int NrToBall()
        {
            int near = (Distance(4, x(0), y(0)) < Distance(5, x(0), y(0)) ? 4 : 5);

            for (int i = 4; i >= 1; i--)
                if (Distance(i, x(0), y(0)) < near)
                    near = i;

            return near;
        }
        static int NrOppToBall()
        {
            int near = (Distance(-1, x(0), y(0)) < Distance(-2, x(0), y(0)) ? -1 : -2);

            for (int i = -3; i >= -5; i--)
                if (Distance(i, x(0), y(0)) < near)
                    near = i;

            return near;
        }
        static int Distance(int Player, int X, int Y)
        {
            return (int)Math.Sqrt(Math.Pow(x(Player) - X, 2) + Math.Pow(y(Player) - Y, 2));
        }
        static int Angle(int Player, int X, int Y)
        {
            return (int)(-Math.Atan2(Y - y(Player), X - x(Player)) * 180 / Math.PI);
        }
        static bool NotSafe(int Player)
        {
            for (int i = -1; i >= -5; i--)
                if (Distance(Player, x(i), y(i)) < 50)
                    return true;
            return false;
        }
        static bool InDanger(int Player)
        {
            for (int i = -1; i >= -5; i--)
                if (Distance(Player, x(i), y(i)) < 30)
                    return true;
            return false;
        }
        static bool SecuredPoint(int Player, int X, int Y, int ang)
        {
            ang = 25;
            int d1, d = Distance(Player, X, Y);
            int a1, a = Angle(Player, X, Y);
            for (int i = -1; i >= -5; i--)
            {
                d1 = Distance(Player, x(i), y(i));
                if (d1 < d)
                {
                    a1 = Angle(Player, x(i), y(i)) - a;

                    if (Math.Abs(a1) < ang)
                        return false;
                }
            }
            return true;
        }
        static int lastBallPosX = 0;
        static int lastBallPosY = 0;


        public static void RedTeam()
        {
            RedTeamName = "تیم قرمز";
            Say(Hero+"b");
            RedTeamName = "نرگس";

            if (Soccer.CurrentGameStatus == Status.Corner && BallOwner == 5)
            {
                pas(3);
            }
            else if (CurrentGameStatus == Status.HandOut && BallOwner == 5)
            {
                pas(3);
            }
            else
            {

                int ballSpeed = (int)(Math.Sqrt((x(0) - lastBallPosX) * (x(BallOwner) - lastBallPosX) + (y(BallOwner) - lastBallPosY) * (y(BallOwner) - lastBallPosY)));
                run(1, 70, (2 * 240 + y(0)) / 3);
                run(2, 150, 280);
                run(3, 150, 180);
                run(4, 400, 350);
                run(5, 400, 130);

                if (BallOwner< 1)
                {

                    if (x(0) > 250)
                    {
                        if (y(0) < 240)
                        {
                            run(5, x(0), y(0));
                            run(4, x(0), y(0));
                        }
                        else
                        {
                            run(4, x(0), y(0));
                            run(5, x(0), y(0));
                        }

                        if ((y(0) < 240) && (x(0) > 450))
                        {
                            run(5, x(0), y(0));
                            run(4, x(0) + 20, y(0) + 50);
                        }
                        else if ((y(0) > 240) && (x(0) > 450))
                        {
                            run(4, x(0), y(0));
                            run(5, x(0) + 20, y(0) - 50);
                        }
                    }

                    else if (x(0) > 150)
                        if (y(0) < 240)
                        {
                            run(3, x(0), y(0));
                            run(2, x(0), 340);
                        }
                        else
                        {
                            run(2, x(0), y(0));
                            run(3, x(0), 140);
                        }
                    else
                        run(1, x(0), y(0));

                    if (BallOwner == 0 && ballSpeed < 1)
                    {
                        run(NrToBall(), x(0), y(0));
                    }
                    else if (ballSpeed < 0)
                    {
                        run(NrToBall(), x(0), y(0));
                    }
                }

                else if (BallOwner== 1)
                    if (!NotSafe(1))
                        run(1, 620, 240);
                    else if (SecuredPoint(1, x(2), y(2), 80))
                        pas(2);
                    else if (SecuredPoint(1, x(3), y(3), 80))
                        pas(3);
                    else if (SecuredPoint(1, x(4), y(4), 80))
                        pas(4);
                    else if (SecuredPoint(1, x(5), y(5), 80))
                        pas(5);
                    else if (SecuredPoint(1, 10, 40, 80))
                        shoot(x(0), 30);
                    else
                        shoot(x(0), 450);

                else if (BallOwner== 2)
                    if (!NotSafe(2))
                        run(2, 620, 240);
                    else if (SecuredPoint(2, x(4), y(4), 80))
                        pas(4);
                    else if (SecuredPoint(2, x(5), y(5), 80))
                        pas(5);
                    else if (SecuredPoint(2, x(3), y(3), 80))
                        pas(3);
                    else
                        shoot(630, 240);

                else if (BallOwner== 3)
                    if (!NotSafe(3))
                        run(3, 620, 240);
                    else if (SecuredPoint(3, x(5), y(5), 80))
                        pas(5);
                    else if (SecuredPoint(3, x(4), y(4), 80))
                        pas(4);
                    else if (SecuredPoint(3, x(2), y(2), 80))
                        pas(2);
                    else
                        shoot(630, 240);

                else if (BallOwner== 4)
                    if (dribble(4, 100, 280))
                    {
                        run(5, x(4) + 60, 220);
                    }
                    else if (!NotSafe(4))
                    {
                        run(4, 620, 240);
                        run(5, x(4) + 30, (y(-2) + y(-3)) / 2);
                    }
                    else if (x(BallOwner) < 500)
                        if (!InDanger(4))
                        {
                            run(4, 580, 340);
                            run(5, 580, 140);
                        }
                        else
                            if (SecuredPoint(4, x(5), y(5), 80) && !InDanger(5))
                                pas(5);
                            else
                                shoot(x(4) + 20, y(4));

                    else
                        if (SecuredPoint(4, 630, 280, 80))
                            shoot(630, 280);
                        else if (SecuredPoint(4, 630, 200, 80))
                            shoot(630, 200);
                        else if (SecuredPoint(4, 630, 240, 80))
                            shoot(630, 240);
                        else if (SecuredPoint(4, x(5), y(5), 80) && !InDanger(5))
                            pas(5);
                        else
                            run(4, 600, 280);

                else
                    if (dribble(5, 100, 200))
                    {
                        run(4, x(5) + 60, 260);
                    }
                    else if (!NotSafe(5))
                    {
                        run(4, x(5) + 30, (y(-2) + y(-3)) / 2);
                        run(5, 620, 240);
                    }
                    else if (x(BallOwner) < 500)
                        if (!InDanger(5))
                        {
                            run(5, 580, 140);
                            run(4, 580, 340);
                        }
                        else
                            if (SecuredPoint(5, x(4), y(4), 80) && !InDanger(4))
                                pas(4);
                            else
                                shoot(x(5) + 20, y(5));

                    else
                        if (SecuredPoint(5, 630, 200, 80))
                            shoot(630, 200);
                        else if (SecuredPoint(5, 630, 280, 80))
                            shoot(630, 280);
                        else if (SecuredPoint(5, 630, 240, 80))
                            shoot(630, 240);
                        else if (SecuredPoint(5, x(4), y(4), 20) && !InDanger(4))
                            pas(4);
                        else
                            run(5, 600, 200);





            }
            lastBallPosX = x(0);
            lastBallPosY = y(0);
        }

    }

}
