using UI.ManageTeam.DragAndDrop;
using UnityEngine;

namespace UI.ManageTeam
{
    [CreateAssetMenu(menuName = "Assets/Asset ManageTeam", fileName = "Asset ManageTeam")]
    public class ManageTeamAsset : ScriptableObject
    {
        public DraggableCard fieldCard;
        public UISlot uiSlot;
    }
}