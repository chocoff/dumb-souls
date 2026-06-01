using UnityEngine;
using UnityEngine.UI;

namespace FR
{
    public class UI_StatBar : MonoBehaviour
    {

        private Slider slider;
        private RectTransform rectTransform;

        [Header("BAR OPTIONS")]
        [SerializeField] protected bool scaleBarLengthWithStats = true;
        [SerializeField] protected float widthScaleMultiplier = 1;
        // Secondary bar may bar for polish effect (yellow bar that shows how much an action/damage takes away from current stat)


        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
            rectTransform = GetComponent<RectTransform>();
        }

        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;

            if (scaleBarLengthWithStats)
            {
                rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);
                
                // Resets position of bars based on their layout group's settings
                PlayerUIManager.instance.playerUIHUDManager.RefreshHUD();
            }
        }

    }    
}

