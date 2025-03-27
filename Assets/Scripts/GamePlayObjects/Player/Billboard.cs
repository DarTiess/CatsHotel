using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlayObjects.Player
{
    public abstract class Billboard : MonoBehaviour
    {
        [FormerlySerializedAs("_timerCanvas")]
        [SerializeField] protected Canvas _canvas;
        protected Camera _camera;
        protected void Update()
        {
            if(_camera!=null)
                _canvas.transform.LookAt(transform.position + _camera.transform.forward);
        }
        
    }
}