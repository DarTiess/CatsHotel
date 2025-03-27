using System;
using System.Collections.Generic;
using DG.Tweening;
using GamePlayObjects.Cat.StateMachine;
using GamePlayObjects.Player;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class SleepSpawner: MonoBehaviour, IAddItem
    {
        [SerializeField] private Transform _sleepingPosition;
        [SerializeField] private MeshRenderer _modelMesh;
        [SerializeField] private Material _cleanMaterial;
        [SerializeField] private Material _dertyMaterial;
        private int _resource;
        private bool _isClean;
        private Queue<StateSleep> _waitingQueue;
        private bool _isOccupied;
        private int _maxCount;
        private int _clientLimit;
        public int WaitingQueue=>_waitingQueue.Count;
        public Transform SleepPosition=>_sleepingPosition;
        public bool IsOccupied=>_isOccupied;
        public bool IsClean => _isClean;
        public event Action<SleepSpawner> FullEmptyPlace;
        public event Action<SleepSpawner> NotFullOccupiedPlace;
        
        public void Init(int maxCount)
        {
            _waitingQueue = new Queue<StateSleep>();
            _maxCount = maxCount;
            _isClean = true;
            _resource = maxCount;
        }

        public void AddItem()
        {
            if(_isClean)
                return;
        
            _resource+=_maxCount;
            _isClean = true;
            _modelMesh.material=_cleanMaterial;
           MoveQueue();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerView player))
            {
                if (!_isClean)
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

        public void SetAtQueue(StateSleep catNavMesh)
        {
            _waitingQueue.Enqueue(catNavMesh);
            if (!_isClean)
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
                _modelMesh.material=_dertyMaterial;
            }
            else
            {
                MoveQueue();
            }
          
        }

        private void MoveQueue()
        {
            if (_waitingQueue.Count > 0)
            {
                _waitingQueue.Dequeue().StartSleeping();
            }
            else
            {
                FullEmptyPlace?.Invoke(this);
            }
        }

        public void Occupied()
        {
            _isOccupied = true;
        }
        public void GetPersoneFromQueue(SleepSpawner spawner)
        {
            _waitingQueue.Dequeue().ReplacePosition(spawner);
        }

        public void OnTutorial()
        {
            _resource = 1;
        }
    }
}