using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

namespace UI.Scripts.Constraints
{
    public class SetConstraint<T> : Constraint<T> where T : IEquatable<T>
    {

        private readonly T[] values;
        private HashSet<T> set;


        public static SetConstraint<T> FromValues(params T[] values)
        {
            return new(values);
        }
        
        private SetConstraint(params T[] values)
        {
            this.values = values;
            ValidateSetValues();
            InitializeSet();
        }

        private void ValidateSetValues()
        {
            if (!IsSetValuesRight())
            {
                ExceptWrongSetValues();
            }
        }

        private bool IsSetValuesRight()
        {
            return values.Length == values.Distinct().Count();
        }
        
        private void ExceptWrongSetValues()
        {
            throw new ApplicationException("Values are wrong");
        }

        private void InitializeSet()
        {
            set = new HashSet<T>();
            set.AddRange(values);
        }

        public bool IsValueSuitable(T value)
        {
            return set.Contains(value);
        }
    }
}