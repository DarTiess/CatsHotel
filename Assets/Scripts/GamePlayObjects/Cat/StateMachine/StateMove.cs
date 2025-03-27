using GamePlayObjects.Player;
using UnityEngine;
using UnityEngine.AI;

namespace GamePlayObjects.Cat.StateMachine
{
    public class StateMove: State
    {
        private IMoveAnimator _animator;
        private float _moveDistance;
        private LayerMask _layerMask=-1;
        private NavMeshAgent _navMesh;
        private IWishBillboard _wishBillboard;
        private Transform _transform;
        private float _timer;
        private float _moveTimer;
        private Vector3 _target;

        public StateMove(StateMachine stateMachine, IMoveAnimator animator,
                         NavMeshAgent navMesh,IWishBillboard wishBillboard,Transform parent, float moveDistance, float moveTimer) 
            : base(stateMachine)
        {
            _animator = animator;
            _navMesh = navMesh;
            _wishBillboard = wishBillboard;
            _moveDistance = moveDistance;
            _moveTimer = moveTimer;
            
            _transform = parent;
        }

        public override void Enter()
        {
            _target = _transform.position;
            _timer = _moveTimer;
            _finishState = false;
            _wishBillboard.SetWish(WishType.None);
        }


        public override void Update()
        {
            if(_finishState)
                return;
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _finishState = true;
                _stateMachine.SetState<StateWishes>();
            }
            else
            {
                Move();
                _animator.MoveAnimation(_navMesh.velocity.magnitude/_navMesh.speed);
            }
        }


        private void Move()
        {
            if (Vector3.Distance(_target, _transform.position) <= 1f)
            {
                _target = RandomNavSphere(_moveDistance);
                _navMesh.SetDestination(_target);

            }
        }

        private Vector3 RandomNavSphere(float distance)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
           randomDirection += Vector3.zero;
           NavMeshHit navHit;
           Vector3 finalPosition = Vector3.zero;
           while (!NavMesh.SamplePosition(randomDirection, out navHit, distance, _layerMask))
           {
               NavMesh.SamplePosition(randomDirection, out navHit, distance, _layerMask);
           }
           finalPosition = navHit.position;            
           
           return finalPosition;
        }
    }
}