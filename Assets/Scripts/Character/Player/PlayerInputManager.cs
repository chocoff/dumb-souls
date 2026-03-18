using UnityEngine;
using UnityEngine.SceneManagement;

namespace FR
{
 
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;  //singleton
        private PlayerControls playerControls;
        [SerializeField] private Vector2 movementInput;


        private void Awake()
        {
            // Singleton / Single instance validation
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

            // When the scene changes, run this logic
            SceneManager.activeSceneChanged += onSceneChange;

            instance.enabled = false;
        }

        private void onSceneChange(Scene oldScene, Scene newScene) 
        {
            // If world scene is loaded, enable our player controls 
            if (newScene.buildIndex == WorldSaveGameManager.instance.getWorldSceneIndex())
            {
                instance.enabled = true;    
            }
            // Otherwise, we don't want to use the player controls since we are in a menu
            else
            {
                instance.enabled = false;
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                // Whenever we press up or down keys, update the Vector2 variable to reflect the values of the key pressed
                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            }

            playerControls.Enable();
        }

        private void OnDestroy()
        {
            // If this object is destroyed, unsubscribe from this event
            SceneManager.activeSceneChanged -= onSceneChange;
        }
    }
   
}