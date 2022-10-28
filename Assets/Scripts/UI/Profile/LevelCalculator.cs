using UI.Profile.Models;
using UnityEngine;

namespace UI.Profile
{
    public class LevelCalculator
    {
        private RewardsUser _rewardsUser;
        public LevelCalculator(RewardsUser rewardsUser)
        {
            _rewardsUser = rewardsUser;
        }

        public string GetLevelString()
        {
            if (_rewardsUser == null)
            {
                return "0";
            }
            return _rewardsUser.points switch
            {
                (<= 100) => "1",
                (> 100 and <= 200) => "2",
                (> 200 and <= 500) => "3",
                (> 500 and <= 1000) => "4",
                (> 1000 and <= 1500) => "5",
                (> 1500 and <= 2000) => "6",
                (> 2000 and <= 5000) => "7",
                (> 5000) => "Master"
            };
        }

        public float GetLevelProgress()
        {
            if (_rewardsUser == null)
            {
                return 0f;
            }
            return _rewardsUser.points switch
            {
                (< 100) => _rewardsUser.points / 100f,
                (>= 100 and < 200) => (_rewardsUser.points - 100f) / 100f,
                (>= 200 and < 500) => (_rewardsUser.points - 200f) / 300f,
                (>= 500 and < 1000) => (_rewardsUser.points - 500f) / 500f,
                (>= 1000 and < 1500) => (_rewardsUser.points - 1000f) / 500f,
                (>= 1500 and < 2000) => (_rewardsUser.points - 1500f) / 500f,
                (>= 2000 and < 5000) => (_rewardsUser.points - 2000f) / 3000f,
                (>= 5000) => 1f
            };
            
        }
    }
}