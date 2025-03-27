using System.Collections.Generic;
using DefaultNamespace;
using GamePlayObjects.Spawners;

namespace GamePlayObjects.Fabrics
{
    public class HospitalFabric
    {
        public HospitalFabric(List<HospitalReserveTable> hospitalReserves, SpawnerSetting hospitalReserveSettings)
        {
            InitializeSpawner(hospitalReserves, hospitalReserveSettings);
        }

        private void InitializeSpawner<T>(List<T> spawnerList, SpawnerSetting settings) where T: SpawnSystem
        {
            foreach (T packSpawner in spawnerList)
            {
                packSpawner.Init(settings);
            }
        }
    }
}