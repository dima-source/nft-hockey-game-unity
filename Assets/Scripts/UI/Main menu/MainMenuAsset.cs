using UI.Main_menu.UIPopups;
using UnityEngine;

namespace UI.Main_menu
{
    
    [CreateAssetMenu(menuName = "Assets/Asset Main menu", fileName = "Asset Main menu")]
    public class MainMenuAsset : ScriptableObject
    {
        public FriendItem friendItem;
        public CardInPackUI cardInPackUI;
    }
}