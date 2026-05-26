using UnityEngine;
using Unity.Netcode;

namespace FR
{
    public class TitleScreenManager : MonoBehaviour
    {
        [SerializeField] GameObject titleScreenMainMenu;
        [SerializeField] GameObject titleScreenLoadMenu;

        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame()
        {
            WorldSaveGameManager.instance.CreateNewGame();
            StartCoroutine(WorldSaveGameManager.instance.LoadWorldScene());
        }    
    
        public void OpenLoadGameMenu()
        {
            // Close main menu, open load menu
            titleScreenMainMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);

            // 
        }
    }
}
