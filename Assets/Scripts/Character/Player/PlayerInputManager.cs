using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FR
{
 
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;  //singleton

        public PlayerManager player;

        private PlayerControls playerControls;

        [Header("CAMERA MOVEMENT INPUT")]
        [SerializeField] private Vector2 cameraInput;
        public float cameraVerticalInput;
        public float cameraHorizontalInput;

        [Header("PLAYER MOVEMENT INPUT")]
        [SerializeField] private Vector2 movementInput;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;    // Referenced in player motion manager, so I just set it to public
        public bool isWalking;

        [Header("PLAYER ACTION INPUT")]
        [SerializeField] private bool dodgeInput = false;
        [SerializeField] private bool sprintInput = false;

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

        private void OnEnable() // When any action is triggered
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                // "WASD", update the Vector2 variable to reflect the values of the key pressed
                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
                
                // Toggle walk binding
                playerControls.PlayerMovement.WalkToggle.performed += ctx => ToggleWalk();

                // "Space" for dodging
                playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;

                // "Left Shift" for sprinting (Hold)
                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;

            }

            playerControls.Enable();
        }

        private void ToggleWalk()
        {
            isWalking = !isWalking;
        }

        // If game window is not focused, do not process input
        private void OnApplicationFocus(bool focus)
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
            HandleAllInputs();
        }

        // INPUT HANDLERS SECTION
        private void HandleAllInputs()
        {
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            HandleSprinting();
        }

        // MOVEMENT
        private void HandlePlayerMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            // Returns the absolute value of the movement
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            // TODO: try testing with and without this clamping logic
            if ((moveAmount <= 0.5 || isWalking) && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5f && moveAmount <= 1)
            {
                moveAmount = 1;
            }

            if (player == null)
                return;
            // 0 in horizontal since we are not locked on (non-strafing movement)
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);

            // If we are locked on, pass the horizontal values as well

        }

        private void HandleCameraMovementInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }
    
        // ACTION
        private void HandleDodgeInput()
        {
            if (dodgeInput)
            {
                dodgeInput = false;
                
                // DO A FLIP!
                player.playerMotionManager.AttemptToPerformDodge();
            }
        }


        private void HandleSprinting()
        {
            if (sprintInput)
            {
                player.playerMotionManager.HandleSprinting();
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
        }
    }
}