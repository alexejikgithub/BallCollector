using UnityEngine;

namespace BallCollector.CoreSystem
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Level[] _levels;

        private Level _currentLevel;

        public void LoadLevel(int levelIndex)
        {
            if (_currentLevel != null)
            {
                // TODO dispose current level
            }
            // TODO instantiate new level
        }
    }
}