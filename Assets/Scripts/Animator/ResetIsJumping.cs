using FR;
using UnityEngine;

namespace FR
{
    public class ResetIsJumping : StateMachineBehaviour
    {
        CharacterManager character;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (character == null)
            {
                character = animator.GetComponent<CharacterManager>();
            }

            // This is called when an action ends, and the state returns to "Empty"
            character.isJumping = false;
        }
    }
}
