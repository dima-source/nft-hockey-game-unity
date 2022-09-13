using System.Collections.Generic;
using System.Linq;
using UI.Scripts.Constraints;
using UnityEngine;

namespace UI.Scripts.Card
{
    public class CardRarenessCharacteristic : CharacteristicImpl<string>
    {

        private static readonly Dictionary<string, Color> RARENESS_COLOR_MAP = new()
        {
            {"Common", new Color32(205, 215, 218, 255)},
            {"Uncommon", new Color32(114, 165, 212, 255)},
            {"Rare", new Color32(62, 88, 222, 255)},
            {"Unique", new Color32(170, 23, 222, 255)},
            {"Exclusive", new Color32(236, 8, 145, 255)}
        };

        public CardRarenessCharacteristic(string characteristic) : base(characteristic,
            SetConstraint<string>.FromValues(RARENESS_COLOR_MAP.Keys.ToArray())) 
        {
            
        }

        public override string ToString()
        {
            return characteristic;
        }

        public Color ToColor()
        {
            return RARENESS_COLOR_MAP[characteristic];
        }

    }
}