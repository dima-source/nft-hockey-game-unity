using DevToDev.Analytics;

namespace Analytics
{
    [System.Serializable]
    public class InEditorEvent
    {
        public EventType type;
        
        // Tutorial
        public int tutorialLevelNumber;
        
        // LevelUp
        public int levelNumber;
        
        // LevelUp, Balance
        public CustomDictionaryElement<string, long>[] balance;
        
        // CurrencyAccrual
        public string currencyName; 
        public int currencyAmount; // TODO: calculate if -1
        public string source;
        public DTDAccrualType accrualType;

        // VirtualCurrencyPayment
        public string purchaseId; // TODO: calculate
        public string purchaseType;
        public int purchaseAmount;
        public string purchaseCurrency;
        public int purchasePrice;
        public CustomDictionaryElement<string, int>[] resources;
        
        // RealCurrencyPayment
        public string orderId; // TODO: calculate
        public string currencyCode;
        public double price;
        public string productId; // TODO: calculate if value is "calculate"
        
        // CustomEvent
        public string eventName;
        public CustomEventParameter[] customEventParameters;
    }
}