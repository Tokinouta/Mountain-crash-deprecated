using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class LinearBarrier
    {
        private Point startPoint, endPoint;

        public LinearBarrier(Point start,Point end)
        {
            startPoint = start;
            endPoint = end;
        }

        public LinearBarrier(int startX, int startY, int endX, int endY)
        {
            startPoint.X = startX;
            startPoint.Y = startY;
            endPoint.X = endX;
            endPoint.Y = endY;
        }

        public LinearBarrier(int Temp, double switchL, Player players, GroupBox BattleField)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            bool flag = false;
            int control = random.Next() % 4;
            do
            {
                int controlL = 0;
                flag = false;
                startPoint.X = random.Next(BattleField.Width);
                startPoint.Y = random.Next(BattleField.Height);
                controlL = Convert.ToInt32((50 + random.Next(150)) * Math.Sqrt(5) * switchL + 1);
                int alpha = random.Next(-90, 90);
                if (startPoint.X + controlL * Math.Cos(alpha * Math.PI / 180) > BattleField.Width ||
                    startPoint.Y + controlL * Math.Sin(alpha * Math.PI / 180) > BattleField.Height ||
                    startPoint.Y + controlL * Math.Sin(alpha * Math.PI / 180) < 0)
                {
                    flag = true;
                }
                else
                {
                    endPoint.X = Convert.ToInt32(startPoint.X + controlL * Math.Cos(alpha * Math.PI / 180));
                    endPoint.Y = Convert.ToInt32(startPoint.Y + controlL * Math.Sin(alpha * Math.PI / 180));
                }
            } while (flag);
        }

        public Point StartPoint
        {
            get { return startPoint; }
            set { startPoint = value; }
        }
        public Point EndPoint
        {
            get { return endPoint; }
            set { endPoint = value; }
        }

        public virtual bool IsCollapsed(Point a)
        {
            if ((a.X - StartPoint.X) * (EndPoint.Y - StartPoint.Y) == (a.Y - StartPoint.Y) * (EndPoint.X - StartPoint.X))
                return true;
            else
                return false;
        }
    }

    public class Mountain : LinearBarrier
    {
        public Mountain(Point start, Point end) : base(start, end) { }
        public Mountain(int startX, int startY, int endX, int endY) : base(startX, startY, endX, endY) { }
        public Mountain(int Temp, double switchL, Player players, GroupBox BattleField) :
            base(Temp, switchL, players, BattleField) { }
        public override bool IsCollapsed(Point a)
        {
            if ((a.X - StartPoint.X) * (EndPoint.Y - StartPoint.Y) == (a.Y - StartPoint.Y) * (EndPoint.X - StartPoint.X))
                return true;
            else
                return false;
        }
    }

    public class River : LinearBarrier
    {
        public River(Point start, Point end) : base(start, end) { }
        public River(int startX, int startY, int endX, int endY) : base(startX, startY, endX, endY) { }
        public River(int Temp, double switchL, Player players, GroupBox BattleField) :
            base(Temp, switchL, players, BattleField) { }

        public override bool IsCollapsed(Point a)
        {
            if ((a.X - StartPoint.X) * (EndPoint.Y - StartPoint.Y) == (a.Y - StartPoint.Y) * (EndPoint.X - StartPoint.X))
                return true;
            else
                return false;
        }
    }

    public class RectangleBarrier
    {
        private int top, left, height, width;

        public RectangleBarrier(int topArgument, int leftArgument, int heightArgument, int widthArgument)
        {
            top = topArgument;
            left = leftArgument;
            height = heightArgument;
            width = widthArgument;
        }

        public RectangleBarrier(int heightArgument, int widthArgument, GroupBox BattleField)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            height = heightArgument;
            width = widthArgument;
            left = random.Next(BattleField.Width - Width + 1);
            top = random.Next(BattleField.Height - Height + 1);
        }
        public int Top { get => top; set => top = value; }
        public int Left { get => left; set => left = value; }
        public int Height { get => height; set => height = value; }
        public int Width { get => width; set => width = value; }

    }

    public class Clinic : RectangleBarrier
    {
        public Clinic(int topArgument, int leftArgument, int heightArgument, int widthArgument)
            : base(topArgument, leftArgument, heightArgument, widthArgument) { }
        public Clinic(int heightArgument, int widthArgument, GroupBox BattleField)
            : base(heightArgument, widthArgument, BattleField) { }
    }

    public class Pit : RectangleBarrier
    {
        public Pit(int topArgument, int leftArgument, int heightArgument, int widthArgument)
            : base(topArgument, leftArgument, heightArgument, widthArgument) { }
        public Pit(int heightArgument, int widthArgument, GroupBox BattleField)
            : base(heightArgument, widthArgument, BattleField) { }

    }
}
