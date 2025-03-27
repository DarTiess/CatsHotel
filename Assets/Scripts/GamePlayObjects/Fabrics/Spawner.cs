using DefaultNamespace;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    [System.Serializable]
    public struct Spawner<T>
    {
        [SerializeField] private T _spawnObject;
        [SerializeField] private SpawnerSetting _settings;

        public T SpawnerObject => _spawnObject;
        public SpawnerSetting Settings => _settings;
    }
}