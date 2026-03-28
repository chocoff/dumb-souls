using UnityEngine;

namespace FR{
    public class CharacterStatsManager : MonoBehaviour
    {

        CharacterManager character;

        [Header("STAMINA REGENERATION")]
        [SerializeField] private int staminaRegenerationAmount = 1;
        private float staminaRegenerationTimer = 0;
        private float staminaTickTimer = 0;
        [SerializeField] private float staminaRegenerationDelay = 1;


        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public int CalculateStaminaBasedOnEnduraceLevel(int endurance)
        {
            float stamina = 0;

            // Create an equation for how the stamina be calculated

            stamina = endurance * 10;

            return Mathf.RoundToInt(stamina);
        }

        public virtual void RegenerateStamina()
        {
            // Only owners can edit their network variable, in this case, stamina
            if (!character.IsOwner)
                return;

            if (character.characterNetworkManager.isSprinting.Value)  // Do not regen if sprinting
                return;

            if (character.isPerformingAction)     // Do not regen if an action is ongoing (jump, dodge, attack)
                return;

            staminaRegenerationTimer += Time.deltaTime;

            if (staminaRegenerationTimer >= staminaRegenerationDelay)
            {
                if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
                {
                    staminaTickTimer += Time.deltaTime;

                    if (staminaTickTimer >= 0.1)
                    {
                        staminaTickTimer = 0;
                        character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                    }
                }
            }
            
        }
    
        public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
        {
            // Reset the regen if the action used stamina or if we already regen stamina
            if (currentStaminaAmount < previousStaminaAmount)
            {
                staminaRegenerationTimer =  0;            
            }
        }
    }
}
