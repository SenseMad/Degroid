using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Пользовательский интерфейс меню выбора главы
/// </summary>
public class UIChapterSelectMenu : MonoBehaviour
{
  [SerializeField, Tooltip("Панель, куда будут создаваться кнопки")]
  private RectTransform _chapterSelectPanel;

  [Header("ПРЕФАБ")]
  [SerializeField, Tooltip("Префаб кнопки выбора локации")]
  private UIChapterSelectButton _prefabButtonChapterSelect;

  [Header("МЕНЮ ВЫБОРА УРОВНЕЙ")]
  [SerializeField, Tooltip("Меню выбора уровня пользовательского интерфейса")]
  private LevelSelectMenu _LevelSelectMenu;

  [Header("ПАНЕЛЬ")]
  [SerializeField, Tooltip("Панель открытия выбора уровней")]
  private Panel _panelOpeningLevelSelection;

  [SerializeField, Tooltip("Текст количества пройденных уровней")]
  private TextMeshProUGUI _numberComplitedLevelsText;

  //------------------------------------------------------------

  private GameManager gameManager;

  private PlayerInputHandler inputHandler;

  private PanelController panelController;

  private AudioManager audioManager;

  /// <summary>
  /// Список кнопок выбора глав
  /// </summary>
  private List<UIChapterSelectButton> listUIChapterSelectButton = new List<UIChapterSelectButton>();

  private List<UIChapterSelectButton> listDestroyUIChapterSelectButton = new List<UIChapterSelectButton>();

  private readonly float timeMoveNextValue = 0.2f; // Время перехода к следующему значению
  private float nextimeMoveNextValue = 0.0f;

  //============================================================

  /// <summary>
  /// Индекс выбранной кнопки
  /// </summary>
  public int IndexActiveButton { get; set; }

  //============================================================

  private void Awake()
  {
    gameManager = GameManager.Instance;

    audioManager = AudioManager.Instance;

    panelController = FindObjectOfType<PanelController>();

    inputHandler = PlayerInputHandler.Instance;
  }

  private void OnEnable()
  {
    gameManager.GetOpenChapters();
    gameManager.GetNumberCompletedLevelChapters();

    UpdateTextNumberComplitedLevels();

    DisplayChapterSelectionButtonsUI();

    listUIChapterSelectButton[IndexActiveButton].UpdateSelectImage(true);

    inputHandler.AI_Player.UI.Back.performed += OnClosePanel;
    inputHandler.AI_Player.UI.Select.performed += OnClickButtonInInterface;
  }

  private void OnDisable()
  {
    ClearListLevelSelect();

    inputHandler.AI_Player.UI.Back.performed -= OnClosePanel;
    inputHandler.AI_Player.UI.Select.performed -= OnClickButtonInInterface;
  }

  private void Update()
  {
    UpdateButtons();
  }

  //============================================================

  /// <summary>
  /// Отобразить кнопки выбора главы в интерфейсе
  /// </summary>
  private void DisplayChapterSelectionButtonsUI()
  {
    for (int i = 0; i < 4; i++)
    {
      UIChapterSelectButton button = Instantiate(_prefabButtonChapterSelect, _chapterSelectPanel);
      button.Button = button.GetComponent<Button>();
      button.UpdateSelectImage(false);
      button.UpdateLockImage(true);

      if (gameManager.OpenChapters[i])
      {
        button.UpdateLockImage(false);
        button.Button.interactable = true;
        listUIChapterSelectButton.Add(button);
      }

      if (i == 3)
      {
        if (gameManager.CountCollectedItems.Count < 60)
        {
          button.NumberItems.SetActive(true);
          button.NumberItemsText.text = $"{gameManager.CountCollectedItems.Count}/60";
        }
      }

      button.Initialize(i + 1, gameManager.NumberCompletedLevelChapters[i + 1].numberLevels, gameManager.NumberCompletedLevelChapters[i + 1].numberCompletedLevels);
      listDestroyUIChapterSelectButton.Add(button);

      button.Button.onClick.AddListener(() => SelectChapter(button.NumberChapter));
    }

    /*if (gameManager.CountCollectedItems.Count >= 0)
    {
      UIChapterSelectButton button = Instantiate(_prefabButtonChapterSelect, _chapterSelectPanel);
      button.Button = button.GetComponent<Button>();
      button.UpdateSelectImage(false);
      button.UpdateLockImage(true);

      if (gameManager.OpenChapters[3] && gameManager.CountCollectedItems.Count >= 60)
      {
        button.UpdateLockImage(false);
        button.Button.interactable = true;
        listUIChapterSelectButton.Add(button);
      }
      else
      {
        button.NumberItemsText.text = $"{gameManager.CountCollectedItems.Count}/60";
      }

      button.Initialize(4 , gameManager.NumberCompletedLevelChapters[4].numberLevels, gameManager.NumberCompletedLevelChapters[4].numberCompletedLevels);
      listDestroyUIChapterSelectButton.Add(button);

      button.Button.onClick.AddListener(() => SelectChapter(4));
    }*/

    IndexActiveButton = listUIChapterSelectButton.Count - 1;
  }

  private void OnClosePanel(InputAction.CallbackContext context)
  {
    Sound();
    panelController.ClosePanel();
  }

  private void ClearListLevelSelect()
  {
    for (int i = 0; i < listDestroyUIChapterSelectButton.Count; i++)
    {
      Destroy(listDestroyUIChapterSelectButton[i].gameObject);
    }

    listDestroyUIChapterSelectButton.Clear();
    listUIChapterSelectButton = new List<UIChapterSelectButton>();
  }

  /// <summary>
  /// Обновить текст количества пройденных уровней
  /// </summary>
  private void UpdateTextNumberComplitedLevels()
  {
    _numberComplitedLevelsText.text = $"{gameManager.CountUnlockedLevel - 1}/68";
  }

  //============================================================

  private void SelectChapter(int index)
  {
    panelController.ShowInHidePanel(_panelOpeningLevelSelection);

    _LevelSelectMenu.DisplayLevelSelectionButtonsUI(index);
  }

  private void Sound()
  {
    audioManager.OnPlaySoundButton?.Invoke();
  }

  private void UpdateButtons()
  {
    if (Time.time > nextimeMoveNextValue)
    {
      nextimeMoveNextValue = Time.time + timeMoveNextValue;
      if (inputHandler.GetChangingValuesInput() > 0)
      {
        listUIChapterSelectButton[IndexActiveButton].UpdateSelectImage(false);

        IndexActiveButton++;
        if (IndexActiveButton + 1 > listUIChapterSelectButton.Count) { IndexActiveButton = 0; }

        Sound();
        listUIChapterSelectButton[IndexActiveButton].UpdateSelectImage(true);
      }
      else if (inputHandler.GetChangingValuesInput() < 0)
      {
        listUIChapterSelectButton[IndexActiveButton].UpdateSelectImage(false);

        IndexActiveButton--;
        if (IndexActiveButton < 0) { IndexActiveButton = listUIChapterSelectButton.Count - 1; }

        Sound();
        listUIChapterSelectButton[IndexActiveButton].UpdateSelectImage(true);
      }
    }

    if (inputHandler.GetChangingValuesInput() == 0)
    {
      nextimeMoveNextValue = Time.time;
    }
  }

  /// <summary>
  /// Нажатие кнопки в интерфейсе
  /// </summary>
  private void OnClickButtonInInterface(InputAction.CallbackContext context)
  {
    audioManager.OnPlaySoundButton?.Invoke();
    listUIChapterSelectButton[IndexActiveButton].Button.onClick?.Invoke();
  }

  //============================================================
}