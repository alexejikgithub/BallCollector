using UnityEngine;

namespace BallCollector.CoreSystem
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LevelManager _levelManager;


        public void OnLevelDone(bool isWin)
        {
            if (isWin)
            {
                _levelManager.LoadNextLevel();
            }
            else
            {
                _levelManager.ReloadLevel();
            }
        }
    }
}