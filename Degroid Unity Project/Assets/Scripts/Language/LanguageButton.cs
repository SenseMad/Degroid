using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour
{
  [SerializeField, Tooltip("Image �����")]
  private Image _imageLanguage;

  [SerializeField, Tooltip("����� ��������� �����")]
  private TextMeshProUGUI _languageSelectionText;

  [SerializeField, Tooltip("������ Arrow ����������� �����")]
  private GameObject _gameObjectSelectArrow;

  [SerializeField, Tooltip("Image Arrow ����������� �����")]
  private Image _imageArrowSelect;

  //============================================================

  private GameManager GameManager { get; set; }

  public Button Button { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// ��������� ����
  /// </summary>
  public Language IndexLanguage { get; set; }

  /// <summary>
  /// True, ���� ���� ������
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
  /// �������� ����� ������ �����
  /// </summary>
  public void UpdateLanguageSelectionText(string parValue)
  {
    _languageSelectionText.text = parValue.ToLower();
  }

  /// <summary>
  /// ������������� ������ ������ �����
  /// </summary>
  public void Initialize(Languages languages)
  {
    IndexLanguage = languages.Language;
    _imageLanguage.sprite = languages.LanguageSprite;
    UpdateLanguageSelectionText(languages.LanguageName);

    _languageSelectionText.font = LocalisationSystem.Instance.GetLocalizationFont(languages.Language);
  }

  /// <summary>
  /// ������� ����
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