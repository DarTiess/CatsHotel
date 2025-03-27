using System.Collections.Generic;
using GamePlayObjects.Cat;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlayObjects.Fabrics
{
    [CreateAssetMenu(menuName = "Configs/Create CatConfig", fileName = "CatConfig", order = 0)]
    public class CatConfig : ScriptableObject
    {
        [SerializeField] private CatView _catPrefab;
        [Header("idle Settings")]
        [SerializeField] private float _idleTimer;
        [Header("Move Settings")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _moveDistance;
        [SerializeField] private float _moveTimer;
        [FormerlySerializedAs("_eatingDistance")]
        [Header("Eat Settings")]
        [SerializeField] private float stopDistance;
        [SerializeField] private float _eatTimer;
        [Header("Sleep Settings")]
        [SerializeField] private float _sleepTimer;
        [Header("Make Toilete Settings")]
        [SerializeField]private float _makeToileteTimer;
        [Header("Make Training")]
        [SerializeField] private float _trainingTimer;
        [Header("Washing Settings")]
        [SerializeField] private float _washTimer;
        [Header("Icons Settings")]
        [SerializeField] private List<Wishes> _wishes;
        public float WashTimer=>_washTimer;
        public float IdleTimer=>_idleTimer;
        public CatView CatPrefab=>_catPrefab;
        public float MoveSpeed => _moveSpeed;
        public float MoveDistance => _moveDistance;
        public float MoveTimer => _moveTimer;
        public float StopDistance => stopDistance;
        public float EatTimer=>_eatTimer;
        public float SleepTimer=>_sleepTimer;

        public float MakeToileteTimer=>_makeToileteTimer;
        public float TrainingTimer=>_trainingTimer;
        public List<Wishes> Wishes => _wishes;
    }
}