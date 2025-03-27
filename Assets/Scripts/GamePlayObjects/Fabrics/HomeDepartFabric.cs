using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class HomeDepartFabric: IPlaceFabric
    {
        private List<HomeDepart> _homeSpawnerList;

        public HomeDepartFabric(List<HomeDepart> homeSpawners, SpawnerSetting settings )
        {
            _homeSpawnerList = homeSpawners;
           

            foreach (HomeDepart packSpawner in _homeSpawnerList)
            {
                packSpawner.Init(settings);
               
            }
        }
        public Transform GetPlacePosition()
        {
            int rnd = Random.Range(0, _homeSpawnerList.Count);
            Transform foodPos= _homeSpawnerList[rnd].transform;
          
            return foodPos;
        }
    }
}