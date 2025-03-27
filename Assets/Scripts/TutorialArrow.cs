using System;
using DG.Tweening;
using GamePlayObjects.Player;
using UnityEngine;

public class TutorialArrow: MonoBehaviour
{
    [SerializeField] private GameObject _arrow;
    private float _bottomPosition;
    private float _moveDuration;

    public event Action PlayerOnPlace;

    public void Initialize(float bottomPosition,float moveDuration)
    {
        _bottomPosition = bottomPosition;
        _moveDuration = moveDuration;
        Hide();
        RotateArrow();
    }

    public void Hide()
    {
        _arrow.SetActive(false);
    }

    private void RotateArrow()
    {
        _arrow.transform.DOMoveY(_bottomPosition, _moveDuration).SetLoops(-1, LoopType.Yoyo);
    }

    public void SetToPosition(Transform tutorialPosition)
    {
        Show();
        transform.position = tutorialPosition.position;
    }

    private void Show()
    {
        _arrow.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerView>(out PlayerView player))
        {
           // Hide();
            PlayerOnPlace?.Invoke();
        }
    }
}