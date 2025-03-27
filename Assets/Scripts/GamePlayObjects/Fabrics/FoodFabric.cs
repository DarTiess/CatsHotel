using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class FoodFabric : IPlaceFabric
    {
        private List<FoodSpawner> _foodSpawnerList;

        public FoodFabric(List<FoodSpawner> foodSpawners, 
                          SpawnerSetting settings)
        {
            _foodSpawnerList = foodSpawners;

            foreach (FoodSpawner packSpawner in _foodSpawnerList)
            {
                packSpawner.Init(settings);
                packSpawner.FullEmptyPlace += CheckAnotherQueue;
                packSpawner.NotFullOccupiedPlace += CheckAnotherFullPlace;
            }
        }

        private void CheckAnotherFullPlace(FoodSpawner spawner)
        {
            foreach (FoodSpawner packSpawner in _foodSpawnerList)
            {
                if (packSpawner.IsFull && !packSpawner.IsOccupied)
                {
                    spawner.GetPersoneFromQueue(packSpawner);
                    return;
                }
            }
        }

        private void CheckAnotherQueue(FoodSpawner spawner)
        {
            foreach (FoodSpawner packSpawner in _foodSpawnerList)
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
            int rnd = Random.Range(0, _foodSpawnerList.Count);
            Transform foodPos= _foodSpawnerList[rnd].transform;
          
            return foodPos;
        }
    }
}