using System;
using DG.Tweening;
using GamePlayObjects.Fabrics;
using GamePlayObjects.Player;
using UnityEngine;
using UnityEngine.AI;

namespace GamePlayObjects.Cat.StateMachine
{
    public class StateSleep: State, IDisposable
    {
        private IPlaceFabric placeFabric;
        private NavMeshAgent _navMesh;
        private ISleepAnimator _animator;
        private ITriggerObserver _trigger;
        private IWishBillboard _wishBillboard;
        private Vector3 _sleepPosition;
        private float _stopDistance;
        private bool _onPlace;
        private float _sleepTimer;
        private float _timer;
        private bool _isSleeping;
        private SleepSpawner _spawner;
        private bool _onTutorial;
        public event Action CatOnPlace;

        public StateSleep(StateMachine stateMachine, IPlaceFabric placeFabric, NavMeshAgent navMesh,
                        ISleepAnimator animator, ITriggerObserver trigger,IWishBillboard wishBillboard,
                        float stopDistance, float sleepTimer) : base(stateMachine)
        {
            this.placeFabric = placeFabric;
            _navMesh = navMesh;
            _animator = animator;
            _trigger = trigger;
            _wishBillboard = wishBillboard;
            _stopDistance = stopDistance;
            _sleepTimer = sleepTimer;

            _trigger.TriggerEnter += OnTrigger;
        }

        public void Dispose()
        {
            _trigger.TriggerEnter -= OnTrigger;
        }

        public override void Enter()
        {
            _activeState = true;
            _timer = _sleepTimer;
            _onPlace = false;
            _isSleeping = false;
            _finishState = false;
            // _navMesh.stoppingDistance = _stopDistance;
            _wishBillboard.SetWish(WishType.Sleep);
           _sleepPosition = placeFabric.GetPlacePosition().position;
           TutorialZone = placeFabric.GetPlacePosition();
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
                if(!_isSleeping)
                    return;
                _timer -= Time.deltaTime;
                if (_timer > 0)
                    return;
                EndSleeping();
            }
           
        }

        private void EndSleeping()
        {
            _finishState = true;
            CatOnPlace?.Invoke();
            _spawner.Empty();
            _stateMachine.SetState<StateIdle>();
        }

        private void Move()
        {
            _navMesh.SetDestination(_sleepPosition);
            _animator.MoveAnimation(_navMesh.velocity.magnitude / _navMesh.speed);
            if (Vector3.Distance(_sleepPosition, _navMesh.transform.position) <= _stopDistance)
            {
                _trigger.EnableTrigger();
            }
        }

        private void OnTrigger(Collider obj)
        {
            if(!_activeState)
                return;
            if (obj.gameObject.TryGetComponent<SleepSpawner>(out SleepSpawner spawner))
            {
                 _onPlace = true;
                 _navMesh.isStopped = true;
                 _spawner = spawner;
                if (_spawner.IsClean && !_spawner.IsOccupied)
                {
                    StartSleeping();
                }
                else
                {
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

        public void StartSleeping()
        {
            var moveToTransport = Vector3.Distance(_navMesh.transform.position, _spawner.transform.position);
           
            _navMesh.transform.DOMove(_spawner.SleepPosition.position, moveToTransport)
                    .OnStart(() =>
                    {
                        _navMesh.transform.LookAt(_spawner.transform);
                        _animator.MoveAnimation(_navMesh.speed);
                        _spawner.Occupied();
                    })
                    .OnComplete(()=>
                    {
                        _isSleeping = true;
                        _animator.SleepAnimation();
                        if (_onTutorial)
                        {
                            _spawner.OnTutorial();
                            _onTutorial = false;
                        }
                       
                        _navMesh.transform.LookAt(_spawner.transform);
                        
                    });
            
        }

        public void ReplacePosition(SleepSpawner spawner)
        {
            _spawner = spawner;
            StartSleeping();
        }

        public void OnTutorial()
        {
            _onTutorial = true;
        }
    }
}