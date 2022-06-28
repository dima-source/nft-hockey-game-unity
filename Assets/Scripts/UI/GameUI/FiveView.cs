using System.Collections.Generic;
using UI.ManageTeam.Buttons;
using UI.ManageTeam.DragAndDrop;
using UnityEngine;
using Event = Near.Models.Game.Event;

namespace UI.GameUI
{
    public class FiveView : MonoBehaviour
    {
        [SerializeField] private List<LineButton> buttons;
        [SerializeField] private FieldPlayerCard leftWing;
        [SerializeField] private FieldPlayerCard center;
        [SerializeField] private FieldPlayerCard rightWing;

        [SerializeField] private FieldPlayerCard leftDefender;
        [SerializeField] private FieldPlayerCard rightDefender;

        [SerializeField] private GoalieCard goalie;

        private int numberFive;

        public void UpdateStats(Event data)
        {
            switch (data.my_team.five.number)
            {
                case "First":
                    buttons[0].ChangeActiveButton();
                    break;
                case "Second":
                    buttons[1].ChangeActiveButton();
                    break;
                case "Third":
                    buttons[2].ChangeActiveButton();
                    break;
                case "Fourth":
                    buttons[3].ChangeActiveButton();
                    break;
            }
            
            leftWing.SetData(data.my_team.five.field_players["LeftWing"]);
            rightWing.SetData(data.my_team.five.field_players["RightWing"]);
            center.SetData(data.my_team.five.field_players["Center"]);
            leftDefender.SetData(data.my_team.five.field_players["LeftDefender"]);
            rightDefender.SetData(data.my_team.five.field_players["RightDefender"]);
            goalie.SetData(data.my_team.goalie);
        }
    }
}