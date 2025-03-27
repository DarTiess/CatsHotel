using System.Collections.Generic;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class IdleFabric
    {
        private List<IdlePoint> _points;

        public IdleFabric(List<IdlePoint> points)
        {
            _points = new List<IdlePoint>();
            _points = points;
        }

        public IdlePoint GetPlacePosition()
        {
            IdlePoint freePosition = null;
            int rnd = Random.Range(0, _points.Count);
            while (!_points[rnd].IsFree)
            {
                rnd = Random.Range(0, _points.Count);
            }
           
            freePosition = _points[rnd];
            _points[rnd].IsOccupied();
            return freePosition;
        }

        public void SetFreePosition(IdlePoint idleState)
        {
            foreach (IdlePoint point in _points)
            {
                if (point.Equals(idleState))
                {
                    point.SetFree();
                    return;
                }
            }
        }
    }
}