//All UI menus that has pagination are derived from this class.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Calcatz.VNDetectiveGUI {
    public class PageMenuBase : FadingMenuBase {

        [System.Serializable]
        public class Item {
            public Sprite sprite;
            public bool locked = true;
        }

        [SerializeField] private PageItemContainer[] itemContainers;
        [SerializeField] private Item[] items;
        [SerializeField] private GameObject pageNumberPrototype;
        [SerializeField] private Button prevButton;
        [SerializeField] private Button nextButton;

        [System.Serializable]
        public class ClickItemEvent : UnityEvent<int> { }

        public ClickItemEvent onClickItem;

        private int currentContainerIndex = 0;
        private int pageCount;
        private int currentPage = -1;
        private Image currentPageBG;
        private Image[] pageBGs;

        protected virtual void Awake() {
            //Add all fade-able graphics
            InitializeGraphicAlphas(GetComponentsInChildren<Graphic>().ToList());
            InitPages();
            InitItemContainers();
        }

        protected virtual void Start() {
            ChangePage(0);
            SelectItem(currentContainerIndex);
        }

        private void InitPages() {
            pageCount = Mathf.CeilToInt(items.Length / (float)itemContainers.Length);
            GameObject[] pages = new GameObject[pageCount];
            pages[0] = pageNumberPrototype;
            pageBGs = new Image[pageCount];

            for (int i = 0; i < pageCount - 1; i++) {
                GameObject pageNumber = Instantiate(pageNumberPrototype);
                pageNumber.transform.SetParent(pageNumberPrototype.transform.parent, false);
                pages[i + 1] = pageNumber;
            }

            for (int i = 0; i < pageCount; i++) {
                int index = i;
                GameObject pageNumber = pages[i];
                InitPageNumber(index, pageNumber);
            }

            prevButton.onClick.AddListener(delegate {
                if (currentPage > 0) {
                    ChangePage(currentPage - 1);
                }
            });

            nextButton.onClick.AddListener(delegate {
                if (currentPage < pageCount - 1) {
                    ChangePage(currentPage + 1);
                }
            });
        }

        private void InitPageNumber(int index, GameObject pageNumber) {
            //Set page number background color.
            Image pageBG = pageNumber.GetComponent<Image>();
            pageBGs[index] = pageBG;
            Color pageBGColor = pageBG.color;
            pageBGColor.a = 0;
            pageBG.color = pageBGColor;

            //Set page number text.
            Text pageText = pageNumber.GetComponentInChildren<Text>();
            pageText.text = (index + 1) + "";
            Color pageTextColor = pageText.color;
            pageTextColor.a = 1;
            pageText.color = pageTextColor;

            //Add callback to page number button.
            Button pageButton = pageNumber.AddComponent<Button>();
            pageButton.transition = Selectable.Transition.None;
            pageButton.onClick.AddListener((UnityAction)delegate {
                if (index != currentPage) {
                    ChangePage((int)index);
                }
            });
        }

        private void InitItemContainers() {
            for (int i = 0; i < itemContainers.Length; i++) {
                int index = i;
                EventTrigger trigger = itemContainers[i].Image.gameObject.AddComponent<EventTrigger>();

                EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
                pointerEnter.eventID = EventTriggerType.PointerEnter;
                pointerEnter.callback.AddListener(delegate {
                    SelectItem(index);
                });
                trigger.triggers.Add(pointerEnter);

                EventTrigger.Entry pointerClick = new EventTrigger.Entry();
                pointerClick.eventID = EventTriggerType.PointerClick;
                pointerClick.callback.AddListener(delegate {
                    int firstIndex = currentPage * itemContainers.Length;
                    onClickItem.Invoke(firstIndex + index);
                });
                trigger.triggers.Add(pointerClick);

                /*EventTrigger.Entry pointerExit = new EventTrigger.Entry();
                pointerExit.eventID = EventTriggerType.PointerExit;
                pointerExit.callback.AddListener(delegate {
                    
                });
                trigger.triggers.Add(pointerExit);*/
            }
        }

        public void SelectItem(int _containerIndex) {
            if (currentContainerIndex >= 0) {
                itemContainers[currentContainerIndex].Unighlight();
            }
            PageItemContainer container = itemContainers[_containerIndex];
            if (!container.itemData.locked) {
                currentContainerIndex = _containerIndex;
                container.Highlight();
            }
        }

        public void ChangePage(int _pageIndex) {
            if (_pageIndex == currentPage) return;

            Color currentBGColor;

            //Restore current page number background color.
            if (currentPageBG != null) {
                currentBGColor = currentPageBG.color;
                currentBGColor.a = 0;
                currentPageBG.color = currentBGColor;
            }

            //Change the new page number background color.
            currentPage = _pageIndex;
            currentPageBG = pageBGs[_pageIndex];
            currentBGColor = currentPageBG.color;
            currentBGColor.a = 1;
            currentPageBG.color = currentBGColor;

            //Calculate the first item index, relative to the page number.
            int firstIndex = currentPage * itemContainers.Length;
            int itemCount = itemContainers.Length;
            if (currentPage == pageCount - 1) {
                itemCount = items.Length % itemContainers.Length;
            }
            for (int i = 0; i < itemContainers.Length; i++) {
                UpdateItemState(firstIndex, itemCount, i);
            }
        }

        /// <summary>
        /// Handles how an item should be shown, whether it's currently highlighted or not, and whether it's locked or not.
        /// </summary>
        /// <param name="firstIndex"></param>
        /// <param name="itemCount"></param>
        /// <param name="i"></param>
        private void UpdateItemState(int firstIndex, int itemCount, int i) {
            if (i < itemCount) {
                Item item = items[firstIndex + i];
                itemContainers[i].itemData = item;
                itemContainers[i].gameObject.SetActive(true);
                if (item.locked) {
                    itemContainers[i].Lock();
                }
                else {
                    itemContainers[i].Unlock();
                    itemContainers[i].ChangeImage(item.sprite);
                }
            }
            else {
                itemContainers[i].gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Select previous item, and prevent from selecting negative indexes;
        /// </summary>
        /// <param name="_steps"></param>
        protected void PrevItem(int _steps) {
            int iterations = 0;
            int prevIndex = currentContainerIndex;
            int page = currentPage;
            do {
                prevIndex -= _steps;
                if (prevIndex < 0) {
                    if (PrevPage()) {
                        prevIndex = itemContainers.Length - 1;
                    }
                    else {
                        return;
                    }
                }
                if (prevIndex == currentContainerIndex && page == currentPage) break;
                iterations++;
            } while (ContainerUnavailable(prevIndex) && iterations < 6);

            SelectItem(prevIndex);
        }

        /// <summary>
        /// Select next item, and prevent it from selecting more than the maximum items provided.
        /// </summary>
        /// <param name="_steps"></param>
        protected void NextItem(int _steps) {
            int iterations = 0;
            int nextIndex = currentContainerIndex;
            int page = currentPage;
            do {
                nextIndex += _steps;
                if (nextIndex >= itemContainers.Length) {
                    if (NextPage()) {
                        nextIndex = 0;
                    }
                    else {
                        return;
                    }
                }
                if (nextIndex == currentContainerIndex && page == currentPage) break;
                iterations++;
            } while (ContainerUnavailable(nextIndex) && iterations < 6);

            SelectItem(nextIndex);
        }

        private bool ContainerUnavailable(int _index) {
            return !itemContainers[_index].gameObject.activeSelf || itemContainers[_index].IsLocked();
        }

        /// <summary>
        /// Go to previous page.
        /// </summary>
        /// <returns>Returns false if on first page.</returns>
        private bool PrevPage() {
            if (currentPage == 0) return false;
            else {
                ChangePage(currentPage - 1);
                return true;
            }
        }

        /// <summary>
        /// Go to next page.
        /// </summary>
        /// <returns>Returns false if on last page.</returns>
        private bool NextPage() {
            if (currentPage >= pageCount-1) return false;
            else {
                ChangePage(currentPage + 1);
                return true;
            }
        }

        public void TestOnClickItem(int _index) {
            Debug.Log("Item " + _index + " is clicked.");
        }
    }
}
