using System.Collections.Generic;
using CamFollow;
using GamePlayObjects.Cat.StateMachine;
using Infrastructure.Level.EventsBus;
using Infrastructure.Level.EventsBus.Signals;
using UnityEngine;

public class TutorialService : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialWindow;
    [SerializeField] private Transform _cameraPosition;
    [SerializeField] private TutorialArrow _tutorialArrow;
    [SerializeField]  private float _bottomPosition;
    [SerializeField] private float _moveDuration;
    private IEventBus _evenBus;
    private List<Queue<Transform>> _stepTutorial;
    private int _indexPosition=0;
    private CamFollower _camera;
    private State cat;

    public void Initialize(IEventBus eventBus,List<Queue<Transform>> tutorialsSteps)
    {
        _evenBus = eventBus;
        _evenBus.Subscribe<MovePlayer>(HideTutorialWindow);
        _evenBus.Subscribe<CatState>(TutorialNextState);
        _evenBus.Subscribe<NextStepTutorial>(TutorialNextStep);
        
        _stepTutorial = new List<Queue<Transform>>(tutorialsSteps.Count);
        _stepTutorial = tutorialsSteps;
        
        _tutorialArrow.Initialize(_bottomPosition, _moveDuration);
       // _tutorialArrow.PlayerOnPlace += NextStepTutorial;
        
        _camera=Camera.main.GetComponent<CamFollower>();

    }

    private void TutorialNextStep(NextStepTutorial obj)
    {
        if(_stepTutorial[_indexPosition].Count>0)
            ShowTutorialArrow(_stepTutorial[_indexPosition].Dequeue());
        else 
        {
            if (_indexPosition > 0)
            {
                _evenBus.Invoke(new CatNextStep());
                Debug.Log("GoNextCatState");
               
            }
            _tutorialArrow.Hide();
            if (_indexPosition == _stepTutorial.Count-2)
            {
                _indexPosition++;
                ShowTutorialArrow(_stepTutorial[_indexPosition].Dequeue());
            }
        }
    }

    private void OnDisable()
    {
        _evenBus.Unsubscribe<MovePlayer>(HideTutorialWindow);
        _evenBus.Unsubscribe<CatState>(TutorialNextState);
    }
    private void TutorialNextState(CatState obj)
    {
        _indexPosition++;
        _stepTutorial[_indexPosition].Enqueue(obj.State);
        _camera.ChangeTarget(obj.State);
      //  ShowTutorialArrow(_stepTutorial[_indexPosition].Dequeue());
    }

    private void HideTutorialWindow(MovePlayer obj)
    {
        _tutorialWindow.SetActive(false);
        _evenBus.Invoke(new TutorialStart());
        ShowTutorialArrow(_stepTutorial[_indexPosition].Dequeue());
    }

    private void ShowTutorialArrow(Transform position)
    {
        _camera.GoToTutorialPosition(position);
        _tutorialArrow.SetToPosition(position);
    }
}