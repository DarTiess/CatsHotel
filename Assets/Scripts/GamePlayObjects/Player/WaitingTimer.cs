using GamePlayObjects.Fabrics;
using UnityEngine;

namespace GamePlayObjects.Player
{
    [System.Serializable]
    public class WaitingTimer
    {
        [SerializeField] private ItemType _type;
        [SerializeField] private float _timer;

        public ItemType Type => _type;
        public float Timer => _timer;
    }
}