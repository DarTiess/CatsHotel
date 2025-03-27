using System;
using GamePlayObjects.Player;
using UnityEngine;
using UnityEngine.AI;

namespace GamePlayObjects.Fabrics
{
    public class ClientMove
    {
        private NavMeshAgent _navMesh;
        private Transform _target;
        private bool _onPlace;
        private Transform _home;
        private bool _goHome;
        private bool _isReplacing;
        private IMoveAnimator _animator;
        private float _stopDistance;

        public event Action Placed; 
        public event Action AtHome; 

        public ClientMove(NavMeshAgent navMesh,IMoveAnimator animator, float moveSpeed,float stopDistance, Transform target, Transform homePosition)
        {
            _navMesh = navMesh;
            _animator = animator;
            _navMesh.speed = moveSpeed;
            _stopDistance = stopDistance;
            _target = target;
            _navMesh.isStopped = false;
            _onPlace = false;
            _home = homePosition;
        }

        public void FixUpdate()
        {
            if (!_onPlace)
            {
                MoveForward();
            }

            if (_goHome)
            {
                MoveToHome();
            }

            if (_isReplacing)
            {
                Replacing();
            }
            _animator.MoveAnimation(_navMesh.velocity.magnitude/_navMesh.speed);
        }

        public void GoHome()
        {
            _goHome = true;
        }

        public void ChangeTarget(Transform target)
        {
            if (_onPlace)
            {
                _target=target;
                _isReplacing = true;
            }
        }

        private void Replacing()
        {
            
          _navMesh.isStopped = false;
            _navMesh.SetDestination(_target.position);
            if (Vector3.Distance(_target.position, _navMesh.transform.position) <= _stopDistance)
            {
                _navMesh.isStopped = true;
                _isReplacing = false;
            }
        }

        private void MoveToHome()
        {
            _navMesh.isStopped = false;
            _navMesh.SetDestination(_home.position);
            if (Vector3.Distance(_home.position, _navMesh.transform.position) <= _stopDistance)
            {
                _navMesh.isStopped = true; 
                _goHome = false;
                AtHome?.Invoke();
               
            }
        }

        private void MoveForward()
        {
            _navMesh.SetDestination(_target.position);
            if (Vector3.Distance(_target.position, _navMesh.transform.position) <=_stopDistance)
            {
                _navMesh.isStopped = true;
                _onPlace = true;
                Placed?.Invoke();
            }
        }
    }
}