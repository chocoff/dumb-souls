using UnityEngine;
using UnityEngine.SceneManagement;

namespace FR
{
 
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;  //singleton
        private PlayerControls playerControls;

        [Header("PLAYER MOVEMENT INPUT")]
        [SerializeField] private Vector2 movementInput;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;    // Referenced in player motion manager, so I just set it to public
        public bool isWalking;

        [Header("CAMERA MOVEMENT INPUT")]
        [SerializeField] private Vector2 cameraInput;
        public float cameraVerticalInput;
        public float cameraHorizontalInput;

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
            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false;
        }

        private void OnSceneChange(Scene oldScene, Scene newScene) 
        {
            // If world scene is loaded, enable our player controls 
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
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
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
                

                // Toggle walk binding
                playerControls.PlayerMovement.WalkToggle.performed += ctx => ToggleWalk();
            }

            playerControls.Enable();
        }

        private void ToggleWalk()
        {
            isWalking = !isWalking;
        }

        // If game window is not focused, do not process input
        private void OplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }          
        }

        private void OnDestroy()
        {
            // If this object is destroyed, unsubscribe from this event
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        private void Update()
        {
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
        }

        private void HandlePlayerMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            // Returns the absolute value of the movement
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            // TODO: try testing with and without this clamping logic
            if (moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5f && moveAmount <= 1)
            {
                moveAmount = 1;
            }
        }

        private void HandleCameraMovementInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }
    }
}