using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DefaultNamespace;
using GamePlayObjects.Cat.StateMachine;
using Infrastructure.Level.EventsBus;
using Infrastructure.Level.EventsBus.Signals;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class ClientFabric: IDisposable
    {
        private Transform _spawnPosition;
        private ObjectPoole<ClientView> _clientPool;
        private IEventBus _levelEvents;
        private ClientConfig _config;
        private Transform _target;
        private Administration _administration;
        private List<ClientView> _clientsList;
        private CancellationTokenSource _cancellationToken;

        public ClientFabric(IEventBus levelEvents, Transform spawnPosition, ClientConfig config, int size, Administration administration)
        {
            _levelEvents = levelEvents;
            _spawnPosition = spawnPosition;
            _config = config;
            _administration = administration;
            _target = administration.transform;
            
            _clientPool = new ObjectPoole<ClientView>();
            _clientPool.CreatePool(_config.Prefabs, size, null);

            _clientsList = new List<ClientView>(size);

            _levelEvents.Subscribe<CatWantHome>(PushClient);
        }

        public void Dispose()
        {
            _levelEvents.Unsubscribe<CatWantHome>(PushClient);
        }

        private void PushClient(CatWantHome obj)
        {
            ClientView clientView=_clientPool.GetObject();
            clientView.transform.SetPositionAndRotation(_spawnPosition.position, _spawnPosition.rotation);
            clientView.Init(_config.CatStack, _config.MoveSpeed, _config.StopDistance, _target, _spawnPosition);
            _clientsList.Add(clientView);
            clientView.ClientPlaced += OnClientPlaced;
            clientView.ClientTakeCat += OnTakeCat;
            _target = clientView.transform;
        }

        private void OnTakeCat(ClientView client)
        {
            client.ClientTakeCat -= OnTakeCat;
            ReplaceClients(client);
            _levelEvents.Invoke(new CatAtHome());
        }

        private void ReplaceClients(ClientView client)
        { 
            _clientsList.Remove(client);
            _cancellationToken?.Cancel();
            _target = _administration.transform;
            
            if (_clientsList.Count > 0)
            {
                ReplaceQueue();
            }
        }

        private async void ReplaceQueue()
        {
            _cancellationToken = new CancellationTokenSource();
            await Task.Delay(1000);
            for (int i = 0; i < _clientsList.Count; i++)
            {
                await Task.Delay(1000);
                _clientsList[i].ChangeTarget(_target);
                _target = _clientsList[i].transform;
            }
        }

        private void OnClientPlaced(ClientView client)
        {
            client.ClientPlaced -= OnClientPlaced;
            _administration.SetAtQueue(client);
        }
    }
}