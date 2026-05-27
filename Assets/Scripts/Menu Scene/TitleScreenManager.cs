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
        [SerializeField] Button deleteCharacterPopUpConfirmButton;
        
        // The following variable will hold refs to the 10 slot buttons in order (when available)
        [SerializeField] Button[] characterSlotButtons; 

        [Header("POP UPS")]
        [SerializeField] GameObject noCharacterSlotsPopUp;
        [SerializeField] Button noCharacterSlotsOkButton;
        [SerializeField] GameObject deleteCharacterSlotPopUp;

        [Header("CHARCTER SLOTS")]
        public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;

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
    
        // Character slots related

        public void SelectCharacterSlot(CharacterSlot characterSlot)
        {
            currentSelectedSlot = characterSlot;
        }

        public void SelectNoSlot()
        {
            currentSelectedSlot = CharacterSlot.NO_SLOT;
        }

        public void AttemptToDeleteCharacterSlot()
        {
            if (currentSelectedSlot != CharacterSlot.NO_SLOT)
            {
                deleteCharacterSlotPopUp.SetActive(true);          
                deleteCharacterPopUpConfirmButton.Select();      
            }
        }

        public void DeleteCharacterSlot()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            WorldSaveGameManager.instance.DeleteGame(currentSelectedSlot);    
            
            // Disable and enable load menu to refresh the slots (after deletion)
            titleScreenLoadMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);
            
            // deleteCharacterPopUpConfirmButton.Select();
            SelectNoSlot();                     //clear the stale slot ref
            SelectFirstAvailableSlotOrReturn(); // focus a real, visible button
        }

        public void CloseDeleteCharacterPopUp()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            loadMenuReturnButton.Select();
        }
 
        private void SelectFirstAvailableSlotOrReturn()
        {
            foreach (Button slotButton in characterSlotButtons)
            {
                if (slotButton != null && slotButton.gameObject.activeInHierarchy)
                {
                    slotButton.Select();
                    return;
                }
            }
            loadMenuReturnButton.Select();
        }
    }
}
