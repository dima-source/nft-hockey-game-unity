using System.Collections.Generic;
using System.Threading.Tasks;
using Near.Models.Game;
using Near.Models.Game.Bid;

namespace UI.GameUI
{
    public class GameController
    {
        public List<Event> Events;

        private bool isGenerated = false;

        public GameController()
        {
            Events = new List<Event>();
        }
        
        public async Task<AvailableGame> GetUserGame()
        {
            return await Near.GameContract.ContractMethods.Views.GetUserGame();
        }

        public async void GenerateEvents(int gameId)
        {
            if (isGenerated)
            {
                return;
            }

            isGenerated = true;

            try
            {
                List<Event> events = await Near.GameContract.ContractMethods
                    .Actions.GenerateEvent(gameId);

                if (events != null)
                {
                    Events.AddRange(events);

                    isGenerated = false;
                }
            }
            catch
            {
                isGenerated = false;
                GenerateEvents(gameId);
            }
        }
    }
}