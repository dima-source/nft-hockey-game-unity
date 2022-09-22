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
                "Offensive Defenceman", "Two-Way Defenceman", 
                "Tough Guy", "Standup", 
                "TryHarder", 
                "DefensiveForward", 
                "DefensiveDefenseman",
                "OffensiveDefenseman", "TwoWayDefenceman", 
                "TwoWay",
                "DefensiveDefenceman", "OffensiveDefenceman",
                "ToughGuy", "Butterfly", "Hybrid")) 
        {
            
        }

        public override string ToString()
        {
            
            return characteristic switch
            {
                "TryHarder" => "Try-Harder", 
                "DefensiveForward" => "Defensive Forward", 
                "DefensiveDefenceman" => "Defensive Defenceman",
                "OffensiveDefenceman" => "Offensive Defenceman", 
                "DefensiveDefenseman" => "Defensive Defenceman",
                "OffensiveDefenseman" => "Offensive Defenceman", 
                "TwoWay" => "Two-Way Defenceman",
                "TwoWayDefenceman" => "Two-Way Defenceman",
                "TwoWayDefenseman" => "Two-Way Defenceman",
                "ToughGuy" => "Tough Guy", 
                _ => characteristic
            };
        }
    }
}