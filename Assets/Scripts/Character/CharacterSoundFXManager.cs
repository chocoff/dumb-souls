using UnityEngine;

namespace FR
{
    public class CharacterSoundFXManager : MonoBehaviour
    {

        private AudioSource audioSource;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayRollSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSFX);
        }

        public void PlayJumpBackSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.jumpBackSFX);
        }
    }
    
}
