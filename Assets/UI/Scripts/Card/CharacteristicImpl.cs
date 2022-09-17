using System;
using UI.Scripts.Constraints;

namespace UI.Scripts.Card
{
    public class CharacteristicImpl<T> : Characteristic<T>, IComparable where T : IComparable
    {
        protected T characteristic;
        private readonly Constraint<T> constraint;

        protected CharacteristicImpl(T characteristic, Constraint<T> constraint)
        {
            this.characteristic = characteristic;
            this.constraint = constraint;
            ApplyConstraint();
        }
        
        public T GetValue()
        {
            return characteristic;
        }
        
        public void ApplyConstraint()
        {
            constraint.GuaranteeValue(characteristic);
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            CharacteristicImpl<T> other = obj as CharacteristicImpl<T>;
            if (other == null)
            {
                throw new ArgumentException("Object is not a CharacteristicImpl");
            }
            
            return characteristic.CompareTo(other.characteristic);   
        }
    }
}