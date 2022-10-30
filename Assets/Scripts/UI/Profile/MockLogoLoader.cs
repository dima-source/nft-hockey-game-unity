using System.Threading.Tasks;
using UI.Profile.Models;

namespace UI.Profile
{
    public class MockLogoLoader: ILogoLoader
    {
        public Task<TeamLogo> LoadLogo()
        {
            return Task.FromResult(new TeamLogo()
            {
                form_name = "ShieldLine",
                pattern_name = "5",
                first_layer_color_number = "3",
                second_layer_color_number = "1"
            });
        }
    }
}