using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace FR
{
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager Instance;

        [Header("MENU OBJECTS")]
        [SerializeField] GameObject titleScreenMainMenu;
        [SerializeField] GameObject titleScreenLoadMenu;

        [Header("BUTTONS")]
        [SerializeField] Button mainMenuNewGameButton;
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button mainMenuLoadGameButton;

        [Header("POP UPS")]
        [SerializeField] GameObject noCharacterSlotsPopUp;
        [SerializeField] Button noCharacterSlotsOkButton;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }   
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame()
        {
            WorldSaveGameManager.instance.AttemptToCreateNewGame();
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
    
        public void DisplayNoFreeCharacterSlotPopUp()
        {
            noCharacterSlotsPopUp.SetActive(true);
            noCharacterSlotsOkButton.Select();
        }
    
        public void CloseNoFreeCharacterSlotPopUp()
        {
            noCharacterSlotsPopUp.SetActive(false);
            mainMenuLoadGameButton.Select();
            
        }
    }
}
