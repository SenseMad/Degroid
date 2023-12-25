using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LanguageSelectMenu : MonoBehaviour
{
  [SerializeField, Tooltip("������ ������ �����")]
  private Panel _languageSelectPanel;

  [SerializeField, Tooltip("���� ����� ����������� ������")]
  private RectTransform _languageSelectSpacer;

  [SerializeField, Tooltip("������ ������ ������ �����")]
  private GameObject _objectButtonLanguageSelect;

  [SerializeField, Tooltip("������ ������")]
  private List<Languages> _listLanguages = new List<Languages>();

  //------------------------------------------------------------

  private const int columns = 6; // ���������� ��������
  private readonly float timeMoveNextValue = 0.2f; // ����� �������� � ���������� ��������
  private float nextimeMoveNextValue = 0.0f;

  private List<LanguageButton> languageButtons = new List<LanguageButton>();

  //============================================================

  private GameManager GameManager { get; set; }

  private PlayerInputHandler InputHandler { get; set; }

  private PanelController PanelController { get; set; }

  private AudioManager AudioManager { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// ������ ��������� ������
  /// </summary>
  public int IndexActiveButton { get; set; }

  /// <summary>
  /// ������ ������
  /// </summary>
  public List<Languages> ListLanguages { get => _listLanguages; }

  //============================================================

  private void Awake()
  {
    GameManager = GameManager.Instance;

    InputHandler = PlayerInputHandler.Instance;

    PanelController = FindObjectOfType<PanelController>();
  }

  private void OnEnable()
  {
    //GameManager.ChangeLanguage.AddListener(parValue => UpdateIconLanguage());

    //InputHandler.AI_Player.UI.Back.performed += OnClosePanel;
    //InputHandler.AI_Player.UI.Pause.performed += OnClosePanel;
    InputHandler.AI_Player.UI.Select.performed += OnClickButtonInInterface;
  }

  private void OnDisable()
  {
    //GameManager.ChangeLanguage.RemoveListener(parValue => UpdateIconLanguage());

    //InputHandler.AI_Player.UI.Back.performed -= OnClosePanel;
    //InputHandler.AI_Player.UI.Pause.performed -= OnClosePanel;
    InputHandler.AI_Player.UI.Select.performed -= OnClickButtonInInterface;
  }

  private void Start()
  {
    AudioManager = AudioManager.Instance;

    UpdateIconLanguage();

    AddLanguagesList();
  }

  private void Update()
  {
    if (PanelController.CurrentPanel == _languageSelectPanel)
    {
      UpdateButtons();

      if (InputHandler.GetButtonBack())
      {
        ClosePanel();
      }
    }
  }

  //============================================================

  private void ClosePanel()
  {
    Sound();
    PanelController.DoNotCloseLastWindow();
  }

  /// <summary>
  /// ������� ������ � ����������
  /// </summary>
  private void OnClickButtonInInterface(InputAction.CallbackContext context)
  {
    if (PanelController.CurrentPanel != _languageSelectPanel) { return; }

    AudioManager.OnPlaySoundButton?.Invoke();
    languageButtons[IndexActiveButton].Button.onClick?.Invoke();
  }

  private void Sound()
  {
    if (PanelController.CurrentPanel != _languageSelectPanel) { return; }

    AudioManager.OnPlaySoundButton?.Invoke();
  }

  /// <summary>
  /// �������� ��������� ������
  /// </summary>
  private void UpdateButtons()
  {
    if (Time.time > nextimeMoveNextValue)
    {
      nextimeMoveNextValue = Time.time + timeMoveNextValue;
      if (InputHandler.GetNavigationInput() < 0)
      {
        languageButtons[IndexActiveButton].EnableDisableLanguageDisplay(false);

        IndexActiveButton++;
        if (IndexActiveButton + 1 > languageButtons.Count) { IndexActiveButton = 0; }

        Sound();
        languageButtons[IndexActiveButton].EnableDisableLanguageDisplay(true);
      }
      else if (InputHandler.GetNavigationInput() > 0)
      {
        languageButtons[IndexActiveButton].EnableDisableLanguageDisplay(false);

        IndexActiveButton--;
        if (IndexActiveButton < 0) { IndexActiveButton = languageButtons.Count - 1; }

        Sound();
        languageButtons[IndexActiveButton].EnableDisableLanguageDisplay(true);
      }
    }

    if (InputHandler.GetNavigationInput() == 0)
    {
      nextimeMoveNextValue = Time.time;
    }
  }

  /// <summary>
  /// �������� ����� � ������
  /// </summary>
  private void AddLanguagesList()
  {
    foreach (var listLanguages in _listLanguages)
    {
      var listLanguageInstance = Instantiate(_objectButtonLanguageSelect, _languageSelectSpacer);
      LanguageButton button = listLanguageInstance.GetComponent<LanguageButton>();

      button.IsLanguageSelected = false;
      button.EnableDisableLanguageDisplay(false);

      if (listLanguages.Language == GameManager.CurrentLanguage) 
      {
        button.IsLanguageSelected = true;
        button.EnableDisableLanguageDisplay(true);
        IndexActiveButton = (int)GameManager.CurrentLanguage;
      }

      button.Initialize(listLanguages);
      languageButtons.Add(button);
    }
  }

  private void UpdateIconLanguage()
  {
    //languageButtons[IndexActiveButton].EnableDisableLanguageDisplay(false);

    IndexActiveButton = (int)GameManager.CurrentLanguage;

    //languageButtons[IndexActiveButton].EnableDisableLanguageDisplay(true);
  }

  //============================================================
}

[Serializable]
public class Languages
{
  public Language Language;
  public string LanguageName;
  public Sprite LanguageSprite;
}