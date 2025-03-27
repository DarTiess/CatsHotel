using System;
using System.Collections.Generic;
using GamePlayObjects.Cat.StateMachine;
using GamePlayObjects.Player;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class VelobikeSpawner: MonoBehaviour, IAddItem
    {
        [SerializeField] private Transform _runPosition;
        [SerializeField] private Transform _lookatPosition;

        private int _resource;
        private bool _isRuning;
        private Queue<StateTraining> _waitingQueue;
        private bool _isOccupied; 
        private int _maxCount;
       // private int _clientLimit;
        public Transform LookAt=>_lookatPosition;
        public int WaitingQueue=>_waitingQueue.Count;
        public Transform RunPosition=>_runPosition;
        public bool IsOccupied=>_isOccupied;
        public bool IsRuning => _isRuning;
        public event Action<VelobikeSpawner> FullEmptyPlace;
        public event Action<VelobikeSpawner> NotFullOccupiedPlace;
        
        public void Init(int maxCount)
        {
            _waitingQueue = new Queue<StateTraining>();
            _maxCount = maxCount;
            _resource = 0;
        }

        public void AddItem()
        {
            if(_isRuning)
                return;
        
            _resource++;
            if (_resource >= _maxCount)
            {
                  _isRuning = true;
                  MoveQueue();
            }
          
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerView player))
            {
                if (!_isRuning && _isOccupied)
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

        public void SetAtQueue(StateTraining catNavMesh)
        {
            _waitingQueue.Enqueue(catNavMesh);
            _isOccupied = true;
            if (!_isRuning)
            {
              //  NotFullOccupiedPlace?.Invoke(this);
            }
        }

        public void Empty()
        {
            _resource--;
            _isOccupied = false;
            
          //  if (_resource<=0)
          //  {
                _isRuning = false;
                
           // }
           // else
           // {
         //       MoveQueue();
          //  }
          
        }

        private void MoveQueue()
        {
            if (_waitingQueue.Count > 0)
            {
                _waitingQueue.Dequeue().StartRunning();
                if (_waitingQueue.Count > 0)
                {
                    NotFullOccupiedPlace?.Invoke(this);
                }
            }
        }
        
        public void GetPersoneFromQueue(VelobikeSpawner spawner)
        {
            _waitingQueue.Dequeue().ReplacePosition(spawner);
        }
    }
}