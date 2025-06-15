using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

namespace Common
{
    public class FontManager : MonoBehaviour
    {
        [SerializeField] private TMP_FontAsset koreanFont;
        [SerializeField] private TMP_FontAsset chineseFont;
        [SerializeField] private TMP_FontAsset japaneseFont;
        [SerializeField] private TMP_FontAsset englishFont;
        [SerializeField] private TMP_FontAsset spanishFont;

        private TMP_Text[] tmpTexts;

        void Awake()
        {
            koreanFont = Resources.Load<TMP_FontAsset>("Fonts & Materials/Jua-Regular SDF");
            chineseFont = Resources.Load<TMP_FontAsset>("Fonts & Materials/ZCOOLKuaiLe-Regular SDF");
            japaneseFont = Resources.Load<TMP_FontAsset>("Fonts & Materials/KiwiMaru-Medium SDF");
            englishFont = Resources.Load<TMP_FontAsset>("Fonts & Materials/Gorditas-Regular SDF");
            spanishFont = Resources.Load<TMP_FontAsset>("Fonts & Materials/Gorditas-Regular SDF");

            tmpTexts = Resources.FindObjectsOfTypeAll<TMP_Text>();
            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
            ApplyFont(LocalizationSettings.SelectedLocale.Identifier.Code);
        }

        private void OnDestroy()
        {
            LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
        }

        private void OnLocaleChanged(UnityEngine.Localization.Locale locale)
        {
            ApplyFont(locale.Identifier.Code);
        }

        private void ApplyFont(string localeCode)
        {
            TMP_FontAsset selectedFont = englishFont;

            // 언어 코드에 따른 폰트 선택
            switch (localeCode)
            {
                case "ko":
                    selectedFont = koreanFont;
                    break;
                case "ja":
                    selectedFont = japaneseFont;
                    break;
                case "zh":
                    selectedFont = chineseFont;
                    break;
                case "es":
                    selectedFont = spanishFont;
                    break;
            }

            foreach (var tmpText in tmpTexts)
            {
                tmpText.font = selectedFont;
                tmpText.SetText(tmpText.text);
            }
        }
    }
}