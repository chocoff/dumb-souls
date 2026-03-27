using UnityEngine;

namespace FR
{

    public class PlayerMotionManager : CharacterMotionManager
    {

        private PlayerManager player;
        // These values are taken from the input manager
        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float moveAmount;

        [Header("MOVEMENT SETTINGS")]
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        [SerializeField] private float walkingSpeed = 2;
        [SerializeField] private float runningSpeed = 5;
        [SerializeField] private float rotationSpeed = 14;


        [Header("DODGE")]
        private Vector3 rollDirection;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (player.IsOwner)
            {
                player.characterNetworkManager.verticalMovement.Value = verticalMovement;
                player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
                player.characterNetworkManager.moveAmount.Value = moveAmount;
            }
            else
            {
                verticalMovement = player.characterNetworkManager.verticalMovement.Value;
                horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
                moveAmount = player.characterNetworkManager.moveAmount.Value;

                // If not locked, pass move amount
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);

                // If locked on, pass horizontal and vertical value
            }
        }

        public void HandleAllMovement()
        {
            //TODO: Handle jump
            HandleGroundedMovement();
            HandleRotation();
        }

        private void GetMovementValues()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
            moveAmount = PlayerInputManager.instance.moveAmount;

            // TODO: clamp movement
        }

        private void HandleGroundedMovement()
        {
            if (!player.canMove)
                return;

            GetMovementValues();


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
            if (!player.canRotate)
                return;
            
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
    
        public void AttemptToPerformDodge()
        {
            if (player.isPerformingAction)
               return;

            // If player is moving when dodge is triggered, roll. Else, backstep
            if (PlayerInputManager.instance.moveAmount > 0)
            {
                rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
                rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
                rollDirection.y = 0;                // We don't want to roll upwards kekw
                rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;
                
                // DO A ROLL
                player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true, true);
            }
            else
            {
                player.playerAnimatorManager.PlayTargetActionAnimation("Jump_Back_01", true, true);

            }
        }
    }

}