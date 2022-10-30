using System.Threading.Tasks;
using UI.Profile.Models;

namespace UI.Profile
{
    public interface ILogoLoader
    {
        Task<TeamLogo> LoadLogo();
    }
}