using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Calcatz.VNDetectiveGUI {
    public class Settings : FadingMenuBase {

        [System.Serializable]
        public class ItemsRow {
            public SettingsItem[] items;
        }

        [SerializeField] private ItemsRow[] itemsRows;

        [Header("Controls")]
        [SerializeField] private KeyCode leftKey = KeyCode.LeftArrow;
        [SerializeField] private KeyCode rightKey = KeyCode.RightArrow;
        [SerializeField] private KeyCode upKey = KeyCode.UpArrow;
        [SerializeField] private KeyCode downKey = KeyCode.DownArrow;
        [SerializeField] private KeyCode selectKey = KeyCode.Return;

        private int[] currentHighlightedItem = new int[2] { 0, 0 };
        private int[] currentSelectedItem = new int[2] { 0, 0 };
        private bool isSelectingItem;

        private void Awake() {
            InitializeGraphicAlphas(GetComponentsInChildren<Graphic>().ToList());
        }

        private void Start() {
            HighlightItem(0, 0);
        }

        private void HighlightItem(int _rowIndex, int _columnIndex) {
            SettingsItem currentItem = itemsRows[currentHighlightedItem[0]].items[currentHighlightedItem[1]];
            currentItem.Unhighlight();

            if (_rowIndex >= 0 && _rowIndex < itemsRows.Length) {
                if (_columnIndex >= 0 && _columnIndex < itemsRows[_rowIndex].items.Length) {
                    currentHighlightedItem[0] = _rowIndex;
                    currentHighlightedItem[1] = _columnIndex;
                    currentItem = itemsRows[currentHighlightedItem[0]].items[currentHighlightedItem[1]];
                    currentItem.Highlight();
                }
            }
        }

        private void DeselectItem(int _rowIndex, int _columnIndex) {
            isSelectingItem = false;
            itemsRows[_rowIndex].items[_columnIndex].Deselect();
        }

        private void SelectItem(int _rowIndex, int _columnIndex) {
            isSelectingItem = true;
            currentSelectedItem[0] = _rowIndex;
            currentSelectedItem[1] = _columnIndex;
            itemsRows[_rowIndex].items[_columnIndex].Select();
        }

        private void PrevItem() {
            int prevRow = currentHighlightedItem[0];
            int prevColumn = currentHighlightedItem[1] - 1;
            if (prevColumn < 0) {
                prevRow--;
                if (prevRow < 0) {
                    prevRow = itemsRows.Length-1;
                }
                prevColumn = itemsRows[prevRow].items.Length-1;
            }
            HighlightItem(prevRow, prevColumn);
        }

        private void NextItem() {
            int nextRow = currentHighlightedItem[0];
            int nextColumn = currentHighlightedItem[1] + 1;
            if (nextColumn >= itemsRows[nextRow].items.Length) {
                nextRow++;
                if (nextRow >= itemsRows.Length) {
                    nextRow = 0;
                }
                nextColumn = 0;
            }
            HighlightItem(nextRow, nextColumn);
        }

        private void PrevRow() {
            int prevRow = currentHighlightedItem[0];
            int prevColumn = currentHighlightedItem[1];
            prevRow--;
            if (prevRow < 0) {
                prevRow = itemsRows.Length -1;
            }
            if (prevColumn >= itemsRows[prevRow].items.Length) {
                prevColumn = itemsRows[prevRow].items.Length - 1;
            }
            HighlightItem(prevRow, prevColumn);
        }

        private void NextRow() {
            int nextRow = currentHighlightedItem[0];
            int nextColumn = currentHighlightedItem[1];
            nextRow++;
            if (nextRow >= itemsRows.Length) {
                nextRow = 0;
            }
            if (nextColumn >= itemsRows[nextRow].items.Length) {
                nextColumn = itemsRows[nextRow].items.Length - 1;
            }
            HighlightItem(nextRow, nextColumn);
        }

        protected override void Update() {
            base.Update();
            if (!isSelectingItem) {
                if (Input.GetKeyDown(leftKey)) {
                    PrevItem();
                }
                if (Input.GetKeyDown(rightKey)) {
                    NextItem();
                }
                if (Input.GetKeyDown(upKey)) {
                    PrevRow();
                }
                if (Input.GetKeyDown(downKey)) {
                    NextRow();
                }
                if (Input.GetKeyDown(selectKey)) {
                    SelectItem(currentHighlightedItem[0], currentHighlightedItem[1]);
                }
            }
            else {
                if (Input.GetKeyDown(leftKey)) {
                    itemsRows[currentSelectedItem[0]].items[currentSelectedItem[1]].DecreaseValue();
                }
                if (Input.GetKeyDown(rightKey)) {
                    itemsRows[currentSelectedItem[0]].items[currentSelectedItem[1]].IncreaseValue();
                }
                if (Input.GetKeyDown(upKey)) {
                    itemsRows[currentSelectedItem[0]].items[currentSelectedItem[1]].DecreaseValue();
                }
                if (Input.GetKeyDown(downKey)) {
                    itemsRows[currentSelectedItem[0]].items[currentSelectedItem[1]].IncreaseValue();
                }
                if (Input.GetKeyDown(selectKey)) {
                    DeselectItem(currentSelectedItem[0], currentSelectedItem[1]);
                }
            }
        }
    }
}
