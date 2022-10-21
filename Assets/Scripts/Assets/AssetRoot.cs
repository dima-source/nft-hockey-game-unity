using UI.GameScene;
using UI.Main_menu;
using UI.ManageTeam;
using UnityEngine;

namespace Assets
{
    [CreateAssetMenu(menuName = "Assets/Asset Root", fileName = "Asset Root")]
    public class AssetRoot : ScriptableObject
    {
        public GameAsset gameAsset;
        public MainMenuAsset mainMenuAsset;
        public ManageTeamAsset manageTeamAsset;
    }
}