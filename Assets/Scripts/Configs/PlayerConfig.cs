using System.Collections.Generic;
using GamePlayObjects.Fabrics;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlayObjects.Player
{
    [CreateAssetMenu(menuName = "Configs/PlayerConfig", fileName = "PlayerConfig", order =52)]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private PlayerView _prefab;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        [FormerlySerializedAs("_foodStack")]
        [SerializeField] private PlayerStackConfig stack;
        [SerializeField] private List<WaitingTimer> _timers;

        public PlayerView Prefab => _prefab;
        public float MoveSpeed => _moveSpeed;
        public float RotationSpeed => _rotationSpeed;
        public PlayerStackConfig Stack => stack;
        public List<WaitingTimer> Timers=>_timers;
    }
}