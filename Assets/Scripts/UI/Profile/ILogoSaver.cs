using System.Threading.Tasks;
using UI.Profile.Models;

namespace UI.Profile
{
    public interface ILogoSaver
    {
        Task<bool> SaveLogo(string logoString);
        Task<bool>SaveLogo(TeamLogo logo);
    }
}