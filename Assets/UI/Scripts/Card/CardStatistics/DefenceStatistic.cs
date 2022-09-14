using UnityEngine;

namespace UI.Scripts.Card.CardStatistics
{
    public class DefenceStatistic : CardStatistic
    {
        public DefenceStatistic(int characteristic) : base(characteristic)
        {
            
        }

        protected override string SpriteName => "Shield";
    }
}