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

            // Handle all of the player's movement 
            playerMotionManager.HandleAllMovement();
        }

    }       

}

