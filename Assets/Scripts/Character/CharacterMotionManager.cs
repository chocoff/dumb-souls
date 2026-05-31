using UnityEngine;

namespace FR {    

    public class CharacterMotionManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("GROUND CHECK & JUMPING")]
        [SerializeField] private float gravityForce = -5.55f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckSphereRadius = 0.3f;
        [SerializeField] protected Vector3 yVelocity;               // Force at which character is pulled up or down (jumping or falling)
        [SerializeField] protected float groundedYVelocity = -20;   // Force at which character is sticking to ground while grounded
        [SerializeField] protected float fallStartYVelocity = -5;   // Force at which character begins to fall
        protected bool fallingVelocityHasBeenSet = false;
        protected float inAirTimer = 0;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {
            HandleGroundCheck();

            if (character.isGrounded)
            {
                // If we are not trying to jump or move upward...
                if (yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                // If not jumping and falling velocity has not been set...
                if (!character.isJumping && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }

                inAirTimer += Time.deltaTime;
                character.animator.SetFloat("InAirTimer", inAirTimer);

                yVelocity.y += gravityForce * Time.deltaTime;
            }

            // There should always be some force applied to the Y velocity of the character
            character.characterController.Move(yVelocity * Time.deltaTime);

        }

        protected void HandleGroundCheck()
        {
            character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
        }


        // Draws our ground check sphere in scene view
        protected void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
        }
    }
    
}
