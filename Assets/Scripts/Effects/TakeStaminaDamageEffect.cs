using UnityEngine;

namespace FR
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage")]
    public class TakeStaminaDamageEffect : InstantCharacterEffect
    {
        public float staminaDamage;

        public override void ProcessEffect(CharacterManager character)
        {
            CalculateStaminaDamage(character);
        }

        private void CalculateStaminaDamage(CharacterManager character)
        {
            // Compared the base stamina damage against other player effects/modifiers
            // Change the value before substracting/adding it
            // Play sound FX or VFX during FX
            if (character.IsOwner)
            {
                // Debug.Log("CHARACTER IS TAKING: " + staminaDamage + " STAMINA DAMAGE");
                character.characterNetworkManager.currentStamina.Value -= staminaDamage;
            }
        }
    

    }    
}
