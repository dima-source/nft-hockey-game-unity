using System.Collections.Generic;
using Near.Models.Tokens;
using UI.Scripts;
using UI.Scripts.Card;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Main_menu.UIPopups
{
    public class FirstEntryPopupAnimation : UiComponent
    {
        public Animation anim;
        
        private List<CardInPackUI> _cards;

        protected override void Initialize()
        {
            for (int i = 1; i <= 6; i++)
            {
                CardInPackUI card = UI.Scripts.Utils.FindChild<CardInPackUI>(transform, $"Card{i}");
                _cards.Add(card);
            }
        }

        public async void LoadCardsFromPack()
        {
            List<Token> tokens = await Near.MarketplaceContract.ContractMethods.Actions.RegisterAccount();
            
            if (tokens.Count == 0) LoadCardsFromPack();

            var tokensToDisplay = GetSixCards(tokens);
            
            for (int i = 0; i < tokens.Count; i++)
            {
                _cards[i].SetData(tokensToDisplay[i]);
            }
        }

        private List<Token> GetSixCards(List<Token> tokens)
        {
            var result = new List<Token>();
            int numberOfGoalies = 0;
            int numberOfFieldPlayers = 0;
            foreach (var token in tokens)
            {
                var playerType = token.player_type;
                if (playerType == "FieldPlayer" && numberOfFieldPlayers < 4)
                {
                    result.Add(token);
                    numberOfFieldPlayers++;
                } else if (playerType == "Goalie" && numberOfGoalies < 2)
                {
                    result.Add(token);
                    numberOfGoalies++;
                }
            }

            return result;
        }
        
        private void Awake()
        {
            // var isPlaying = anim.Play();
            //Debug.Log(isPlaying.ToString());
        }

        public void OnTrigger()
        {
            anim.Stop();
        }

        public void OnClick()
        {
        }
        public void GoManageTeam()
        {
            SceneManager.LoadScene("ManageTeam");
        }
    }
}