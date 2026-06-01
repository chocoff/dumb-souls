using UnityEngine;

namespace FR{
    public class PlayerStatsManager : CharacterStatsManager
    {

        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();

            // When we make a character creation menu, and set stats depending on the class, this will be calculated there
            // Unilt then however, stats are never calc, so we do it here on start, if a save file exists they will be over written when loading into a scnen
            CalculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vitality.Value);
            CalculateStaminaBasedOnEnduraceLevel(player.playerNetworkManager.endurance.Value);
        }

    }
}