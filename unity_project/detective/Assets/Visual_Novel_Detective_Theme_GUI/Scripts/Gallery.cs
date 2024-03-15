using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Calcatz.VNDetectiveGUI {
    public class Gallery : PageMenuBase {

        [Header("Controls")]
        [SerializeField] private KeyCode leftKey = KeyCode.LeftArrow;
        [SerializeField] private KeyCode rightKey = KeyCode.RightArrow;
        [SerializeField] private KeyCode upKey = KeyCode.UpArrow;
        [SerializeField] private KeyCode downKey = KeyCode.DownArrow;

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
            if (Input.GetKeyDown(upKey)) {
                PrevItem(3);
            }
            if (Input.GetKeyDown(downKey)) {
                NextItem(3);
            }
        }

    }
}
