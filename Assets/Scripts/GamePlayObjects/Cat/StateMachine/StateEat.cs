using System;
using DG.Tweening;
using GamePlayObjects.Fabrics;
using GamePlayObjects.Player;
using UnityEngine;
using UnityEngine.AI;

namespace GamePlayObjects.Cat.StateMachine
{
    public class StateEat: State, IDisposable
    {
        private IPlaceFabric _placeFabric;
        private NavMeshAgent _navMesh;
        private IEatAnimator _animator;
        private ITriggerObserver _trigger;
        private IWishBillboard _wishBillboard;
        private Vector3 _foodPosition;
        private float _stopDistance;
        private float _eatTimer;
        private float _timer;
        private bool _onPlace;
        private bool _isEating;
        private FoodSpawner _spawner;
        public event Action CatOnPlace;
        

        public StateEat(StateMachine stateMachine, IPlaceFabric placeFabric, NavMeshAgent navMesh,
                        IEatAnimator animator, ITriggerObserver trigger, IWishBillboard wishBillboard,
                        float stopDistance, float eatTimer) : base(stateMachine)
        {
            _placeFabric = placeFabric;
            _navMesh = navMesh;
            _animator = animator;
            _trigger = trigger;
            _wishBillboard = wishBillboard;
            _stopDistance = stopDistance;
            _eatTimer = eatTimer;

            _trigger.TriggerEnter += OnTrigger;
        }

        public void Dispose()
        {
            _trigger.TriggerEnter -= OnTrigger;
        }

        public override void Enter()
        {
            _activeState = true;
            _timer = _eatTimer;
            _onPlace = false;
            _isEating = false;
            _finishState = false;
          // _navMesh.stoppingDistance = _stopDistance;
           _wishBillboard.SetWish(WishType.Eat);
           _foodPosition = _placeFabric.GetPlacePosition().position;
           TutorialZone =_navMesh.transform;
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
                if(!_isEating)
                    return;
                _timer -= Time.deltaTime;
                if (_timer > 0)
                    return;
                EndEating();
            }
           
        }

        private void EndEating()
        {
            _finishState = true;
            _spawner.Empty();
            _stateMachine.SetState<StateIdle>();
        }

        private void Move()
        {
            _navMesh.SetDestination(_foodPosition);
            _animator.MoveAnimation(_navMesh.velocity.magnitude / _navMesh.speed);
            if (Vector3.Distance(_foodPosition, _navMesh.transform.position) <= _stopDistance)
            {
                _trigger.EnableTrigger();
            }
        }

        private void OnTrigger(Collider obj)
        {
            if(!_activeState)
                return;
            if (obj.gameObject.TryGetComponent<FoodSpawner>(out FoodSpawner spawner))
            {
                 _onPlace = true;
                 _navMesh.isStopped = true;
                 _spawner = spawner;
                if (_spawner.IsFull && !_spawner.IsOccupied)
                {
                    StartEating();
                }
                else
                {
                    _navMesh.transform.LookAt(spawner.transform);
                    _animator.SitAnimation();
                    _spawner.SetAtQueue(this);
                    _trigger.DisableTrigger();
                    CatOnPlace?.Invoke();
                }
            }
            else
            {
                _trigger.EnableTrigger();
            }
        }

        public void StartEating()
        {
            var moveToTransport = Vector3.Distance(_navMesh.transform.position, _spawner.transform.position);
           
            _navMesh.transform.DOMove(_spawner.EatingPosition.position, moveToTransport)
                    .OnStart(() =>
                    {
                        _navMesh.transform.LookAt(_spawner.transform);
                        _animator.MoveAnimation(_navMesh.speed);
                        _spawner.Occupied();
                    })
                    .OnComplete(()=>
                    {
                        _isEating = true;
                        _animator.EatAnimation();
                        _navMesh.transform.LookAt(_spawner.transform);
                    });
        }

        public void ReplacePosition(FoodSpawner spawner)
        {
            _spawner = spawner;
             StartEating();
        }
    }
}