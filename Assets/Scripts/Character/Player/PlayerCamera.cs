using UnityEngine;

namespace FR
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance; //singleton 

        public Camera cameraObject;

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
