using UnityEngine;

namespace Infrastructure.Level.EventsBus.Signals
{
    public struct CatWish
    {
        private Transform _transform;
        private Sprite _sprite;

        public Transform Transform => _transform;
        public Sprite Sprite => _sprite;
        public CatWish(Transform transform, Sprite sprite)
        {
            _transform = transform;
            _sprite = sprite;
        }
    }
}