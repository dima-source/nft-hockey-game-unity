using System.Threading.Tasks;
using Newtonsoft.Json;
using UI.Profile.Models;
using UnityEngine;

namespace UI.Profile
{
    public class ConsoleLogoSaver: ILogoSaver
    {
        public Task<bool> SaveLogo(string logoString)
        {
            Debug.Log("Saved: " + logoString);
            return Task.FromResult(true);
        }

        public Task<bool> SaveLogo(TeamLogo logoData)
        {
            return SaveLogo(JsonConvert.SerializeObject(logoData));
        }
    }
}