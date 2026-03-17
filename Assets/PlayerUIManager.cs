using UnityEngine;
using Unity.Netcode;

namespace FR{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance;

        [Header("NETWORK JOIN")]
        [SerializeField] bool startGameAsClient;

        private void Awake()
        {
        // There should always be only one instance
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

        private void Update()
        {
            if (startGameAsClient)
            {
                startGameAsClient = false;

                // Shut down the network as a host to start is as a client
                NetworkManager.Singleton.Shutdown();

                // Then start the network as a client
                NetworkManager.Singleton.StartClient();
            }
        }
    
    }
}
