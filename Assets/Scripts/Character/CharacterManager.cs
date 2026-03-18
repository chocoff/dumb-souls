using UnityEngine;

namespace FR{
    public class CharacterManager : MonoBehaviour
    {
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);   
        }

        protected virtual void Update()
        {
            
        }
    }
}
