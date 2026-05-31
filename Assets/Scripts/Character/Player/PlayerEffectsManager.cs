using UnityEngine;

namespace FR
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        [Header("DEBUG DELETE LATER")]
        [SerializeField] private InstantCharacterEffect effectToTest;
        [SerializeField] private bool processEffect = false;

        private void Update()
        {
            if (processEffect)
            {
                processEffect = false;
                InstantCharacterEffect effect = Instantiate(effectToTest);
                ProcessInstantEffect(effect);
                /*NOTE
                When we instantiate it, the original is not affected
                    TakeStaminaDamageEffect effect = Instantiate(effectToTest) as TakeStaminaDamageEffect;
                    effect.staminaDamage = 1;
                
                
                When we don't instantiate it, the original is changed (we don't want this in most cases but worth noting)
                    TakeStaminaDamageEffect effect = Instantiate(effectToTest) as TakeStaminaDamageEffect;
                    effect.staminaDamage = 1;
                */
            }
        }
    }   
}
