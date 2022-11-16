using System.Collections.Generic;
using System.Threading.Tasks;
using GameScene.Puck;
using UnityEngine;
using Event = Near.Models.Game.Event;

namespace GameScene
{
    public class Field : MonoBehaviour
    {
        [SerializeField] private Puck.Puck puck;
        [SerializeField] private Puck.Puck puckTest;
        private PuckMovement _puckMovement;
        private List<List<Vector3>> _puckTrajectories;
        
        private List<Event> _eventsData;
        private int _numberOfRenderingEvents;
        private int _numberOfRenderingTrajectories;
        
        private int _renderingSpeed;
        private bool _isGameFinished;

        private const float FieldLenght = 60.0f;
        private const float FieldWidth = 26.0f;

        private void Awake()
        {
            _puckMovement = new PuckMovement(FieldLenght, FieldWidth);
            _puckTrajectories = new List<List<Vector3>>();
            _numberOfRenderingEvents = 0;
            _numberOfRenderingTrajectories = 0;
            _renderingSpeed = 1;

            // RenderEvents();
        }

        public void UpdateEventsData(IEnumerable<Event> eventsData)
        {
            // TODO 
            _eventsData.AddRange(eventsData);

            UpdatePuckTrajectories();
        }

        private void UpdatePuckTrajectories()
        {
            // TODO Calculate current puck position
            var puckPosition = puck.transform.position;
            for (int i = _numberOfRenderingEvents; i < _eventsData.Count; i++)
            {
                var trajectory = _puckMovement.GetTrajectory(_eventsData[i], puckPosition);
                _puckTrajectories.Add(trajectory);
            }
        }

        private async void RenderEvents()
        {
            while (WhetherStopRendering())
            {
                if (_puckTrajectories.Count > _numberOfRenderingTrajectories)
                {
                    var trajectoryCoordinates = _puckTrajectories[_numberOfRenderingTrajectories];
                    _numberOfRenderingTrajectories++;

                    var trajectory = new Trajectory()
                    {
                        Coordinates = trajectoryCoordinates,
                        // TODO Calculate rendering speed
                        RenderingSpeed = 1000,
                    };
                    
                    await puck.Move(trajectory);
                    
                    // TODO Rendering players
                }
            }
        }

        private bool WhetherStopRendering()
        {
            if (_eventsData.Count == 0 || _numberOfRenderingTrajectories != _puckTrajectories.Count) return false;
            
            var lastActions = _eventsData[^1].Actions;
            if (lastActions.Count == 0) return false;

            var lastActionType = lastActions[^1].action_type;

            return lastActionType == "GameFinished";
        }

        public async void MockMove()
        {
            //await MockMoveTest();
            //await MockShot();
            //await MockDumpTest();
            await MockPockCheck();
        }

        private async Task MockPockCheck()
        {
            puck.transform.position = new Vector3(-10.5f, 0.18f, 6.1f);
            var puckPosition = puck.transform.position;

            var rndZ = Random.Range(-19f, -12.2f);
            var rndX = Random.Range(-12f, -6f);
            puckTest.transform.position = new Vector3(rndX, puckPosition.y, rndZ);
            var dangleDestination = puckTest.transform.position;
            
            var drndX = Random.Range(-14f, -1f);
            var drndZ = Random.Range(-7f, -5f);
            var destination = new Vector3(drndX, puckPosition.y, drndZ);
            const int numberOfVectors = 1000;
            var dangleMovement = new Hit(puckPosition, dangleDestination, destination, numberOfVectors);
            
            var coordinates = dangleMovement.GetTrajectory();

            var trajectory = new Trajectory()
            {
                Coordinates = coordinates,
                RenderingSpeed = 1000,
                NumberOfPoints = 1000,
            };
            
            await puck.Move(trajectory);
        }
        
        private async Task MockShot()
        {
            //puck.transform.position = new Vector3(0, 0.18f, 13);
            var puckPosition = puck.transform.position;

            var rndZ = Random.Range(-27.3f, -27.2f);
            var rndX = Random.Range(-1.2f, 1.2f);
            puckTest.transform.position = new Vector3(rndX, puckPosition.y, rndZ);
            var destination = puckTest.transform.position;
            const int numberOfVectors = 1000;
            var dangleMovement = new ShotMovement(new List<Vector3> {destination}, puckPosition, numberOfVectors);
            
            var coordinates = dangleMovement.GetTrajectory();

            var trajectory = new Trajectory()
            {
                Coordinates = coordinates,
                RenderingSpeed = 1000,
                NumberOfPoints = 1000,
            };
            
            await puck.Move(trajectory);
        }
        
        private async Task MockMoveTest()
        {
            puck.transform.position = new Vector3(0, 0.18f, 0);
            var puckPosition = puck.transform.position;

            var rndZ = Random.Range(-20.3f, -20.2f);
            var rndX = Random.Range(-5f, 5f);
            puckTest.transform.position = new Vector3(rndX, puckPosition.y, rndZ);
            var destination = puckTest.transform.position;
            const int numberOfVectors = 1000;
            var dangleMovement = new DangleMovement(puckPosition, destination, numberOfVectors);
            
            var coordinates = dangleMovement.GetTrajectory();

            var trajectory = new Trajectory()
            {
                Coordinates = coordinates,
                RenderingSpeed = 1000,
                NumberOfPoints = 1000,
            };
            
            await puck.Move(trajectory);
        }
        
        private async Task MockDumpTest()
        {
            puck.transform.position = new Vector3(4.5f, 0.18f, -7.8f);
            var puckPosition = puck.transform.position;

            const int numberOfVectors = 1000;
            var dangleMovement = new ShotMovement(new List<Vector3>
            {
                new (14.45f, 0.18f, +13.85f),
                new (7.59f, 0.18f, +29.56f),
                new (2.49f, 0.18f, +20.55f),
            }, puckPosition, numberOfVectors);

            var coordinates = dangleMovement.GetTrajectory();

            var trajectory = new Trajectory()
            {
                Coordinates = coordinates,
                RenderingSpeed = 1000,
                NumberOfPoints = 1000,
            };
            
            await puck.Move(trajectory);
        }
    }
}