using System.Collections.Generic;
using System.Threading.Tasks;
using Near.Models.Tokens;
using UI.Scripts;
using UI.Scripts.Card;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Main_menu.UIPopups
{
    public class FirstEntryPopupAnimation : UiComponent
    {
        private List<CardView> _cards;
        public Animation Animation;

        protected override void Initialize()
        {
            _cards = new List<CardView>();
            
            for (int i = 1; i <= 6; i++)
            {
                CardView card = UiUtils.FindChild<CardView>(transform, $"Card{i}");
                _cards.Add(card);
            }

            LoadCards();
        }

        private async void LoadCards()
        {
            string path = Configurations.PrefabsFolderPath + "MainMenu/LoadingPopup";

            GameObject prefab = UiUtils.LoadResource<GameObject>(path);
            var loadingPopup = Instantiate(prefab, transform.parent);
            
            await LoadCardsFromPack();
            Destroy(loadingPopup);
        }
        
        private async Task LoadCardsFromPack()
        {
            List<Token> tokens = await Near.MarketplaceContract.ContractMethods.Actions.RegisterAccount();
            
            if (tokens.Count == 0) await LoadCardsFromPack();

            var tokensToDisplay = GetSixCards(tokens);
            
            for (int i = 0; i < tokensToDisplay.Count; i++)
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
        
        public void GoManageTeam()
        {
            SceneManager.LoadScene("ManageTeam");
        }

        public void OnClick()
        {
            Animation.Play();
        }
    }
}