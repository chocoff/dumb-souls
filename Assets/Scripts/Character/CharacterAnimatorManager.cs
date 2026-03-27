using UnityEngine;
using Unity.Netcode;

namespace  FR
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        private float vertical;
        private float horizontal;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue)
        {
            character.animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
            character.animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
        }

        // isPerformingAction is for disabling actions/animations that we don't want to interrupt (ex: don't roll while already rolling)
        public virtual void PlayTargetActionAnimation(
            string targetAnimation, 
            bool isPerformingAction, 
            bool applyRootMotion = true, 
            bool canRotate = false, 
            bool canMove = false)
        {
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);    //blend the animation over a 0.2f time
            // May be used to stop character from attempting new actions
            character.isPerformingAction = isPerformingAction;
            character.canRotate = canRotate;
            character.canMove = canMove;

            // Host needs to know when an animation is played by others
            character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }
    }

}
