using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogSystem : MonoBehaviour
{
  private static DialogSystem instance;

  //------------------------------------------------------------

  [SerializeField, Tooltip("������ ���������� ������")]
  private List<DialogData> _dialogData = new List<DialogData>();

  [Header("UI")]
  [SerializeField, Tooltip("������� ������� Degroid (����� ������)")]
  private DialogUI _dialogDegroidUI;
  [SerializeField, Tooltip("������� ������� Doctor (������ ������)")]
  private DialogUI _dialogDoctorUI;

  //------------------------------------------------------------

  private LevelManager levelManager;

  private GameManager gameManager;

  private PlayerInputHandler inputHandler;

  /// <summary>
  /// ������� ������ �������
  /// </summary>
  private int currentDialogueIndex = 0;

  private DialogUI currentDialogUI;

  //============================================================

  public static DialogSystem Instance { get => instance; }

  //------------------------------------------------------------

  /// <summary>
  /// True, ���� ������ �������
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
  /// ��������� ������
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
  /// ������ � ��������
  /// </summary>
  [System.Serializable]
  public class DialogData
  {
    /// <summary>
    /// True, ���� ����� ������
    /// </summary>
    public bool _leftDialog;

    /// <summary>
    /// ���� �������
    /// </summary>
    public string _dialogKey;

    /// <summary>
    /// ������ ���������
    /// </summary>
    public Sprite _characterSprite;
  }

  //============================================================
}