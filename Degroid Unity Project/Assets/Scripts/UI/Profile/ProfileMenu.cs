using TMPro;
using UnityEngine;

public class ProfileMenu : MonoBehaviour
{
  [Header("Текст")]
  [SerializeField, Tooltip("Текст количества смертей")]
  private TextMeshProUGUI _deathText;
  [SerializeField, Tooltip("Текст количества убийств")]
  private TextMeshProUGUI _murderText;
  [SerializeField, Tooltip("Текст количества прыжков")]
  private TextMeshProUGUI _jumpsText;
  [SerializeField, Tooltip("Текст количества сыгранных игр")]
  private TextMeshProUGUI _gamesText;
  [SerializeField, Tooltip("Текст общего времени игры")]
  private TextMeshProUGUI _globalTimeText;
  [SerializeField, Tooltip("Текст общего времени (Секретные уровни)")]
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
  /// Обновить текст количества смертей
  /// </summary>
  private void UpdateTextDeath(int parValue)
  {
    _deathText.text = $"{parValue}";
  }

  /// <summary>
  /// Обновить текст убийств
  /// </summary>
  private void UpdateTextMurder(int parValue)
  {
    _murderText.text = $"{parValue}";
  }

  /// <summary>
  /// Обновить текст количества прыжков
  /// </summary>
  /// <param name="parValue"></param>
  private void UpdateTextNumberJumps(int parValue)
  {
    _jumpsText.text = $"{parValue}";
  }

  /// <summary>
  /// Обновить текст количество сыграных игр
  /// </summary>
  private void UpdateTextNumberGamesPlayed(int parValue)
  {
    _gamesText.text = $"{parValue}";
  }

  /// <summary>
  /// Обновить текст общего времени игры
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
  /// Обновить текст общего времени (Секретные уровни)
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