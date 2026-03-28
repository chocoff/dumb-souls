using UnityEngine;
using UnityEngine.UI;

namespace FR
{
    public class UI_StatBar : MonoBehaviour
    {

        private Slider slider;
        // Variable to scale bar size depending on stat (higher stat = longer bar)
        // Secondary bar may bar for polish effect (yellow bar that shows how much an action/damage takes away from current stat)


        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
        }

        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }

    }    
}

