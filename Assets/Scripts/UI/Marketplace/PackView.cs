using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using Near.MarketplaceContract.ContractMethods;
using Near.Models.Tokens;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Scripts
{
    public enum PackTypes
    {
        Bronze = 7,
        Silver = 10,
        Gold = 13,
        Platinum = 15,
        Brilliant = 20,
    }
    
    public class PackView: UiComponent
    {
        private static readonly string SPRITES_PATH = Configurations.SpritesFolderPath + "Packs/";
        //[SerializeField]
        private PackTypes type;
        private Image _packImage;
        private TMP_Text _description;
        private TMP_Text _price;
        private Button _buyButton;
        //[SerializeField] 
        private string description;
        //[SerializeField] 
        private int price;
        private string PuckSpritePath => SPRITES_PATH + type + "Pack";
        public UnityAction<List<Token>, PackTypes> BoughtCallback;
        public UnityAction<PackTypes> BuyingCallback;

        public void SetData(PackTypes packType, int packPrice, string packDescription, UnityAction<PackTypes> buyingCallback, UnityAction<List<Token>, PackTypes> boughtCallback)
        {
            type = packType;
            price = packPrice;
            description = packDescription;
            BuyingCallback = buyingCallback;
            BoughtCallback = boughtCallback;
        }
        
        protected override void Initialize()
        {
            _packImage = UiUtils.FindChild<Image>(transform, "PackIcon");
            //_packImage.sprite = Utils.LoadSprite(PuckSpritePath);
            _description = UiUtils.FindChild<TMP_Text>(transform, "Description");
            _price = UiUtils.FindChild<TMP_Text>(transform, "PriceText");
            _buyButton = UiUtils.FindChild<Button>(transform, "BuyButton");
            _buyButton.onClick.AddListener(BuyPack);
        }

        protected override void OnUpdate()
        {
            _packImage.sprite = UiUtils.LoadSprite(PuckSpritePath);
            _description.text = description;
            _price.text = GetPriceString();
        }

        private string GetPriceString()
        {
            return price + " N";
        }

        public async void BuyPack()
        {
            if (BuyingCallback != null)
                BuyingCallback(type);
            var tokensBought = await Actions.BuyPack(price.ToString());
            if (SceneManager.GetActiveScene().name != "Marketplace")
                return;
            if (BoughtCallback != null)
                BoughtCallback(tokensBought, type);
        }
    }
}