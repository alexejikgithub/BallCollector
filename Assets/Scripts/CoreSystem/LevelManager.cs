using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BallCollector.CoreSystem
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private SceneAsset[] _levels;

        private int _currentLevelIndex;

        public void LoadNextLevel()
        {
            _currentLevelIndex++;
            StartCoroutine(LoadYourAsyncScene());
        }

        public void ReloadLevel()
        {
            StartCoroutine(LoadYourAsyncScene());
        }
        
        private IEnumerator LoadYourAsyncScene()
        {
            // The Application loads the Scene in the background as the current Scene runs.
            // This is particularly good for creating loading screens.
            // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
            // a sceneBuildIndex of 1 as shown in Build Settings.

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_levels[_currentLevelIndex].name);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
                Debug.Log("tick");
            }
            Debug.Log("loaded");
        }
    }
}