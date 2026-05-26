using UnityEngine;
using TMPro;

namespace FR
{
    public class UI_Character_Save_Slot : MonoBehaviour
    {
        SaveFileDataWriter saveFileDataWriter;

        [Header("Game Slot")]
        public CharacterSlot characterSlot;

        [Header("Character Info")]
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI timePlayed;

        private void OnEnable()
        {
            LoadSaveSlots();
        }

        private void LoadSaveSlots()
        {
            saveFileDataWriter = new SaveFileDataWriter
            {
                saveDataDirectoryPath = Application.persistentDataPath
            };

            // Save slot 01
            switch(characterSlot)
            {
                case CharacterSlot.CharacterSlot_01:
                    saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                    
                    // If the file exists we get the information from it. Otherwise, disable this game object
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                    
                case CharacterSlot.CharacterSlot_02:
                    saveFileDataWriter.saveFileName = "characterSlot_02";
                    break;
                    
                case CharacterSlot.CharacterSlot_03:
                    saveFileDataWriter.saveFileName = "characterSlot_03";
                    break;
                    
                case CharacterSlot.CharacterSlot_04:
                    saveFileDataWriter.saveFileName = "characterSlot_04";
                    break;
                    
                case CharacterSlot.CharacterSlot_05:
                    saveFileDataWriter.saveFileName = "characterSlot_05";
                    break;
                    
                case CharacterSlot.CharacterSlot_06:
                    saveFileDataWriter.saveFileName = "characterSlot_06";
                    break;
                    
                case CharacterSlot.CharacterSlot_07:
                    saveFileDataWriter.saveFileName = "characterSlot_07";
                    break;
                    
                case CharacterSlot.CharacterSlot_08:
                    saveFileDataWriter.saveFileName = "characterSlot_08";
                    break;
                    
                case CharacterSlot.CharacterSlot_09:
                    saveFileDataWriter.saveFileName = "characterSlot_09";
                    break;
                    
                case CharacterSlot.CharacterSlot_10:
                    saveFileDataWriter.saveFileName = "characterSlot_10";
                    break;

                default:
                    break;
            }


        }
    }    
}

