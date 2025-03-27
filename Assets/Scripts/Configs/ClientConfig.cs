using System.Collections.Generic;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    [CreateAssetMenu(menuName = "Configs/Create ClientConfig", fileName = "ClientConfig", order = 0)]
    public class ClientConfig : ScriptableObject
    {
        [SerializeField] private List<ClientView> prefabs;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private PlayerStackConfig _catStack;
        [SerializeField] private float _stopDistance;
        public List<ClientView> Prefabs=>prefabs;
        public float MoveSpeed=>_moveSpeed;
        public PlayerStackConfig CatStack => _catStack;
        public float StopDistance => _stopDistance;
    }
}