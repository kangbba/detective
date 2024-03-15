using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Calcatz.VNDetectiveGUI {
    public class EvidenceSectionItem : MonoBehaviour {

        [SerializeField] private Button concludeButton;
        [SerializeField] private GameObject concludeSection;

        private void Awake() {
            InitConcludeSection();

            concludeButton.onClick.AddListener(delegate {
                Conclude();
            });

            ShowConcludeButton();
        }

        /// <summary>
        /// Initialize conclude section choice buttons so that highlighting the button using mouse are the same with selecting by keyboard.
        /// </summary>
        private void InitConcludeSection() {
            Button[] choiceButtons = concludeSection.GetComponentsInChildren<Button>();
            foreach(Button button in choiceButtons) {
                EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                Button btn = button;
                entry.callback.AddListener(delegate {
                    btn.Select();
                });
                trigger.triggers.Add(entry);
            }
        }

        public void Show() {
            gameObject.SetActive(true);
            ShowConcludeButton();
            concludeButton.Select();
        }

        private void ShowConcludeButton() {
            concludeButton.gameObject.SetActive(true);
            concludeSection.SetActive(false);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

        public void Conclude() {
            concludeButton.gameObject.SetActive(false);
            concludeSection.SetActive(true);

            Button nearestButton = concludeSection.GetComponentInChildren<Button>();
            nearestButton.Select();
        }

    }
}
