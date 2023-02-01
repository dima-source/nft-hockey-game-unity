using System.Collections.Generic;
using Near.Models.Tokens;
using UI.Scripts.Card;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Scripts
{
    public class PackAnimation : UiComponent
    {
        private List<CardView> _cards;
        private Animation _animation;

        protected override void Initialize()
        {
            _cards = new List<CardView>();
            _animation = UiUtils.FindChild<Animation>(transform, "MainArea");
            
            for (int i = 1; i <= 3; i++)
            {
                var card = UiUtils.FindChild<CardView>(transform, "Card" + i + "MP");
                _cards.Add(card);
            }
        }

        public void SetData(List<Token> tokens)
        {
            if (tokens.Count != _cards.Count) return;

            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].SetData(tokens[i]);
            }
        }
        public void GoManageTeam()
        {
            SceneManager.LoadScene("ManageTeam");
        }
        public void GoMarketPlace()
        {
            SceneManager.LoadScene("Marketplace");
        }

        public void Play()
        {
            _animation.Play();
        }
    }
}