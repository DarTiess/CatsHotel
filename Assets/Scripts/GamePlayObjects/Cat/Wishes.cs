using UnityEngine;

namespace GamePlayObjects.Cat
{
    [System.Serializable]
    public class Wishes
    {
        [SerializeField] private WishType _wishType;
        [SerializeField] private Sprite _wishSprite;

        public WishType Type => _wishType;
        public Sprite Sprite => _wishSprite;
    }
}