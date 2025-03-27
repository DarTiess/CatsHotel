using System.Collections.Generic;
using GamePlayObjects.Player;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class Administration: MonoBehaviour
    {
        [SerializeField] private Transform _triggerZone;
        public Transform TriggerZone=>_triggerZone;
        private Queue<ClientView> _clients;


        public void Init()
        {
            _clients = new Queue<ClientView>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerView player))
            {
                if (player.HadCat)
                {
                   player.DisplayTimer(this);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerView player))
            {
                player.ExitTriggerPlace();
            }
        }

        public ClientView GetClient()
        {
            return _clients.Dequeue();
        }
        public void SetAtQueue(ClientView client)
        {
            _clients.Enqueue(client);
        }
        
    }
}