using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DeleteSave : MonoBehaviour
{
  [SerializeField, Tooltip("")]
  private Button _yesButton;
  [SerializeField, Tooltip("")]
  private Button _noButton;

  //------------------------------------------------------------

  private List<Button> buttons = new List<Button>();

  private readonly float timeMoveNextValue = 0.2f; // Время перехода к следующему значения
  private float nextimeMoveNextValue = 0.0f;

  //============================================================

  private GameManager GameManager { get; set; }

  private PlayerInputHandler InputHandler { get; set; }

  private PanelController PanelController { get; set; }

  private AudioManager AudioManager { get; set; }

  private int IndexActiveButton { get; set; }

  //============================================================

  private void Awake()
  {
    GameManager = GameManager.Instance;

    InputHandler = PlayerInputHandler.Instance;

    PanelController = FindObjectOfType<PanelController>();
  }

  private void Start()
  {
    AudioManager = AudioManager.Instance;

    buttons.Add(_yesButton);
    buttons.Add(_noButton);

    IndexActiveButton = 1;
    buttons[IndexActiveButton].GetComponentInChildren<TextMeshProUGUI>().color = ColorsGame.SELECTED_COLOR;
  }

  private void OnEnable()
  {
    InputHandler.AI_Player.UI.Select.performed += OnClickButtonInInterface;

    _yesButton.onClick.AddListener(() => OnDeleteSave());
    _noButton.onClick.AddListener(() => PanelController.DoNotCloseLastWindow());
  }

  private void OnDisable()
  {
    InputHandler.AI_Player.UI.Select.performed -= OnClickButtonInInterface;

    _yesButton.onClick.RemoveListener(() => OnDeleteSave());
    _noButton.onClick.RemoveListener(() => PanelController.DoNotCloseLastWindow());
  }

  private void Update()
  {
    if (InputHandler.GetButtonBack())
    {
      ClosePanel();
    }

    if (Time.time > nextimeMoveNextValue)
    {
      nextimeMoveNextValue = Time.time + timeMoveNextValue;
      if (InputHandler.GetChangingValuesInput() < 0)
      {
        buttons[IndexActiveButton].GetComponentInChildren<TextMeshProUGUI>().color = ColorsGame.STANDART_COLOR;

        IndexActiveButton++;
        if (IndexActiveButton + 1 > buttons.Count) { IndexActiveButton = 0; }

        Sound();
        buttons[IndexActiveButton].GetComponentInChildren<TextMeshProUGUI>().color = ColorsGame.SELECTED_COLOR;
      }
      else if (InputHandler.GetChangingValuesInput() > 0)
      {
        buttons[IndexActiveButton].GetComponentInChildren<TextMeshProUGUI>().color = ColorsGame.STANDART_COLOR;

        IndexActiveButton--;
        if (IndexActiveButton < 0) { IndexActiveButton = buttons.Count - 1; }

        Sound();
        buttons[IndexActiveButton].GetComponentInChildren<TextMeshProUGUI>().color = ColorsGame.SELECTED_COLOR;
      }
    }

    if (InputHandler.GetChangingValuesInput() == 0)
    {
      nextimeMoveNextValue = Time.time;
    }
  }

  //============================================================

  public void OnDeleteSave()
  {
    GameManager.OnDeleteSave();
    PanelController.DoNotCloseLastWindow();
  }

  //============================================================

  private void ClosePanel()
  {
    Sound();
    PanelController.DoNotCloseLastWindow();
  }

  /// <summary>
  /// Нажатие кнопки в интерфейсе
  /// </summary>
  private void OnClickButtonInInterface(InputAction.CallbackContext context)
  {
    AudioManager.OnPlaySoundButton?.Invoke();
    buttons[IndexActiveButton].onClick?.Invoke();
  }

  private void Sound()
  {
    AudioManager.OnPlaySoundButton?.Invoke();
  }

  //============================================================
}