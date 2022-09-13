using System;
using UI.Scripts.Constraints;

namespace UI.Scripts.Card
{
    
    public class CardNumberCharacteristic : CharacteristicImpl<int>
    {

        private static readonly int LOW_NUMBER_BOUND = 0;
        private static readonly int UPPER_NUMBER_BOUND = 100;
        
        public CardNumberCharacteristic(int characteristic) : base(characteristic, 
            RangeConstraint<int>.FromBounds(LOW_NUMBER_BOUND, UPPER_NUMBER_BOUND))
        {
            
        }

        public override string ToString()
        {
            return characteristic.ToString();
        }
    }
    
}