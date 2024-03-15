using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Calcatz.VNDetectiveGUI {
    public class SaveLoad : PageMenuBase {

        [Header("Controls")]
        [SerializeField] private KeyCode leftKey = KeyCode.LeftArrow;
        [SerializeField] private KeyCode rightKey = KeyCode.RightArrow;

        protected override void Awake() {
            base.Awake();
        }

        protected override void Update() {
            base.Update();
            if (Input.GetKeyDown(leftKey)) {
                PrevItem(1);
            }
            if (Input.GetKeyDown(rightKey)) {
                NextItem(1);
            }
        }

    }
}
