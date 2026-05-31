using UnityEngine;

namespace FR
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        // Process instant effects (dmg, heal)

        // Process timed effects (poison, build ups)

        // Process static effects (adding/removing buffs)

        CharacterManager character;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            effect.ProcessEffect(character);
        }

        public void ProcessTimedEffect()
        {
            
        }

        public void ProcessStaticEffect()
        {
            
        }
    }    
}
