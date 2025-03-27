using System.Collections.Generic;
using DefaultNamespace;

namespace GamePlayObjects.Fabrics
{
    public class ToiletsFabric
    {
        public ToiletsFabric(List<ToiletriesReserveTable> toiletsReserves, SpawnerSetting toiletsReserveSettings)
        {
            InitializeSpawner(toiletsReserves, toiletsReserveSettings);
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