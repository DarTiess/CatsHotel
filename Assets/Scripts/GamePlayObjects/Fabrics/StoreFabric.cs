using System.Collections.Generic;
using DefaultNamespace;

namespace GamePlayObjects.Fabrics
{
    public class StoreFabric<T> where T:SpawnSystem
    {
        public StoreFabric(List<Spawner<StoreSpawner>> spawners)
        {
            foreach (var spawner in spawners)
            {
                InitializeSpawners(spawner.SpawnerObject, spawner.Settings);
            }
        }

        private static void InitializeSpawners(StoreSpawner packageSpawners, SpawnerSetting setting)
        {
            packageSpawners.Init(setting);
        }
    }
}