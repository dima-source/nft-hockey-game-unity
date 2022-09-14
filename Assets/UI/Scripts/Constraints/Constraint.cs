using System;

namespace UI.Scripts.Constraints
{
    public interface Constraint<in T>
    {
        public bool IsValueSuitable(T value);

        public void GuaranteeValue(T value)
        {
            if (!IsValueSuitable(value))
            {
                throw new Exception($"Value {value} is not suitable");
            }
        }
        
    }
}