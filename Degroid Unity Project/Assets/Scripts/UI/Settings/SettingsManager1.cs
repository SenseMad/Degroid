using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class SettingsManager1 : MonoBehaviour
{
  [SerializeField] private RectTransform _soundTitle;
  [SerializeField] private RangeSpinBox _musicValue;
  [SerializeField] private RangeSpinBox _soundValue;
  [SerializeField] private ToggleSpinBox _vibrationValue;
  [SerializeField] private RectTransform _videoTitle;
  [SerializeField] private ToggleSpinBox _fullscreenValue;
  [SerializeField] private SwitchSpinBox _resolutionValue;
  [SerializeField] private ToggleSpinBox _vSyncValue;
  [SerializeField] private RectTransform _languageTitle;
  [SerializeField] private ButtonSpinBox _languageButton;
  [SerializeField] private ButtonSpinBox _deleteSavesButton;
  [SerializeField] private ButtonSpinBox _backButton;

  [Header("Language")]
  [SerializeField, Tooltip("Icon выбранного языка")]
  private Image _iconSelectedLanguage;
  [SerializeField, Tooltip("")]
  private TextMeshProUGUI _textLanguage;

  //------------------------------------------------------------

  private List<SpinBoxBase> SpinBoxBases = new List<SpinBoxBase>();
  private LanguageSelectMenu languageSelectMenu;

  //------------------------------------------------------------

  private readonly float timeMoveNextLine = 0.4f; // Время перехода к следующей строке
  private float nextTimeMoveNextLine = 0.0f;

  private Panel settingsPanel;
  
  //============================================================

  private GameManager GameManager { get; set; }
  public PlayerInputHandler InputHandler { get; set; }
  private PanelController PanelController { get; set; }
  private AudioManager AudioManager { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// Индекс активной кнопки
  /// </summary>
  private int IndexActiveButton { get; set; }

  //============================================================

  private void Awake()
  {
    GameManager = GameManager.Instance;
    AudioManager = AudioManager.Instance;
    InputHandler = PlayerInputHandler.Instance;
    PanelController = FindObjectOfType<PanelController>();

    languageSelectMenu = FindObjectOfType<LanguageSelectMenu>();

    settingsPanel = GetComponent<Panel>();

    _musicValue.OnValueChanged += MusicValue_OnValueChanged;
    _soundValue.OnValueChanged += SoundValue_OnValueChanged;
    _vibrationValue.OnValueChanged += VibrationValue_OnValueChanged;
#if !UNITY_PS4
    _fullscreenValue.OnValueChanged += FullscreenValue_OnValueChanged;
    _resolutionValue.OnValueChanged += ResolutionValue_OnValueChanged;
    _vSyncValue.OnValueChanged += VSyncValue_OnValueChanged;
#else
    _videoTitle.gameObject.SetActive(false);
    _fullscreenValue.gameObject.SetActive(false);
    _resolutionValue.gameObject.SetActive(false);
    _vSyncValue.gameObject.SetActive(false);
#endif
  }

  private void Start()
  {
    if (_musicValue) SpinBoxBases.Add(_musicValue);
    if (_soundValue) SpinBoxBases.Add(_soundValue);
    if (_vibrationValue) SpinBoxBases.Add(_vibrationValue);
#if !UNITY_PS4
    if (_fullscreenValue) SpinBoxBases.Add(_fullscreenValue);
    if (_resolutionValue) SpinBoxBases.Add(_resolutionValue);
    if (_vSyncValue) SpinBoxBases.Add(_vSyncValue);
#endif
    if (_languageButton) SpinBoxBases.Add(_languageButton);
    if (_deleteSavesButton) SpinBoxBases.Add(_deleteSavesButton);
    if (_backButton) SpinBoxBases.Add(_backButton);

    foreach (var spinBoxBase in SpinBoxBases)
    {
      spinBoxBase.IsSelected = false;
    }

    IndexActiveButton = 0;
    SpinBoxBases[IndexActiveButton].IsSelected = true;
  }

  private void OnEnable()
  {
    InputHandler.AI_Player.UI.Back.performed += OnClosePanel;
    //InputHandler.AI_Player.UI.Pause.performed += OnClosePanel;
    InputHandler.AI_Player.UI.Select.performed += OnClickOnButton;

    if (languageSelectMenu != null)
    {
      foreach (var language in languageSelectMenu.ListLanguages)
      {
        if (GameManager.CurrentLanguage != language.Language)
          continue;

        ChangeIconSelectedLanguage(language.LanguageSprite, language.LanguageName.ToUpper());
      }
    }

    _musicValue.SetValueWithoutNotify(GameManager.MusicValue);
    _soundValue.SetValueWithoutNotify(GameManager.SoundValue);
    _vibrationValue.SetValueWithoutNotify(GameManager.VibrationOn);
#if !UNITY_PS4
    _fullscreenValue.SetValueWithoutNotify(GameManager.FullScreenValue);
    _resolutionValue.SetValueWithoutNotify(GameManager.CurrentSelectedResolution);
    _resolutionValue.UpdateText(UpdateResolutionText());
    _vSyncValue.SetValueWithoutNotify(GameManager.VSyncValue);
#endif
  }

  private void OnDisable()
  {
    InputHandler.AI_Player.UI.Back.performed -= OnClosePanel;
    //InputHandler.AI_Player.UI.Pause.performed -= OnClosePanel;
    InputHandler.AI_Player.UI.Select.performed -= OnClickOnButton;
  }

  private void Update()
  {
    MovingThroughList();
  }

  private void OnDestroy()
  {
    _musicValue.OnValueChanged -= MusicValue_OnValueChanged;
    _soundValue.OnValueChanged -= SoundValue_OnValueChanged;
    _vibrationValue.OnValueChanged -= VibrationValue_OnValueChanged;
#if !UNITY_PS4
    _fullscreenValue.OnValueChanged -= FullscreenValue_OnValueChanged;
    _resolutionValue.OnValueChanged -= ResolutionValue_OnValueChanged;
    _vSyncValue.OnValueChanged -= VSyncValue_OnValueChanged;
#endif
  }

  //============================================================

  private void MusicValue_OnValueChanged(int parValue)
  {
    GameManager.MusicValue = parValue;
    AudioManager.OnPlaySoundButton?.Invoke();
  }

  private void SoundValue_OnValueChanged(int parValue)
  {
    GameManager.SoundValue = parValue;
    AudioManager.OnPlaySoundButton?.Invoke();
  }

  private void VibrationValue_OnValueChanged(bool parValue)
  {
    GameManager.VibrationOn = parValue;
    AudioManager.OnPlaySoundButton?.Invoke();
  }

#if !UNITY_PS4
  private void FullscreenValue_OnValueChanged(bool parValue)
  {
    GameManager.FullScreenValue = parValue;

    Screen.fullScreen = GameManager.FullScreenValue;
    AudioManager.OnPlaySoundButton?.Invoke();
  }

  private void ResolutionValue_OnValueChanged(int parValue)
  {
    if (parValue > GameManager.Resolutions.Count - 1)
    {
      parValue = 0;
      _resolutionValue.SetValueWithoutNotify(0);
    }    
    if (parValue < 0)
    {
      parValue = GameManager.Resolutions.Count - 1;
      _resolutionValue.SetValueWithoutNotify(GameManager.Resolutions.Count - 1);
    }
    GameManager.CurrentSelectedResolution = parValue;
    _resolutionValue.UpdateText(UpdateResolutionText());

    Screen.SetResolution(GameManager.Resolutions[parValue].width, GameManager.Resolutions[parValue].height, GameManager.FullScreenValue);
    AudioManager.OnPlaySoundButton?.Invoke();
  }

  private string UpdateResolutionText()
  {
    return $"{GameManager.Resolutions[GameManager.CurrentSelectedResolution].width} X " +
      $"{GameManager.Resolutions[GameManager.CurrentSelectedResolution].height} ";
  }

  private void VSyncValue_OnValueChanged(bool parValue)
  {
    GameManager.VSyncValue = parValue;
    AudioManager.OnPlaySoundButton?.Invoke();
  }
#endif

  /// <summary>
  /// Изменить иконку выбранного языка
  /// </summary>
  private void ChangeIconSelectedLanguage(Sprite parSprite, string parText)
  {
    _iconSelectedLanguage.sprite = parSprite;
    _textLanguage.text = parText;
  }

  //============================================================

  private void OnClosePanel(InputAction.CallbackContext context)
  {
    if (PanelController.CurrentPanel != settingsPanel)
      return;

    Sound();
    PanelController.ClosePanel();

    GameManager.SaveData();
  }

  //============================================================

  private void Sound()
  {
    AudioManager.OnPlaySoundButton?.Invoke();
  }

  /// <summary>
  /// Перемещение по списку
  /// </summary>
  private void MovingThroughList()
  {
    if (Time.time > nextTimeMoveNextLine)
    {
      nextTimeMoveNextLine = Time.time + timeMoveNextLine;
      // Листаем вверх
      if (InputHandler.GetNavigationInput() > 0)
      {
        SpinBoxBases[IndexActiveButton].IsSelected = false;

        IndexActiveButton--;
        if (IndexActiveButton < 0) { IndexActiveButton = SpinBoxBases.Count - 1; }

        SpinBoxBases[IndexActiveButton].GetComponent<Animator>().SetTrigger("Play");
        Sound();
        SpinBoxBases[IndexActiveButton].IsSelected = true;
      }

      // Листаем вниз
      if (InputHandler.GetNavigationInput() < 0)
      {
        SpinBoxBases[IndexActiveButton].IsSelected = false;

        IndexActiveButton++;
        if (IndexActiveButton > SpinBoxBases.Count - 1) { IndexActiveButton = 0; }

        SpinBoxBases[IndexActiveButton].GetComponent<Animator>().SetTrigger("Play");
        Sound();
        SpinBoxBases[IndexActiveButton].IsSelected = true;
      }
    }

    if (InputHandler.GetNavigationInput() == 0)
    {
      nextTimeMoveNextLine = Time.time;
    }
  }

  /// <summary>
  /// Нажать на кнопку
  /// </summary>
  private void OnClickOnButton(InputAction.CallbackContext context)
  {
    var spinBoxBase = SpinBoxBases[IndexActiveButton];
    if (spinBoxBase.GetComponent<ButtonSpinBox>())
    {
      spinBoxBase.GetComponent<Button>().onClick?.Invoke();
      AudioManager.OnPlaySoundButton?.Invoke();
    }
  }

  //============================================================
}