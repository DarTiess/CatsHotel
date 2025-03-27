using UnityEngine;


namespace Infrastructure.Level
{
    public class StartGame : MonoBehaviour
    {
        [SerializeField]private LevelLoader _levelLoader;
        
        private void Awake()
        {
            _levelLoader.StartGame();    
        }
     
    }
}
