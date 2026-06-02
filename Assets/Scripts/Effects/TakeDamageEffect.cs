using UnityEngine;

namespace FR
{

    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]

    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("CHARACTER CAUSING DAMAGE")]
        public CharacterManager characterCausingDamage;         // If the damage is caused by another characters attack it will be stored here

        [Header("DAMAGE")]
        public float physicalDamage = 0;                        // In the future will be split into standard, strike, slash, and pierce
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage= 0;
        public float holyDamage = 0;

        [Header("FINAL DAMAGE")]
        private int finalDamageDealt = 0;                     // The damage the character takes after ALL calculations have been made

        [Header("POISE")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;                      // If a character's poise is broke, will be stunned and play dmg animation

        // BUILD UPS (TODO)
        // Build up effect amounts

        [Header("ANIMATION")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("SOUND FX")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSoundFX;                // Used on top of regular SFX if there is elemental dmg present (magic/fire/lightning/holy)

        [Header("DIRECTION DAMAGE TAKEN FROM")]
        public float angleHitFrom;                              // Used to define what dmg animation to play (move backwards, left, right, etc)
        public Vector3 contactPoint;                            // Used to define where the blood FX instantiate

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            // If character is dead, no additional damage effects should be processed
            if (character.isDead.Value)
                return;

            // Check for "invulnerability"

            CalculateDamage(character);
            // Check which direction DMG came from
            // Play a DMG animation
            // Check for build ups (poison, bleed, etc)
            // Play DMG SOUND FX
            // Play DMG VFX (blood)


            // If character is A.I., check for new target if character causing DMG is present
        }

        private void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            if (characterCausingDamage != null)
            {
                // Check for DMG modifiers and modify base DMG (physical dmg buff, elemental dmg buff)
            }

            // Check character for flat defenses and subtract them from DMG

            // Check for armor absorptions, and subtract the percentage from the DMG

            // Add all DMG types together, and apply final DMG
            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }

            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;

            // Calculate poise DMG to define if the character will be stunned
        }
    }    
}

