using UnityEngine;

namespace BallCollector.CoreSystem
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LevelManager _levelManager;
        
        
        public void LevelDone(bool isWin)
        {
            if (isWin)
            {
                _levelManager.UnlockNextLevel();
            }
           
            _levelManager.LoadMainScene();
        }

        public void ChoseLevel(int index)
        {
            _levelManager.ChoseLevel(index);
        }

        public void PlayChosenLevel()
        {
            _levelManager.LoadLevel();
        }
        
        
    }
}