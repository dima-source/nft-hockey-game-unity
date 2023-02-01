using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Near.Models.Tokens;
using Near.Models.Tokens.Filters;
using Near.Models.Tokens.Filters.ToggleFilters;
using NearClientUnity.Utilities;
using Runtime;
using TMPro;
using UI.Scripts.Card;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class CardsFilter : MonoBehaviour
    {
        private class LayoutSettings
        {
            private int _columns;
            private Vector2 _cellSize;
            private Vector2 _spacing;

            public LayoutSettings(int columns, Vector2 cellSize, Vector2 spacing)
            {
                _columns = columns;
                _cellSize = cellSize;
                _spacing = spacing;
            }

            public void CopyValues(GridLayoutGroup layout)
            {
                layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                layout.constraintCount = _columns;
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
            new(1, new Vector2(500, 800), 
                new Vector2(300, 150));
        
        private static readonly LayoutSettings Settings2x2 = 
            new(2, new Vector2(450, 720), 
                new Vector2(100, 100));
        
        private static readonly LayoutSettings Settings3x3 = 
            new(3, new Vector2(300, 480), 
                new Vector2(20, 20));
        

        private GameObject _cardViewPrefab;

        private Marketplace _marketplace;
        
        private List<CardView> _pull;
        private int _numberOfLoadedCards;

        private List<Toggle> _toggles;

        private void Awake()
        {
            _pull = new List<CardView>();
            _numberOfLoadedCards = 0;
            
            _layoutContainer = UiUtils.FindChild<RectTransform>(transform, "Layout");
            _layout = UiUtils.FindChild<GridLayoutGroup>(_layoutContainer, "Content");
            Transform temp = UiUtils.FindChild<Transform>(transform, "FilterMenu");
            _togglesContainer = UiUtils.FindChild<RectTransform>(temp, "Content");
            _cardViewPrefab = UiUtils.LoadResource<GameObject>(Configurations.PrefabsFolderPath + "Marketplace/CardView");

            _marketplace = FindObjectOfType(typeof(Marketplace)).GetComponent<Marketplace>();
            
            foreach (Transform child in _togglesContainer)
            {
                ToggleGroup toggleGroup = child.GetComponent<ToggleGroup>();
                toggleGroup.onChangeToggle = OnToggleChanged;
            }
            
            foreach (Transform child in _layout.transform)
            {
                Destroy(child.gameObject);
            }
            
            Settings3x3.CopyValues(_layout);
        }
        
        private void OnDisable()
        {
            ClearArea();
        }

        private void OnEnable()
        {
            CallLoadNewPortion();
            ScrollRect rect = _layoutContainer.GetComponent<ScrollRect>();
            rect.verticalNormalizedPosition = 1.0f;
            Settings3x3.CopyValues(_layout);
            _marketplace.TopBar.SetBackButtonAction(() => Game.LoadMainMenu());
        }

        public void OnGrid3x3Click()
        {
            PlaySound();
            Settings3x3.CopyValues(_layout);
        }
        
        public void OnGrid2x2Click()
        {
            PlaySound();
            Settings2x2.CopyValues(_layout);
        }

        public void OnLinesButtonClick()
        {
            PlaySound();
            Settings1x1.CopyValues(_layout);
        }

        [SerializeField] private TMP_InputField searchByName;
        public async void OnSearchChanged()
        {
            PlaySound();
            ClearArea();
            
            PlayerFilter filter = GetPlayerFilter();
            Pagination pagination = GetPagination();

            List<Token> tokens = await Near.MarketplaceContract.ContractMethods.Views
                .GetTokens(filter, pagination);

            ShowLoadedNewPortion(tokens);
        }

        private void ClearArea()
        {
            foreach (Transform child in _layout.transform)
            {
                Destroy(child.gameObject);
            }  
            
            _currentLoad = 1;
            _pull.Clear();
            _numberOfLoadedCards = 0; 
        }
        
        private async void OnToggleChanged(string toggleText)
        {
            ClearArea();
            
            Pagination pagination = GetPagination();
            pagination.skip = 0;
            
            PlayerFilter filter = GetPlayerFilter();
            
            List<Token> tokens = await Near.MarketplaceContract.ContractMethods.Views
                .GetTokens(filter, pagination);
            
            ShowLoadedNewPortion(tokens);
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
                Rect cardRect = GetWorldRect(card.rectTransform);
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

        private async void OnLoadNewPortion()
        {
            List<Token> tokens = await LoadNewCards();
            ShowLoadedNewPortion(tokens);
        }

        private void ShowLoadedNewPortion(List<Token> tokens)
        {
            foreach (var token in tokens)
            {
                _numberOfLoadedCards += 1;
                
                if (_marketplace.TopBar.NowPage == "SellCards" && token.marketplace_data != null)
                {
                    continue;
                }
                
                CardView view = Instantiate(_cardViewPrefab, _layout.transform).GetComponent<CardView>();
                view.SetData(token);
                
                Button button = view.GetComponent<Button>();
                button.enabled = true;
                
                button.onClick.AddListener(() =>
                {
                    PlaySound();
                   
                    CardDisplay cardDisplay = _marketplace.SwitchPage("CardDisplay").GetComponent<CardDisplay>();
                    cardDisplay.SetData(token);
                    
                    switch (_marketplace.TopBar.NowPage)
                    {
                        case "BuyCards":
                            cardDisplay.SetButton(0, "Buy", () =>
                            {
                                Popup popup; 
                                if (token.marketplace_data.isAuction)
                                {
                                    List<PopupManager.BetInfo> betInfo = new List<PopupManager.BetInfo>();

                                    foreach (var offer in token.marketplace_data.offers)
                                    {
                                        float bet = (float) Near.NearUtils.FormatNearAmount(UInt128.Parse(offer.price));
                                        betInfo.Add(new PopupManager.BetInfo(offer.user.id, bet));
                                    }
                                    
                                    popup = _marketplace.GetComponent<RectTransform>()
                                        .GetPlaceBet(betInfo.ToArray(), token.tokenId);
                                }
                                else
                                {
                                    UInt128 price = UInt128.Parse(token.marketplace_data.price);
                                    float formattedPrice = (float) Near.NearUtils.FormatNearAmount(price);
                                    popup = _marketplace.GetComponent<RectTransform>()
                                        .GetBuy(formattedPrice, async () =>
                                        {
                                            await Offer(token.tokenId, token.marketplace_data.price);
                                            Popup success = _marketplace.GetComponent<RectTransform>().GetDefaultOk("Success", $"You have successfully bought nft");
                                            success.Show();
                                        });
                                }
                                popup.Show();
                            });
                            cardDisplay.SetButton(1, "");
                            cardDisplay.SetButton(2, "");
                            break;
                        case "SellCards":
                            cardDisplay.SetButton(0, "Sell", () =>
                            {
                                Popup popup = _marketplace.GetComponent<RectTransform>().GetSellCard(token.tokenId);
                                popup.Show();
                            });
                            
                            cardDisplay.SetButton(1, "");
                            cardDisplay.SetButton(2, "");
                            
                            break;
                        case "OnSale":
                            cardDisplay.SetButton(0, "Change sale conditions", () =>
                            {
                                Popup popup = _marketplace.GetComponent<RectTransform>().GetInputNear("Enter new price", async (value) =>
                                    {
                                        Dictionary<string,string> newSaleConditions = new Dictionary<string, string>
                                        {
                                            {"near", Near.NearUtils.ParseNearAmount(value.ToString()).ToString()}
                                        };
                                        try
                                        {
                                            await Near.MarketplaceContract.ContractMethods.Actions.SaleUpdate(
                                                newSaleConditions, token.tokenId, token.marketplace_data.isAuction);
                                            
                                            Popup success = _marketplace.GetComponent<RectTransform>().GetDefaultOk("Success", $"You have successfully changed price to {value}N");
                                            success.Show();
                                        }
                                        catch (Exception e)
                                        {
                                            string messageError = e.Message.Contains("NotEnoughBalance")
                                                ? "Not enough balance to sign a transaction"
                                                : "Something went wrong";
                        
                                            Popup error = _marketplace.GetComponent<RectTransform>().GetDefaultOk("Error", messageError);
                                            error.Show();
                                            return;
                                        }
                                    });
                                popup.Show();
                            });
                            
                            cardDisplay.SetButton(1, "Take off the market", async () =>
                            {
                                try
                                {
                                    await Near.MarketplaceContract.ContractMethods.Actions.RemoveSale(token.tokenId);
                                }
                                catch (Exception e)
                                {
                                    string messageError = e.Message.Contains("NotEnoughBalance")
                                        ? "Not enough balance to sign a transaction"
                                        : "Something went wrong";
                        
                                    Popup error = _marketplace.GetComponent<RectTransform>().GetDefaultOk("Error", messageError);
                                    error.Show();
                                    return;
                                }
                    
                                Popup success = _marketplace.GetComponent<RectTransform>().GetDefaultOk("Success", $"You have successfully took off the market token ");
                                success.Show(); 
                            });
                            
                            if (token.marketplace_data.isAuction)
                            {
                                cardDisplay.SetButton(2, "Accept the bet", () =>
                                {
                                    List<PopupManager.BetInfo> betInfo = new List<PopupManager.BetInfo>();

                                    foreach (var offer in token.marketplace_data.offers)
                                    {
                                        betInfo.Add(new PopupManager.BetInfo(offer.user.id,
                                            (float)Near.NearUtils.FormatNearAmount(UInt128.Parse(offer.price))));
                                    }
                                    Popup popup = _marketplace.GetComponent<RectTransform>().GetAcceptBet(betInfo.ToArray(), async () =>
                                    {
                                        try
                                        {
                                            await Near.MarketplaceContract.ContractMethods.Actions.AcceptOffer(token.tokenId);
                                        }
                                        catch (Exception e)
                                        {
                                            string messageError = e.Message.Contains("NotEnoughBalance")
                                                ? "Not enough balance to sign a transaction"
                                                : "Something went wrong";
                        
                                            Popup error = _marketplace.GetComponent<RectTransform>().GetDefaultOk("Error", messageError);
                                            error.Show();
                                            return;
                                        }
                    
                                        Popup success = _marketplace.GetComponent<RectTransform>().GetDefaultOk("Success", $"You have successfully accepted the offer");
                                        success.Show(); 
                                    });
                                    popup.Show();
                                });   
                            }
                            else
                            {
                                cardDisplay.SetButton(2, "");
                            }
                            break;
                        default:
                            throw new ApplicationException($"Unknown '{_marketplace.TopBar.NowPage}' page");
                    }

                    _marketplace.TopBar.SetBackButtonAction(() =>
                    {
                        // TODO: Set previous page here
                        _marketplace.SwitchPage("FilterCards");
                    });
                });
                
                _pull.Add(view);
            }
        }

        private async Task Offer(string tokenId, string price)
        {
            try
            {
                await Near.MarketplaceContract.ContractMethods.Actions.Offer(
                    tokenId,  price);
            }
            catch (Exception e)
            {
                string messageError = e.Message.Contains("NotEnoughBalance")
                    ? "Not enough balance"
                    : "Something went wrong";
                        
                Popup error = _marketplace.GetComponent<RectTransform>().GetDefaultOk("Error", messageError);
                error.Show();
                return;
            }
        }
        
        private async Task<List<Token>> LoadNewCards()
        {
            Pagination pagination = GetPagination();
            PlayerFilter filter = GetPlayerFilter();
            
            List<Token> tokens = await Near.MarketplaceContract.ContractMethods.Views.GetTokens(filter, pagination);
            
            return tokens;
        }

        private Pagination GetPagination()
        {
            Pagination pagination = new Pagination
            {
                first = cardsValueToLoad,
                skip = _numberOfLoadedCards
            };

            return pagination;
        }

        private PlayerFilter GetPlayerFilter()
        {
            PlayerFilter filter = new PlayerFilter();

            string ownerId = Near.NearPersistentManager.Instance.GetAccountId();
            if (_marketplace.TopBar.NowPage is "SellCards" or "OnSale")
            {
                filter.ownerId = ownerId;
            }
            else
            {
                filter.ownerId_not = ownerId;
            }

            if (searchByName.text != "")
            {
                filter.title_contains_nocase = searchByName.text;
            }
            
            ToggleFilterFactory toggleFilterFactory = new ToggleFilterFactory();
            foreach (Transform child in _togglesContainer)
            {
                ToggleGroup toggleGroup = child.GetComponent<ToggleGroup>();

                if (toggleGroup.groupName == "Sale option" && _marketplace.TopBar.NowPage == "SellCards")
                {
                    continue;
                }
                
                IToggleFilter toggleFilter = toggleFilterFactory.GetToggleFilter(toggleGroup.groupName);
                toggleFilter.AddToPlayerFilter(filter, toggleGroup._toggles);
            }
            
            return filter;
        }
        
        private void PlaySound()
        {
            AudioController.LoadClip(Configurations.DefaultButtonSoundPath);
            AudioController.source.Play();
        }
    }
}