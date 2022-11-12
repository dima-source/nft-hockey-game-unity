using GameScene.UI;
using UnityEngine;

namespace GameScene
{
    [CreateAssetMenu(menuName = "Assets/Asset ManageTeam", fileName = "Asset ManageTeam")]
    public class GameAsset : ScriptableObject
    {
        public ActionMessage actionMessage;
    }
}