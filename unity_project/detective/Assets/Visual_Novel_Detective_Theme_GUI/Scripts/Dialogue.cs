//This is intended to show an illustration of how Dialogue UI behaves

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Calcatz.VNDetectiveGUI {
    public class Dialogue : FadingMenuBase {

        [System.Serializable]
        public class ChoiceButtonEvent : UnityEvent<int> {}

        [System.Serializable]
        public class Message {
            public string name;
            public Sprite image;
            public string content;
            public string showChoice;
        }

        [System.Serializable]
        public class Choice {
            public string text;
            public UnityEvent onSelect;
        }

        [System.Serializable]
        public class Choices {
            public string id;
            public Choice[] choices;
        }

        [Header("Control")]
        [SerializeField] private KeyCode choiceUpKey = KeyCode.UpArrow;
        [SerializeField] private KeyCode choiceDownKey = KeyCode.DownArrow;
        [SerializeField] private KeyCode selectKey = KeyCode.Return;

        [Header("Choices")]
        [SerializeField] private GameObject choiceSection;
        [SerializeField] private RectTransform pointer;
        [SerializeField] private float pointerMoveDuration = 0.25f;
        [SerializeField] private Button[] choiceButtons;
        [SerializeField] private ChoiceButtonEvent onClickChoiceButton;

        [Header("Texts")]
        [SerializeField] private float textFadeDuration = 0.25f;
        [SerializeField] private Button nextMessageTriggerArea;
        [SerializeField] private Text nameText;
        [SerializeField] private Text contentText;
        [SerializeField] private Image characterImage;
        [SerializeField] private List<Choices> choices = new List<Choices>();
        [SerializeField] private List<Message> messages = new List<Message>();

        private int currentChoiceIndex = 0;
        private int currentMessageIndex = 0;
        private Coroutine pointerCoroutine;
        private Coroutine changeMessageCoroutine;
        private Text[] choiceTexts;
        private Dictionary<string, Choice[]> choiceDictionary;
        private UnityAction<int> onClickChoiceTemp;

        private void Awake() {
            //Add all fade-able graphics
            InitializeGraphicAlphas(GetComponentsInChildren<Graphic>().ToList());

            choiceTexts = new Text[choiceButtons.Length];

            for (int i=0; i<choiceButtons.Length; i++) {
                int index = i;
                choiceButtons[i].onClick.AddListener(delegate {
                    onClickChoiceButton.Invoke(index);
                    onClickChoiceTemp.Invoke(index);
                });

                choiceTexts[i] = choiceButtons[i].GetComponentInChildren<Text>();

                EventTrigger trigger = choiceButtons[i].gameObject.AddComponent<EventTrigger>();
                EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
                pointerEnter.eventID = EventTriggerType.PointerEnter;
                pointerEnter.callback.AddListener(delegate {
                    MovePointer(index);
                });
                trigger.triggers.Add(pointerEnter);
            }

            choiceDictionary = new Dictionary<string, Choice[]>();
            foreach(Choices choice in choices) {
                choiceDictionary.Add(choice.id, choice.choices);
            }

            nextMessageTriggerArea.onClick.AddListener(delegate {
                NextMessage();
            });
        }

        private void Start() {
            ChangeMessageIndex(currentMessageIndex);
        }

        public void MovePointer(int _index) {
            choiceButtons[_index].Select();
            currentChoiceIndex = _index;
            Vector2 pos = pointer.anchoredPosition;
            pos.y = ((RectTransform)choiceButtons[_index].transform).anchoredPosition.y;
            if (pointerCoroutine != null) {
                StopCoroutine(pointerCoroutine);
            }
            pointerCoroutine = StartCoroutine(MovePointer(pointer, pos));
        }

        private IEnumerator MovePointer(RectTransform _rt, Vector2 _targetPosition) {
            for (float t = 0; t <= pointerMoveDuration; t += Time.deltaTime) {
                _rt.anchoredPosition = Vector2.Lerp(_rt.anchoredPosition, _targetPosition, Mathf.Clamp01(t / pointerMoveDuration));
                yield return null;
            }
            pointerCoroutine = null;
        }

        public void ChangeMessageIndex(int _index) {
            if (changeMessageCoroutine != null) {
                StopCoroutine(changeMessageCoroutine);
            }
            changeMessageCoroutine = StartCoroutine(ChangeMessageCoroutine(_index));
        }

        private IEnumerator ChangeMessageCoroutine(int _messageIndex) {

            Color nameColor = nameText.color;
            Color contentColor = contentText.color;

            for (float t = 0; t <= textFadeDuration; t += Time.deltaTime) {
                Color nameColorTarget = nameColor;
                nameColorTarget.a = 0;
                Color contentColorTarget = contentColor;
                contentColorTarget.a = 0;

                nameText.color = Color.Lerp(nameText.color, nameColorTarget, Mathf.Clamp01(t / textFadeDuration));
                contentText.color = Color.Lerp(contentText.color, contentColorTarget, Mathf.Clamp01(t / textFadeDuration));
                yield return null;
            }

            currentMessageIndex = _messageIndex;
            Message message = messages[currentMessageIndex];
            nameText.text = message.name;
            contentText.text = message.content;
            characterImage.sprite = message.image;

            if (message.showChoice != null && message.showChoice != "") {
                choiceSection.SetActive(true);
                MovePointer(0);
                if (choiceDictionary.ContainsKey(message.showChoice)) {
                    Choice[] choices = choiceDictionary[message.showChoice];
                    for (int i = 0; i < choiceTexts.Length; i++) {
                        if (i >= choices.Length) {
                            choiceButtons[i].gameObject.SetActive(false);
                        }
                        else {
                            choiceButtons[i].gameObject.SetActive(true);
                            choiceTexts[i].text = choices[i].text;
                        }
                    }
                    onClickChoiceTemp = (_index) => {
                        if (choices[_index].onSelect != null) {
                            choices[_index].onSelect.Invoke();
                        }
                    };
                }
            }
            else {
                choiceSection.SetActive(false);
            }


            for (float t = 0; t <= textFadeDuration; t += Time.deltaTime) {
                nameText.color = Color.Lerp(nameText.color, nameColor, Mathf.Clamp01(t / textFadeDuration));
                contentText.color = Color.Lerp(contentText.color, contentColor, Mathf.Clamp01(t / textFadeDuration));
                yield return null;
            }
        }

        public void NextMessage() {
            if (choiceSection.activeSelf) {
                return;
            }
            int nextIndex = currentMessageIndex + 1;
            if (nextIndex < messages.Count) {
                ChangeMessageIndex(nextIndex);
            }
        }

        protected override void Update() {
            base.Update();
            if (choiceSection.activeSelf) {
                if (Input.GetKeyDown(choiceUpKey)) {
                    int prevIndex = currentChoiceIndex;
                    do {
                        prevIndex--;
                        if (prevIndex < 0) {
                            prevIndex = choiceButtons.Length - 1;
                        }
                        if (prevIndex == currentChoiceIndex) break;
                    } while (!choiceButtons[prevIndex].gameObject.activeSelf);

                    MovePointer(prevIndex);

                }
                if (Input.GetKeyDown(choiceDownKey)) {
                    int nextIndex = currentChoiceIndex;
                    do {
                        nextIndex++;
                        if (nextIndex >= choiceButtons.Length) {
                            nextIndex = 0;
                        }
                        if (nextIndex == currentChoiceIndex) break;
                    } while (!choiceButtons[nextIndex].gameObject.activeSelf);

                    MovePointer(nextIndex);
                }
                if (Input.GetKeyDown(selectKey)) {
                    onClickChoiceTemp(currentChoiceIndex);
                }
            }
            else {
                if (Input.GetKeyDown(selectKey)) {
                    NextMessage();
                }
            }
        }
    }
}
