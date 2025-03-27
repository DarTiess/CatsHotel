using GamePlayObjects.Player;
using Infrastructure.Input;
using Infrastructure.Level.EventsBus;
using UnityEngine;

namespace DefaultNamespace
{
    public class Factory
    {
        private ObjectPoole<PlayerView> _playerPoole;

        public PlayerView CreatePlayer(IInputService input,PlayerConfig playerConfig,IEventBus eventBus, Transform spawnPosition)
        {
            _playerPoole = new ObjectPoole<PlayerView>();
            _playerPoole.CreatePool(playerConfig.Prefab,1, null);
            PlayerView player = _playerPoole.GetObject();
            player.transform.position = spawnPosition.position;
            player.Init(input, playerConfig, eventBus);
            return player;
        }
    }
}