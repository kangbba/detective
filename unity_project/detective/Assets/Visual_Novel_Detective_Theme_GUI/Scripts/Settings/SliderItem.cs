using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Calcatz.VNDetectiveGUI {

    public abstract class SliderItem : SettingsItem {

        [SerializeField] protected Slider slider;

        public virtual void ChangeValue(float _value) {
            slider.value = _value;
            onValueChanged.Invoke(_value);
        }

    }
}
