using System;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using GamePlayObjects.Cat;
using GamePlayObjects.Cat.StateMachine;
using Infrastructure.Level.EventsBus;
using Infrastructure.Level.EventsBus.Signals;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class CatFabric: IDisposable
    {
        private ObjectPoole<CatView> _catsPool;
        private List<PackSpawner> _packsSpawnerList;
        private int _maxCatsCount;
        private int _currentCatsCount=0;
       
        private CatConfig _catConfig;
      
        private IEventBus _eventBus;
        private IPlaceFabric _foodFabric;
        private IPlaceFabric _sleepFabric;
        private IPlaceFabric _homeFabric;
        private IPlaceFabric _trayFabric;
        private IPlaceFabric _treatmentFabric;
        private IPlaceFabric _trainingFabric;
        private IPlaceFabric _washFabric;
        private IdleFabric _idleFabric;
        private bool _onTutorial;
        private CatView _tutorialCat;


        public CatFabric(IEventBus eventBus,List<PackSpawner> packSpawners, int maxCatCount, 
                         SpawnerSetting spawnerSettings, CatConfig catConfig,
                         IdleFabric idleFabric,
                         IPlaceFabric foodFabric, IPlaceFabric sleepFabric, 
                         IPlaceFabric homeFabric, IPlaceFabric trayFabric,
                         IPlaceFabric treatmentFabric, 
                         IPlaceFabric trainingFabric, IPlaceFabric washFabric)
        {
            _eventBus = eventBus;
            _packsSpawnerList = packSpawners;
            _maxCatsCount = maxCatCount;
            _catConfig = catConfig;
            _idleFabric = idleFabric;
            _foodFabric = foodFabric;
            _sleepFabric = sleepFabric;
            _homeFabric = homeFabric;
            _trayFabric = trayFabric;
            _treatmentFabric = treatmentFabric;
            _trainingFabric = trainingFabric;
            _washFabric = washFabric;
          
            _catsPool = new ObjectPoole<CatView>();
            _catsPool.CreatePool(_catConfig.CatPrefab,_maxCatsCount,null);

            foreach (PackSpawner packSpawner in _packsSpawnerList)
            {
                packSpawner.Init(spawnerSettings);
                packSpawner.PushingCat += OnPushCat;
            }
            
            _eventBus.Subscribe<CatAtHome>(SpawnNewCat);
            _eventBus.Subscribe<TutorialStart>(OnTutorialStart);
            _eventBus.Subscribe<CatNextStep>(OnCatNextStepTutorial);
        }

        private void OnCatNextStepTutorial(CatNextStep obj)
        {
            _tutorialCat.GoTutorialStates();
        }

        private void OnTutorialStart(TutorialStart obj)
        {
            _onTutorial = true;
        }

        private void SpawnNewCat(CatAtHome obj)
        {
            if (_onTutorial)
            {
                _onTutorial = false;
                _tutorialCat.CatState -= OnCatState;
                _tutorialCat.OnPlace -= OnPlace;
            }
            else
            {
                _currentCatsCount -= 1;
            }
           
            ContinueSpawnCats();
        }

        private void ContinueSpawnCats()
        {
            foreach (PackSpawner packSpawner in _packsSpawnerList)
            {
                packSpawner.StartSpawn();
            }
        }

        public void Dispose()
        {
            foreach (PackSpawner packSpawner in _packsSpawnerList)
            {
                packSpawner.PushingCat -= OnPushCat;
            }
            _eventBus.Unsubscribe<CatAtHome>(SpawnNewCat);
        }

        private void OnPushCat(Transform transform, Transform catBornPosition)
        {
            CatView cat= _catsPool.GetObject();
            
            cat.transform.position=transform.position;
            cat.transform.DOLocalJump(catBornPosition.position, 1f, 1, 0.3f)
               .OnStart(() =>
               {
                   cat.transform.LookAt(catBornPosition);
               })
               .OnComplete(() =>
               {
                   cat.Init(_idleFabric,_foodFabric,_sleepFabric,_homeFabric, _trayFabric, _treatmentFabric,_trainingFabric,_washFabric, _catConfig);
                   cat.CatWantHome += OnCatWantHome;
                   if (_onTutorial)
                   {
                       cat.CatState += OnCatState;
                       cat.OnPlace += OnPlace;
                       cat.TutorialCat();
                       _tutorialCat = cat;
                       StopSpawnCats();
                       return;
                   }
                   _currentCatsCount++;
                   if (_currentCatsCount >= _maxCatsCount)
                   {
                       StopSpawnCats();
                   }
               });
           
        }

        private void OnPlace()
        {
            _eventBus.Invoke(new NextStepTutorial());
        }

        private void OnCatState(Transform statePosition)
        {
            _eventBus.Invoke(new CatState(statePosition));
        }

        private void StopSpawnCats()
        {
            foreach (PackSpawner packSpawner in _packsSpawnerList)
            {
                packSpawner.StopSpawn();
            }
        }

        private void OnCatWantHome(CatView cat)
        {
            cat.CatWantHome -= OnCatWantHome;
            _eventBus.Invoke(new CatWantHome());
        }
    }
}