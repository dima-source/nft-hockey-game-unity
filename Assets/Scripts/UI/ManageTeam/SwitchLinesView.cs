using System.Collections.Generic;
using System.Linq;
using UI.Scripts;
using UnityEngine;

namespace UI.ManageTeam
{
    public class SwitchLinesView: UiComponent
    {
        [SerializeField] private List<SwitchLineButton> buttons;
        [SerializeField] private TeamView teamView;
        
        protected override void Initialize()
        {
            buttons = GetComponentsInChildren<SwitchLineButton>(true).ToList();
            //teamView = Scripts.Utils.FindParent<TeamView>(transform, "Team");
            SetButtonsCallbacks();
        }

        private void SetButtonsCallbacks()
        {
            foreach (var switchLineButton in buttons)
            {
                switchLineButton.ClearCallbacks();
                var lineToShow = switchLineButton.lineToShow;
                switchLineButton.SetCallback(() =>
                {
                    teamView.HideCurrentFive();
                    teamView.ShowLine(lineToShow);
                });
            }
        }
    }
}