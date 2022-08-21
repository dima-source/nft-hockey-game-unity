using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class CardsFilter : MonoBehaviour
    {
        
        private class LayoutSettings
        {
            private Vector2 _cellSize;
            private Vector2 _spacing;

            public LayoutSettings(Vector2 cellSize, Vector2 spacing)
            {
                _cellSize = cellSize;
                _spacing = spacing;
            }

            public void CopyValues(GridLayoutGroup layout)
            {
                layout.spacing = _spacing;
                layout.cellSize = _cellSize;
            }
        }


        [SerializeField]
        private int cardsValueToLoad;

        private int _currentLoad = 1;
        
        private RectTransform _layoutContainer, _togglesContainer;
        private GridLayoutGroup _layout;

        private static readonly LayoutSettings Settings1x1 = 
            new(new Vector2(500, 800), 
                new Vector2(300, 150));
        
        private static readonly LayoutSettings Settings2x2 = 
            new(new Vector2(450, 720), 
                new Vector2(100, 100));
        
        private static readonly LayoutSettings Settings3x3 = 
            new(new Vector2(400, 640), 
                new Vector2(20, 20));
        

        private GameObject _cardViewPrefab;

        private Marketplace _marketplace;

        private int _currentGridColumnSize = 3;
        private List<CardView> _pull;


        private Button _grid3x3Button, _grid2x2Button, _gridLinesButton;
        
        private void Awake()
        {
            _pull = new List<CardView>();
            _layoutContainer = Utils.FindChild<RectTransform>(transform, "Layout");
            _layout = Utils.FindChild<GridLayoutGroup>(_layoutContainer, "Content");
            Transform temp = Utils.FindChild<Transform>(transform, "FilterMenu");
            _togglesContainer = Utils.FindChild<RectTransform>(temp, "Content");
            _cardViewPrefab = Utils.LoadResource<GameObject>(Configurations.PrefabsFolderPath + "Marketplace/CardView");

            _grid3x3Button = Utils.FindChild<Button>(transform, "Grid3x3Button");
            _grid2x2Button = Utils.FindChild<Button>(transform, "Grid2x2Button");
            _gridLinesButton = Utils.FindChild<Button>(transform, "LinesButton");
            
            _marketplace = FindObjectOfType(typeof(Marketplace)).GetComponent<Marketplace>();
            
            foreach (Transform child in _togglesContainer)
            {
                child.GetComponent<ToggleGroup>().onChange = OnInputChanged;
            }
            
            foreach (Transform child in _layout.transform)
            {
                Destroy(child.gameObject);
            }
            
            CallLoadNewPortion();
            //CheckButtons();
        }
        

        private IEnumerator GetSize(Action<Vector2Int> callback)
        {
            yield return new WaitForEndOfFrame();
            int itemsCount = _layout.transform.childCount;
            float prevX = float.NegativeInfinity;
            int xCount = 0;

            for (int i = 0; i < itemsCount; i++)
            {
                Vector2 pos = ((RectTransform)_layout.transform.GetChild(i)).anchoredPosition;

                if (pos.x <= prevX)
                    break;

                prevX = pos.x;
                xCount++;
            }

            int yCount = GetAnotherAxisCount(itemsCount, xCount);
            callback.Invoke(new Vector2Int(xCount, yCount));
        }

        private static int GetAnotherAxisCount(int totalCount, int axisCount)
        {
            return totalCount / axisCount + Mathf.Min(1, totalCount % axisCount);
        }

        private void CheckButtons()
        {
            StartCoroutine(GetSize(result =>
            {
                _grid3x3Button.gameObject.SetActive(true);
                _grid2x2Button.gameObject.SetActive(true);
                _gridLinesButton.gameObject.SetActive(true);
                Debug.Log(result);
                if (_currentGridColumnSize == 3)
                {
                    _grid3x3Button.gameObject.SetActive(_currentGridColumnSize == result.x);   
                } 
                else if (_currentGridColumnSize == 2)
                {
                    _grid2x2Button.gameObject.SetActive(_currentGridColumnSize == result.x);   
                } 
                else if (_currentGridColumnSize == 1)
                {
                    _gridLinesButton.gameObject.SetActive(_currentGridColumnSize == result.x);   
                } 
            }));
        }

        private void OnDisable()
        {
            foreach (Transform child in _layout.transform)
            {
                Destroy(child.gameObject);
            }  
            _currentLoad = 1;
            _pull.Clear();
            
            CallLoadNewPortion();
            ScrollRect rect = _layoutContainer.GetComponent<ScrollRect>();
            rect.verticalNormalizedPosition = 1.0f;
            _currentGridColumnSize = 3;
            Settings3x3.CopyValues(_layout);
            //CheckButtons();
        }

        public void OnGrid3x3Click()
        {
            PlaySound();
            Settings3x3.CopyValues(_layout);
            _currentGridColumnSize = 3;
            //CheckButtons();
        }
        
        public void OnGrid2x2Click()
        {
            PlaySound();
            Settings2x2.CopyValues(_layout);
            _currentGridColumnSize = 2;
            //CheckButtons();
        }

        public void OnLinesButtonClick()
        {
            PlaySound();
            Settings1x1.CopyValues(_layout);
            _currentGridColumnSize = 1;
            //CheckButtons();
        }

        public void OnSearchChanged()
        {
            PlaySound();
            OnInputChanged();
        }

        private void OnInputChanged()
        {
            // Rebuild content here 
            Debug.Log("changed");
        }

        private bool _isIn = true;


        private void CallLoadNewPortion()
        {
            OnLoadNewPortion();
            _isIn = false;
        }
        
        private static Rect GetWorldRect(RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            Vector3 position = corners[0];
         
            Vector2 size = new Vector2(
                rectTransform.lossyScale.x * rectTransform.rect.size.x,
                rectTransform.lossyScale.y * rectTransform.rect.size.y);
 
            return new Rect(position, size);
        }
        
        public void OnScroll()
        {
            ScrollRect rect = _layoutContainer.GetComponent<ScrollRect>();
            float position = rect.verticalNormalizedPosition;
            for (int i = 0; i < _pull.Count; i++)
            {
                var card = _pull[i];
                Rect cardRect = GetWorldRect(card.RectTransform);
                Rect layoutRect = GetWorldRect(_layoutContainer);
                card.Enable(layoutRect.Overlaps(cardRect));
            }
            
            if (position <= 0.05f)
            {
                if (_isIn)
                {
                    // Down
                    _currentLoad++;
                    CallLoadNewPortion();
                }
            } 
            else if (position >= 0.95f)
            {
                if (_isIn && _currentLoad > 0)
                {
                    // Up
                    _currentLoad--;
                    CallLoadNewPortion();  
                }
            }
            else
            {
                _isIn = true;
            }
        }

        private void OnLoadNewPortion()
        {
            // Load new portion here
            for (int i = 0; i < cardsValueToLoad; i++)
            {
                CardView view = Instantiate(_cardViewPrefab, _layout.transform).GetComponent<CardView>();
                Button button = view.GetComponent<Button>();
                button.enabled = true;
                button.onClick.AddListener(() =>
                {
                    PlaySound();
                    _marketplace.SwitchPage("CardDisplay");
                    _marketplace.TopBar.SetBackButtonAction(() =>
                    {
                        // TODO: Set previous page here
                        _marketplace.SwitchPage("FilterCards");
                    });
                });
                _pull.Add(view);
            }
        }

        private void PlaySound()
        {
            AudioController.LoadClip(Configurations.DefaultButtonSoundPath);
            AudioController.source.Play();
        }
    }
}
