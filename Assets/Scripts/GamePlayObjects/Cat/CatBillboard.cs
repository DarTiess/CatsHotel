using System;
using System.Collections.Generic;
using DG.Tweening;
using GamePlayObjects.Player;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlayObjects.Cat
{
    public class CatBillboard: Billboard, IWishBillboard
    {
        [SerializeField] private Image _wishIcon; 
        [SerializeField] private float _interfaceScale = 100;
        [SerializeField] private Transform _canvasPosition;
        private Dictionary<WishType, Sprite> _wishesList;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _canvasUI;
        private Vector3 startPointerSize;
        private Vector3 realPos;
        private Vector3 outPos;
        private float direction;
        private bool _onDisplay;

        public event Action<Sprite> WishCat; 

        private void Start()
        {
            Reset();
        }

        private void LateUpdate()
        {
            if(_onDisplay)
                MovePointer();
        }

        public void Init(List<Wishes> wishes)
        {
            _wishesList = new Dictionary<WishType, Sprite>(wishes.Count);
            foreach (Wishes wish in wishes)
            {
                _wishesList.Add(wish.Type, wish.Sprite);
            }
            _camera = Camera.main;
           // _rectTransform = _canvas.gameObject.GetComponent<RectTransform>();
            startPointerSize = _rectTransform.sizeDelta;
            Reset();
        }

        private void Reset()
        { 
            _canvas.gameObject.SetActive(false);
        }

        public void SetWish(WishType type)
        {
            if (type == WishType.None)
            {
                Reset();
                return;
            }
            Sprite sprite= _wishesList[type];
            _wishIcon.sprite = sprite;
           // WishCat?.Invoke(sprite);
            Display();
        }
        
        private void Display()
        {
            _canvas.gameObject.SetActive(true);
            _onDisplay = true;
        }
        public void MovePointer()
        {
            realPos = _camera.WorldToScreenPoint(_canvasPosition.position); 

            Rect rect = new Rect(0, 0, Screen.width, Screen.height);
            outPos = realPos;
            direction = 1;
            
            if (IsBehind(_canvasPosition.position))
            {
                Debug.Log("IsBehind");

                realPos = -realPos;
                outPos = new Vector3(realPos.x, 0, 0); 
                if (_camera.transform.position.y < _canvasPosition.position.y)
                {
                    direction = -1;
                    outPos.y = Screen.height; 			
                }
            }
          
            float offsetX = _rectTransform.sizeDelta.x /2;
            float offsetY = _rectTransform.sizeDelta.y /2;
            outPos.x = Mathf.Clamp(outPos.x, offsetX, Screen.width+_canvasUI.sizeDelta.x);
            outPos.y = Mathf.Clamp(outPos.y, offsetY, Screen.height+_canvasUI.sizeDelta.y);

            Vector3 pos = realPos - outPos; 

            //  _pointerUI[i].RotatePointer(direction * pos);

            _rectTransform.sizeDelta = new Vector2(startPointerSize.x / 100 * _interfaceScale, 
                                                   startPointerSize.y / 100 * _interfaceScale);
            _rectTransform.anchoredPosition = outPos;
        }
        private bool IsBehind(Vector3 point) 
        {		
            Vector3 forward = _camera.transform.TransformDirection(Vector3.forward);
            Vector3 toOther = point - _camera.transform.position;
            if (Vector3.Dot(forward, toOther) < 0) 
                return true;
		
            return false; 
        }

    }
}