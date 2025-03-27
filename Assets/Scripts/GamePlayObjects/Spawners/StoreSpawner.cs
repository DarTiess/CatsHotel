using GamePlayObjects.Player;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class StoreSpawner : SpawnSystem
    {
        [SerializeField] private Transform _triggerZone;
        public Transform TriggerZone=>_triggerZone;

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
                if (!player.StackIsFull)
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
    }
}