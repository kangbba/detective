using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Calcatz.VNDetectiveGUI {
    public class PageItemContainer : MonoBehaviour {

        [HideInInspector] public PageMenuBase.Item itemData;

        [SerializeField] private Image highlighter;
        [SerializeField] private Image image;

        [SerializeField] private GameObject[] highlightedGameObjects;
        [SerializeField] private GameObject[] unhighlightedGameObjects;
        [SerializeField] private GameObject[] lockedGameObjects;

        public Image Image { get { return image; } }

        public void ChangeImage(Sprite _sprite) {
            if (_sprite == null) {
                image.gameObject.SetActive(false);
            }
            else {
                image.gameObject.SetActive(true);
                image.sprite = _sprite;
            }
        }

        public bool IsLocked() {
            return itemData.locked;
        }

        public void Lock() {
            foreach(GameObject go in lockedGameObjects) {
                go.SetActive(true);
            }
            image.gameObject.SetActive(false);
            highlighter.gameObject.SetActive(false);
        }

        public void Unlock() {
            foreach (GameObject go in lockedGameObjects) {
                go.SetActive(false);
            }
            image.gameObject.SetActive(true);
        }

        public void Highlight() {
            if (!IsLocked()) {
                highlighter.gameObject.SetActive(true);
                if (highlightedGameObjects != null) {
                    foreach(GameObject go in highlightedGameObjects) {
                        go.SetActive(true);
                    }
                }
                if (unhighlightedGameObjects != null) {
                    foreach (GameObject go in unhighlightedGameObjects) {
                        go.SetActive(false);
                    }
                }
            }
        }
        public void Unighlight() {
            highlighter.gameObject.SetActive(false);
            if (highlightedGameObjects != null) {
                foreach (GameObject go in highlightedGameObjects) {
                    go.SetActive(false);
                }
            }
            if (unhighlightedGameObjects != null) {
                foreach (GameObject go in unhighlightedGameObjects) {
                    go.SetActive(true);
                }
            }
        }
    }
}
