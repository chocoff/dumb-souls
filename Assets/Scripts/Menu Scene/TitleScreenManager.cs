using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace FR
{
    public class TitleScreenManager : MonoBehaviour
    {
        [Header("MENU OBJECTS")]
        [SerializeField] GameObject titleScreenMainMenu;
        [SerializeField] GameObject titleScreenLoadMenu;

        [Header("BUTTONS")]
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button mainMenuLoadGameButton;


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

            // Select the return button first
            loadMenuReturnButton.Select();

        }
    
        public void CloseLoadGameMenu()
        {
            // Close load menu, open main menu
            titleScreenMainMenu.SetActive(true);
            titleScreenLoadMenu.SetActive(false);

            // Select the load button
            mainMenuLoadGameButton.Select();
        }
    }
}
