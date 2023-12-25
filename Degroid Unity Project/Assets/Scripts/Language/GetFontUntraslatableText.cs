using TMPro;
using UnityEngine;

/// <summary>
/// Получить шрифт для непереводимого текста
/// </summary>
public class GetFontUntraslatableText : MonoBehaviour
{
  private TextMeshProUGUI textField;

  private GameManager gameManager;

  //============================================================

  private void Awake()
  {
    textField = GetComponent<TextMeshProUGUI>();

    gameManager = GameManager.Instance;
  }

  private void OnEnable()
  {
    ChangeLanguage();

    gameManager.ChangeLanguage.AddListener(parValue => ChangeLanguage());
  }

  private void OnDisable()
  {
    ChangeLanguage();

    gameManager.ChangeLanguage.RemoveListener(parValue => ChangeLanguage());
  }

  //============================================================

  private void ChangeLanguage()
  {
    textField.font = LocalisationSystem.Instance.GetFont();
  }

  //============================================================
}