using UnityEngine;

namespace FR
{

    public class PlayerMotionManager : CharacterMotionManager
    {

        PlayerManager player;
        // These values are taken from the input manager
        public float verticalMovement;
        public float horizontalMovement;
        public float moveAmount;

        private Vector3 moveDirection;
        [SerializeField] private float walkingSpeed = 2;
        [SerializeField] private float runningSpeed = 5;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }




        public void HandleAllMovement()
        {
            //TODO: Handle ground, jump, rotation movement of the player
            
        }


        private void HandleGroundedMovement()
        {
            // Movement direction is based on player's input (duh) and the perspective of the camera (third person) ~ Note: possible first person camera implementation?
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection += PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0; 

            if (PlayerInputManager.instance.moveAmount > 0.5f)  //Note: remember the 0~1/2~1 clamp for walking/jogging/running (made this way to easily implement controller input)
            {
                
            }
            else if (PlayerInputManager.instance.moveAmount <= 0.5f)
            {
                // move at a walking speed
            }
        }
    }

}