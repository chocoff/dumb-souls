using UnityEngine;

namespace FR
{

    public class PlayerMotionManager : CharacterMotionManager
    {

        private PlayerManager player;
        // These values are taken from the input manager
        public float verticalMovement;
        public float horizontalMovement;
        public float moveAmount;

        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        [SerializeField] private float walkingSpeed = 2;
        [SerializeField] private float runningSpeed = 5;
        [SerializeField] private float rotationSpeed = 14;


        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public void HandleAllMovement()
        {
            //TODO: Handle ground, jump, rotation movement of the player
            HandleGroundedMovement();
            HandleRotation();
        }

        private void GetVerticalAndHorizontalInputs()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;

            // TODO: clamp movement
        }

        private void HandleGroundedMovement()
        {
            GetVerticalAndHorizontalInputs();

            // Movement direction is based on player's input (duh) and the perspective of the camera (third person) ~ Note: possible first person camera implementation?
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection += PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0; 

            // Change speed depending on walk toggle (keyboard) or moveAmount (joystick)
            float speed;

            if (PlayerInputManager.instance.isWalking)
            {
                speed = walkingSpeed;
            }
            else
            {
                speed = PlayerInputManager.instance.moveAmount > 0.5f ? runningSpeed : walkingSpeed;
            }

            player.characterController.Move(moveDirection * speed * Time.deltaTime);
        }
    
        private void HandleRotation()
        {
            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;

            // Default to current rotation direction
            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }

            // Add rotation
            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
    }

}