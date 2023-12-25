using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class LevelCompliteUI : MonoBehaviour
{
  [Header("�������")]
  [SerializeField, Tooltip("������ �����")]
  private GameObject _enemyObject;

  [SerializeField, Tooltip("Icon ��������")]
  private Image _iconItem;

  [Header("�����")]
  [SerializeField, Tooltip("����� �������")]
  private TextMeshProUGUI _timeText;
  [SerializeField, Tooltip("����� ������� �������")]
  private TextMeshProUGUI _timeBestText;

  [Header("������")]
  [SerializeField, Tooltip("������ ����������� ������")]
  private Panel _levelComplitePanel;

  //============================================================

  private GameManager GameManager { get; set; }

  private PlayerInputHandler PlayerInputHandler { get; set; }

  private LevelManager LevelManager { get; set; }

  private PanelController PanelController { get; set; }

  private TimeLeaderboard TimeLeaderboard { get; set; }

  private AudioManager AudioManager { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// ������ �����
  /// </summary>
  public GameObject EnemyObject { get => _enemyObject; set => _enemyObject = value; }
  /// <summary>
  /// Icon ��������
  /// </summary>
  public Image IconItem { get => _iconItem; set => _iconItem = value; }

  /// <summary>
  /// ����� �������
  /// </summary>
  public TextMeshProUGUI TimeText { get => _timeText; set => _timeText = value; }

  /// <summary>
  /// ������ ����������� ������
  /// </summary>
  public Panel LevelComplitePanel { get => _levelComplitePanel; set => _levelComplitePanel = value; }

  //============================================================

  private void Awake()
  {
    GameManager = GameManager.Instance;

    PlayerInputHandler = PlayerInputHandler.Instance;

    LevelManager = FindObjectOfType<LevelManager>();

    PanelController = FindObjectOfType<PanelController>();

    TimeLeaderboard = LevelComplitePanel.GetComponent<TimeLeaderboard>();

    AudioManager = AudioManager.Instance;
  }

  private void OnEnable()
  {
    LevelManager.OnLevelComplite.AddListener(UpdateText);

    PlayerInputHandler.AI_Player.UI.Select.performed += OnNextLevel;
    PlayerInputHandler.AI_Player.UI.Reload.performed += OnReloadLevel;
    PlayerInputHandler.AI_Player.UI.Back.performed += OnLoadMainMenu;
    PlayerInputHandler.AI_Player.UI.Pause.performed += OnLoadMainMenu;
  }

  private void OnDisable()
  {
    LevelManager.OnLevelComplite.RemoveListener(UpdateText);

    PlayerInputHandler.AI_Player.UI.Select.performed -= OnNextLevel;
    PlayerInputHandler.AI_Player.UI.Reload.performed -= OnReloadLevel;
    PlayerInputHandler.AI_Player.UI.Back.performed -= OnLoadMainMenu;    
    PlayerInputHandler.AI_Player.UI.Pause.performed -= OnLoadMainMenu;
  }

  //============================================================

  private void Sound()
  {
    AudioManager.OnPlaySoundButton?.Invoke();
  }

  private void OnNextLevel(InputAction.CallbackContext context)
  {
    if (!TimeLeaderboard.AnimatorFinish)
      return;

    LevelManager.NextLevel();
    Sound();
  }

  private void OnReloadLevel(InputAction.CallbackContext context)
  {
    if (!TimeLeaderboard.AnimatorFinish)
      return;

    LevelManager.ReloadLevel();
    Sound();
  }

  private void OnLoadMainMenu(InputAction.CallbackContext context)
  {
    if (!TimeLeaderboard.AnimatorFinish)
      return;

    LevelManager.LoadMainMenu();
    Sound();
  }

  private void UpdateText()
  {
    UpdateTextTime(LevelManager.TimeOnLevel, out string parTimeText);
    TimeText.text = parTimeText;
    UpdateTextTime(LevelManager.BestTimeOnLevel, out string parTimeBestText);
    _timeBestText.text = parTimeBestText;

    PanelController.AddPanelList(LevelComplitePanel);
  }

  /// <summary>
  /// �������� ����� �������
  /// </summary>
  private void UpdateTextTime(float parValue, out string parText)
  {
    var intTime = (int)(parValue * 1000f);
    var formatedTime = (float)intTime;
    var minutes = ((int)formatedTime / 1000 / 60).ToString();
    var secunds = (formatedTime / 1000f % 60f).ToString("f3");

    parText = $"{minutes}:{secunds}";
  }

  //============================================================
}