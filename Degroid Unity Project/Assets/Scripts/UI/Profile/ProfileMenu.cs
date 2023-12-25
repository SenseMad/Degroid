using TMPro;
using UnityEngine;

public class ProfileMenu : MonoBehaviour
{
  [Header("�����")]
  [SerializeField, Tooltip("����� ���������� �������")]
  private TextMeshProUGUI _deathText;
  [SerializeField, Tooltip("����� ���������� �������")]
  private TextMeshProUGUI _murderText;
  [SerializeField, Tooltip("����� ���������� �������")]
  private TextMeshProUGUI _jumpsText;
  [SerializeField, Tooltip("����� ���������� ��������� ���")]
  private TextMeshProUGUI _gamesText;
  [SerializeField, Tooltip("����� ������ ������� ����")]
  private TextMeshProUGUI _globalTimeText;
  [SerializeField, Tooltip("����� ������ ������� (��������� ������)")]
  private TextMeshProUGUI _globalTimeSecretLevelText;
  
  //============================================================

  private GameManager GameManager { get; set; }

  //============================================================

  private void Awake()
  {
    GameManager = GameManager.Instance;
  }

  private void OnEnable()
  {
    UpdateTextDeath(GameManager.NumberDeaths);
    UpdateTextMurder(GameManager.NumberMurders);
    UpdateTextNumberJumps(GameManager.NumberJumps);
    UpdateTextNumberGamesPlayed(GameManager.NumberGamesPlayed);
    UpdateTextGlobalTime();
    UpdateTextGlobalTimeSecretLevel();

    GameManager.ChangeNumberDeaths.AddListener(UpdateTextDeath);
    GameManager.ChangeNumberMurders.AddListener(UpdateTextMurder);
    GameManager.ChangeNumberJumps.AddListener(UpdateTextNumberJumps);
    GameManager.ChangeNumberGamesPlayed.AddListener(UpdateTextNumberGamesPlayed);
  }

  private void OnDisable()
  {
    GameManager.ChangeNumberDeaths.RemoveListener(UpdateTextDeath);
    GameManager.ChangeNumberMurders.RemoveListener(UpdateTextMurder);
    GameManager.ChangeNumberJumps.RemoveListener(UpdateTextNumberJumps);
    GameManager.ChangeNumberGamesPlayed.RemoveListener(UpdateTextNumberGamesPlayed);
  }

  //============================================================

  /// <summary>
  /// �������� ����� ���������� �������
  /// </summary>
  private void UpdateTextDeath(int parValue)
  {
    _deathText.text = $"{parValue}";
  }

  /// <summary>
  /// �������� ����� �������
  /// </summary>
  private void UpdateTextMurder(int parValue)
  {
    _murderText.text = $"{parValue}";
  }

  /// <summary>
  /// �������� ����� ���������� �������
  /// </summary>
  /// <param name="parValue"></param>
  private void UpdateTextNumberJumps(int parValue)
  {
    _jumpsText.text = $"{parValue}";
  }

  /// <summary>
  /// �������� ����� ���������� �������� ���
  /// </summary>
  private void UpdateTextNumberGamesPlayed(int parValue)
  {
    _gamesText.text = $"{parValue}";
  }

  /// <summary>
  /// �������� ����� ������ ������� ����
  /// </summary>
  private void UpdateTextGlobalTime()
  {
    if (GameManager.CountUnlockedLevel <= 60)
    {
      _globalTimeText.text = "-";
      return;
    }

    _globalTimeText.text = $"{GameManager.GetGlobalBestTimeMainLevels()}";
  }

  /// <summary>
  /// �������� ����� ������ ������� (��������� ������)
  /// </summary>
  private void UpdateTextGlobalTimeSecretLevel()
  {
    if (GameManager.CountUnlockedLevel <= 68)
    {
      _globalTimeSecretLevelText.text = "-";
      return;
    }

    _globalTimeSecretLevelText.text = $"{GameManager.GetGlobalBestTimeSecretLevels()}";
  }

  //============================================================
}