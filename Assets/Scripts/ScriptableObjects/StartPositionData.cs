using System.Collections.Generic;
using UnityEngine;

namespace BallCollector.ScriptableObjects
{
    [CreateAssetMenu(fileName = "StartPositionData", menuName = "ScriptableObjects/StartPositionData", order = 1)]
    public class StartPositionData : ScriptableObject
    {
        public List<Vector3> StartPositions;
        
    }
}