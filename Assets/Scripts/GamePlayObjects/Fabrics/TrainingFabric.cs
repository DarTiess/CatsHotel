using System.Collections.Generic;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class TrainingFabric : IPlaceFabric
    {
        private List<VelobikeSpawner> _trainingSpawnerList;
        private int _indexSpawner;

        public TrainingFabric(List<VelobikeSpawner> trainingSpawners, int maxRunCounts)
        {
            _trainingSpawnerList = trainingSpawners;
            _indexSpawner = 0;

            foreach (VelobikeSpawner packSpawner in _trainingSpawnerList)
            {
                packSpawner.Init(maxRunCounts);
                packSpawner.FullEmptyPlace += CheckAnotherQueue;
                packSpawner.NotFullOccupiedPlace += CheckAnotherFreePlace;

            }
        }
        private void CheckAnotherFreePlace(VelobikeSpawner spawner)
        {
            foreach (VelobikeSpawner packSpawner in _trainingSpawnerList)
            {
                if (!packSpawner.IsOccupied)
                {
                    spawner.GetPersoneFromQueue(packSpawner);
                    return;
                }
            }
        }

        private void CheckAnotherQueue(VelobikeSpawner spawner)
        {
            foreach (VelobikeSpawner packSpawner in _trainingSpawnerList)
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
            Transform pos= _trainingSpawnerList[_indexSpawner].transform;
            _indexSpawner++;
            if (_indexSpawner >= _trainingSpawnerList.Count)
            {
                _indexSpawner = 0;
            }
            return pos;
        }
    }
}