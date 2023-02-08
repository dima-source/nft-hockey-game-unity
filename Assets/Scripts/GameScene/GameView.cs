using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameScene.UI;
using UI.Scripts;
using UnityEngine;
using Event = Near.Models.Game.Event;

namespace GameScene
{
    public class GameView : UiComponent
    {
        private ActionMessages _actionMessages;
        [SerializeField] private Field field;
        
        private int _gameId;
        private bool _isGameFinished;
        private int _numberOfRenderedEvents;
        private List<Event> _events;

        protected override void Initialize()
        {
            _actionMessages = UiUtils.FindChild<ActionMessages>(transform, "ActionMessages");
        }

        protected override async void OnAwake()
        {
            return;
            var user = await Near.GameContract.ContractMethods.Views.GetUserInGame();
            if (user != null && user.games[0].winner_index == null)
            {
                _gameId = user.games[0].id;
                _events = new List<Event>();

                GenerateEvent();
                UpdateGameData();
            }
            else
            {
                Debug.Log("Error");
            }
        }

        private async Task GenerateEvent()
        {
            while (!_isGameFinished)
            {
                try
                {
                    Debug.Log("Generate event");
                    await Near.GameContract.ContractMethods.Actions.GenerateEvent(_gameId);
                    await UpdateGameData();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private async Task UpdateGameData()
        {
            List<Event> generatedEvents;
            try
            {
                Debug.Log("Update event");
                generatedEvents = await Near.GameContract.ContractMethods.Views
                    .GetGameEvents(_gameId, _events.Count);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }
            
            _events.AddRange(generatedEvents);
           
            CheckGameFinished(_events);
            RenderEvents(_events);
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
            if (_actionMessages.enabled)
            {
                _actionMessages.RenderMessages(events);
            }
            
            field.UpdateEventsData(events);
        }
    }
}