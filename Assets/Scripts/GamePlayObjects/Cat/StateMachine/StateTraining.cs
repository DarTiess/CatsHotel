using System;
using DG.Tweening;
using GamePlayObjects.Fabrics;
using GamePlayObjects.Player;
using UnityEngine;
using UnityEngine.AI;

namespace GamePlayObjects.Cat.StateMachine
{
    public class StateTraining: State, IDisposable
    {
        private IPlaceFabric _placeFabric;
        private NavMeshAgent _navMesh;
        private IRunAnimator _animator;
        private ITriggerObserver _trigger;
        private IWishBillboard _wishBillboard;
        private Vector3 _runPosition;
        private float _stopDistance;
        private bool _onPlace;
        private float _runTimer;
        private float _timer;
        private bool _isRunning;
        private VelobikeSpawner _spawner;
        public event Action CatOnPlace;
        

        public StateTraining(StateMachine stateMachine, IPlaceFabric placeFabric, NavMeshAgent navMesh,
                             IRunAnimator animator, ITriggerObserver trigger,IWishBillboard wishBillboard,
                             float stopDistance, float runTimer) : base(stateMachine)
        {
            _placeFabric = placeFabric;
            _navMesh = navMesh;
            _animator = animator;
            _trigger = trigger;
            _wishBillboard = wishBillboard;
            _stopDistance = stopDistance;
            _runTimer = runTimer;

            _trigger.TriggerEnter += OnTrigger;
        }

        public void Dispose()
        {
            _trigger.TriggerEnter -= OnTrigger;
        }

        public override void Enter()
        {
            _activeState = true;
            _timer = _runTimer;
            _onPlace = false;
            _isRunning = false;
            _finishState = false;
            // _navMesh.stoppingDistance = _stopDistance;
            _wishBillboard.SetWish(WishType.Training);
           _runPosition = _placeFabric.GetPlacePosition().position;
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
                if(!_isRunning)
                    return;
                _timer -= Time.deltaTime;
                if (_timer > 0)
                    return;
                EndTraining();
            }
           
        }

        private void EndTraining()
        {
            _finishState = true;
            _spawner.Empty();
            _stateMachine.SetState<StateIdle>();
        }

        private void Move()
        {
            _navMesh.SetDestination(_runPosition);
            _animator.MoveAnimation(_navMesh.velocity.magnitude / _navMesh.speed);
            if (Vector3.Distance(_runPosition, _navMesh.transform.position) <= _stopDistance)
            {
                _trigger.EnableTrigger();
            }
        }

        private void OnTrigger(Collider obj)
        {
            if(!_activeState)
                return;
            if (obj.gameObject.TryGetComponent<VelobikeSpawner>(out VelobikeSpawner spawner))
            {
                 _onPlace = true;
                 _navMesh.isStopped = true;
                 _spawner = spawner;
               
                    _animator.SitAnimation();
                    _spawner.SetAtQueue(this);
                    _trigger.DisableTrigger();
                    CatOnPlace?.Invoke();
                
            }
            else
            {
                _trigger.EnableTrigger();
            }
        }

        public void ReplacePosition(VelobikeSpawner spawner)
        {
            _spawner = spawner;
          //  StartRunning();
        }

        public void StartRunning()
        {
            var moveToTransport = Vector3.Distance(_navMesh.transform.position, _spawner.transform.position);
           
            _navMesh.transform.DOMove(_spawner.RunPosition.position, moveToTransport)
                    .OnStart(() =>
                    {
                        _navMesh.transform.LookAt(_spawner.LookAt);
                        _animator.MoveAnimation(_navMesh.speed);
                       // _spawner.Occupied();
                    })
                    .OnComplete(()=>
                    {
                        _isRunning = true;
                        _animator.RunAnimation();
                        _navMesh.transform.LookAt(_spawner.LookAt);
                    });
        }
    }
}