using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UI.Scripts
{
    public static class PopupManager
    {
        private static readonly string PATH = Configurations.PrefabsFolderPath + "Popup";
        private static Popup _instance;

        public static Popup GetDefaultOk(this RectTransform parent, string title, string message, UnityAction onClose = null)
        {
            _instance = GetDefault(parent);
            _instance.SetTitle(title);
            _instance.SetMessage(message);
            
            _instance.buttons = new[]
            {
                new Popup.ButtonView(Popup.ButtonType.Positive, "Okay")
            };

            _instance.onClose = onClose;
           _instance.OnButtonClick(0, _instance.Close);
           return _instance;
        }
        
        public static Popup GetInputNear(this RectTransform parent, string title, UnityAction<float> onChange = null,
            Func<Boolean> onValidate = null)
        {
            _instance = GetDefault(parent);
            _instance.SetTitle(title);
            _instance.DeleteMessageSlot();
            
            _instance.buttons = new[]
            {
                new Popup.ButtonView(Popup.ButtonType.Neutral, "Go back"),
                new Popup.ButtonView(Popup.ButtonType.Positive, "Okay")
            };

            string path = Configurations.PrefabsFolderPath + "Inputs/InputNear";
            GameObject prefab = Utils.LoadResource<GameObject>(path);
            Transform inputObj = Object.Instantiate(prefab, parent).transform;
            inputObj.name = "InputNear"; 
            _instance.AddAdditional(inputObj);
            
            InputNear input = Utils.FindChild<InputNear>(_instance.transform, "InputNear");
            
            _instance.OnButtonClick(0, _instance.Close);
            _instance.OnButtonClick(1, () =>
            {
                float value = input.Value;
                
                if (value <= 0.0f || (onValidate != null && onValidate.Invoke()))
                {
                    Popup error = parent.GetDefaultOk("Error", $"Invalid price value");
                    error.Show();
                    return;
                }
                    
                onChange?.Invoke(value);
                Popup success = parent.GetDefaultOk("Success", $"You have successfully changed sale conditions");
                success.Show();
            });
            
            return _instance;
        }
        
        
        public class BetInfo
        {
            public string person;
            public float bet;

            public BetInfo(string person, float bet)
            {
                this.person = person;
                this.bet = bet;
            }

            public override string ToString()
            {
                return $"{person} <sprite name=\"RightArrow\"> {bet} <sprite name=\"NearLogo\">";
            }
        }

        public static Popup GetBuy(this RectTransform parent, float value, UnityAction onBuy)
        {
            _instance = GetDefault(parent);
            _instance.SetTitle("Buy a card");
            _instance.SetMessage($"Do you really want to buy this card for {value} <sprite name=NearLogo> ?");
            _instance.buttons = new[]
            {
                new Popup.ButtonView(Popup.ButtonType.Neutral, "Go back"),
                new Popup.ButtonView(Popup.ButtonType.Positive, "Buy")
            };
            _instance.OnButtonClick(0, _instance.Close);
            _instance.OnButtonClick(1, () =>
            {
                onBuy?.Invoke();
                Popup success = parent.GetDefaultOk("Success", "You have successfully bought the card");
                success.Show();
            });

            return _instance;
        }

        public static Popup GetAcceptBet(this RectTransform parent, BetInfo[] betInfo, UnityAction onAcceptBet)
        {
            _instance = GetDefault(parent);
            _instance.SetTitle($"Accept a bet on {betInfo.Select(x => x.bet).Max()} <sprite name=NearLogo> ?");
            _instance.DeleteMessageSlot();
            
            _instance.buttons = new[]
            {
                new Popup.ButtonView(Popup.ButtonType.Neutral, "Go back"),
                new Popup.ButtonView(Popup.ButtonType.Positive, "Accept a bet")
            };
            
            _instance.OnButtonClick(0, _instance.Close);
            _instance.OnButtonClick(1, () =>
            {
                onAcceptBet?.Invoke();
                Popup success = parent.GetDefaultOk("Success", "You have successfully accepted a bet");
                success.Show();
            });
            
            string path = Configurations.PrefabsFolderPath + "VerticalScrollView";
            GameObject prefab = Utils.LoadResource<GameObject>(path);
            Transform scrollText = Object.Instantiate(prefab, parent).transform;
            Transform content = Utils.FindChild<Transform>(scrollText, "Content");
            _instance.AddAdditional(scrollText);

            TMP_SpriteAsset spriteAsset = Utils.LoadResource<TMP_SpriteAsset>(Configurations.SpritesFolderPath + "SpriteAsset");
            foreach (var info in betInfo)
            {
                GameObject obj = new GameObject("Text");
                obj.transform.SetParent(content, false);
                TextMeshProUGUI text = obj.AddComponent<TextMeshProUGUI>();
                text.enableAutoSizing = true;
                text.fontSizeMin = 5;
                text.fontSizeMax = 40;
                text.spriteAsset = spriteAsset;
                text.text = info.ToString();
            }
            
            return _instance;
        }

        public static Popup GetPlaceBet(this RectTransform parent, BetInfo[] betInfo, string tokenId)
        {
            _instance = GetAcceptBet(parent, betInfo, () => {});
            
            _instance.SetTitle("Set a new bet?");
            
            string path = Configurations.PrefabsFolderPath + "Inputs/InputNear";
            GameObject prefab = Utils.LoadResource<GameObject>(path);
            Transform inputObj = Object.Instantiate(prefab, parent).transform;
            inputObj.name = "InputNear"; 
            _instance.AddAdditional(inputObj);
            InputNear input = Utils.FindChild<InputNear>(_instance.transform, "InputNear");
            
            _instance.buttons = new[]
            {
                new Popup.ButtonView(Popup.ButtonType.Neutral, "Go back"),
                new Popup.ButtonView(Popup.ButtonType.Positive, "Set"),
            };
            
            _instance.OnButtonClick(0, _instance.Close);
            _instance.OnButtonClick(1, () =>
            {
                float value = input.Value;
                float max = betInfo.Select(x => x.bet).Max();
                if (value <= max)
                {
                    Popup error = parent.GetDefaultOk("Error", $"Invalid bet value. Bet must be greater than {max} <sprite name=NearLogo>");
                    error.Show();
                    return;
                }
                    
                Near.MarketplaceContract.ContractMethods.Actions.Offer(tokenId, "near", value.ToString());
                Popup success = parent.GetDefaultOk("Success", "You have successfully placed the bet");
                success.Show();
            });

            return _instance;
        }
        
        
        public static Popup GetSellCard(this RectTransform parent, string tokenId)
        {
            _instance = GetInputNear(parent, "Sell card");
            InputNear input = Utils.FindChild<InputNear>(_instance.transform, "InputNear");
            
            string path = Configurations.PrefabsFolderPath + "Toggle";
            GameObject prefab = Utils.LoadResource<GameObject>(path);
            Transform obj = Object.Instantiate(prefab, parent).transform;
            _instance.AddAdditional(obj);

            Toggle toggle = obj.GetComponent<Toggle>();
            toggle.text = "Place on auction";
            toggle.isOn = false;
            
            var element = obj.AddComponent<LayoutElement>();
            element.preferredHeight = 30;
            TextMeshProUGUI text = Utils.FindChild<TextMeshProUGUI>(obj, "Label");
            text.margin = new Vector4(0, 20, 0, 20);
            text.horizontalAlignment = HorizontalAlignmentOptions.Left;
            
            _instance.buttons = new[]
            {
                new Popup.ButtonView(Popup.ButtonType.Neutral, "Go back"),
                new Popup.ButtonView(Popup.ButtonType.Positive, "Sell"),
            };
            _instance.OnButtonClick(0, _instance.Close);
            _instance.OnButtonClick(1, () =>
            {
                string message = toggle.isOn ? "on the auction" : "on the market";
                Popup info = parent.GetDefaultOk("Confirm action", $"Do you really want to place this card {message}?");
                info.buttons = new[]
                {
                    new Popup.ButtonView(Popup.ButtonType.Neutral, "No"),
                    new Popup.ButtonView(Popup.ButtonType.Positive, "Yes"),
                };
                info.OnButtonClick(0, info.Close);
                info.OnButtonClick(1, async () =>
                {
                    float value = input.Value;
                    
                    if (value <= 0.0f)
                    {
                        Popup error = parent.GetDefaultOk("Error", $"Invalid price value");
                        error.Show();
                        return;
                    }
                    
                    Dictionary<string,string> newSaleConditions = new Dictionary<string, string>
                    {
                        {"near", Near.NearUtils.ParseNearAmount(value.ToString()).ToString()}
                    };
                    try
                    {
                        await Near.MarketplaceContract.ContractMethods.Actions.SaleUpdate(
                            newSaleConditions, tokenId, toggle.isOn);
                    }
                    catch (Exception e)
                    {
                        string messageError = e.Message.Contains("NotEnoughBalance")
                            ? "Not enough balance to sign a transaction"
                            : "Something went wrong";
                        
                        Popup error = parent.GetDefaultOk("Error", messageError);
                        error.Show();
                        return;
                    }
                    
                    Popup success = parent.GetDefaultOk("Success", $"You have successfully placed this card {message}");
                    success.Show();
                });
                info.Show();
            });
            
            return _instance;
        }

        private static Popup GetDefault(this RectTransform parent)
        {
            if (_instance != null)
            {
                Object.Destroy(_instance.gameObject);
            }

            GameObject prefab = Utils.LoadResource<GameObject>(PATH);
            _instance = Object.Instantiate(prefab, parent).GetComponent<Popup>();
            return _instance;
        }
    }
}