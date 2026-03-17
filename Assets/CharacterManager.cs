using UnityEngine;

namespace FR{
    public class CharacterManager : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);   
        }
    }
}
