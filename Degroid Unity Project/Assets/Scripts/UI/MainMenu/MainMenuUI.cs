using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
//using Steamworks;
using TMPro;
using UnityEngine.InputSystem;
using System.Linq;

public class MainMenuUI : MonoBehaviour
{
  [SerializeField, Tooltip("Список кнопок")]
  private List<Button> _listButtons;

  //============================================================

  private GameManager GameManager { get; set; }

  private PlayerInputHandler PlayerInputHandler { get; set; }

  private PanelController PanelController { get; set; }

  private AudioManager AudioManager { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// Список кнопок
  /// </summary>
  public List<Button> ListButtons { get; private set; }//{ get => _listButtons; set => _listButtons = value; }

  //------------------------------------------------------------

  /// <summary>
  /// Индекс выбранной кнопки
  /// </summary>
  public int IndexActiveButton { get; set; }

  private bool IsPressed { get; set; } = false;

  //============================================================

  /// <summary>
  /// Событие: Проиграть звук нажатия кнопки
  /// </summary>
  public CustomUnityEvent OnPlaySoundButton { get; } = new CustomUnityEvent();

  //============================================================

  private void Awake()
  {
    GameManager = GameManager.Instance;

    PanelController = FindObjectOfType<PanelController>();

    PlayerInputHandler = PlayerInputHandler.Instance;
  }

  private void OnEnable()
  {
    PlayerInputHandler.AI_Player.UI.Select.performed += OnClickButtonInInterface;

    ListButtons = new List<Button>(_listButtons.Where((button) => button.gameObject.activeSelf));

    EnableMenu();
  }

  private void OnDisable()
  {
    PlayerInputHandler.AI_Player.UI.Select.performed -= OnClickButtonInInterface;
  }

  private void Start()
  {
    AudioManager = AudioManager.Instance;
  }

  private void Update()
  {
    UpdateButtons();
  }

  //============================================================

  /// <summary>
  /// Нажатие кнопки в интерфейсе
  /// </summary>
  private void OnClickButtonInInterface(InputAction.CallbackContext context)
  {
    AudioManager.OnPlaySoundButton?.Invoke();
    ListButtons[IndexActiveButton].onClick?.Invoke();
  }

  private void Sound()
  {
    AudioManager.OnPlaySoundButton?.Invoke();
  }

  private void UpdateButtons()
  {
    if (!IsPressed)
    {
      if (PlayerInputHandler.GetNavigationInput() > 0)
      {
        ListButtons[IndexActiveButton].GetComponentInChildren<TextMeshProUGUI>().color = ColorsGame.STANDART_COLOR;

        IndexActiveButton--;
        if (IndexActiveButton < 0) { IndexActiveButton = ListButtons.Count - 1; }

        ListButtons[IndexActiveButton].GetComponent<Animator>().SetTrigger("Play");
        Sound();
        ListButtons[IndexActiveButton].GetComponentInChildren<TextMeshProUGUI>().color = ColorsGame.SELECTED_COLOR;
        IsPressed = true;
      }
      else if (PlayerInputHandler.GetNavigationInput() < 0)
      {
        ListButtons[IndexActiveButton].GetComponentInChildren<TextMeshProUGUI>().color = ColorsGame.STANDART_COLOR;

        IndexActiveButton++;
        if (IndexActiveButton + 1 > ListButtons.Count) { IndexActiveButton = 0; }

        ListButtons[IndexActiveButton].GetComponent<Animator>().SetTrigger("Play");
        Sound();
        ListButtons[IndexActiveButton].GetComponentInChildren<TextMeshProUGUI>().color = ColorsGame.SELECTED_COLOR;
        IsPressed = true;
      }
    }

    if (IsPressed && PlayerInputHandler.GetNavigationInput() == 0)
    {
      IsPressed = false;
    }
  }

  /// <summary>
  /// При открытии меню
  /// </summary>
  private void EnableMenu()
  {
    ListButtons[IndexActiveButton].GetComponentInChildren<TextMeshProUGUI>().color = ColorsGame.STANDART_COLOR;

    IndexActiveButton = 0;

    ListButtons[IndexActiveButton].GetComponent<Animator>().SetTrigger("Play");
    ListButtons[IndexActiveButton].GetComponentInChildren<TextMeshProUGUI>().color = ColorsGame.SELECTED_COLOR;
  }

  public void PlayGame()
  {
    SceneManager.LoadScene(1);
  }

  /// <summary>
  /// Выход из игры
  /// </summary>
  public void ExitGame()
  {
#if !UNITY_PS4
    GameManager.SaveData();
    Application.Quit();
#endif
  }

  //============================================================
}