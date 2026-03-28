using UnityEngine;

namespace FR
{
    // Not attached to any game object, so no need for mono behaviour
    [System.Serializable]   // Reference this data for every save file
    public class CharacterSaveData
    {
        [Header("CharacterName")]
        public string characterName;

        [Header("TIME PLAYED")]
        public float secondsPlayed;

        // Use common types for JSON (float > Vector3)
        [Header("WORLD COORDS")]
        public float xPosition;
        public float yPosition;
        public float zPosition;
    }

}
