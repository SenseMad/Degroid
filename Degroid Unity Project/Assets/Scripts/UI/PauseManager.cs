using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;
using UnityEngine.InputSystem;
using System.Linq;

public class PauseManager : MonoBehaviour
{
  [SerializeField, Tooltip("Панель паузы")]
  private Panel _pausePanel;

  [SerializeField, Tooltip("Список кнопок")]
  private List<Button> _listButtons;

  //------------------------------------------------------------

  private float tempTime = 0;

  //============================================================

  private GameManager GameManager { get; set; }

  private PlayerInputHandler PlayerInputHandler { get; set; }

  private PanelController PanelController { get; set; }
  private LevelManager LevelManager { get; set; }

  private AudioManager AudioManager { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// Панель паузы
  /// </summary>
  public Panel PausePanel { get => _pausePanel; set => _pausePanel = value; }

  /// <summary>
  /// Список кнопок
  /// </summary>
  public List<Button> ListButtons { get; private set; }//{ get => _listButtons; set => _listButtons = value; }

  //------------------------------------------------------------

  public int IndexActiveButton { get; set; }

  /// <summary>
  /// True, если игра остановлена
  /// </summary>
  private bool IsPause { get; set; }

  private bool IsPressed { get; set; } = false;

  //============================================================

  /// <summary>
  /// Событие: Проиграть звук нажатия кнопки
  /// </summary>
  public CustomUnityEvent OnPlaySoundButton { get; } = new CustomUnityEvent();

  //============================================================

  private void Awake()
  {
    PlayerInputHandler = PlayerInputHandler.Instance;
    GameManager = GameManager.Instance;

    PanelController = FindObjectOfType<PanelController>();
    LevelManager = FindObjectOfType<LevelManager>();
  }

  private void OnEnable()
  {
    PlayerInputHandler.AI_Player.UI.Select.performed += OnClickButtonInInterface;
    PlayerInputHandler.AI_Player.UI.Back.performed += OnBack;
    PlayerInputHandler.AI_Player.UI.Pause.performed += OnSetIsPause;

    ListButtons = new List<Button>(_listButtons.Where((button) => button.gameObject.activeSelf));
  }

  private void OnDisable()
  {
    PlayerInputHandler.AI_Player.UI.Select.performed -= OnClickButtonInInterface;
    PlayerInputHandler.AI_Player.UI.Back.performed -= OnBack;
    PlayerInputHandler.AI_Player.UI.Pause.performed -= OnSetIsPause;
  }

  private void Start()
  {
    AudioManager = AudioManager.Instance;
  }

  private void Update()
  {
    if (LevelManager.TransitionBetweenScenes.IsSceneTransition)
      return;

    UpdateButtons();

    if (IsPause)
    {
      tempTime += Time.deltaTime;
    }
    else
    {
      tempTime = 0;
    }
  }

  //============================================================

  private void Sound()
  {
    AudioManager.OnPlaySoundButton?.Invoke();
  }

  private void UpdateButtons()
  {
    if (IsPause && PanelController.CurrentPanel == PausePanel)
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
  }

  /// <summary>
  /// Нажатие кнопки в интерфейсе
  /// </summary>
  private void OnClickButtonInInterface(InputAction.CallbackContext context)
  {
    if (LevelManager.TransitionBetweenScenes.IsSceneTransition)
      return;

    if (PanelController.CurrentPanel != PausePanel)
      return;

    AudioManager.OnPlaySoundButton?.Invoke();
    ListButtons[IndexActiveButton].onClick?.Invoke();
  }

  /// <summary>
  /// Включить/Выключить паузу
  /// </summary>
  private void OnSetIsPause(InputAction.CallbackContext context)
  {
    if (LevelManager.TransitionBetweenScenes.IsSceneTransition)
      return;

    if (LevelManager.IsLevelComplite)
      return;

    if (!IsPause)
    {
      Pause();
    }
    else
    {
      UnPause();
    }

    Sound();
  }

  private void OnBack(InputAction.CallbackContext context)
  {
    if (LevelManager.TransitionBetweenScenes.IsSceneTransition)
      return;

    if (LevelManager.IsLevelComplite)
      return;

    if (!IsPause)
      return;

    if (PanelController.CurrentPanel != PausePanel)
      return;

    if (tempTime < 0.5f)
      return;

    UnPause();
    Sound();
  }

  private void Pause()
  {
    EnablePause();

    IsPause = true;
    LevelManager.IsPause = IsPause;
    LevelManager.OnPause?.Invoke(IsPause);
    PanelController.AddPanelList(PausePanel);
    //Time.timeScale = 0;
  }

  private void UnPause()
  {
    if (PanelController.CurrentPanel == PausePanel)
    {
      if (PanelController.ListAllOpenPanels.Count > 1)
      {
        PanelController.ClosePanel();
      }
      else
      {
        IsPause = false;
        LevelManager.IsPause = IsPause;
        LevelManager.OnPause?.Invoke(IsPause);
        InitialStateButton();
        PanelController.CloseAllPanels();
      }
    }
  }

  /// <summary>
  /// При открытии паузы
  /// </summary>
  private void EnablePause()
  {
    IndexActiveButton = 0;
    ListButtons[IndexActiveButton].GetComponent<Animator>().SetTrigger("Play");
    Sound();
    ListButtons[IndexActiveButton].GetComponentInChildren<TextMeshProUGUI>().color = ColorsGame.SELECTED_COLOR;
  }

  /// <summary>
  /// Начальное состояние кнопок
  /// </summary>
  private void InitialStateButton()
  {
    ListButtons[IndexActiveButton].GetComponentInChildren<TextMeshProUGUI>().color = ColorsGame.STANDART_COLOR;
  }

  //============================================================

  /// <summary>
  /// Нажатие на кнопку продолжить
  /// </summary>
  public void OnButtonContinue()
  {
    Sound();
    IsPause = false;
    LevelManager.IsPause = false;
    LevelManager.OnPause?.Invoke(IsPause);
    InitialStateButton();
    PanelController.ClosePanel();
  }

  /// <summary>
  /// Нажатие на кнопку перезапуска
  /// </summary>
  public void OnButtonRestart()
  {
    Sound();
    LevelManager.IsPause = false;
    LevelManager.OnPause?.Invoke(IsPause);
    GameManager.NumberGamesPlayed++;

    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  /// <summary>
  /// Нажатие на кнопку выхода в меню
  /// </summary>
  public void OnButtonExitMenu()
  {
    Sound();

    GameManager.SaveData();

    LevelManager.TransitionBetweenScenes.StartSceneChange("MainMenu");
  }

  //============================================================
}