using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Calcatz.VNDetectiveGUI {
    public class ContinuousSliderItem : SliderItem {

        [SerializeField] private float valueStep = 1;
        [SerializeField] private Text valueText;

        private void Awake() {
            ChangeValue(0);
        }

        public override void ChangeValue(float _value) {
            base.ChangeValue(_value);
            float value = slider.value / slider.maxValue;
            valueText.text = (Mathf.RoundToInt(value * 100)) + "%";
            if (value > 0.4f) {
                valueText.color = unselectedColor;
            }
            else {
                valueText.color = normalColor;
            }
        }

        public override void IncreaseValue() {
            base.IncreaseValue();
            ChangeValue(slider.value + valueStep);
        }

        public override void DecreaseValue() {
            base.DecreaseValue();
            ChangeValue(slider.value - valueStep);
        }
    }
}