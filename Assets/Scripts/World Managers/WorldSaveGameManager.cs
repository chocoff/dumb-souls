using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FR {

    public class WorldSaveGameManager : MonoBehaviour
    {
        // There can only be one instance of this object per scene
        public static WorldSaveGameManager instance;

        [SerializeField] private int worldSceneIndex = 1;

        private void Awake()
        {

            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        public IEnumerator LoadNewGame()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);    
            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}
