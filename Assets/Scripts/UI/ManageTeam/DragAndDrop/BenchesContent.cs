using UI.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.ManageTeam.DragAndDrop
{
    public class BenchesContent: UiComponent, IDropHandler
    {
        [SerializeField] private ManageTeamView manageTeamView;


        protected override void Initialize()
        {
            manageTeamView = Scripts.Utils.FindParent<ManageTeamView>(transform, "ManageTeam");
        }

        public void OnDrop(PointerEventData eventData)
        {
            DraggableCard draggableCardDropped = eventData.pointerDrag.GetComponent<DraggableCard>();
            draggableCardDropped.uiSlot.draggableCard = null;
            manageTeamView.CurrentBench.AddPlayer(draggableCardDropped);
            if (manageTeamView.CurrentBench.transform == manageTeamView.fieldPlayersBenchContent.transform)
            {
                manageTeamView.RemoveFieldPlayerFromTeam(draggableCardDropped);
                manageTeamView.ShowStatsChanges(draggableCardDropped);
            }
            else if (manageTeamView.CurrentBench.transform == manageTeamView.powerPlayersBenchContent.transform ||
                     manageTeamView.CurrentBench.transform == manageTeamView.penaltyKillBenchContent.transform)
            {
                manageTeamView.ShowStatsChanges(draggableCardDropped);
            } 
        }
    }
}