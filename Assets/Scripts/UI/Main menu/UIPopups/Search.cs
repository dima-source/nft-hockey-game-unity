using System.Collections;
using Near;
using Runtime;
using TMPro;
using UnityEngine;

namespace UI.Main_menu.UIPopups
{
    public class Search : MonoBehaviour
    {
        [SerializeField] private Transform requestButtons;
        [SerializeField] private Loading loading;
        [SerializeField] private Timer timer;
        [SerializeField] private TextMeshProUGUI bidText;
        
        public void SetBidText(string bid)
        {
            bidText.text = bid;
        }
        
        private bool _isWaitForOpponent;
        
        private void OnEnable()
        {
            _isWaitForOpponent = true;
            
            timer.gameObject.SetActive(true);

            StartCoroutine(WaitForOpponent());
        }

        private IEnumerator WaitForOpponent()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);

                if (!_isWaitForOpponent)
                {
                    yield break;
                }
                
                CheckGame();
            }
        }

        private async void CheckGame()
        {
            var user = await Near.GameContract.ContractMethods.Views.GetUserInGame();

            if (user == null || user.games[0].winner_index != null) return;
            
            _isWaitForOpponent = false;
            Game.LoadGame();
        }

        public async void MakeUnavailable()
        {
            gameObject.SetActive(false);
            loading.gameObject.SetActive(true);
            
            bool result = await Near.GameContract.ContractMethods.Actions.MakeUnavailable(bidText.text);

            if (result)
            {
                _isWaitForOpponent = false;
            
                timer.gameObject.SetActive(false);
                
                requestButtons.gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(true);
            }
            
            loading.gameObject.SetActive(false);
        }
    }
}