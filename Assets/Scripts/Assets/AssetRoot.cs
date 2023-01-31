using GameScene;
using UI.Main_menu;
using UI.ManageTeam;
using UI.Profile;
using UnityEngine;

namespace Assets
{
    [CreateAssetMenu(menuName = "Assets/Asset Root", fileName = "Asset Root")]
    public class AssetRoot : ScriptableObject
    {
        public ManageTeamAsset manageTeamAsset;
    }
}