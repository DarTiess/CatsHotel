using DG.Tweening;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    [System.Serializable]
    public class IdlePoint
    {
        [SerializeField] private IdleType _type;
        [SerializeField] private Transform _position;
        private bool _isFree=true;

        public IdleType Type => _type;
        public Transform Position => _position;
        public bool IsFree => _isFree;
        
        public void IsOccupied()
        {
            _isFree = false;
        }

        public void SetFree()
        {
            _isFree = true;
            switch (_type)
            {
                case IdleType.Ball:
                    ResetBall();
                    break;
            }
            _position.DOKill();
        }

        private void ResetBall()
        {
            _position.localScale /= 2f;
        }


        public void MakeActivity()
        {
            switch (_type)
            {
                case IdleType.Ball:
                    PlayBall();
                    break;
                case IdleType.Plants:
                  //  ShakePlants();
                    break;
            }
        }

        private void ShakePlants()
        {
            _position.DOShakeRotation(0.3f, new Vector3(0,45,0), 10, 90f, true, ShakeRandomnessMode.Harmonic)
                     .SetLoops(-1, LoopType.Yoyo);
        }

        private void PlayBall()
        {
            _position.DOScale(_position.localScale * 2f, 0.3f)
                     .SetLoops(-1, LoopType.Yoyo);
        }

        public Transform TakeSitPlace()
        {
            return _position.GetComponent<SitPlace>().Sit;
        }
    }
}