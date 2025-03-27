using System;
using DG.Tweening;
using GamePlayObjects.Fabrics;
using GamePlayObjects.Player;
using UnityEngine;
using UnityEngine.AI;

namespace GamePlayObjects.Cat.StateMachine
{
    public class StateGoHome: State, IDisposable
    {
        private IPlaceFabric _placeFabric;
        private NavMeshAgent _navMesh;
        private ISitAnimator _animator;
        private ITriggerObserver _trigger;
        private IWishBillboard _wishBillboard;
        private Vector3 _homePosition;
        private float _stopDistance;
        private bool _onPlace;
        private HomeDepart depart;
        
        public event Action ReadyCat;
        public event Action CatOnPlace;

        public StateGoHome(StateMachine stateMachine, IPlaceFabric placeFabric, NavMeshAgent navMesh,
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
           _wishBillboard.SetWish(WishType.Home);
           _homePosition = _placeFabric.GetPlacePosition().position;
           TutorialZone = _placeFabric.GetPlacePosition();
           ReadyCat?.Invoke();
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
            _navMesh.SetDestination(_homePosition);
            _animator.MoveAnimation(_navMesh.velocity.magnitude / _navMesh.speed);
            if (Vector3.Distance(_homePosition, _navMesh.transform.position) <= _stopDistance)
            {
                _trigger.EnableTrigger();
            }
        }

        private void OnTrigger(Collider obj)
        {
            if(!_activeState)
                return;
            if (obj.gameObject.TryGetComponent<HomeDepart>(out HomeDepart spawner))
            {
                 _onPlace = true;
                 _navMesh.isStopped = true;
                 depart = spawner;
                 _animator.SitAnimation();
                 depart.AddItem();
                 depart.SetAtQueue(this);
                 _trigger.DisableTrigger();
                 CatOnPlace?.Invoke();
                 Debug.Log("Wait Home");

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
                        _navMesh.enabled = false;
                        _navMesh.gameObject.SetActive(false);
                        _finishState = true;
                        player.AddItemInStack(ItemType.Depart);
                    });
           
        }
    }
}