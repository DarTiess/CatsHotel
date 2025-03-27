using Infrastructure.Level.EventsBus;
using Infrastructure.Level.EventsBus.Signals;
using UI.UIPanels;
using UnityEngine;

namespace UI
{
    public class UIControl : MonoBehaviour
    {
        
        [Header("Panels")]
        [SerializeField] private GamePlayWindow _playWindowInGame;
        [SerializeField] private SettingsWindow _settingsWindow;
        [SerializeField] private RewardWindow _rewardWindow;

        private IEventBus _eventBus;
        public void Init(IEventBus eventBus)
        {
            _eventBus =eventBus;
            _eventBus.Subscribe<LevelStart>(OnLevelStart);
            _eventBus.Subscribe<LevelWin>(OnLevelWin);
           _eventBus.Subscribe<LevelLost>(OnLevelLost);

           //  _panelMenu.ClickedPanel += OnPlayGame;
            _rewardWindow.ClickedPanel += OnShowAds;
            _playWindowInGame.ClickedPanel += OnSettingsWindow;
            _settingsWindow.ClickedPanel += OnPlayGame;
            OnLevelStart();
        }

        private void OnLevelLost(LevelLost obj)
        {
            Debug.Log("Level Lost");  
            HideAllPanels();
            _rewardWindow.Show();
        }

        private void OnLevelWin(LevelWin obj)
        {
            Debug.Log("Level Win"); 
            HideAllPanels();
            _settingsWindow.Show(); 
        }

        private void OnLevelStart(LevelStart obj)
        {
            OnLevelStart();
        }

        private void OnDisable()
        {
            _eventBus.Unsubscribe<LevelStart>(OnLevelStart);
            _eventBus.Unsubscribe<LevelWin>(OnLevelWin);
            _eventBus.Unsubscribe<LevelLost>(OnLevelLost);

            //  _panelMenu.ClickedPanel -= OnPlayGame;
            _rewardWindow.ClickedPanel -= OnShowAds;
            _playWindowInGame.ClickedPanel -= OnSettingsWindow;
            _settingsWindow.ClickedPanel -= OnPlayGame;
        }

        private void OnLevelStart()      
        {   
            HideAllPanels();
            _playWindowInGame.Show();
          //  _panelMenu.Show();
        }
        private void OnSettingsWindow()
        {
            _eventBus.Invoke(new PauseGame());
            HideAllPanels(); 
            _settingsWindow.Show();  
        }

        private void OnPlayGame()
        { 
            _eventBus.Invoke(new PlayGame());
            HideAllPanels(); 
            _playWindowInGame.Show();         
        }

        private void OnShowAds()
        {
          // pause game
          //show ads
          //continue game
        }

        private void HideAllPanels()
        {
          //  _panelMenu.Hide();
            _rewardWindow.Hide();
            _settingsWindow.Hide();
            _playWindowInGame.Hide();
        }
    
    }
}
