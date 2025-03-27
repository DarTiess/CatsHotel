using System;
using DG.Tweening;
using GamePlayObjects.Fabrics;
using GamePlayObjects.Player;
using UnityEngine;
using UnityEngine.AI;

namespace GamePlayObjects.Cat.StateMachine
{
    public class StateTreatment: State, IDisposable
    {
        private IPlaceFabric _placeFabric;
        private NavMeshAgent _navMesh;
        private ISitAnimator _animator;
        private ITriggerObserver _trigger;
        private IWishBillboard _wishBillboard;
        private Vector3 _hospitalPosition;
        private float _stopDistance;
        private bool _onPlace;
        private TreatmentDepart treatment;
        public StateTreatment(StateMachine stateMachine, IPlaceFabric placeFabric, NavMeshAgent navMesh,
                           ISleepAnimator animator ,ITriggerObserver trigger,IWishBillboard wishBillboard, float stopDistance) 
            : base(stateMachine)
        {
          
            _placeFabric = placeFabric;
            _navMesh = navMesh;
            _animator = animator;
            _trigger = trigger;
            _wishBillboard = wishBillboard;
            _stopDistance = stopDistance;
         
            _trigger.TriggerEnter += OnTrigger;
        }

        public void Dispose()
        {
            _trigger.TriggerEnter -= OnTrigger;
        }

        public override void Enter()
        {
            _activeState = true;
            _onPlace = false;
            _navMesh.isStopped = false;
            _finishState = false;
            //_navMesh.stoppingDistance = _stopDistance;
           _wishBillboard.SetWish(WishType.Hospital);
           _hospitalPosition = _placeFabric.GetPlacePosition().position;
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
        }

        private void Move()
        {
            _navMesh.SetDestination(_hospitalPosition);
            _animator.MoveAnimation(_navMesh.velocity.magnitude / _navMesh.speed);
            if (Vector3.Distance(_hospitalPosition, _navMesh.transform.position) <= _stopDistance)
            {
                _trigger.EnableTrigger(); 
            }
        }

        private void OnTrigger(Collider obj)
        {
            if(!_activeState)
                return;
            if (obj.gameObject.TryGetComponent<TreatmentDepart>(out TreatmentDepart spawner))
            {
                _onPlace = true;
                _navMesh.isStopped = true;
                treatment = spawner;
                _animator.SitAnimation();
                treatment.AddItem();
                treatment.SetAtQueue(this);
                _trigger.DisableTrigger();

            }
            else
            {
                _trigger.EnableTrigger();
            }
        }

        public void Hide(PlayerView player, float jumpForce, float jumpDuration)
        {
            _navMesh.transform.DOJump(player.transform.position, jumpForce, 1, jumpDuration)
                    .OnComplete(() =>
                    {
                        _navMesh.gameObject.SetActive(false);
                        player.AddItemInStack(ItemType.Hospital);
                    });
           
        }

        public void MoveCatToExit(Vector3[] targetPosition, Transform originPosition)
        {
            _navMesh.enabled = false;
            _navMesh.gameObject.transform.position = originPosition.position;
            _navMesh.gameObject.SetActive(true);
         //   var moveToTransport = Vector3.Distance(_navMesh.transform.position, targetPosition[targetPosition.Length-1].transform.position);
            _navMesh.transform.DOPath(targetPosition, _navMesh.speed)
                    .OnStart(() =>
                    {
                        _animator.MoveAnimation(_navMesh.speed);
                        _navMesh.transform.DOLookAt(targetPosition[^1],0.5f );
                    })
                    .OnComplete(() =>
                    {
                        _navMesh.enabled= true;
                        _finishState = true;
                        _stateMachine.SetState<StateIdle>();
                    });
        }
    }
}