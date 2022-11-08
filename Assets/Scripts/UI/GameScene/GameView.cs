using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Event = Near.Models.Game.Event;

namespace UI.GameScene
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private ActionMessages actionMessages;

        private int _gameId;
        private bool _isGameFinished;
        private int _numberOfRenderedEvents;
        private List<Event> _events;

        private async void Awake()
        {
            var user = await Near.GameContract.ContractMethods.Views.GetUserInGame();
            if (user != null && user.games[0].winner_index == null)
            {
                _gameId = user.games[0].id;
                _events = new List<Event>();

                var generateEventTask = new Task(GenerateEvent);
                //generateEventTask.Start();
                
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
                Near.GameContract.ContractMethods.Actions.GenerateEvent(_gameId);

                return;
                await Task.Delay(1500);
            }
            
            Near.GameContract.ContractMethods.Actions.GenerateEvent(_gameId);
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

                await Task.Delay(500);
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
        }
    }
}