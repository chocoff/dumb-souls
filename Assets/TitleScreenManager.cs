using UnityEngine;
using Unity.Netcode;

namespace FR{

    public class TitleScreenManager : MonoBehaviour
    {
        public void StartNetworkAsHost() {
            NetworkManager.Singleton.StartHost();
        }    
    }
}
