using System;
using System.Drawing;
using System.Windows.Forms;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Player
    {
        private static int playerNumber;

        //sx、sy为玩家速度方向，lf为玩家横坐标排序，hp为玩家生命主，lv为玩家战力等级,alive为判断是否幸存标志
        private double combatForceLevel, speedOfX, speedOfY;
        private int horizontalOrder, hitPoint;
        private bool isAlive;

        //score为玩家得分,time为幸存时间,kill为殉职人数,ranka幸存排名,rankb为总分排名,scoret是时间得分,scorek为交战得分,bonus为加分
        private double survivalTime;
        private int score, killNumber, survivalRrank, scoreRank,
            timeScore, attackScore, bonus;

        //fist为玩家划拳状态,game为是否进行划拳
        int fingerGuessState;
        bool isStatemate;

        private Label playerLabel;

        public Player(GroupBox BattleField)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            speedOfX = random.Next(-10, 10);
            speedOfY = random.Next(-10, 10);
            combatForceLevel = 0;
            horizontalOrder = 0;
            hitPoint = 510;
            isAlive = true;

            survivalTime = 0;
            score = 0;
            killNumber = 0;
            survivalRrank = 0;
            scoreRank = 0;
            timeScore = 0;
            attackScore = 0;
            bonus = 0;
            fingerGuessState = 0;
            isStatemate = false;
            playerLabel = new Label
            {
                Height = 10,
                Width = 100,
                Top = random.Next(BattleField.Height - 10),
                Left = random.Next(BattleField.Width - 100),
                BackColor = Color.Aquamarine
            };
            BattleField.Controls.Add(playerLabel);
        }

        public Player(int creatingOrder, int combatForceOption, GroupBox BattleField)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            speedOfX = random.Next(-10, 10);
            speedOfY = random.Next(-10, 10);
            horizontalOrder = creatingOrder;
            hitPoint = 510;
            isAlive = true;
            switch (combatForceOption)
            {
                case 0:
                    int temp = random.Next(5);
                    combatForceLevel = temp == 0 ?
                        (random.Next(1, 7) * 2) :
                        (random.Next(1, 7) * 1);
                    break;
                case 1:
                    combatForceLevel =
                        Convert.ToInt32(5 * 10 / Player.PlayerNumber * (Player.PlayerNumber - (creatingOrder + 1))) / 10 + 1;
                    break;
                case 2:
                    double tempd = random.NextDouble(), temp1 = random.NextDouble();
                    combatForceLevel = random.Next(1, 6);
                    if (tempd < 1 / (5 + 5 * (creatingOrder + 1) / (Player.PlayerNumber - 1)))
                    {
                        if (combatForceLevel == 1 && temp1 > ((creatingOrder + 1) + 1) / Player.PlayerNumber)
                        {
                            combatForceLevel = 12;
                        }
                        else
                        {
                            combatForceLevel *= 2;
                        }
                    }
                    else
                    {
                        if (combatForceLevel == 1 && temp1 > ((creatingOrder + 1) + 1) / Player.PlayerNumber)
                        {
                            combatForceLevel = 6;
                        }
                    }
                    break;
                default: break;
            }

            survivalTime = 0;
            score = 0;
            killNumber = 0;
            survivalRrank = 0;
            scoreRank = creatingOrder;
            timeScore = 0;
            attackScore = 0;
            bonus = 0;
            fingerGuessState = 0;
            isStatemate = false;
            playerLabel = new Label
            {
                Height = 10,
                Width = 100,
                Top = random.Next(BattleField.Height - 10),
                Left = random.Next(BattleField.Width - 100),
                BackColor = Color.Aquamarine
            };
            BattleField.Controls.Add(playerLabel);
        }

        public static int PlayerNumber { get => playerNumber; set => playerNumber = value; }

        public double SpeedOfX { get => speedOfX; set => speedOfX = value; }
        public double SpeedOfY { get => speedOfY; set => speedOfY = value; }
        public int HorizontalOrder { get => horizontalOrder; set => horizontalOrder = value; }
        public int HitPoint { get => hitPoint; set => hitPoint = value; }
        public double CombatForceLevel { get => combatForceLevel; set => combatForceLevel = value; }
        public bool IsAlive { get => isAlive; set => isAlive = value; }

        public int Score { get => score; set => score = value; }
        public int KillNumber { get => killNumber; set => killNumber = value; }
        public int SurvivalRrank { get => survivalRrank; set => survivalRrank = value; }
        public int ScoreRank { get => scoreRank; set => scoreRank = value; }
        public int TimeScore { get => timeScore; set => timeScore = value; }
        public int AttackScore { get => attackScore; set => attackScore = value; }
        public int Bonus { get => bonus; set => bonus = value; }
        public double SurvivalTime { get => survivalTime; set => survivalTime = value; }
        public int FingerGuessState { get => fingerGuessState; set => fingerGuessState = value; }
        public bool IsStatemate { get => isStatemate; set => isStatemate = value; }

        public Label PlayerLabel { get => playerLabel; set => playerLabel = value; }
    }

    public class Hat : Player
    {
        // 草帽大叔特权倒计时
        int countDown;
        // 殉职草帽大叔的玩家
        Player playerKilledHat;
        Timer timerCountDown;

        public Hat(GroupBox BattleField) : base(BattleField)
        {
            countDown = 60;
            playerKilledHat = null;
            PlayerLabel.BackColor = Color.Green;
        }

        public Hat(int combatForceOption, GroupBox BattleField) : base(0, combatForceOption, BattleField)
        {
            countDown = 60;
            playerKilledHat = null;
            switch (combatForceOption)
            {
                case 0:
                case 1:
                    CombatForceLevel = 5;
                    break;
                case 2:
                    CombatForceLevel = 5.5;
                    break;
                default:
                    break;
            }
            PlayerLabel.Text = "草帽大叔 " + CombatForceLevel.ToString();
            PlayerLabel.BackColor = Color.Green;
            timerCountDown = new Timer
            {
                Interval = 1000,
                Enabled = false
            };
            timerCountDown.Tick += new EventHandler(timerCountDown_Tick);
        }

        private void timerCountDown_Tick(object sender, EventArgs e)
        {
            countDown = countDown - 1;
            // determine whether playerKilledHat is null
            if (countDown > 0)
            {
                playerKilledHat.CombatForceLevel = 12;
                playerKilledHat.HitPoint = 510;
            }
            else
            {
                timerCountDown.Enabled = false;
                countDown = 60;
                playerKilledHat = null;
            }
        }

        public int CountDown { get => countDown; set => countDown = value; }
        public Player PlayerKilledHat { get => playerKilledHat; set => playerKilledHat = value; }
        public Timer TimerCountDown { get => timerCountDown; set => timerCountDown = value; }
    }

    public class Elf : Player
    {
        // 精灵回血周期
        int periodOfRecharge;
        // 精灵保护的玩家
        Player playerProtectedByElf;
        Timer timerOfRecharge;

        public Elf(GroupBox BattleField) : base(BattleField)
        {
            periodOfRecharge = 60;
            playerProtectedByElf = null;
            PlayerLabel.Text = "精灵 3.5";
            CombatForceLevel = 3.5;
            PlayerLabel.BackColor = Color.Green;
            timerOfRecharge = new Timer
            {
                Interval = 1000,
                Enabled = false
            };
            timerOfRecharge.Tick += new EventHandler(TimerOfRecharge_Tick);
        }

        private void TimerOfRecharge_Tick(object sender, EventArgs e)
        {
            periodOfRecharge = periodOfRecharge - 1;
            if (periodOfRecharge == 0)
            {
                Random random = new Random(Guid.NewGuid().GetHashCode());
                timerOfRecharge.Enabled = false;
                periodOfRecharge = 60;
                PlayerLabel.Visible = true;
                PlayerLabel.Left = 0;
                PlayerLabel.Top = random.Next();
                HitPoint = 510;
                SpeedOfX = random.Next(-10, 10);
                SpeedOfY = random.Next(-10, 10);
                IsAlive = true;
                playerProtectedByElf = null;
            }
            else
            {
                playerProtectedByElf.HitPoint = 510;
            }
        }

        public int PeriodOfRecharge { get => periodOfRecharge; set => periodOfRecharge = value; }
        public Player PlayerProtectedByElf { get => playerProtectedByElf; set => playerProtectedByElf = value; }
        public Timer TimerOfRecharge { get => timerOfRecharge; set => timerOfRecharge = value; }
    }

    public class Egg : Player
    {
        // 弱蛋遁地时刻
        int timeGettingIntoEarth;

        public Egg(GroupBox BattleField) : base(BattleField)
        {
            HitPoint = 100;
            PlayerLabel.BackColor = Color.PaleVioletRed;
            PlayerLabel.Text = "弱蛋 2";
            CombatForceLevel = 2;
        }

        public int TimeGettingIntoEarth { get => timeGettingIntoEarth; set => timeGettingIntoEarth = value; }
    }

    public class Proprieter : Player
    {
        //fist0为社长划拳状态
        int fistProprieter;

        public Proprieter(GroupBox BattleField) : base(BattleField)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            PlayerLabel.BackColor = Color.Green;
            PlayerLabel.Text = "社长 999";
            CombatForceLevel = 999;
            fistProprieter = random.Next() % 3;
        }

        public int FistProprieter { get => fistProprieter; set => fistProprieter = value; }
    }

    public class Ozone : Player
    {
        public Ozone(GroupBox BattleField) : base(BattleField)
        {
            PlayerLabel.BackColor = Color.Green;
        }
        public Ozone(int combatForceOption, GroupBox BattleField) : base(0, combatForceOption, BattleField)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            switch (combatForceOption)
            {
                case 0:
                case 1:
                    CombatForceLevel = random.Next(6) + 0.5;
                    break;
                case 2:
                    CombatForceLevel = random.Next(6) + 1;
                    break;
                default:
                    break;
            }
            PlayerLabel.BackColor = Color.Green;
            PlayerLabel.Text = "臭氧加速器 " + CombatForceLevel.ToString();
        }
    }
}
