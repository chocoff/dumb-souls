using UnityEngine;
using Unity.Netcode;
using System.Collections;

namespace FR{
    public class CharacterManager : NetworkBehaviour
    {

        [Header("STATUS")]
        public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;

        [HideInInspector] public CharacterNetworkManager characterNetworkManager;
        [HideInInspector] public CharacterEffectsManager characterEffectsManager;
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;

        [Header("FLAGS")]
        public bool isPerformingAction = false;
        public bool isJumping = false;
        public bool isGrounded = true;
        public bool applyRootMotion = false;
        public bool canRotate = true;
        public bool canMove = true;

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);   

            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        }

        protected virtual void Update()
        {
            animator.SetBool("isGrounded", isGrounded);

            // If the character is controlled from one's side, assign its network position to one's transform
            if (IsOwner)
            {
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            // If it is controlled else where, assign position locally depending on its network transform
            else //donottouchunlessitisstrictlynecessaryplease,yeahIamtalkingtoyoumyfutureself
            {
                // Position Section //
                transform.position = Vector3.SmoothDamp(
                    transform.position, 
                    characterNetworkManager.
                    networkPosition.Value, 
                    ref characterNetworkManager.networkPositionVelocity, 
                    characterNetworkManager.networkPositionSmoothTime
                );
                
                // Rotation Section //
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, 
                    characterNetworkManager.networkRotation.Value, 
                    characterNetworkManager.networkRotationSmoothTime
                );

            }
        }
    
        protected virtual void LateUpdate()
        {
            
        }

        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;

                // Reset any flags that need to be reset
                // NOTHING YET

                // If we are not grounded, play an aerial death animation

                if (!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);;
                }
            }

            // Play some death SFX

            yield return new WaitForSeconds(5);

            // Award player with currency

            // Disable character

        }
    
        public virtual void ReviveCharacter()
        {
            
        }
    }
}
