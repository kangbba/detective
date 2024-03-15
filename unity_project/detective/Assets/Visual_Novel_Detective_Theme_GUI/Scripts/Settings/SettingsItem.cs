using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Calcatz.VNDetectiveGUI {
    [RequireComponent(typeof(Image))]
    public abstract class SettingsItem : MonoBehaviour {

        [System.Serializable]
        public class FloatEvent : UnityEvent<float> { }

        [SerializeField] private Text titleText;
        [SerializeField] protected Color normalColor;
        [SerializeField] protected Color highlightColor;
        [SerializeField] protected Color unselectedColor;

        public FloatEvent onValueChanged;

        private EventTrigger eventTrigger;
        private bool selected;
        protected bool IsSelected() { return selected; }

        private void Awake() {
            eventTrigger = gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
            pointerEnter.eventID = EventTriggerType.PointerEnter;
            pointerEnter.callback.AddListener(delegate {

            });
            eventTrigger.triggers.Add(pointerEnter);

            EventTrigger.Entry pointerExit = new EventTrigger.Entry();
            pointerExit.eventID = EventTriggerType.PointerExit;
            pointerExit.callback.AddListener(delegate {

            });
            eventTrigger.triggers.Add(pointerExit);
        }

        public virtual void Highlight() {
            titleText.color = highlightColor;
        }

        public virtual void Unhighlight() {
            titleText.color = normalColor;
        }

        public virtual void Select() {
            selected = true;
            Highlight();
        }

        public virtual void Deselect() {
            selected = false;
        }


        public virtual void IncreaseValue() {

        }

        public virtual void DecreaseValue() {

        }
    }
}
