//This is intended to show an illustration of how Evidence UI behaves

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Calcatz.VNDetectiveGUI {
    public class Evidence : FadingMenuBase {

        [SerializeField] private Button[] evidenceButtons;
        [SerializeField] private GameObject examineSection;
        [SerializeField] private Button examineSectionBackButton;
        [SerializeField] private EvidenceSectionItem[] evidenceSectionItems;

        [Header("Control")]
        [SerializeField] private KeyCode leftKey = KeyCode.LeftArrow;
        [SerializeField] private KeyCode rightKey = KeyCode.RightArrow;
        [SerializeField] private KeyCode selectKey = KeyCode.Return;
        [SerializeField] private KeyCode backKey = KeyCode.Escape;

        private int currentEvidenceIndex = -1;

        private void Awake() {

            //Add all fade-able graphics
            InitializeGraphicAlphas(GetComponentsInChildren<Graphic>().ToList());

            for (int i=0; i<evidenceButtons.Length; i++) {
                int index = i;
                if (i < evidenceSectionItems.Length) {
                    evidenceButtons[i].onClick.AddListener(delegate {
                        SelectEvidence(index);
                    });
                }

                EventTrigger trigger = evidenceButtons[i].gameObject.AddComponent<EventTrigger>();
                EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
                pointerEnter.eventID = EventTriggerType.PointerEnter;
                pointerEnter.callback.AddListener(delegate {
                    currentEvidenceIndex = index;
                });
                trigger.triggers.Add(pointerEnter);
            }

            examineSectionBackButton.onClick.AddListener(delegate {
                CloseExamineSection();
            });

            CloseExamineSection();
        }

        private void CloseExamineSection() {
            foreach (EvidenceSectionItem section in evidenceSectionItems) {
                section.Hide();
            }
            examineSection.SetActive(false);
        }

        public void HighlightEvidence(int _index) {
            currentEvidenceIndex = _index;
            evidenceButtons[_index].Select();
        }

        private void SelectEvidence(int _index) {
            if (_index >= 0 && currentEvidenceIndex < evidenceSectionItems.Length) {
                examineSection.SetActive(true);
                evidenceSectionItems[_index].Show();
            }
        }

        protected override void Update() {
            base.Update();
            if (!examineSection.activeSelf) {

                if (Input.GetKeyDown(leftKey)) {
                    int prevIndex = currentEvidenceIndex;
                    do {
                        prevIndex--;
                        if (prevIndex < 0) {
                            prevIndex = evidenceButtons.Length - 1;
                        }
                        if (prevIndex == currentEvidenceIndex) break;
                    } while (!evidenceButtons[prevIndex].gameObject.activeSelf);
                    HighlightEvidence(prevIndex);
                }

                if (Input.GetKeyDown(rightKey)) {
                    int nextIndex = currentEvidenceIndex;
                    do {
                        nextIndex++;
                        if (nextIndex >= evidenceButtons.Length) {
                            nextIndex = 0;
                        }
                        if (nextIndex == currentEvidenceIndex) break;
                    } while (!evidenceButtons[nextIndex].gameObject.activeSelf);
                    HighlightEvidence(nextIndex);
                }

                if (Input.GetKeyDown(selectKey)) {
                    
                }
            }
            else {
                if (Input.GetKeyDown(backKey)) {
                    CloseExamineSection();
                }
                if (Input.GetKeyDown(selectKey)) {
                    
                }
            }
        }
    }
}
