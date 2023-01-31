using System.Collections.Generic;

namespace UI.Scripts
{
    public static class ButtonUtils
    {
        
        public static readonly Dictionary<UiButton.ButtonType, string> StrokeMatPath = new()
        {
            {UiButton.ButtonType.Negative, ""}
        };
        
        public static readonly Dictionary<UiButton.ButtonType, string> BackgroundMatPath = new()
        {
            {UiButton.ButtonType.Negative, ""}
        };

        public static readonly Dictionary<UiButton.ButtonType, string> StrokeSpritePath = new()
        {

        };
        
        public static readonly Dictionary<UiButton.ButtonType, string> BackgroundSpritePath = new()
        {

        };
        
    }
}