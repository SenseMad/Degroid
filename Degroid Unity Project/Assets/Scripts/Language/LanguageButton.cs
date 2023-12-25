using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour
{
  [SerializeField, Tooltip("Image языка")]
  private Image _imageLanguage;

  [SerializeField, Tooltip("Текст выделения языка")]
  private TextMeshProUGUI _languageSelectionText;

  [SerializeField, Tooltip("Объект Arrow выделенного языка")]
  private GameObject _gameObjectSelectArrow;

  [SerializeField, Tooltip("Image Arrow выделенного языка")]
  private Image _imageArrowSelect;

  //============================================================

  private GameManager GameManager { get; set; }

  public Button Button { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// Выбранный язык
  /// </summary>
  public Language IndexLanguage { get; set; }

  /// <summary>
  /// True, если язык выбран
  /// </summary>
  public bool IsLanguageSelected { get; set; }

  //============================================================

  private void Awake()
  {
    GameManager = GameManager.Instance;

    Button = GetComponent<Button>();
  }

  //============================================================

  public void EnableDisableLanguageDisplay(bool parValue)
  {
    if (parValue)
    {
      _languageSelectionText.color = ColorsGame.SELECTED_COLOR;
      _gameObjectSelectArrow.SetActive(true);
      _imageArrowSelect.color = ColorsGame.SELECTED_COLOR;
    }
    else
    {
      _languageSelectionText.color = ColorsGame.STANDART_COLOR;
      _gameObjectSelectArrow.SetActive(false);
      _imageArrowSelect.color = ColorsGame.STANDART_COLOR;
    }
  }

  /// <summary>
  /// Обновить текст выбора языка
  /// </summary>
  public void UpdateLanguageSelectionText(string parValue)
  {
    _languageSelectionText.text = parValue.ToLower();
  }

  /// <summary>
  /// Инициализация кнопки выбора языка
  /// </summary>
  public void Initialize(Languages languages)
  {
    IndexLanguage = languages.Language;
    _imageLanguage.sprite = languages.LanguageSprite;
    UpdateLanguageSelectionText(languages.LanguageName);

    _languageSelectionText.font = LocalisationSystem.Instance.GetLocalizationFont(languages.Language);
  }

  /// <summary>
  /// Выбрать язык
  /// </summary>
  public void ChangeLanguage()
  {
    if (GameManager.CurrentLanguage == IndexLanguage) { return; }

    GameManager.CurrentLanguage = IndexLanguage;
    IsLanguageSelected = true;

    GameManager.SaveData();
  }

  //============================================================
}