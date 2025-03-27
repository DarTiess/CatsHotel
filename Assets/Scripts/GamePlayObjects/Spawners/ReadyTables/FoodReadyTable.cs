using GamePlayObjects.Player;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class FoodReadyTable : SpawnSystem, IAddItem
    {
        [SerializeField] private Transform _triggerZone;
        public Transform TriggerZone=>_triggerZone;
        private int _resource;
        private bool _isFull;
        private bool _isEmpty=true;
        private int _count;
        public bool IsFull => _isFull;
       

        public void AddItem()
        {
            if(_isFull)
                return;
        
            _resource++;
            _isPushing = true;
            _isEmpty = false;
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out PlayerView player))
            {
                if (!_isEmpty)
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
                _resource -= 1;
                _count++;
                if ( _count>=_stackConfig.MaxCountItems)
                {
                    _isFull = true;
                } 
            }
        }

        public override void PushItemToPlayer(PlayerView player)
        {
            base.PushItemToPlayer(player); 
            _isFull = false;
            _count--;
            if (_queue.Count <= 0)
            {
                _isEmpty = true;
               
            }
        }
    }
}