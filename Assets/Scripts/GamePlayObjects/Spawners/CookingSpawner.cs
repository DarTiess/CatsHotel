using System;
using GamePlayObjects.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlayObjects.Fabrics
{
    public class CookingSpawner:SpawnSystem, IAddItem
    {
        [FormerlySerializedAs("foodReserveTable")]
        [FormerlySerializedAs("reserveTable")]
        [FormerlySerializedAs("_foodReserve")]
        [SerializeField] private FoodReserveTable _foodReserveTable;
        [FormerlySerializedAs("_readyTable")]
        [SerializeField] private FoodReadyTable foodReadyTable;
        [SerializeField] private Transform _triggerZone;
        public Transform TriggerZone=>_triggerZone;
        private int _resource;
       
        public void AddItem()
        {
            _resource++;
            _isPushing = true;
            _timer = 0;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out PlayerView player))
            {
                if (!player.StackIsFull && !player.HadFood)
                {
                    if (!_foodReserveTable.Empty && !foodReadyTable.IsFull)
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

        protected override void SpawnItem()
        {
            if (_resource <= 0)
            {
                return;
            }
           
            ItemObject obj = _poole.GetObject();
            if (obj != null)
            {
                obj.Init(_stackConfig.JumpDuration, _stackConfig.JumpForce, _stackConfig.Type);

                Vector3 placePosition = new Vector3(_itemsPlaces[_indexPlace].position.x,
                                                    _itemsPlaces[_indexPlace].position.y + _yAxis,
                                                    _itemsPlaces[_indexPlace].position.z);
                obj.PlacedItem(transform);
                obj.MoveToStackPlace(placePosition);
                _queue.Push(obj);
                ChangePlaceIndex();
                _isPushing = true;
                _timer = 0;
                _resource -= 1;
                PushItemToReadyTable();
            }
        }

        private void PushItemToReadyTable()
        {
              _isPushing = true;
                _isPushing = true;
                _timer = 0;
                _queue.Pop().MoveToTarget(foodReadyTable.ItemPlace, foodReadyTable);
                ChangeFreePlace();
        }

        public void PushItemToKitchen()
        {
            _foodReserveTable.PushItemToTarget(this);
        }
        
        
    }
}