using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BallCollector.CoreSystem
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private string _mainScene;
        [SerializeField] private string[] _levels;

        public int LevelsCount=> _levels.Length;


        private const string _unlockedLevelIndex = "UnlockedLevelIndex";
        private const string _chosenLevelIndex = "ChosenLevelIndex";

        public void UnlockNextLevel()
        {
            var newIndex = PlayerPrefs.GetInt(_unlockedLevelIndex) + 1;
            PlayerPrefs.SetInt(_unlockedLevelIndex, newIndex);
        }

        public void ChoseLevel(int index)
        {
            PlayerPrefs.SetInt(_chosenLevelIndex, index);
        }

        public void LoadMainScene()
        {
            StartCoroutine(LoadScene(_mainScene));
        }
       
        public void LoadLevel()
        {
            var currentIndex = PlayerPrefs.GetInt(_chosenLevelIndex);
            StartCoroutine(LoadScene(_levels[currentIndex]));
        }
        
        private IEnumerator LoadScene(string sceneName)
        {
            
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncLoad.isDone)
            {
                yield return null;
                Debug.Log("tick");
            }
            Debug.Log("loaded");
        }
    }
}