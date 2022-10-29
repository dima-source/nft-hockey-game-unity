using TMPro;
using UnityEngine;
using Utils;

namespace UI.Main_menu.UIPopups
{
    public class SeedPhraseView : MonoBehaviour
    {
        [SerializeField] public TMP_Text SeedPhraseText;

        public void CopySeedPhrase()
        {
            SeedPhraseText.text.CopyToClipboard();
        }
    }
}