using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogSystem : MonoBehaviour
{
  private static DialogSystem instance;

  //------------------------------------------------------------

  [SerializeField, Tooltip("Список диалоговых данных")]
  private List<DialogData> _dialogData = new List<DialogData>();

  [Header("UI")]
  [SerializeField, Tooltip("Объекта диалога Degroid (Левый диалог)")]
  private DialogUI _dialogDegroidUI;
  [SerializeField, Tooltip("Объекта диалога Doctor (Правый диалог)")]
  private DialogUI _dialogDoctorUI;

  //------------------------------------------------------------

  private LevelManager levelManager;

  private GameManager gameManager;

  private PlayerInputHandler inputHandler;

  /// <summary>
  /// Текущий индекс диалога
  /// </summary>
  private int currentDialogueIndex = 0;

  private DialogUI currentDialogUI;

  //============================================================

  public static DialogSystem Instance { get => instance; }

  //------------------------------------------------------------

  /// <summary>
  /// True, если диалог запущен
  /// </summary>
  public bool IsDialogStarted
  {
    get
    {
      return currentDialogUI != null;
    }
  }

  //============================================================

  private void Awake()
  {
    inputHandler = PlayerInputHandler.Instance;

    levelManager = LevelManager.Instance;

    gameManager = GameManager.Instance;

    instance = this;
  }

  private void Start()
  {
    if (GameManager.Instance.GetDialogsViewed(levelManager.LevelData.IndexLevel))
      return;

    StartDialog();
  }

  private void OnEnable()
  {
    inputHandler.AI_Player.UI.Select.performed += OnNextDialog;
  }

  private void OnDisable()
  {
    inputHandler.AI_Player.UI.Select.performed -= OnNextDialog;
  }

  //============================================================

  /// <summary>
  /// Следующий диалог
  /// </summary>
  private void OnNextDialog(InputAction.CallbackContext context)
  {
    if (levelManager.IsPause || !IsDialogStarted)
      return;

    if (currentDialogUI.IsPrinted())
    {
      currentDialogUI.SkipAllText();
      return;
    }
    else
    {
      currentDialogueIndex++;
      ShowCurrentDialog(currentDialogueIndex);
    }
  }

  private DialogUI GetDialog(bool parLeft)
  {
    return parLeft ? _dialogDegroidUI : _dialogDoctorUI;
  }

  //============================================================

  private void StartDialog()
  {
    if (IsDialogStarted)
      return;

    ShowCurrentDialog(0);
  }

  private void ShowCurrentDialog(int parIndexDialog)
  {
    currentDialogueIndex = parIndexDialog;
    if (currentDialogueIndex < _dialogData.Count)
    {
      var dialogData = _dialogData[currentDialogueIndex];
      DialogUI dialog = GetDialog(dialogData._leftDialog);
      if (dialog != currentDialogUI)
      {
        currentDialogUI?.Hide();
        currentDialogUI = dialog;
        currentDialogUI.Show();
      }

      var localisationSystem = LocalisationSystem.Instance.GetLocalisedValue(dialogData._dialogKey);
      currentDialogUI.ChangeCurrentText(localisationSystem);
      currentDialogUI.ChangeImage(dialogData._characterSprite);
      return;
    }

    currentDialogUI?.Hide();
    currentDialogUI = null;
    gameManager.AddDialogsViewed(levelManager.LevelData.IndexLevel);
  }

  private void HideCurrentDialog()
  {
    currentDialogUI?.Hide();
  }

  //============================================================

  private void OnTriggerEnter2D(Collider2D other)
  {
    //StartDialog();
  }

  //============================================================

  /// <summary>
  /// Данные о диалогах
  /// </summary>
  [System.Serializable]
  public class DialogData
  {
    /// <summary>
    /// True, если левый диалог
    /// </summary>
    public bool _leftDialog;

    /// <summary>
    /// Ключ диалога
    /// </summary>
    public string _dialogKey;

    /// <summary>
    /// Спрайт персонажа
    /// </summary>
    public Sprite _characterSprite;
  }

  //============================================================
}