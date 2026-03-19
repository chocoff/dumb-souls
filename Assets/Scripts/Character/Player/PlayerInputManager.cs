using UnityEngine;
using UnityEngine.SceneManagement;

namespace FR
{
 
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;  //singleton
        private PlayerControls playerControls;

        [SerializeField] private Vector2 movementInput;
        [SerializeField] private float verticalInput;
        [SerializeField] private float horizontalInput;
        public float moveAmount;    // Referenced in player motion manager, so I just set it to public
        [SerializeField] private bool walkToggle;

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

        private void Update()
        {
            HandleMovementInput();
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

                // Toggle walk input
                playerControls.PlayerMovement.ToggleWalk.performed +=  i => walkToggle = !walkToggle;
            }

            playerControls.Enable();
        }

        private void OnDestroy()
        {
            // If this object is destroyed, unsubscribe from this event
            SceneManager.activeSceneChanged -= onSceneChange;
        }

        private void HandleMovementInput()
        {
           
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            // Returns absolute number 
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            // Move amount will always be 0, 1/2, or 1  (todo: create toggle/hold walk button so we can work on 0~0.5 moveAmount)
            if ((moveAmount <= 0.5  && moveAmount > 0) || walkToggle)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1.0f;
            }
            /*
                NOTE: Check this when testing for controller
                Since the 0.49 or 0.51 values are possible with a joystick, it may feel unnatural or snappy due the logic behind the moveAmount delimitation
                A possible solution to this (AI-generated) is the following snippet:

                    // 1. First, check if we are actually moving
                    if (moveAmount > 0)
                    {
                        // 2. If the toggle is on, we FORCE the cap at 0.5
                        if (walkToggle)
                        {
                            moveAmount = Mathf.Clamp(moveAmount, 0, 0.5f);
                        }
                        else 
                        {
                            // 3. If toggle is OFF and using a keyboard (binary 1.0), 
                            // we keep it at 1.0. If using a stick, we let the raw value through.
                            if (moveAmount > 0.5f) 
                                moveAmount = 1.0f; 
                            else 
                                moveAmount = 0.5f;
                        }
                    }
            */
        }
    }
   
}