using UnityEngine;

namespace FR
{
    public class InstantCharacterEffect : ScriptableObject
    {
        [Header("EFFECT ID")]
        public int instantEffectID;

        public virtual void ProcessEffect(CharacterManager character)
        {
            
        }
    }    
}

