using System.Collections.Generic;
using GamePlayObjects.Fabrics;
using UnityEngine;

namespace DefaultNamespace
{
    public class IdlePointsSettings : MonoBehaviour
    {
        [SerializeField] private List<IdlePoint> _idlePoints;
        public List<IdlePoint> IdlePoints => _idlePoints;
    }
}