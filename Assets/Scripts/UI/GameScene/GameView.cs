using System.Collections;
using System.Collections.Generic;
using Near;
using Near.Models.Game;
using UnityEngine;
using Event = Near.Models.Game.Event;

namespace UI.GameScene
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private EventMessages eventMessages;
        
        private int _gameId;
        private int _numberOfRenderedEvents;
        private GameData _gameData;
        
        private async void Awake()
        {
            var user = await Near.GameContract.ContractMethods.Views.GetUserInGame();
            if (user != null && user.games[0].winner_index == null)
            {
                _gameId = user.games[0].id;
                //StartCoroutine(GenerateEvent());
                UpdateGameData();
            }
            else
            {
                Debug.Log("Error");
            }
        }

        private IEnumerator GenerateEvent()
        {
            while (_gameData.winner_index != null)
            {
                Debug.Log("1");
                Near.GameContract.ContractMethods.Actions.GenerateEvent(_gameId);

                yield return new WaitForSeconds(1);
            }
        }

        private async void UpdateGameData()
        {
            var filter = new GameDataFilter()
            {
                id = _gameId
            };
            
            _gameData = await Near.GameContract.ContractMethods.Views.GetGame(filter);
            
            while (_gameData.winner_index != null)
            {
                _gameData = await Near.GameContract.ContractMethods.Views.GetGame(filter);
                RenderEvents(_gameData);
            }
        }

        private void RenderEvents(GameData gameData)
        {
            Debug.Log("1");
            if (eventMessages.enabled)
            {
                eventMessages.RenderMessages(gameData.events);
            }
        }
    }
}