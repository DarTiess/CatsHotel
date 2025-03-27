using System.Collections.Generic;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class SleepFabric : IPlaceFabric
    {
        private List<SleepSpawner> _sleepSpawnerList;
        
        public SleepFabric(List<SleepSpawner> sleepSpawners, int maxSleepCounts)
        {
            _sleepSpawnerList = sleepSpawners;
           

            foreach (SleepSpawner packSpawner in _sleepSpawnerList)
            {
                packSpawner.Init(maxSleepCounts);
                packSpawner.FullEmptyPlace += CheckAnotherQueue;
                packSpawner.NotFullOccupiedPlace += CheckAnotherFullPlace;

            }
        }
        private void CheckAnotherFullPlace(SleepSpawner spawner)
        {
            foreach (SleepSpawner packSpawner in _sleepSpawnerList)
            {
                if (packSpawner.IsClean && !packSpawner.IsOccupied)
                {
                    spawner.GetPersoneFromQueue(packSpawner);
                    return;
                }
            }
        }

        private void CheckAnotherQueue(SleepSpawner spawner)
        {
            foreach (SleepSpawner packSpawner in _sleepSpawnerList)
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
            int rnd = Random.Range(0, _sleepSpawnerList.Count);
            Transform foodPos= _sleepSpawnerList[rnd].transform;
          
            return foodPos;
        }
    }
}