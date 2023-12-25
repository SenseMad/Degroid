using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LanguageKey : MonoBehaviour
{
  [SerializeField, Tooltip("Ключ перевода")]
  private string _key;

  //============================================================

  private GameManager GameManager { get; set; }

  private TextMeshProUGUI TextField;

  //------------------------------------------------------------

  /// <summary>
  /// Ключ перевода
  /// </summary>
  public string Key { get => _key; set => _key = value; }

  //============================================================

  private void Awake()
  {
    GameManager = GameManager.Instance;
    TextField = GetComponent<TextMeshProUGUI>();
  }

  private void OnEnable()
  {
    UpdateText();

    GameManager.ChangeLanguage.AddListener(ChangeLanguage);
  }

  private void OnDisable()
  {
    GameManager.ChangeLanguage.RemoveListener(ChangeLanguage);
  }

  //============================================================

  private void ChangeLanguage(Language language)
  {
    UpdateText();
  }

  private void UpdateText()
  {
    if (_key == "") { return; }

    string value = LocalisationSystem.Instance.GetLocalisedValue(Key);

    value = value.TrimStart(' ', '"');
    value = value.Replace("\"", "");
    TextField.text = value;
    TextField.font = LocalisationSystem.Instance.GetFont();
  }

  //============================================================
}