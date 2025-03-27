using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UIPanels
{
    public class PointerUI : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private float _interfaceScale = 100;

        public event Action<Transform> HidePointer;
        private RectTransform _rectTransform;
        private Vector3 startPointerSize;
        private Camera mainCamera;
        private Vector3 realPos;
        private Vector3 outPos;
        private float direction;

        private void Start()
        {
            _rectTransform = gameObject.GetComponent<RectTransform>();
            startPointerSize = _rectTransform.sizeDelta;
            mainCamera = Camera.main;
          //  Hide();

        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        public void DisplaySprite(Sprite catWishSprite)
        {
            _image.sprite = catWishSprite;
            Show();
        }

        private void RotatePointer(Vector2 direction) 
        {		
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _rectTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }


        public void MovePointer(Transform target)
        {
            realPos = mainCamera.WorldToScreenPoint(target.position); 

            Rect rect = new Rect(0, 0, Screen.width, Screen.height);
            outPos = realPos;
            direction = 1;
            if (!IsBehind(target.position)) 
            {
                if (rect.Contains(realPos)) 
                {
                   Hide();
                   HidePointer?.Invoke(target);
                }
            }
            else
            {
                Show();
                realPos = -realPos;
                outPos = new Vector3(realPos.x, 0, 0); 
                if (mainCamera.transform.position.y < target.position.y)
                {
                    direction = -1;
                    outPos.y = Screen.height; 			
                }
            }
		        
            float offset = _rectTransform.sizeDelta.x /2;
            outPos.x = Mathf.Clamp(outPos.x, offset, Screen.width- offset*2);
            outPos.y = Mathf.Clamp(outPos.y, offset, Screen.height - offset);

            Vector3 pos = realPos - outPos; 

            //  _pointerUI[i].RotatePointer(direction * pos);

            _rectTransform.sizeDelta = new Vector2(startPointerSize.x / 100 * _interfaceScale,
                                                   startPointerSize.y / 100 * _interfaceScale);
            _rectTransform.anchoredPosition = outPos;
        }
        private bool IsBehind(Vector3 point) 
        {		
            Vector3 forward = mainCamera.transform.TransformDirection(Vector3.forward);
            Vector3 toOther = point - mainCamera.transform.position;
            if (Vector3.Dot(forward, toOther) < 0) 
                return true;
		
            return false; 
        }

    }
}