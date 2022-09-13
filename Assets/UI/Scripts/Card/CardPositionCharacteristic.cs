using System;
using UI.Scripts.Constraints;

namespace UI.Scripts.Card
{
    public class CardPositionCharacteristic : CharacteristicImpl<string>
    {
        public CardPositionCharacteristic(string characteristic) : base(characteristic,
            SetConstraint<string>.FromValues(
                "RW", "LW",
                "RD", "LD", 
                "G", "C"))
        {
            
        }

        public override string ToString()
        {
            return characteristic;
        }
    }
}