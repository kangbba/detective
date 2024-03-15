//This is intended to show an illustration of how Alert UI behaves

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Calcatz.VNDetectiveGUI {
    public class Alert : FadingMenuBase {

        public Button yesButton;
        public Button noButton;

        [Header("Events")]
        public UnityEvent onClickYes;
        public UnityEvent onClickNo;

        private void Awake() {
            //Add all fade-able graphics
            InitializeGraphicAlphas(GetComponentsInChildren<Graphic>().ToList());

            yesButton.onClick.AddListener(delegate {
                onClickYes.Invoke();
            });
            noButton.onClick.AddListener(delegate {
                onClickNo.Invoke();
            });
        }

        public void YesExample() {
            Debug.Log("Yes");
        }

        public void NoExample() {
            Debug.Log("No");
        }
    }
}