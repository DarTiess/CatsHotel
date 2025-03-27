using System;
using System.Collections.Generic;
using DefaultNamespace;
using GamePlayObjects.Cat.StateMachine;
using GamePlayObjects.Player;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class TraySpawner: SpawnSystem, IAddItem
    {
        [SerializeField] private Transform _toiletePosition;
        [SerializeField] private GameObject _dirtyPrefab;
        private int _resource;
        private bool _isFull;
        private bool _isClean;
        private Queue<StateToilete> _waitingQueue;
        private bool _isOccupied;
        private GameObject _dirtyToilet;
        public bool IsClean=>_isClean;
        public Transform ToiletePosition=>_toiletePosition;
        public int WaitingQueue=>_waitingQueue.Count;
        public bool IsOccupied=>_isOccupied;
        public bool IsFull => _isFull;
        public event Action<TraySpawner> FullEmptyPlace;
        public event Action<TraySpawner> NotFullOccupiedPlace;

        public override void Init(SpawnerSetting settings)
        {
            base.Init(settings);
            _waitingQueue = new Queue<StateToilete>();
            _dirtyToilet = Instantiate(_dirtyPrefab, _toiletePosition.position, Quaternion.identity);
            _dirtyToilet.SetActive(false);
            _isClean = true;
        }

        public void AddItem()
        {
            if(_isFull)
                return;
        
            _resource+=_stackConfig.MaxCountItems;
            _isPushing = true;
            _timer = 0;

            _isFull = true;
            MoveQueue();


        }

        private void MoveQueue()
        {
            if (_waitingQueue.Count > 0)
            {
                _waitingQueue.Dequeue().StartMakeToilete();
            }
            else
            {
                FullEmptyPlace?.Invoke(this);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerView player))
            {
                if (!_isFull)
                {
                    if(player.HasPapper)
                     player.DisplayTimer(this);
                }
                else
                {
                    if (!_isClean)
                    {
                        player.DisplayTimer(this);
                    }
                }
            }
        }

        public void MakeClean()
        {
            _isClean = true;
            _isFull = false;
            _dirtyToilet.SetActive(false);
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
            }
        }

        public void SetAtQueue(StateToilete catNavMesh)
        {
            _waitingQueue.Enqueue(catNavMesh);
            if (!_isFull)
            {
                NotFullOccupiedPlace?.Invoke(this);
            }
        }

        public void Empty()
        {
            _resource--;
           _isOccupied = false;
           if (_resource<=0)
           {
               _isClean = false;
               MakeDirty();
           }
           else
           {
               MoveQueue();
           }
        }

        private void MakeDirty()
        {
            _queue.Pop().HideItem();
            _dirtyToilet.SetActive(true);
        }

        public void Occupied()
        {
            _isOccupied = true;
        }

        public void GetPersoneFromQueue(TraySpawner spawner)
        {
            _waitingQueue.Dequeue().ReplacePosition(spawner);
        }
    }
}