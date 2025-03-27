using System;
using System.Collections.Generic;
using DefaultNamespace;
using GamePlayObjects.Cat.StateMachine;
using GamePlayObjects.Player;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class WashSpawner: MonoBehaviour, IAddItem
    {
        [SerializeField] private Transform _bathPosition;
        private int _resource;
       
        private Queue<StateWash> _waitingQueue;
        private bool _isOccupied;
        private int _maxCount;
        public Transform BathPosition=>_bathPosition;
        public int WaitingQueue=>_waitingQueue.Count;
        public bool IsOccupied=>_isOccupied;
        public event Action<WashSpawner> EmptyPlace;
        public event Action<WashSpawner> OccupiedPlace;

        public void Init(int maxCount)
        {
            _waitingQueue = new Queue<StateWash>();
            _maxCount = maxCount;
            _resource = 0;
        }

        public void AddItem()
        {
            if(!_isOccupied)
                return;
        
            _resource++;
            if (_resource >= _maxCount)
            {
                MoveQueue();
            }
        }

        private void MoveQueue()
        {
            if (_waitingQueue.Count > 0)
            {
                _waitingQueue.Dequeue().StartWashing();
            }
            else
            {
                EmptyPlace?.Invoke(this);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerView player))
            {
                if (_isOccupied)
                {
                    if(player.HasBath)
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

        public void SetAtQueue(StateWash catNavMesh)
        {
            _waitingQueue.Enqueue(catNavMesh);
            _isOccupied = true;
          /*  if (_waitingQueue.Count>=2)
            {
                OccupiedPlace?.Invoke(this);
                //refactoring
            }*/
        }

        public void Empty()
        {
            _resource--;
            _isOccupied = false;
        }

        public void GetPersoneFromQueue(WashSpawner spawner)
        {
            _waitingQueue.Dequeue().ReplacePosition(spawner);
        }

        public void GetAnotherPlace(StateWash stateWash)
        {
           _waitingQueue.Enqueue(stateWash);
           OccupiedPlace?.Invoke(this);
        }
    }
}