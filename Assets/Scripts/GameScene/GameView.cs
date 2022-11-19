using System.Collections.Generic;
using System.Threading.Tasks;
using GameScene.UI;
using UnityEngine;
using Event = Near.Models.Game.Event;

namespace GameScene
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private ActionMessages actionMessages;
        [SerializeField] private Field field;
        
        private int _gameId;
        private bool _isGameFinished;
        private int _numberOfRenderedEvents;
        private List<Event> _events;

        private async void Awake()
        {
            return;
            
            var user = await Near.GameContract.ContractMethods.Views.GetUserInGame();
            if (user != null && user.games[0].winner_index == null)
            {
                _gameId = user.games[0].id;
                _events = new List<Event>();

                var generateEventTask = new Task(GenerateEvent);
                generateEventTask.Start();
                
                UpdateGameData();
            }
            else
            {
                Debug.Log("Error");
            }
        }

        public async void GenerateEvent()
        {
            while (!_isGameFinished)
            {
                Debug.Log("Generate event");
                await Near.GameContract.ContractMethods.Actions.GenerateEvent(_gameId);
            }
        }

        private async void UpdateGameData()
        {
            do
            {
                var generatedEvents = await Near.GameContract.ContractMethods.Views
                    .GetGameEvents(_gameId, _events.Count);
                
                _events.AddRange(generatedEvents);
                
                CheckGameFinished(_events);
                RenderEvents(_events);
            } while (!_isGameFinished);
        }

        private void CheckGameFinished(List<Event> events)
        {
            if (events.Count == 0) return;

            var lastEvent = events[^1];
            if (lastEvent.Actions.Count == 0) return;

            var lastAction = lastEvent.Actions[^1];
            if (lastAction.action_type == "GameFinished")
            {
                _isGameFinished = true;
            }
        }

        private void RenderEvents(List<Event> events)
        {
            if (actionMessages.enabled)
            {
                actionMessages.RenderMessages(events);
            }
            
            field.UpdateEventsData(events);
        }
    }
}