using DG.Tweening;
using GamePlayObjects.Fabrics;
using UnityEngine;
using UnityEngine.AI;

namespace GamePlayObjects.Cat.StateMachine
{
    public class StateIdle: State
    {
        private IIdleAnimator _animator;
        private NavMeshAgent _navMesh;
        private IWishBillboard _wishBillboard;
        private Transform _transform;
        private float _timer;
        private float _idleTimer;
        private bool _onPlace;
        private IdleFabric _placeFabric;
        private IdlePoint _idleState;
        private bool _onTutorial;
        private bool _finishedTutorialStep;

        public StateIdle(StateMachine stateMachine,IdleFabric placeFabric, IIdleAnimator animator,
                         NavMeshAgent navMesh,IWishBillboard wishBillboard,Transform parent, float idleTimer) 
            : base(stateMachine)
        {
            _placeFabric = placeFabric;
            _animator = animator;
            _navMesh = navMesh;
            _wishBillboard = wishBillboard;
            _idleTimer = idleTimer;
            _transform = parent;
        }
        public override void Enter()
        {
            _timer = _idleTimer;
            if (_onTutorial)
            {
                _timer = 2f;
            }
            _onPlace = false;
            _navMesh.stoppingDistance = 1.5f;
            _navMesh.isStopped = false;
            _finishState = false;
            _wishBillboard.SetWish(WishType.None);
            _idleState = _placeFabric.GetPlacePosition();
        }

        public override void Exit()
        {
            _navMesh.enabled = true;
            _navMesh.isStopped = false;
            _navMesh.stoppingDistance = 0f;
            _finishedTutorialStep = false;
            _placeFabric.SetFreePosition(_idleState);
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
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    _finishState = true;
                    if (_idleState.Type == IdleType.Sit)
                    {
                        JumpBackToFloor();
                    }
                    else
                    {
                        GoNextState();
                    }
                }
            }
        }
       
        private void JumpBackToFloor()
        {
            _navMesh.transform.DOJump(_idleState.Position.position, 1f, 1, 0.3f)
                    .OnComplete(() => { GoNextState(); });
        }

        private void GoNextState()
        {
            if (_onTutorial)
            {
                if (_finishedTutorialStep)
                {
                    _stateMachine.SetState<StateWishes>();
                }
            }
            else
            {
                _stateMachine.SetState<StateWishes>();
            }
           
        }

        private void Move()
        {
            _navMesh.SetDestination(_idleState.Position.position);
            _animator.MoveAnimation(_navMesh.velocity.magnitude/_navMesh.speed);
            if (Vector3.Distance(_idleState.Position.position, _transform.position) <= _navMesh.stoppingDistance)
            {
                _onPlace = true;
                _navMesh.isStopped = true;
                _transform.LookAt(_idleState.Position);
                MakeActivity();
            }
        }

        private void MakeActivity()
        {
            switch (_idleState.Type)
            {
                case IdleType.Sit:
                    MakeJumpToSofa();
                    break;
                case IdleType.None:
                    _animator.SitAnimation();
                   
                    break;
                case IdleType.Ball:
                    _animator.PlayBallAnimation();
                    _idleState.MakeActivity();
                    break;
                case IdleType.Plants:
                    _animator.PlayPlantAnimation();
                    _idleState.MakeActivity();
                    break;
            }
        }

        private void MakeJumpToSofa()
        {
            _navMesh.enabled = false;
            Transform sitPlace = _idleState.TakeSitPlace();
            _navMesh.transform.DOJump(sitPlace.position, 1f, 1, 0.3f)
                    .OnComplete(() =>
                    {
                        _navMesh.transform.LookAt(Vector3.back);
                        _animator.SitAnimation();
                    });
        }

        public void OnTutorial()
        {
            _onTutorial = true;
            _timer = 2f;
        }

        public void OnFinishedStep()
        {
            _finishedTutorialStep = true;
            _finishState = false;
        }
    }
}