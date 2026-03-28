using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FR {

    public class WorldSaveGameManager : MonoBehaviour
    {
        // There can only be one instance of this object per scene
        public static WorldSaveGameManager instance;

        [SerializeField] private PlayerManager player;

        [Header("SAVE/LOAD")]
        [SerializeField] private bool saveGame;
        [SerializeField] private bool loadGame;

        [Header("WOLRD SCENE INDEX")]
        [SerializeField] private int worldSceneIndex = 1;

        [Header("SAVE DATA WRITER")]
        private SaveFileDataWriter saveFileDataWriter;

        [Header("CURRENT CHARACTER DATA")]
        public CharacterSlot currentCharacterSlotBeingUsed;
        public CharacterSaveData currentCharacterData;
        private string saveFileName;

        [Header("CHARACTER SLOTS")]
        public CharacterSaveData characterSlot01;
        public CharacterSaveData characterSlot02;
        public CharacterSaveData characterSlot04;
        public CharacterSaveData characterSlot03;
        public CharacterSaveData characterSlot05;


        private void Awake()
        {

            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (saveGame)
            {
                saveGame = false;
                SaveGame();
            }

            if (loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        private void DecideCharacterFileNameBasedOnCharacterSlotBeingUsed()
        {
            switch (currentCharacterSlotBeingUsed)
            {
                case CharacterSlot.CharacterSlot_01:
                    saveFileName = "characterSlot_01";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    saveFileName = "characterSlot_02";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    saveFileName = "characterSlot_03";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    saveFileName = "characterSlot_04";
                    break;
                case CharacterSlot.CharacterSlot_05:
                    saveFileName = "characterSlot_05";
                    break;
                default:
                    break;
            }
        }

        public void CreateNewGame()
        {
            // Create a new file, name depends on which slot is used
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

            currentCharacterData = new CharacterSaveData();
        }

        public void LoadGame()
        {
            // Load a file, name depends on which slot is used
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

            saveFileDataWriter = new SaveFileDataWriter
            {
                saveDataDirectoryPath = Application.persistentDataPath,  // General datapath used across many computers
                saveFileName = saveFileName
            };
            currentCharacterData = saveFileDataWriter.LoadSaveFile();

            StartCoroutine(LoadWorldScene());
        }

        public void SaveGame()
        {
            // Save the current file (depends again on the slot used)
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

            saveFileDataWriter = new SaveFileDataWriter
            {
                saveDataDirectoryPath = Application.persistentDataPath,
                saveFileName = saveFileName
            };

            // Pass the player's info, game --> save file
            player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

            // Write on JSON, saved to this computer
            saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
        }

        public IEnumerator LoadWorldScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);    
            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}
