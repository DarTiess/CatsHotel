using System;
using System.Collections.Generic;
using DefaultNamespace;
using GamePlayObjects.Cat.StateMachine;
using GamePlayObjects.Player;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class TreatmentDepart: SpawnSystem, IAddItem
    {
        private int _resource;
        private Queue<StateTreatment> _waitingQueue;
        private bool _notEmpty;
        public event Action<StateTreatment> CatDepartToTakeTreatment;

        public override void Init(SpawnerSetting settings)
        {
            base.Init(settings);
            _waitingQueue = new Queue<StateTreatment>();
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

        public void SetAtQueue(StateTreatment catNavMesh)
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
                var cat = _waitingQueue.Dequeue();

                cat.Hide(player, _stackConfig.JumpForce, _stackConfig.JumpDuration);
                CatDepartToTakeTreatment?.Invoke(cat);
                _resource--;
                if (_resource <= 0)
                    _notEmpty = false;
            }

        }
    }
}