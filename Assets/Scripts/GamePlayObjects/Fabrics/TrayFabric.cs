using System.Collections.Generic;
using GamePlayObjects.Fabrics;
using UnityEngine;

namespace DefaultNamespace
{
    public class TrayFabric : IPlaceFabric
    {
        private List<TraySpawner> _traySpawnerList;
      
        public TrayFabric(List<TraySpawner> traySpawners, 
                         SpawnerSetting settings)
        {
            _traySpawnerList = traySpawners;
           
            foreach (TraySpawner packSpawner in _traySpawnerList)
            {
                packSpawner.Init(settings);
                packSpawner.FullEmptyPlace += CheckAnotherQueue;
                packSpawner.NotFullOccupiedPlace += CheckAnotherFullPlace;
            }
        }

        private void CheckAnotherFullPlace(TraySpawner spawner)
        {
            foreach (TraySpawner packSpawner in _traySpawnerList)
            {
                if (packSpawner.IsFull && !packSpawner.IsOccupied && packSpawner.IsClean)
                {
                    spawner.GetPersoneFromQueue(packSpawner);
                    return;
                }
            }
        }

        private void CheckAnotherQueue(TraySpawner spawner)
        {
            foreach (TraySpawner packSpawner in _traySpawnerList)
            {
                if (packSpawner.WaitingQueue > 0)
                {
                    packSpawner.GetPersoneFromQueue(spawner);
                    return;
                }
            }
        }

        public Transform GetPlacePosition()
        {
            int rnd = Random.Range(0, _traySpawnerList.Count);
            Transform foodPos= _traySpawnerList[rnd].transform;
          
            return foodPos;
        }
    }
}