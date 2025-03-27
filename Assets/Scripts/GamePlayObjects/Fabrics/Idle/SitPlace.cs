using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class SitPlace : MonoBehaviour
    {
        [SerializeField] private Transform _sitPlace;
        public Transform Sit => _sitPlace;
    }
}