using System;
using UI.Scripts.Constraints;

namespace UI.Scripts.Card
{
    public class CardRoleCharacteristic : CharacteristicImpl<string>
    {

        public CardRoleCharacteristic(string characteristic) : base(characteristic,
            SetConstraint<string>.FromValues(
                "Playmaker", "Enforcer",
                "Shooter", "Try-Harder", 
                "Defensive Forward", 
                "Grinder", "Defensive Defenceman",
                "Offensive Defenceman", "Two-Way Defencemen", 
                "Tough Guy", "Standup", 
                "Butterfly", "Hybrid")) 
        {
            
        }

        public override string ToString()
        {
            return characteristic;
        }
    }
}