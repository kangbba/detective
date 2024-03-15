using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Calcatz.VNDetectiveGUI {
    public class PointerItem : SettingsItem {

        [SerializeField] private Image pointer;
        [SerializeField] private Text[] options;

        private int value = 0;
        private Coroutine pointerCoroutine;

        private void Awake() {
            for(int i=0; i<options.Length; i++) {
                options[i].color = unselectedColor;
            }
            options[value].color = normalColor;
        }

        private void Start() {
            StartCoroutine(InitNextFrame());
        }

        private IEnumerator InitNextFrame() {
            yield return null;
            yield return null;
            ChangeValue(value);
        }

        private IEnumerator PointerCoroutine() {
            Vector2 targetPosition = pointer.transform.position;
            targetPosition.y = options[value].transform.position.y;

            for (float t = 0; t <= 0.2f; t += Time.deltaTime) {
                pointer.transform.position = Vector2.Lerp(pointer.transform.position, targetPosition, Mathf.Clamp01(t / 0.2f));
                yield return null;
            }
        }

        public void ChangeValue(int _value) {
            options[value].color = unselectedColor;
            value = _value;
            if (IsSelected()) {
                options[value].color = highlightColor;
            }
            else {
                options[value].color = normalColor;
            }
            if (pointerCoroutine != null) {
                StopCoroutine(pointerCoroutine);
            }
            pointerCoroutine = StartCoroutine(PointerCoroutine());
            onValueChanged.Invoke(value);
        }

        public override void Select() {
            base.Select();
            pointer.color = highlightColor;
            options[value].color = highlightColor;
        }

        public override void Deselect() {
            base.Deselect();
            pointer.color = normalColor;
            options[value].color = normalColor;
        }

        public override void IncreaseValue() {
            base.IncreaseValue();
            int newValue = value + 1;
            if (newValue >= options.Length) {
                newValue = 0;
            }
            ChangeValue(newValue);
        }

        public override void DecreaseValue() {
            base.DecreaseValue();
            int newValue = value - 1;
            if (newValue < 0) {
                newValue = options.Length - 1;
            }
            ChangeValue(newValue);
        }

    }
}