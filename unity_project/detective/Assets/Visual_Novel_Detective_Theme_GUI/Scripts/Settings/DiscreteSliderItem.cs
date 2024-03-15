using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Calcatz.VNDetectiveGUI {
    public class DiscreteSliderItem : SliderItem {

        [SerializeField] private int valueStep = 1;

        private void Awake() {
            slider.wholeNumbers = true;
            ChangeValue(0);
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