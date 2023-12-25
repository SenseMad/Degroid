using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogUI : MonoBehaviour
{
  [SerializeField, Tooltip("")]
  private Image _dialogImage;
  [SerializeField, Tooltip("Текстовый объект, в который будет записан текст")]
  private TextMeshProUGUI _dialogText;

  //------------------------------------------------------------

  private CanvasGroup canvasGroup;
  private LevelManager levelManager;

  /// <summary>
  /// Задержка написания текста
  /// </summary>
  private const float letterDelay = 0.1f;

  /// <summary>
  /// Текущий текст
  /// </summary>
  private string currentText;

  private float timer = 0;

  //============================================================

  private void Awake()
  {
    canvasGroup = GetComponent<CanvasGroup>();

    levelManager = LevelManager.Instance;
  }

  private void Update()
  {
    if (canvasGroup.alpha < 1 || levelManager.IsPause)
      return;

    if (_dialogText.text.Length < currentText.Length)
    {
      timer += Time.deltaTime;
      if (timer >= letterDelay)
      {
        _dialogText.text += currentText[_dialogText.text.Length];
        _dialogText.text = _dialogText.text.Replace('"'.ToString(), string.Empty);
        timer = 0;
      }
    }
  }

  //============================================================

  /// <summary>
  /// Изменить текущий текст
  /// </summary>
  /// <param name="parText"></param>
  public void ChangeCurrentText(string parText)
  {
    _dialogText.text = "";
    timer = 0;
    currentText = parText;
  }

  /// <summary>
  /// Изменить картинку
  /// </summary>
  /// <param name="parSprite"></param>
  public void ChangeImage(Sprite parSprite)
  {
    _dialogImage.sprite = parSprite;
  }

  /// <summary>
  /// Напечатать весь текст
  /// </summary>
  public void SkipAllText()
  {
    _dialogText.text = currentText;
    _dialogText.text = _dialogText.text.Replace('"'.ToString(), string.Empty);
  }

  public void Show()
  {
    gameObject.SetActive(true);
  }

  public void Hide()
  {
    gameObject.SetActive(false);
  }

  /// <summary>
  /// True, если диалог показан
  /// </summary>
  public bool DialogIsShown()
  {
    return gameObject.activeSelf;
  }

  /// <summary>
  /// True, если текст печатается
  /// </summary>
  public bool IsPrinted()
  {
    return _dialogText.text.Length != currentText.Length;
  }

  //============================================================
}