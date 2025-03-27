using System;
using System.Collections.Generic;
using DefaultNamespace;
using GamePlayObjects.Cat.StateMachine;
using GamePlayObjects.Player;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class FoodSpawner: SpawnSystem, IAddItem
    {
        [SerializeField] private Transform _eatingPosition;
        private int _resource;
        private bool _isFull;
        private Queue<StateEat> _waitingQueue;
        private bool _isOccupied;
        public Transform EatingPosition=>_eatingPosition;
        public int WaitingQueue=>_waitingQueue.Count;
        public bool IsOccupied=>_isOccupied;
        public bool IsFull => _isFull;
        public event Action<FoodSpawner> FullEmptyPlace;
        public event Action<FoodSpawner> NotFullOccupiedPlace;

        public override void Init(SpawnerSetting settings)
        {
            base.Init(settings);
            _waitingQueue = new Queue<StateEat>();
        }

        public void AddItem()
        {
            if(_isFull)
                return;
        
            _resource++;
            _isPushing = true;
            _timer = 0;
        
            if ( _resource>=_stackConfig.MaxCountItems)
            {
                _isFull = true;

                if (_waitingQueue.Count > 0)
                {
                    _waitingQueue.Dequeue().StartEating();
                }
                else
                {
                    FullEmptyPlace?.Invoke(this);
                }
                    
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerView player))
            {
                if (player.HadFood && !_isFull)
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
               obj.Init(_stackConfig.JumpDuration,_stackConfig.JumpForce, _stackConfig.Type);
              
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
           }
        }

        public void SetAtQueue(StateEat catNavMesh)
        {
            _waitingQueue.Enqueue(catNavMesh);
            if (!_isFull)
            {
                NotFullOccupiedPlace?.Invoke(this);
            }
        }

        public void Empty()
        {
            _queue.Pop().HideItem();
            _isPushing = true;
            _timer = 0;
            _isFull = false;
            _isOccupied = false;
        }

        public void Occupied()
        {
            _isOccupied = true;
        }

        public void GetPersoneFromQueue(FoodSpawner spawner)
        {
            _waitingQueue.Dequeue().ReplacePosition(spawner);
        }
    }
}