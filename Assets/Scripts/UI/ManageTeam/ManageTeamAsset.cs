using UI.ManageTeam.DragAndDrop;
using UnityEngine;

namespace UI.ManageTeam
{
    [CreateAssetMenu(menuName = "Assets/Asset ManageTeam", fileName = "Asset ManageTeam")]
    public class ManageTeamAsset : ScriptableObject
    {
        public DraggableCard fieldCard;
        public DraggableCard goalie;
        public UISlot uiSlot;
    }
}