using System.Collections.Generic;
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

        public void ShowLine(LineNumbers lineNumber)
        {
            teamFivesContent.ShowFive(lineNumber);
        }
        
        
    }
}