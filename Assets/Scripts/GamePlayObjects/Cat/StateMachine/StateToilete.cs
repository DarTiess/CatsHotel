using System;
using DG.Tweening;
using GamePlayObjects.Fabrics;
using GamePlayObjects.Player;
using UnityEngine;
using UnityEngine.AI;

namespace GamePlayObjects.Cat.StateMachine
{
    public class StateToilete:State, IDisposable
    {
        private IPlaceFabric _placeFabric;
        private NavMeshAgent _navMesh;
        private ISitAnimator _animator;
        private ITriggerObserver _trigger;
        private IWishBillboard _wishBillboard;
        private UnityEngine.Vector3 _toiletePosition;
        private float _stopDistance;
        private float _toileteTimer;
        private float _timer;
        private bool _onPlace;
        private bool _isMakeToilete;
        private TraySpawner _spawner;
        
        public StateToilete(StateMachine stateMachine, IPlaceFabric placeFabric, NavMeshAgent navMesh,
                            ISitAnimator animator, ITriggerObserver trigger, IWishBillboard wishBillboard,
                            float stopDistance, float toileteTimer) : base(stateMachine)
        {
            _placeFabric = placeFabric;
            _navMesh = navMesh;
            _animator = animator;
            _trigger = trigger;
            _wishBillboard = wishBillboard;
            _stopDistance = stopDistance;
            _toileteTimer = toileteTimer;

            _trigger.TriggerEnter += OnTrigger;
        }

        public void Dispose()
        {
            _trigger.TriggerEnter -= OnTrigger;
        }
          public override void Enter()
        {
            _activeState = true;
            _timer = _toileteTimer;
            _onPlace = false;
            _isMakeToilete = false;
            _finishState = false;
            // _navMesh.stoppingDistance = _stopDistance;
           _wishBillboard.SetWish(WishType.MakeToiletes);
           _toiletePosition = _placeFabric.GetPlacePosition().position;
           TutorialZone = _placeFabric.GetPlacePosition();
        }

        public override void Exit()
        {
            _trigger.DisableTrigger();
            _activeState = false;
            _navMesh.stoppingDistance =0;
            _navMesh.isStopped = false;
        }

        public override void Update()
        {
            if(_finishState)
                return;
            if (!_onPlace)
            {
                Move();
            }
            else
            {
                if(!_isMakeToilete)
                    return;
                _timer -= Time.deltaTime;
                if (_timer > 0)
                    return;
                EndMakeToilete();
            }
           
        }

        private void EndMakeToilete()
        {
            _finishState = true;
            _spawner.Empty();
            _stateMachine.SetState<StateIdle>();
        }

        private void Move()
        {
            _navMesh.SetDestination(_toiletePosition);
            _animator.MoveAnimation(_navMesh.velocity.magnitude / _navMesh.speed);
            if (UnityEngine.Vector3.Distance(_toiletePosition, _navMesh.transform.position) <= _stopDistance)
            {
                _trigger.EnableTrigger();
            }
        }

        private void OnTrigger(Collider obj)
        {
            if(!_activeState)
                return;
            if (obj.gameObject.TryGetComponent<TraySpawner>(out TraySpawner spawner))
            {
                 _onPlace = true;
                 _navMesh.isStopped = true;
                 _spawner = spawner;
                if (_spawner.IsFull && !_spawner.IsOccupied && _spawner.IsClean)
                {
                    StartMakeToilete();
                }
                else
                {
                    _navMesh.transform.LookAt(spawner.transform);
                    _animator.SitAnimation();
                    _spawner.SetAtQueue(this);
                    _trigger.DisableTrigger();
                }
            }
            else
            {
                _trigger.EnableTrigger();
            }
        }

        public void StartMakeToilete()
        {
            var moveToTransport = UnityEngine.Vector3.Distance(_navMesh.transform.position, _spawner.transform.position);
           
            _navMesh.transform.DOMove(_spawner.ToiletePosition.position, moveToTransport)
                    .OnStart(() =>
                    {
                        _navMesh.transform.LookAt(_spawner.transform);
                        _animator.MoveAnimation(_navMesh.speed);
                        _spawner.Occupied();
                    })
                    .OnComplete(()=>
                    {
                        _isMakeToilete = true;
                        _animator.SitAnimation();
                        _navMesh.transform.LookAt(_spawner.transform);
                    });
        }

        public void ReplacePosition(TraySpawner spawner)
        {
            _spawner = spawner;
             StartMakeToilete();
        }
        
    }
}