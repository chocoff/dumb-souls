using UnityEngine;
using System;
using System.IO;

namespace FR
{
    public class SaveFileDataWriter
    {
        public string saveDataDirectoryPath = "";
        public string saveFileName = "";

        // Before creating a new file, we check if one already exists
        public bool CheckToSeeIfFileExists()
        {
            if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    
        // Delete character save files
        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
        }

        // Create a save file upon starting a new game
        public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
        {
            // Define a path to save the file
            string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

            try
            {
                // Create the directory the file will be written to (if it doesnt exists yet)
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("CREATING SAVE FILE, AT SAVE PATH: " + savePath);

                // Serialize C# game data object into JSON
                string dataToStore = JsonUtility.ToJson(characterData, true);

                // Write file to system
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter fileWriter = new StreamWriter(stream))
                    {
                        fileWriter.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("ERROR WHILE SAVING CHARACTER DATA, GAME NOT SAVED" + savePath + "\n" + e);
            }
        }

        // Load a save file upon loading a previous game
        public CharacterSaveData LoadSaveFile()
        {
            CharacterSaveData characterData = null;

            string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

            if (File.Exists(loadPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                    {
                        using (StreamReader fileReader = new StreamReader(stream))
                        {
                            dataToLoad = fileReader.ReadToEnd();
                        }
                    }

                    // Deserialize data from JSON back to unity
                    characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);                    
                }
                catch (Exception e)
                {
                    Debug.LogError("ERROR WHILE LOADING CHARACTER DATA, GAME NOT LOADED" + loadPath + "\n" + e);
                }

            }

            return characterData;
        }
    }    
}

