using System;
using System.Collections.Generic;
using DG.Tweening;
using GamePlayObjects.Fabrics;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlayObjects.Player
{
    public class PlayerBillboard: Billboard
    {
        [SerializeField] private Image _timerImage;
        public event Action EndTimer;
        private Dictionary<ItemType, float> _timers = new Dictionary<ItemType, float>();

        public void Init(List<WaitingTimer> timers)
        {
            Reset();
            _camera = Camera.main;
            foreach (WaitingTimer waitingTimer in timers)
            {
                _timers.Add(waitingTimer.Type, waitingTimer.Timer);
            }
        }

        public void Display(ItemType type)
        {
            _timers.TryGetValue(type, out var time);
            if (_canvas.gameObject.activeInHierarchy)
            {
                return;
            }
            _canvas.gameObject.SetActive(true);
            _timerImage.DOFillAmount(1, time)
                       .OnComplete(() =>
                       {
                           EndTimer?.Invoke();
                          Reset();
                       });
        }

        public void Reset()
        {
            _timerImage.DOPause();
            _timerImage.fillAmount = 0;
            _canvas.gameObject.SetActive(false);
        }
    }
}