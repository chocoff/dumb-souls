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

        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        //[Header("MOVEMENT SETTINGS")]
        [Header("MOVEMENT SETTINGS")]
        [SerializeField] private float walkingSpeed = 2;
        [SerializeField] private float runningSpeed = 4.5f;
        [SerializeField] private float sprintingSpeed= 8;
        [SerializeField] private float rotationSpeed = 14;
        [SerializeField] private int sprintingStaminaCost = 3;


        [Header("DODGE")]
        private Vector3 rollDirection;
        [SerializeField] private float dodgeStaminaCost = 3;

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
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);

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

            if (player.playerNetworkManager.isSprinting.Value)
            {
                player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
            }
            else
            {
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
    
        public void HandleSprinting()
        {
            if (player.isPerformingAction)
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }

            // If out of stamina, sprint is false
            if (player.playerNetworkManager.currentStamina.Value <= 0)
            {
                player.playerNetworkManager.isSprinting.Value = false;
                return;
            }

            // If moving, sprint is true, otherwise false, this can be a oneliner but meh
            if (moveAmount >= 0.5)
            {
                player.playerNetworkManager.isSprinting.Value = true;
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }

            if (player.playerNetworkManager.isSprinting.Value)
            {
                player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
            }
        }

        public void AttemptToPerformDodge()
        {
            if (player.isPerformingAction)
               return;

            if (player.playerNetworkManager.currentStamina.Value <= 0)
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

            player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;
        }
    

    }

}