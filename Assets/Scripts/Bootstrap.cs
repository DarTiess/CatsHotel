using System.Collections.Generic;
using CamFollow;
using GamePlayObjects.Fabrics;
using GamePlayObjects.Player;
using GamePlayObjects.Spawners;
using Infrastructure.Input;
using Infrastructure.Level.EventsBus;
using UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class Bootstrap: MonoBehaviour
    {
        [Header("Tutorial")]
        [SerializeField] private TutorialService _tutorialPrefab;
        [SerializeField] private bool _isOnTutorial;
        [Header("UI Settings")]
        [SerializeField] private UIControl _canvasPrefab;
        [Header("Players Settings")]
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private Transform _playerSpawnPosition;
        [Space(20)]
        [Header("Idle Settings")]
        [SerializeField] private IdlePointsSettings _idlePointsSettings;
        [Space(20)]
        [Header("Cats Settings")]
        [SerializeField] private int _maxCatsCount;
        [SerializeField] private CatConfig _catConfig;
        [Space(20)]
        [Header("Pack Settings")]
        [SerializeField] private List<PackSpawner> _packSpawners;
        [SerializeField] private SpawnerSetting _packSettings;
        [Space(20)]
        [Header("Store Settings")]
        [SerializeField] private List<Spawner<StoreSpawner>> _storeSpawners; 
        [Space(20)]
        [Header("Food Settings")]
        [SerializeField] private List<FoodSpawner> _foodSpawners;
        [SerializeField] private SpawnerSetting _foodSettings;
        [Space(20)]
        [Header("Sleep Settings")]
        [SerializeField] private List<SleepSpawner> _sleepSpawners;
        [SerializeField] private int _maxUseSleepPlace;
        [Space(20)]
        [Header("Home Departure Settings")]
        [SerializeField] private List<HomeDepart> _homeSpawners;
        [SerializeField] private SpawnerSetting _departSettings;
        [Space(20)]
        [Header("Clients Settings")]
        [SerializeField] private Transform _clientSpawnPosition;
        [SerializeField] private ClientConfig _clientConfig;
        [SerializeField] private Administration _administration;
        [Space(20)]
        [Header("Kitchen")]
        [SerializeField] private List<Kitchen> _kitchens; 
        [Space(20)]
        [Header("Toilets")]
        [SerializeField] private List<ToiletriesReserveTable> _toiletsReservesSpawners;
        [SerializeField] private SpawnerSetting _toileteReserveSettings;
        [Space(20)]
        [Header("Tray Settings")]
        [SerializeField] private List<TraySpawner> _traySpawners;
        [SerializeField] private SpawnerSetting _traySettings;
        [Space(20)]
        [Header("Hospital Settings")]
        [SerializeField] private List<HospitalReserveTable> _hospitalReservesSpawners;
        [SerializeField] private SpawnerSetting _hospitalSettings;
        [SerializeField] private List<HospitalTreatmentTable> _hospitalTreatmentTables;
        [SerializeField] private SpawnerSetting _hospitalTreatmentSettings;
        [SerializeField] private SpawnerSetting _hospitalMedicamentSettings;
        [SerializeField] private List<Transform> _waypoints;
        [Space(20)]
        [Header("Treatment Settings")]
        [SerializeField] private List<TreatmentDepart> _treatmentDeparts;
        [SerializeField] private SpawnerSetting _treatmentSettings;
        [Space(20)]
        [Header("Training Settings")]
        [SerializeField] private List<VelobikeSpawner> _trainingSpawners;
        [SerializeField] private int _maxRuningCats;
        [Space(20)]
        [Header("Bath Settings")]
        [SerializeField] private List<BathReserveTable> _bathReservesSpawners;
        [SerializeField] private SpawnerSetting _bathReserveSettings;
        [Space(20)]
        [Header("Wash Settings")]
        [SerializeField] private List<WashSpawner> _washSpawners;
        [SerializeField] private int _maxWashingCats;


        private IInputService _inputService;
        private UIControl _uiControl;
        private Factory _factory;
        private EventBus _eventBus;
        private PlayerView _player;
        private CamFollower _camera;
        private CatFabric _catFabric;
        private StoreFabric<StoreSpawner> _storeFabric;
        private FoodFabric _foodFabric;
        private SleepFabric _sleepFabric;
        private HomeDepartFabric _homeFabric;
        private ClientFabric _clientFabric;
        private KitchenFabric _kitchenFabric;
        private ToiletsFabric _toiletsFabric;
        private BathFabric _bathFabric;
        private TrayFabric _trayFabric;
        private HospitalFabric _hospitalFabric;
        private TreatmentDepartFabric _treatmentDepartFabric;
        private TrainingFabric _trainingFabric;
        private WashFabric _washFabric;
        private IdleFabric _idlefabric;
        private TutorialService _tutorial;


        private void Awake()
        {
            _factory = new Factory();
            _eventBus = new EventBus();
            _inputService = InputService();
            CreateTutorialService();
            CreatePlayer();
            InitCamera();
            CreateUICanvas();
            CreateFabricsSpawner();
        }

        private void CreateTutorialService()
        {
            if(!_isOnTutorial)
                return;
            
            _tutorial = Instantiate(_tutorialPrefab);
            List<Queue<Transform>> positions = new List<Queue<Transform>>();
            var startStep = new Queue<Transform>();
            var firstStep = new Queue<Transform>();
            var secondStep = new Queue<Transform>();
            var thirdStep = new Queue<Transform>();
            var fourthStep = new Queue<Transform>();
            var fifthStep = new Queue<Transform>();
          
            startStep.Enqueue(_packSpawners[0].TriggerZone);
           
            firstStep.Enqueue(_storeSpawners[0].SpawnerObject.TriggerZone);
            firstStep.Enqueue(_kitchens[0].KitchenSpawner.FoodReserve.TriggerZone());
            firstStep.Enqueue(_kitchens[0].KitchenSpawner.CookingSpawner.TriggerZone);
            firstStep.Enqueue(_kitchens[0].KitchenSpawner.ReadyTable.TriggerZone);
            
           // fourthStep.Enqueue(_administration.TriggerZone);
            fifthStep.Enqueue(_administration.TriggerZone);

            positions.Add(startStep);
            positions.Add(firstStep);
            positions.Add(secondStep);
            positions.Add(thirdStep);
            positions.Add(fourthStep);
            positions.Add(fifthStep);
          
            _tutorial.Initialize(_eventBus, positions);
        }

        private void CreateUICanvas()
        {
            _uiControl = Instantiate(_canvasPrefab);
            _uiControl.Init(_eventBus);
        }

        private IInputService InputService()
        {
            if (Application.isEditor)
            {
                return new StandaloneInputService();
            }
            else
            {
                return new MobileInputService();
            }
        }

        private void CreatePlayer()
        {
            _player = _factory.CreatePlayer(_inputService, _playerConfig, _eventBus, _playerSpawnPosition);
        }

        private void InitCamera()
        {
            _camera = Camera.main.GetComponent<CamFollower>();
            _camera.Init(_eventBus, _player.transform);
        }

        private void CreateFabricsSpawner()
        {
            _idlefabric = new IdleFabric(_idlePointsSettings.IdlePoints);
            _storeFabric = new StoreFabric<StoreSpawner>(_storeSpawners);
            _foodFabric = new FoodFabric(_foodSpawners, _foodSettings);
            _sleepFabric = new SleepFabric(_sleepSpawners, _maxUseSleepPlace);
            _homeFabric = new HomeDepartFabric(_homeSpawners, _departSettings);
            _trayFabric = new TrayFabric(_traySpawners,_traySettings);
            _treatmentDepartFabric = new TreatmentDepartFabric(_treatmentDeparts, _treatmentSettings,
                                                               _hospitalTreatmentTables, _hospitalTreatmentSettings,
                                                               _hospitalMedicamentSettings, _waypoints);
            _trainingFabric = new TrainingFabric(_trainingSpawners, _maxRuningCats);
            _washFabric = new WashFabric(_washSpawners, _maxWashingCats);

            _catFabric = new CatFabric(_eventBus,_packSpawners,_maxCatsCount,
                                       _packSettings, _catConfig, _idlefabric,
                                       _foodFabric, _sleepFabric, _homeFabric, _trayFabric, _treatmentDepartFabric, _trainingFabric, _washFabric);
            _administration.Init();
            _clientFabric = new ClientFabric(_eventBus, _clientSpawnPosition, _clientConfig, _maxCatsCount, _administration);
            _kitchenFabric = new KitchenFabric(_kitchens);

            _toiletsFabric = new ToiletsFabric(_toiletsReservesSpawners,_toileteReserveSettings);
            _bathFabric = new BathFabric(_bathReservesSpawners,_bathReserveSettings);
            
            _hospitalFabric = new HospitalFabric(_hospitalReservesSpawners, _hospitalSettings);
        }
    }
}
