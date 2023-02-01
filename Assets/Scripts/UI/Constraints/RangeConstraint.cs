using System;

namespace UI.Scripts.Constraints
{
    public class RangeConstraint<T> : Constraint<T> where T : IComparable
    {

        private T lower;
        private T upper;
        
        public static RangeConstraint<T> FromBounds(T lower, T upper)
        {
            return new(lower, upper);
        }

        private RangeConstraint(T lower, T upper)
        {
            this.lower = lower;
            this.upper = upper;
            ValidateBounds();
        }
        
        private void ValidateBounds()
        {
            if (!IsBoundsRight())
            {
                ExceptWrongBounds();
            }
        }

        private bool IsBoundsRight()
        {
            return upper.CompareTo(lower) > 0;
        }

        private void ExceptWrongBounds()
        {
            throw new ApplicationException($"Bounds lower: '{lower}' and upper: '{upper}' are wrong.");
        }

        public bool IsValueSuitable(T value)
        {
            return value.CompareTo(lower) >= 0 && value.CompareTo(upper) < 0;
        }
        
    }
}