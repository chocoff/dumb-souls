using UnityEngine;


namespace FR
{
    public class WorldSoundFXManager : MonoBehaviour
    {
     
        public static WorldSoundFXManager instance;

        [Header("ACTION SOUNDS")]
        public AudioClip rollSFX;
        public AudioClip jumpBackSFX;

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


    }    
}

