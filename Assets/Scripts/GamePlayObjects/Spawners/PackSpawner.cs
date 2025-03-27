using System;
using GamePlayObjects.Player;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class PackSpawner : SpawnSystem
    {
        [SerializeField] private Transform _catBornPosition;
        [SerializeField] private Transform _triggerZone;

        public Transform TriggerZone => _triggerZone;
        public event Action<Transform, Transform> PushingCat;
        protected override void SpawnItem()
        {
            ItemObject obj = _poole.GetObject();
           if (obj != null)
           {
               obj.Init(_stackConfig.JumpDuration,_stackConfig.JumpForce, _stackConfig.Type);
              
               Vector3 placePosition = new Vector3(_itemsPlaces[_indexPlace].position.x,
                                                   _itemsPlaces[_indexPlace].position.y + _yAxis,
                                                   _itemsPlaces[_indexPlace].position.z);
               obj.PlacedItem(transform); 
               obj.MoveToStackPlace(placePosition);
               _queue.Push(obj);
               ChangePlaceIndex();
               _isPushing = true;
           }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerView player))
            {
                player.DisplayTimer(this);
                
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerView player))
            {
                player.ExitTriggerPlace();
            }
        }

        public override void PushItemToPlayer(PlayerView player)
        {
            base.PushItemToPlayer(player);
           PushingCat?.Invoke(_itemsPlaces[0], _catBornPosition);
        }

        public void StopSpawn()
        {
            _stopPushing = true;
        }

        public void StartSpawn()
        {
            _stopPushing = false;
            _isPushing = true;
        }
    }
}