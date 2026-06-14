using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace FR
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("DAMAGE")]
        public float physicalDamage = 0;                        // In the future will be split into standard, strike, slash, and pierce
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage= 0;
        public float holyDamage = 0;

        [Header("CONTACT POINT")]
        protected Vector3 contactPoint;

        [Header("CHARACTERS DAMAGED")]
        protected List<CharacterManager> characterDamaged = new List<CharacterManager>();

        private void OnTriggerEnter(Collider other)
        {
            // Check the object is a character
            // if (other.gameObject.layer == LayerMask.NameToLayer("Character"))

            CharacterManager damageTarget = other.GetComponent<CharacterManager>();
            
            if (damageTarget != null)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // Check if we can damage this target based on friendly fire

                // Check if target is blocking

                // Check if target is invulnerable

                // DAMAGE TARGET
                DamageTarget(damageTarget);
            }
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
        {
             // We don't want to damage the same target more than once in a single attack
             // Add them to a list that checks before applying damage

             if (characterDamaged.Contains(damageTarget))
                return;

            characterDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage= magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.contactPoint = contactPoint;

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);

               
        }
    }    
}

