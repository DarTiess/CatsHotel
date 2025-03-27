using System.Collections.Generic;
using DefaultNamespace;
using GamePlayObjects.Cat.StateMachine;
using GamePlayObjects.Player;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class HomeDepart: SpawnSystem, IAddItem
    {
        private int _resource;
        private Queue<StateGoHome> _waitingQueue;
        private bool _notEmpty;
     
        public override void Init(SpawnerSetting settings)
        {
            base.Init(settings);
            _waitingQueue = new Queue<StateGoHome>();
        }

        public void AddItem()
        {
            _notEmpty = true;
            _resource++;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerView player))
            {
                if (!player.StackIsFull && player.Empty && _notEmpty)
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

        public void SetAtQueue(StateGoHome catNavMesh)
        {
            _waitingQueue.Enqueue(catNavMesh);
        }

        protected override void SpawnItem()
        {
        }

        public override void PushItemToPlayer(PlayerView player)
        {
            MoveQueue(player);
        }

        private void MoveQueue(PlayerView player)
        {
            if (_waitingQueue.Count > 0)
            {
                _waitingQueue.Dequeue().Hide(player, _stackConfig.JumpForce, _stackConfig.JumpDuration);
                _resource--;
                if (_resource <= 0)
                    _notEmpty = false;
            }

        }
    }
}