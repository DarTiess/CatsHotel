using System;
using DG.Tweening;
using GamePlayObjects.Fabrics;
using GamePlayObjects.Player;
using UnityEngine;
using UnityEngine.AI;

namespace GamePlayObjects.Cat.StateMachine
{
    public class StateWash: State, IDisposable
    {
        private IPlaceFabric _placeFabric;
        private NavMeshAgent _navMesh;
        private IWashAnimator _animator;
        private ITriggerObserver _trigger;
        private IWishBillboard _wishBillboard;
        private Vector3 _bathPosition;
        private float _stopDistance;
        private bool _onPlace;
        private float _washTimer;
        private float _timer;
        private bool _isWashing;
        private WashSpawner washSpawner;
        
        public StateWash(StateMachine stateMachine, IPlaceFabric placeFabric, NavMeshAgent navMesh,
                         IWashAnimator animator, ITriggerObserver trigger,IWishBillboard wishBillboard,
                         float stopDistance, float washTimer) : base(stateMachine)
        {
            _placeFabric = placeFabric;
            _navMesh = navMesh;
            _animator = animator;
            _trigger = trigger;
            _wishBillboard = wishBillboard;
            _stopDistance = stopDistance;
            _washTimer = washTimer;

            _trigger.TriggerEnter += OnTrigger;
        }

        public void Dispose()
        {
            _trigger.TriggerEnter -= OnTrigger;
        }
        public override void Enter()
        {
            _activeState = true;
            _timer = _washTimer;
            _onPlace = false;
            _isWashing = false;
            _finishState = false;
            // _navMesh.stoppingDistance = _stopDistance;
            _wishBillboard.SetWish(WishType.Wash);
            _bathPosition = _placeFabric.GetPlacePosition().position;
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
                if(!_isWashing)
                    return;
                _timer -= Time.deltaTime;
                if (_timer > 0)
                    return;
                EndWashing();
            }
           
        }

        private void EndWashing()
        { 
            _finishState = true;
            _navMesh.transform.DOJump(_bathPosition, 1f, 1, 0.3f)
                    .OnComplete(() =>
                    {
                        washSpawner.Empty();
                        _navMesh.enabled = true;
                        _stateMachine.SetState<StateIdle>();
                    });
          
        }
        private void Move()
        {
            _navMesh.SetDestination(_bathPosition);
            _animator.MoveAnimation(_navMesh.velocity.magnitude / _navMesh.speed);
            if (Vector3.Distance(_bathPosition, _navMesh.transform.position) <= _stopDistance)
            {
                _trigger.EnableTrigger();
            }
        }

        private void OnTrigger(Collider obj)
        {
            if(!_activeState)
                return;
            if (obj.gameObject.TryGetComponent<WashSpawner>(out WashSpawner spawner))
            {
                _onPlace = true;
                _navMesh.isStopped = true;
                washSpawner = spawner;
               
              //  _animator.SitAnimation();
                if (!spawner.IsOccupied)
                {
                    washSpawner.SetAtQueue(this);
                    JumpToBath();
                }
                else
                {
                    washSpawner.GetAnotherPlace(this);
                }
                _trigger.DisableTrigger();
                
            }
            else
            {
                _trigger.EnableTrigger();
            }
        }

        private void JumpToBath()
        {
            var moveToTransport = Vector3.Distance(_navMesh.transform.position, washSpawner.transform.position);
            _navMesh.enabled = false;
            _navMesh.transform.DOJump(washSpawner.BathPosition.position,1f, 1, moveToTransport)
                    .OnComplete(()=>
                    {
                        _animator.SitAnimation();
                        // _navMesh.transform.LookAt(_spawner.LookAt);
                    });
        }


        public void StartWashing()
        {
            _isWashing = true;
            _animator.WashingAnimation();
                    
        }

        public void ReplacePosition(WashSpawner spawner)
        {
            washSpawner = spawner;
        }
        
    }
}