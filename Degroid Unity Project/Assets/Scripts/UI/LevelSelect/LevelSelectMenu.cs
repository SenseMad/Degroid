using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// ���� ������ �������
/// </summary>
public class LevelSelectMenu : MonoBehaviour
{
  [SerializeField, Tooltip("������ ������ �������")]
  private Panel _levelsSelectPanel;

  [SerializeField, Tooltip("������, ���� ����� ����������� ������")]
  private RectTransform _levelSelectPanel;

  [SerializeField, Tooltip("������ ������ ������ ������")]
  private GameObject _objectButtonLevelSelect;

  [SerializeField, Tooltip("������ ������ � �������")]
  private List<LevelData> _listLevelData;

  [SerializeField, Tooltip("����� ���������� ���������� �������")]
  private TextMeshProUGUI _numberComplitedLevelsText;

  //------------------------------------------------------------

  private int columns = 8; // ���������� ��������
  private readonly float timeMoveNextValue = 0.2f; // ����� �������� � ���������� ��������
  private float nextimeMoveNextValue = 0.0f;

  private List<LevelSelectButton> levelSelectButtons = new List<LevelSelectButton>();

  //============================================================

  private GameManager GameManager { get; set; }

  private PlayerInputHandler InputHandler { get; set; }

  private PanelController PanelController { get; set; }

  private AudioManager AudioManager { get; set; }

  private TransitionBetweenScenes TransitionBetweenScenes { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// ������ ������ �������
  /// </summary>
  public Panel LevelsSelectPanel { get => _levelsSelectPanel; set => _levelsSelectPanel = value; }

  /// <summary>
  /// ������, ���� ����� ����������� ������
  /// </summary>
  public RectTransform LevelSelectPanel { get => _levelSelectPanel; set => _levelSelectPanel = value; }

  /// <summary>
  /// ������ ������ � �������
  /// </summary>
  public List<LevelData> ListLevelData { get => _listLevelData; set => _listLevelData = value; }

  /// <summary>
  /// ����� ���������� ���������� �������
  /// </summary>
  public TextMeshProUGUI NumberComplitedLevelsText { get => _numberComplitedLevelsText; set => _numberComplitedLevelsText = value; }

  //------------------------------------------------------------

  /// <summary>
  /// ������ ��������� ������
  /// </summary>
  public int IndexActiveButton { get; set; }
  /// <summary>
  /// ������ ���������� ��������� ������
  /// </summary>
  public int IndexLastOpenLevel { get; set; }

  //============================================================

  private void Awake()
  {
    GameManager = GameManager.Instance;

    InputHandler = PlayerInputHandler.Instance;

    PanelController = FindObjectOfType<PanelController>();

    TransitionBetweenScenes = FindObjectOfType<TransitionBetweenScenes>();

    /*columns = (int)(LevelSelectPanel.GetComponent<RectTransform>().sizeDelta.x / 
      (LevelSelectPanel.GetComponent<GridLayoutGroup>().cellSize.x + LevelSelectPanel.GetComponent<GridLayoutGroup>().spacing.x));*/
  }

  private void Start()
  {
    AudioManager = AudioManager.Instance;
  }

  private void OnEnable()
  {
    UpdateTextNumberComplitedLevels();

    InputHandler.AI_Player.UI.Back.performed += OnClosePanel;
    InputHandler.AI_Player.UI.Select.performed += OnClickButtonInInterface;
  }

  private void OnDisable()
  {
    ClearListLevelSelect();

    InputHandler.AI_Player.UI.Back.performed -= OnClosePanel;
    InputHandler.AI_Player.UI.Select.performed -= OnClickButtonInInterface;
  }

  private void Update()
  {
    if (TransitionBetweenScenes.IsSceneTransition)
      return;

    if (PanelController.CurrentPanel == LevelsSelectPanel)
    {
      UpdateButtons();
    }
  }

  //============================================================

  private void OnClosePanel(InputAction.CallbackContext context)
  {
    if (TransitionBetweenScenes.IsSceneTransition)
      return;

    if (PanelController.CurrentPanel != LevelsSelectPanel) { return; }

    Sound();

    PanelController.ClosePanel();
  }

  /// <summary>
  /// �������� ������ ������ �������
  /// </summary>
  private void ClearListLevelSelect()
  {
    IndexActiveButton = 0;
    IndexLastOpenLevel = 0;

    for (int i = 0; i < levelSelectButtons.Count; i++)
    {
      Destroy(levelSelectButtons[i].gameObject);
    }

    levelSelectButtons.Clear();
  }

  public void DisplayLevelSelectionButtonsUI(int parNumberChapter)
  {
    int index = 0;
    switch (parNumberChapter)
    {
      case 1:
        for (int i = 0; i < 20; i++)
        {
          index = CreatingButton(i, index);
          NumberComplitedLevelsText.text = $"{index - 1}/20";
          if (GameManager.CountUnlockedLevel > 20)
          {
            NumberComplitedLevelsText.text = $"20/20";
          }
        }
        IndexActiveButton = index - 1;
        break;
      case 2:
        for (int i = 20; i < 40; i++)
        {
          index = CreatingButton(i, index);
          NumberComplitedLevelsText.text = $"{index - 1}/20";
          if (GameManager.CountUnlockedLevel > 40)
          {
            NumberComplitedLevelsText.text = $"20/20";
          }
        }

        IndexActiveButton = index - 1;
        break;
      case 3:
        for (int i = 40; i < 60; i++)
        {
          index = CreatingButton(i, index);
          NumberComplitedLevelsText.text = $"{index - 1}/20";
          if (GameManager.CountUnlockedLevel > 60)
          {
            NumberComplitedLevelsText.text = $"20/20";
          }
        }

        IndexActiveButton = index - 1;
        break;
      case 4:
        for (int i = 60; i < 68; i++)
        {
          index = CreatingButton(i, index);
          NumberComplitedLevelsText.text = $"{index - 1}/8";
          if (GameManager.CountUnlockedLevel > 68)
          {
            NumberComplitedLevelsText.text = $"8/8";
          }
        }

        IndexActiveButton = index - 1;
        break;
    }

    levelSelectButtons[IndexActiveButton].UpdateSelectImage(true);
  }

  private int CreatingButton(int parValue, int index)
  {
    var listLevelDataInstance = Instantiate(_objectButtonLevelSelect, LevelSelectPanel);
    LevelSelectButton button = listLevelDataInstance.GetComponent<LevelSelectButton>();

    if (ListLevelData[parValue].IndexLevel + 1 <= GameManager.CountUnlockedLevel)
    {
      index++;
      button.IsLevelOpen = true;
      button.IndexLevelText.gameObject.SetActive(true);
      button.UpdateLockImage(false);
      button.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }
    else
    {
      button.IsLevelOpen = false;
      button.IndexLevelText.gameObject.SetActive(false);
      button.UpdateLockImage(true);
      button.GetComponent<Image>().color = new Color32(90, 90, 90, 255);
    }

    button.UpdateSelectImage(false);
    button.Initialize(ListLevelData[parValue]);
    levelSelectButtons.Add(button);

    return index;
  }

  /// <summary>
  /// ������� ������ � ����������
  /// </summary>
  private void OnClickButtonInInterface(InputAction.CallbackContext context)
  {
    if (TransitionBetweenScenes.IsSceneTransition)
      return;

    if (PanelController.CurrentPanel != LevelsSelectPanel) { return; }

    AudioManager.OnPlaySoundButton?.Invoke();
    levelSelectButtons[IndexActiveButton].Button.onClick?.Invoke();

    TransitionBetweenScenes.StartSceneChange($"Level_{levelSelectButtons[IndexActiveButton].IndexLevel}");
  }

  private void Sound()
  {
    AudioManager.OnPlaySoundButton?.Invoke();
  }

  private void UpdateButtons()
  {
    if (Time.time > nextimeMoveNextValue)
    {
      nextimeMoveNextValue = Time.time + timeMoveNextValue;
      if (InputHandler.GetChangingValuesInput() > 0)
      {
        if (IndexActiveButton + 1 > levelSelectButtons.Count - 1) { return; }
        if (!levelSelectButtons[IndexActiveButton + 1].IsLevelOpen) { return; }

        levelSelectButtons[IndexActiveButton].UpdateSelectImage(false);
        IndexActiveButton++;
        levelSelectButtons[IndexActiveButton].UpdateSelectImage(true);
        Sound();
      }
      else if (InputHandler.GetChangingValuesInput() < 0)
      {
        if (IndexActiveButton - 1 < 0) { return; }

        levelSelectButtons[IndexActiveButton].UpdateSelectImage(false);
        IndexActiveButton--;
        levelSelectButtons[IndexActiveButton].UpdateSelectImage(true);
        Sound();
      }

      //------------------------------------------------------------

      if (InputHandler.GetNavigationInput() > 0)
      {
        if (IndexActiveButton - columns < 0) { return; }

        levelSelectButtons[IndexActiveButton].UpdateSelectImage(false);
        IndexActiveButton -= columns;
        levelSelectButtons[IndexActiveButton].UpdateSelectImage(true);
        Sound();
      }
      else if (InputHandler.GetNavigationInput() < 0)
      {
        if (IndexActiveButton + columns > levelSelectButtons.Count - 1) { return; }
        if (!levelSelectButtons[IndexActiveButton + columns].IsLevelOpen) { return; }

        levelSelectButtons[IndexActiveButton].UpdateSelectImage(false);
        IndexActiveButton += columns;
        levelSelectButtons[IndexActiveButton].UpdateSelectImage(true);
        Sound();
      }
    }

    if (InputHandler.GetChangingValuesInput() == 0 && InputHandler.GetNavigationInput() == 0)
    {
      nextimeMoveNextValue = Time.time;
    }
  }

  /// <summary>
  /// �������� ����� ���������� ���������� �������
  /// </summary>
  private void UpdateTextNumberComplitedLevels()
  {
    NumberComplitedLevelsText.text = $"{GameManager.CountUnlockedLevel - 1}/{ListLevelData.Count}";
  }

  //============================================================
}