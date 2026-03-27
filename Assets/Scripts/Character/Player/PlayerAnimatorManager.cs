using UnityEngine;

namespace FR
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        private PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        private void OnAnimatorMove()   // This function is called every single frame an animation is playing
        {
            if (player.applyRootMotion)
            {
                Vector3 velocity = player.animator.deltaPosition;
                player.characterController.Move(velocity);
                player.transform.rotation *= player.animator.deltaRotation; 
            }
        }
    }
}

