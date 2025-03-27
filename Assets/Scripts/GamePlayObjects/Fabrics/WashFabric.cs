using System.Collections.Generic;
using GamePlayObjects.Fabrics;
using UnityEngine;

namespace DefaultNamespace
{
    public class WashFabric : IPlaceFabric
    {
        private List<WashSpawner> _spawnerList;
        private int _indexSpawner;
      
        public WashFabric(List<WashSpawner> washSpawners, 
                          int maxCount)
        {
            _spawnerList = washSpawners;
            _indexSpawner = 0;
           
            foreach (WashSpawner packSpawner in _spawnerList)
            {
                packSpawner.Init(maxCount);
                packSpawner.EmptyPlace += CheckAnotherQueue;
                packSpawner.OccupiedPlace += CheckAnotherFreePlace;
            }
        }

        private void CheckAnotherFreePlace(WashSpawner spawner)
        {
            foreach (WashSpawner packSpawner in _spawnerList)
            {
                if (!packSpawner.IsOccupied)
                {
                    spawner.GetPersoneFromQueue(packSpawner);
                    return;
                }
            }
        }

        private void CheckAnotherQueue(WashSpawner spawner)
        {
            foreach (WashSpawner packSpawner in _spawnerList)
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
            Transform pos= _spawnerList[_indexSpawner].transform;
            _indexSpawner++;
            if (_indexSpawner >= _spawnerList.Count)
            {
                _indexSpawner = 0;
            }
            return pos;
        }
    }
}