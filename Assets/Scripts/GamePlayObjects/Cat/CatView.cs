using System;
using System.Collections.Generic;
using GamePlayObjects.Cat.StateMachine;
using GamePlayObjects.Fabrics;
using UnityEngine;
using UnityEngine.AI;

namespace GamePlayObjects.Cat
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(TriggerObserver))]
    [RequireComponent(typeof(CatBillboard))]
    public class CatView : MonoBehaviour
    {
        private float _moveSpeed;
        private float _moveDistance;
        private float _moveTimer;
        private float _timer;
       
        private NavMeshAgent _navMesh;
        private StateMachine.StateMachine _stateMachine;
        private CatAnimator _catAnimator;
        private Animator _animator;
        private TriggerObserver _trigger;
        private CatBillboard _catBillboard;
      
        private StateWishes _wishState;
        private StateEat _eatState;
        private StateSleep _sleepState;
        private StateGoHome _homeState;
        private StateToilete _toileteState;
        private StateTreatment _treatmentState;
        private StateTraining _trainingState;
        private StateWash _washState;
        private StateIdle _idleState;

        private IPlaceFabric _foodfabric;
        private IPlaceFabric _sleepFabric;
        private IPlaceFabric _homefabric;
        private IPlaceFabric _trayFabric;
        private IPlaceFabric _treatmentFabric;
        private IPlaceFabric _trainingFabric;
        private IPlaceFabric _washFabric;
        private IdleFabric _idleFabric;
        private float _stopDistance;
        private float _eatTimer;
        private float _sleepTimer;
        private bool _canMove;
        private float _makeToileteTimer;
        private float _trainingTimer;
        private float _washTimer;
        private float _idleTimer;
        private bool _onTutorial;
        public event Action<CatView> CatWantHome;
        public event Action<Transform> CatState;
        public event Action OnPlace;

        public void Init(IdleFabric idleFabric,IPlaceFabric foodFabric,IPlaceFabric sleepFabric,IPlaceFabric homeFabric, IPlaceFabric trayFabric, IPlaceFabric 
                             treatmentFabric, 
                         IPlaceFabric trainingFabric, IPlaceFabric washFabric, CatConfig config)
        {
            _canMove = false;
            _idleFabric = idleFabric;
            _foodfabric = foodFabric;
            _sleepFabric = sleepFabric;
            _homefabric = homeFabric;
            _trayFabric = trayFabric;
            _treatmentFabric = treatmentFabric;
            _trainingFabric = trainingFabric;
            _washFabric = washFabric;

            _idleTimer = config.IdleTimer;
            _moveSpeed = config.MoveSpeed;
            _moveDistance = config.MoveDistance;
            _moveTimer = config.MoveTimer;
            _stopDistance = config.StopDistance;
            _eatTimer = config.EatTimer;
            _sleepTimer = config.SleepTimer;
            _trainingTimer = config.TrainingTimer;
            _washTimer = config.WashTimer;
            _makeToileteTimer = config.MakeToileteTimer;
         
            _navMesh = GetComponent<NavMeshAgent>();
            _navMesh.speed = _moveSpeed;
            _navMesh.enabled = true;
            _animator = GetComponent<Animator>();
            
            if(_catAnimator==null)
                _catAnimator = new CatAnimator(_animator);
            
            _trigger = GetComponent<TriggerObserver>();
            _catBillboard = GetComponent<CatBillboard>();
            _catBillboard.Init(config.Wishes);

            CreateStateMachine();
        }

        private void Update()
        {
            if (_canMove)
            {
                _stateMachine.Update();
            }
        }

        private void CreateStateMachine()
        {
            if (_stateMachine == null)
            {
                _stateMachine = new StateMachine.StateMachine();
               // _stateMachine.AddState(new StateMove(_stateMachine, _catAnimator,
                                                 //    _navMesh,_catBillboard, transform, _moveDistance, _moveTimer));
                _idleState = new StateIdle(_stateMachine, _idleFabric, _catAnimator, _navMesh, _catBillboard,
                                                                            transform, _idleTimer);
                _stateMachine.AddState(_idleState);
                _eatState = new StateEat(_stateMachine, _foodfabric, _navMesh, _catAnimator,
                                         _trigger,_catBillboard, _stopDistance, _eatTimer);
                _eatState.CatOnPlace += CatOnPlace;
                _sleepState = new StateSleep(_stateMachine, _sleepFabric, _navMesh, _catAnimator, _trigger,_catBillboard, _stopDistance, _sleepTimer);
                _sleepState.CatOnPlace += CatOnPlace;
                _toileteState = new StateToilete(_stateMachine, _trayFabric, _navMesh, _catAnimator, _trigger, _catBillboard, _stopDistance,
                                                 _makeToileteTimer); 
                _treatmentState = new StateTreatment(_stateMachine, _treatmentFabric, _navMesh, _catAnimator, _trigger, _catBillboard, _stopDistance);
                _trainingState = new StateTraining(_stateMachine, _trainingFabric, _navMesh, _catAnimator, _trigger, _catBillboard, _stopDistance,
                                                   _trainingTimer);
                _trainingState.CatOnPlace += CatOnPlace;
               // _washState = new StateWash(_stateMachine, _washFabric, _navMesh, _catAnimator, _trigger, _catBillboard, _stopDistance, _washTimer);
                List<State> states = new List<State>() { _eatState, _sleepState, _toileteState, _treatmentState , _trainingState};
                _wishState = new StateWishes(_stateMachine, states);
                _wishState.TutorialWisheState += OnTutorialWishCat;
                _homeState = new StateGoHome(_stateMachine, _homefabric, _navMesh, _catAnimator, _trigger, _catBillboard,_stopDistance);
                _homeState.ReadyCat += OnReadyCat;
                _homeState.CatOnPlace += CatOnPlace;
                _stateMachine.AddState(_eatState);
                _stateMachine.AddState(_sleepState);
                _stateMachine.AddState(_toileteState); 
                _stateMachine.AddState(_treatmentState);
                _stateMachine.AddState(_trainingState);
                _stateMachine.AddState(_wishState);
                _stateMachine.AddState(_homeState);
            }
            
            _stateMachine.SetState<StateIdle>();
            _canMove = true;
        }

        private void CatOnPlace()
        {
            OnPlace?.Invoke();
        }

        private void OnTutorialWishCat(State statePosition)
        {
            CatState?.Invoke(transform);
        }

        private void OnReadyCat()
        {
          CatWantHome?.Invoke(this);
        }

        public void TutorialCat()
        {
            _onTutorial = true;
            _wishState.OnTutorial();
            _idleState.OnTutorial();
            _sleepState.OnTutorial();
            GoTutorialStates();
        }

        public void GoTutorialStates()
        {
            _idleState.OnTutorial();
            _idleState.OnFinishedStep();
        }
        
    }
}