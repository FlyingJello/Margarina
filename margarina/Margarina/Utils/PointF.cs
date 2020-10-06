using System;

namespace Margarina.Utils
{
    public class PointF
    {
        public double X { get; }
        public double Y { get; }

        public PointF(double x, double y)
        {
            X = x;
            Y = y;
        }

        public PointF(Point point)
        {
            X = point.X;
            Y = point.Y;
        }

        protected bool Equals(PointF other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PointF) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
