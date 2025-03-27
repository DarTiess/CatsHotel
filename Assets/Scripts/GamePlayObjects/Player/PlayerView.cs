using System.Collections.Generic;
using GamePlayObjects.Fabrics;
using GamePlayObjects.Spawners;
using Infrastructure.Input;
using Infrastructure.Level.EventsBus;
using Infrastructure.Level.EventsBus.Signals;
using UnityEngine;
using UnityEngine.AI;

namespace GamePlayObjects.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(PlayerStack))]
    [RequireComponent(typeof(PlayerCatStack))]
    public class PlayerView: MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerBillboard playerBillboard;
        [SerializeField] private List<Transform> _foodStackPlaces;
        private PlayerMovement _playerMovement;
        private PlayerAnimator _playerAnimator;
        private NavMeshAgent _nav;
        private bool _canMove;
        private SpawnSystem _spawner;
        private PlayerStack _stack;
        private PlayerCatStack _catStack;
       
        private int _currentStackCount;
        private int _maxStackCount;
       
        private IAddItem _sleepSpawner;
        private Administration _administration;
        private FoodReserveTable _foodReserveTable;
        private ToiletriesReserveTable _toiletsReserve;
       
        private bool _stackFull;
        private bool _catOnHand;
        private bool _hasFood;
        private bool _hasPackages;
        private bool _hasPaper;
        private bool _hasMeds;
        private bool _catToHospital;
        private bool _hasBath;
        private WashSpawner _bathSpawner;
        private IEventBus _eventBus;
        private bool _ontutorial;
        public bool Empty=> !_hasFood && !_hasMeds && !_hasPackages && !_hasPaper;
        public int StackCount=>_currentStackCount;
        public bool HadCat=>_catOnHand;

        public bool HasPackages=>_hasPackages;
        public bool HasPapper => _hasPaper;
        public bool HadFood=>_hasFood;
        public bool HasMeds => _hasMeds;
        public bool StackIsFull => _stackFull;
        public bool CatHospital => _catToHospital;
        public bool HasBath => _hasBath;

        public void Init(IInputService input,PlayerConfig config, IEventBus eventBus)
        {
            _nav = GetComponent<NavMeshAgent>();
            _stack = GetComponent<PlayerStack>();
            _stack.Init(config.Stack, _foodStackPlaces);
            _currentStackCount = config.Stack.MaxCountItems;
            _maxStackCount = _currentStackCount;
            _eventBus = eventBus;
            
            _stack.Full += OnFullStack;
            _stack.Empty += OnEmptyStack;
            _stack.EmptyType += OnEmptyTypeInStack;

            _playerAnimator = new PlayerAnimator(_animator);
            _playerMovement = new PlayerMovement(input, _nav, transform,_playerAnimator,config.MoveSpeed, config.RotationSpeed);
            _playerMovement.PlayerStartMove += OnStartMovement;
           
            _canMove = true;
            playerBillboard.Init(config.Timers);
            playerBillboard.EndTimer += OnEndTimer;
        }

        private void OnStartMovement()
        {
            _playerMovement.PlayerStartMove -= OnStartMovement;
            _eventBus.Invoke(new MovePlayer());
            _ontutorial = true;

        }

        private void FixedUpdate()
        {
            if (!_canMove) return;
        
            _playerMovement.OnMove();
        }

        private void OnDestroy()
        {
            playerBillboard.EndTimer -= OnEndTimer;
            _stack.Full -= OnFullStack;
            _stack.Empty -= OnEmptyStack;
        }
        
        public void ExitTriggerPlace()
        {
            playerBillboard.Reset();
            _sleepSpawner = null;
            _administration = null;
        }

        private void OnEmptyTypeInStack(ItemType type)
        {
            switch (type)
            {
                case ItemType.ReadyFood:
                    _hasFood = false;
                    break;
                case ItemType.FoodPackage:
                    _hasPackages = false;
                    break;
            }
        }

        private void CatOnHand()
        {
            _catOnHand = true;
            _stackFull = true;
        }

        private void CatToHospital()
        {
            _stackFull = true;
            _catToHospital = true;
        }

        private void OnEmptyStack()
        {
            _stackFull = false;
            _catOnHand = false;
            _hasFood = false;
            _hasPackages = false;
            _hasPaper = false;
            _hasMeds = false;
            _hasBath = false;
            _currentStackCount = _maxStackCount;
            _playerAnimator.StackEmpty();
        }

        private void OnFullStack()
        {
             _stackFull = true; 
        }

        private void HadReadyFood()
        {
            _hasFood = true;
        }

        private void HasFoodPackage()
        {
            _hasPackages=true;
        }

        public void DisplayTimer(Administration administration)
        {
            playerBillboard.Display(ItemType.Depart);
            _administration = administration;
        }

        public void DisplayTimer(SpawnSystem spawner)
        {
            if(spawner.IsPushing || spawner.StopPushing)
                return;
            
            playerBillboard.Display(spawner.Type);
            _spawner = spawner;
        }

        public void DisplayTimer(SleepSpawner spawner)
        {
            playerBillboard.Display(ItemType.Sleep);
            _sleepSpawner = spawner;
        }
        public void DisplayTimer(VelobikeSpawner spawner)
        {
            playerBillboard.Display(ItemType.Velobike);
            _sleepSpawner = spawner;
        } 
        public void DisplayTimer(WashSpawner spawner)
        {
            playerBillboard.Display(ItemType.Wash);
            _bathSpawner = spawner;
            _stack.PushItemsToTarget(_sleepSpawner);
        }

        private void OnEndTimer()
        {
            if (_sleepSpawner != null)
            {
                PushItemToSpawner();
            }
            else if (_administration != null)
            {
                PushCatToClient();
            }
            else
            {
                PushItemToTarget();
            }
        }

        private void PushEventOfStepTutorial()
        {
            if (_ontutorial)
            {
                _eventBus.Invoke(new NextStepTutorial());
            }
        }

        private void PushCatToClient()
        {
             _stack.PushItemsToClient(_administration.GetClient());
             OnEmptyStack();
             _administration = null;
             PushEventOfStepTutorial();
        }

        private void PushItemToSpawner()
        {
            _sleepSpawner.AddItem();
            _sleepSpawner = null;
            PushEventOfStepTutorial();
        }

        private void PushItemToTarget()
        {
            PushEventOfStepTutorial();
            switch (_spawner)
            {
                case PackSpawner:
                    _spawner.PushItemToPlayer(this);
                    break;
                case FoodReadyTable:
                case StoreSpawner:
                    for (int i = 0; i < _currentStackCount; i++)
                    {
                        _spawner.PushItemToPlayer(this);
                    }
                    break;
                case TreatmentDepart:
                case HomeDepart:
                    _spawner.PushItemToPlayer(this);
                    break;
                case TraySpawner spawner:
                    if (spawner.IsClean)
                    {
                        PushItemFromStack(spawner);
                    }
                    else
                    {
                        spawner.MakeClean();
                    }
                    
                    break;
                case HospitalTreatmentTable spawner:
                    PushItemFromStack(spawner);
                    OnEmptyStack();
                    break;
                case HospitalMedsTable spawner:
                    PushItemFromStack(spawner);
                    break;
                case FoodSpawner spawner:
                    PushItemFromStack(spawner);
                    break;
                case FoodReserveTable spawner:
                    for (int i = 0; i < _foodReserveTable.FreePlace; i++)
                    {
                       PushItemFromStack(spawner);
                    }
                    break;
                case ToiletriesReserveTable spawner:
                    for (int i = 0; i < _toiletsReserve.FreePlace; i++)
                    {
                        PushItemFromStack(spawner);
                    }
                    break;
                case CookingSpawner spawner:
                   spawner.PushItemToKitchen();
                   break;

                default:
                    Debug.Log("No Spawner That Type");
                    break;
            }
           
        }

        public void PushItemFromStack(IAddItem spawner)
        {
            if (_currentStackCount < _maxStackCount)
                _currentStackCount++;

            _stack.PushItemsToTarget(spawner);
        }

        public void AddItemInStack(ItemType type)
        { 
           
            switch (type)
           {
              case ItemType.CatPackage:
                  break;
              
              case ItemType.ReadyFood:
                  HadReadyFood(); 
                  _playerAnimator.HasStack();
                  _stack.AddItem(type);
                  _currentStackCount--;
                  break;
              
              case ItemType.ToiletsPackage:
                  _hasPaper = true;
                  _playerAnimator.HasStack();
                  _stack.AddItem(type);
                  _currentStackCount--;
                  break;
              case ItemType.HospitalPackages:
                  _hasMeds = true;
                  _playerAnimator.HasStack();
                  _stack.AddItem(type);
                  _currentStackCount--;
                  break;
              case ItemType.FoodPackage:
                  HasFoodPackage();
                  _playerAnimator.HasStack();
                    _stack.AddItem(type);
                    _currentStackCount--;
                    break;
              
              case ItemType.Hospital:
                  _playerAnimator.HasStack();
                  _stack.AddItem(type);
                  CatToHospital();
                  break;
              case ItemType.Depart:
                  _playerAnimator.HasStack();
                    _stack.AddItem(type);
                    CatOnHand();
                    break;
              case ItemType.Bath:
                  _hasBath = true;
                  _playerAnimator.HasStack();
                  _stack.AddItem(type);
                  _currentStackCount--;
                  break;

              default:
                    Debug.Log("I don't need type like this");
                    break;
           }
        }

        public void NextTutorialStep()
        {
            PushEventOfStepTutorial();
        }
    }
}