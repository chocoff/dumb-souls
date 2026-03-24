using UnityEngine;

namespace FR

{

    public class PlayerManager : CharacterManager
    {

        PlayerMotionManager playerMotionManager;

        protected override void Awake()
        {
            base.Awake();   // Get all functionality from the base class 

            playerMotionManager = GetComponent<PlayerMotionManager>();
        }

        protected override void Update()
        {
            base.Update();

            // If we are not the owner of this object, we don't control/edit it
            if (!IsOwner)
                return;

            // Handle all of the player's movement 
            playerMotionManager.HandleAllMovement();
        }

        protected override void LateUpdate()
        {
            if (!IsOwner)
                return;

            base.LateUpdate();

            PlayerCamera.instance.HandleAllCameraActions();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // If this is the player object owned by this client
            if (IsOwner)
            {
                PlayerCamera.instance.player = this;
            }
        }
    }       

}

