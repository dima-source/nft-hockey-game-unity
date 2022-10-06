using System;
using UI.Scripts;

namespace Near.Models.Tokens.Filters.ToggleFilters
{
    public class ToggleFilterAge : IToggleFilter
    {
        public void AddToPlayerFilter(PlayerFilter playerFilter, Toggle[] toggles)
        {
            playerFilter.birthday_lte = GetBirthdayInUnix(0);
 
            foreach (Toggle toggle in toggles)
            {
                if (!toggle.isOn || toggle.text == "Some toggle")
                {
                    continue;
                }
                
                string[] range = toggle.text.Split("-");
                if (range.Length == 2)
                {
                    long minAge = GetBirthdayInUnix(int.Parse(range[0]) - 1);
                    long maxAge = GetBirthdayInUnix(int.Parse(range[1]));

                    playerFilter.birthday_lte = minAge;
                    playerFilter.birthday_gte = maxAge;
                }
                else
                {
                    playerFilter.birthday_lte = GetBirthdayInUnix(34);
                }
                
                return;
            }
        }

        private long GetBirthdayInUnix(int age)
        {
            DateTime currentTime = DateTime.Now;

            DateTime minAge = new DateTime(currentTime.Year - age, 
                currentTime.Month,
                currentTime.Day);
            
            return ((DateTimeOffset)minAge).ToUnixTimeSeconds();
        }
    }
}