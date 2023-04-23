using System.Collections.Generic;
using System.Linq;
using Near.Models.Game.TeamIds;
using UI.ManageTeam.DragAndDrop;
using UI.Scripts;
using Unity.VisualScripting;
using UnityEngine;

namespace UI.ManageTeam
{
    public class TeamView: UiComponent
    {
        [DoNotSerialize] public readonly Dictionary<LineNumbers, Dictionary<SlotPositionEnum, UISlot>> Fives = new();
        // [SerializeField] private Transform switchBrigadesContent;
        // [SerializeField] private Transform switchLinesContent;
        [SerializeField] private TeamFivesView teamFivesContent;
        [SerializeField] private GoaliesView goaliesView;
        private LineNumbers _currentLineNumber;

        protected override void Initialize()
        {
            // switchBrigadesContent = Scripts.Utils.FindChild<Transform>(transform, "SwitchBrigades");
            // switchLinesContent = Scripts.Utils.FindChild<Transform>(transform, "SwitchLine");
            teamFivesContent = Scripts.Utils.FindChild<TeamFivesView>(transform, "TeamFive");
            goaliesView = Scripts.Utils.FindChild<GoaliesView>(transform, "Goalies");
        }

        public void InitFives()
        {
            teamFivesContent.InitFives();
        }

        public void InitGoalies()
        {
            goaliesView.InitGoalies();
        }

        public void HideCurrentFive()
        {
            if (_currentLineNumber == LineNumbers.Goalie)
            {
                goaliesView.gameObject.SetActive(false);
                teamFivesContent.gameObject.SetActive(true);
            }

            foreach (var five in Fives.Values)
            {
                five.Values.ToList().ForEach(slot => slot.gameObject.SetActive(false));
            }
            // iceTimePrioritySlider.SetValueWithoutNotify(0f);
            // iceTimePriority.text = "Select ice time priority";
            // tactictsDropdown.SetValueWithoutNotify(0);
        }

        public void ShowLine(LineNumbers lineNumber)
        {
            _currentLineNumber = lineNumber;
            if (lineNumber == LineNumbers.Goalie)
            {
                teamFivesContent.gameObject.SetActive(false);
                goaliesView.gameObject.SetActive(true);
                return;
            }
            goaliesView.gameObject.SetActive(false);
            teamFivesContent.gameObject.SetActive(true);
            teamFivesContent.ShowFive(lineNumber);
        }
    }
}