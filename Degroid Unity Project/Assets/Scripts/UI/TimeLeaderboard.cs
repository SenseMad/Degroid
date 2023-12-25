/*using Steamworks;
using Steamworks.Data;*/
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeLeaderboard : MonoBehaviour
{
  [SerializeField]
  private RawImage[] _avatarImage;

  [SerializeField]
  private TextMeshProUGUI[] _playerText;

  [SerializeField]
  private TextMeshProUGUI[] _scoreText;

  [SerializeField, Tooltip("Текст рекорда")]
  private TextMeshProUGUI _recordText;

  [SerializeField, Tooltip("Стандартная аватарка")]
  private Texture2D _defaultAvatar;

  [SerializeField, Tooltip("Текст лидеров уровня")]
  private TextMeshProUGUI _uILevelLeadersText;

  [SerializeField, Tooltip("Текст времени друзей")]
  private TextMeshProUGUI _uIFriendsTimeText;

  //============================================================

  private GameManager GameManager { get; set; }

  private LevelManager LevelManager { get; set; }

  private PlayerInputHandler PlayerInputHandler { get; set; }

  //public Task<Leaderboard?> Leaderboard { get; set; }

  private int NumberCount { get; set; } = 0;

  //------------------------------------------------------------

  public RawImage[] AvatarImage { get => _avatarImage; set => _avatarImage = value; }

  public TextMeshProUGUI[] PlayerText { get => _playerText; set => _playerText = value; }

  public TextMeshProUGUI[] ScoreText { get => _scoreText; set => _scoreText = value; }

  /// <summary>
  /// Текст рекорда
  /// </summary>
  public TextMeshProUGUI RecordText { get => _recordText; set => _recordText = value; }

  /// <summary>
  /// Стандартная аватарка
  /// </summary>
  public Texture2D DefaultAvatar { get => _defaultAvatar; set => _defaultAvatar = value; }

  /// <summary>
  /// Текст лидеров уровня
  /// </summary>
  public TextMeshProUGUI UILevelLeadersText { get => _uILevelLeadersText; set => _uILevelLeadersText = value; }

  /// <summary>
  /// Текст времени друзей
  /// </summary>
  public TextMeshProUGUI UIFriendsTimeText { get => _uIFriendsTimeText; set => _uIFriendsTimeText = value; }

  //------------------------------------------------------------

  /// <summary>
  /// True, если просмотр лидеров друзей
  /// </summary>
  private bool IsFriendsTime { get; set; }

  /// <summary>
  /// True, если запущена смена просмотра лидеров уровня
  /// </summary>
  private bool IsActive { get; set; }

  /// <summary>
  /// True, если анимация закончена
  /// </summary>
  public bool AnimatorFinish { get; set; }

  //============================================================

  private void Awake()
  {
    /*GameManager = GameManager.Instance;

    LevelManager = FindObjectOfType<LevelManager>();

    PlayerInputHandler = PlayerInputHandler.Instance;*/
  }

  private void Start()
  {
    AnimatorFinish = false;

    /*if (SteamClient.IsValid)
    {
      var leaderboard = SteamUserStats.FindOrCreateLeaderboardAsync($"Level_{LevelManager.LevelData.IndexLevel}", LeaderboardSort.Ascending, LeaderboardDisplay.TimeMilliSeconds);

      Leaderboard = leaderboard;

      NumberCount = 0;
    }*/

    /*TimeLeaderboards();
    UpdateTextBestTime();*/
  }

  private void Update()
  {
    //TimeFriendsLeaderboards();
  }

  //============================================================

  /// <summary>
  /// Обновить текст лучшего времени
  /// </summary>
  private async void UpdateTextBestTime()
  {
    /*if (Leaderboard != null)
    {
      var leaderboard = await SteamUserStats.FindLeaderboardAsync($"Level_{LevelManager.LevelData.IndexLevel}");
      var surroundScores = await leaderboard.Value.GetScoresAroundUserAsync(0, 1000000);

      if (GameManager.BestTimeCompliteLevels.TryGetValue($"Level_{LevelManager.LevelData.IndexLevel}", out int value))
      {
        if (value > (int)(LevelManager.TimeOnLevel * 1000))
        {
          GameManager.BestTimeCompliteLevels[$"Level_{LevelManager.LevelData.IndexLevel}"] = (int)(LevelManager.TimeOnLevel * 1000f);
        }

        foreach (var e in surroundScores)
        {
          if (e.User.Name == SteamClient.Name)
          {
            if (value != e.Score)
            {
              GameManager.BestTimeCompliteLevels[$"Level_{LevelManager.LevelData.IndexLevel}"] = e.Score;
            }
            break;
          }
        }
      }
      else
      {
        GameManager.BestTimeCompliteLevels.Add($"Level_{LevelManager.LevelData.IndexLevel}", (int)(LevelManager.TimeOnLevel * 1000f));

        foreach (var e in surroundScores)
        {
          if (e.User.Name == SteamClient.Name)
          {
            if (e.Score != 0)
            {
              GameManager.BestTimeCompliteLevels[$"Level_{LevelManager.LevelData.IndexLevel}"] = e.Score;
            }
            break;
          }
        }
      }
    }
    else
    {
      if (GameManager.BestTimeCompliteLevels.TryGetValue($"Level_{LevelManager.LevelData.IndexLevel}", out int value))
      {
        if (value > (int)(LevelManager.TimeOnLevel * 1000))
        {
          GameManager.BestTimeCompliteLevels[$"Level_{LevelManager.LevelData.IndexLevel}"] = (int)(LevelManager.TimeOnLevel * 1000);
        }
      }
      else
      {
        GameManager.BestTimeCompliteLevels.Add($"Level_{LevelManager.LevelData.IndexLevel}", (int)(LevelManager.TimeOnLevel * 1000));
      }
    }

    var formatedTime = (float)GameManager.BestTimeCompliteLevels[$"Level_{LevelManager.LevelData.IndexLevel}"];
    var minutes = ((int)formatedTime / 1000 / 60).ToString();
    var secunds = (formatedTime / 1000f % 60f).ToString("f3");

    RecordText.text = $"{minutes}:{secunds}";*/
  }

  /// <summary>
  /// Лидеры уровня
  /// </summary>
  public void TimeLeaderboards()
  {
    IsFriendsTime = false;
    UpdateLeaderboardEntry(false);
  }

  /// <summary>
  /// Время друзей
  /// </summary>
  private void TimeFriendsLeaderboards()
  {
    /*if (PlayerInputHandler.GetFInput() && Leaderboard != null && AnimatorFinish)
    {
      if (IsFriendsTime && !IsActive)
      {
        IsActive = true;
        IsFriendsTime = false;
        UILevelLeadersText.gameObject.SetActive(true);
        UIFriendsTimeText.gameObject.SetActive(false);
        TimeLeaderboards();
      }
      else if (!IsFriendsTime && !IsActive)
      {
        IsActive = true;
        IsFriendsTime = true;
        UILevelLeadersText.gameObject.SetActive(false);
        UIFriendsTimeText.gameObject.SetActive(true);
        UpdateLeaderboardEntry(true);
      }
    }*/
  }

  private async void UpdateLeaderboardEntry(bool friends)
  {
    /*if (Leaderboard != null)
    {
      for (int i = 0; i < PlayerText.Length; i++)
      {
        AvatarImage[i].texture = DefaultAvatar;
        AvatarImage[i].color = new Color32(255, 255, 255, 100);
        PlayerText[i].text = "...";
        ScoreText[i].text = "";
      }

      var leaderboard = (Leaderboard)await Leaderboard;
      await leaderboard.SubmitScoreAsync((int)(LevelManager.TimeOnLevel * 1000));
      LeaderboardEntry[] globalScore = friends ? await leaderboard.GetScoresFromFriendsAsync() : await leaderboard.GetScoresAsync(7);

      NumberCount = 0;
      foreach (var score in globalScore)
      {
        var formatedTime = (float)score.Score;
        var minutes = ((int)formatedTime / 1000 / 60).ToString();
        var secunds = (formatedTime / 1000f % 60f).ToString("f3");
        var avatar = await SteamFriends.GetLargeAvatarAsync(score.User.Id);
        AvatarImage[NumberCount].texture = GetTextureFromImage(avatar.Value);
        AvatarImage[NumberCount].color = new Color32(255, 255, 255, 255);
        PlayerText[NumberCount].text = $"{score.User.Name}";
        ScoreText[NumberCount].text = $"{minutes}:{secunds}";
        NumberCount++;
      }

      IsActive = false;
    }*/
  }

  /// <summary>
  /// Анимация закончена
  /// </summary>
  public void SetAnimatorFinish()
  {
    AnimatorFinish = true;
  }

  //============================================================
}