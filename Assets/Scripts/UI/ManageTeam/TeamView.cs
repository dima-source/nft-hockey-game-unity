using System.Collections.Generic;
using UI.ManageTeam.DragAndDrop;
using UI.Scripts;
using Unity.VisualScripting;
using UnityEngine;

namespace UI.ManageTeam
{
    public class TeamView: UiComponent
    {
        [DoNotSerialize] public readonly Dictionary<LineNumbers, Dictionary<SlotPositionEnum, UISlot>> Fives = new();
        [SerializeField] private Transform switchBrigadesContent;
        [SerializeField] private Transform switchLinesContent;
        [SerializeField] private TeamFivesView teamFivesContent;
        [SerializeField] private Transform goaliesContent;

        protected override void Initialize()
        {
            switchBrigadesContent = Scripts.Utils.FindChild<Transform>(transform, "SwitchBrigades");
            switchLinesContent = Scripts.Utils.FindChild<Transform>(transform, "SwitchLine");
            teamFivesContent = Scripts.Utils.FindChild<TeamFivesView>(transform, "TeamFive");
            goaliesContent = Scripts.Utils.FindChild<Transform>(transform, "Goalies");
        }

        public void InitFives()
        {
            teamFivesContent.InitFives();
        }

        public void ShowLine(LineNumbers lineNumber)
        {
            teamFivesContent.ShowFive(lineNumber);
        }
        
        
    }
}