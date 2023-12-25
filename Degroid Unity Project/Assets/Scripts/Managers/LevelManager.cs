/*using Steamworks;
using Steamworks.Data;*/
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
  private static LevelManager _instance;

  [SerializeField, Tooltip("Äàííûå óðîâíÿ")]
  private LevelData _levelData;

  [SerializeField, Tooltip("Ñïðàéòû ïðåäìåòîâ")]
  private StoringItemSprites _storingItemSprites;

  //------------------------------------------------------------

  private float _timeOnLevel; // Âðåìÿ íà óðîâíå
  private int _numberEnemytoLevel; // Êîëè÷åñòâî âðãàîâ íà êàðòå

  private DialogSystem dialogSystem;

  //============================================================

  public static LevelManager Instance
  {
    get
    {
      if (_instance == null) { _instance = FindObjectOfType<LevelManager>(); }
      return _instance;
    }
  }

  private GameManager GameManager { get; set; }

  public LevelSelectMenu LevelSelectMenu { get; set; }

  public TransitionBetweenScenes TransitionBetweenScenes { get; set; }

  private PlayerInputHandler InputHandler { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// Âðåìÿ íà óðîâíå
  /// </summary>
  public float TimeOnLevel
  {
    get => _timeOnLevel;
    private set
    {
      _timeOnLevel = value;
      ChangingTimeOnLevel?.Invoke(value);
    }
  }

  /// <summary>
  /// Ëó÷øåå âðåìÿ
  /// </summary>
  public float BestTimeOnLevel { get; private set; }

  /// <summary>
  /// Êîëè÷åñòâî óáèòûõ âðàãîâ íà óðîâíå
  /// </summary>
  internal int NumberKillEnemytoLevel
  {
    get => _numberEnemytoLevel;
    set
    {
      _numberEnemytoLevel = value;
      ChangeNumberEnemytoLevel?.Invoke(value);
    }
  }

  //------------------------------------------------------------

  /// <summary>
  /// Îáùåå êîëè÷åñòâî âðàãîâ íà êàðòå
  /// </summary>
  public int TotalNumberEnemytoLevel { get; private set; }

  /// <summary>
  /// True, åñëè êëþ÷ ñîáðàí
  /// </summary>
  public bool IsKey { get; private set; }

  /// <summary>
  /// True, åñëè ïðåäìåò ñîáðàí
  /// </summary>
  public bool IsItem { get; private set; }

  /// <summary>
  /// True, åñëè óðîâåíü çàâåðøåí
  /// </summary>
  public bool IsLevelComplite { get; private set; }

  /// <summary>
  /// True, åñëè èãðà çàêîí÷åíà
  /// </summary>
  public bool GameIsEnding { get; private set; }

  /// <summary>
  /// True, åñëè ïàóçà
  /// </summary>
  public bool IsPause { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// Äàííûå óðîâíÿ
  /// </summary>
  public LevelData LevelData { get => _levelData; set => _levelData = value; }

  /// <summary>
  /// Îáúåêò äâåðè
  /// </summary>
  private LevelComplite LevelComplite { get; set; }

  /// <summary>
  /// Îáúåêò ïðåäìåòà
  /// </summary>
  private ItemPickup ItemPickup { get; set; }

  /// <summary>
  /// Îáúåêò êëþ÷à
  /// </summary>
  private KeyPickup KeyPickup { get; set; }

  /// <summary>
  /// Îáúåêò âðàãà
  /// </summary>
  private KillingEnemy[] KillingEnemy { get; set; }

  //============================================================

  /// <summary>
  /// Ñîáûòèå: Èçìåíåíèå ïàðàìåòðà "Âðåìÿ ïðîâåäåííîå íà óðîâíå"
  /// </summary>
  public CustomUnityEvent<float> ChangingTimeOnLevel { get; } = new CustomUnityEvent<float>();

  /// <summary>
  /// Ñîáûòèå: Èçìåíåíèå çíà÷åíèÿ êîëè÷åñòâà âðàãîâ íà êàðòå
  /// </summary>
  public CustomUnityEvent<int> ChangeNumberEnemytoLevel { get; } = new CustomUnityEvent<int>();

  /// <summary>
  /// Ñîáûòèå: Ñìåðòü
  /// </summary>
  public CustomUnityEvent OnDeath { get; } = new CustomUnityEvent();

  /// <summary>
  /// Ñîáûòèå: Ïîäíÿòü êëþ÷
  /// </summary>
  public CustomUnityEvent OnKeyPickup { get; } = new CustomUnityEvent();

  /// <summary>
  /// Ñîáûòèå: Ïîäíÿòü ïðåäìåò
  /// </summary>
  public CustomUnityEvent OnItemPickup { get; } = new CustomUnityEvent();

  /// <summary>
  /// Ñîáûòèå: Óáèòü âðàãà
  /// </summary>
  public CustomUnityEvent OnKillingEnemy { get; } = new CustomUnityEvent();

  /// <summary>
  /// Ñîáûòèå: Çàéòè â äâåðü
  /// </summary>
  public CustomUnityEvent OnThroughDoor { get; } = new CustomUnityEvent();

  /// <summary>
  /// Ñîáûòèå: Óðîâåíü çàâåðøåí
  /// </summary>
  public CustomUnityEvent OnLevelComplite { get; } = new CustomUnityEvent();

  /// <summary>
  /// Ñîáûòèå: Ïàóçà
  /// </summary>
  public CustomUnityEvent<bool> OnPause { get; } = new CustomUnityEvent<bool>();

  //============================================================

  private void Awake()
  {
    if (_instance != null && _instance != this)
    {
      Destroy(this);
      return;
    }
    _instance = this;

    GameManager = GameManager.Instance;

    LevelSelectMenu = FindObjectOfType<LevelSelectMenu>();
    TransitionBetweenScenes = FindObjectOfType<TransitionBetweenScenes>();
    InputHandler = PlayerInputHandler.Instance;
    dialogSystem = DialogSystem.Instance;

    FindObjectsLevel();
  }

  private void OnEnable()
  {
    //InputHandler.AI_Player.UI.Reload.performed += Reload_performed;

    OnDeath.AddListener(OnPlayerDeath);
    OnKeyPickup.AddListener(TakeKey);
    OnItemPickup.AddListener(TakeItem);
    OnKillingEnemy.AddListener(KillEnemy);
    OnThroughDoor.AddListener(ThroughDoor);
  }

  private void OnDisable()
  {
    //InputHandler.AI_Player.UI.Reload.performed -= Reload_performed;

    OnDeath.RemoveListener(OnPlayerDeath);
    OnKeyPickup.RemoveListener(TakeKey);
    OnItemPickup.RemoveListener(TakeItem);
    OnKillingEnemy.RemoveListener(KillEnemy);
    OnThroughDoor.RemoveListener(ThroughDoor);
  }

  private void Start()
  {
    TimeOnLevel = 0;

    LevelComplite.CloseDoor(LevelData);

    TransferObjectData();

    CollectedToLevel();
  }

  private void LateUpdate()
  {
    if (IsPause) { return; }

    if (!IsLevelComplite)
    {
      var dialog = DialogSystem.Instance;
      if (dialog != null)
      {
        if (dialog.IsDialogStarted)
          return;
      }

      TimeOnLevel += Time.deltaTime;
    }
  }

  //============================================================

  private void Reload_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
  {
    if (IsPause)
      return;

    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  /// <summary>
  /// Íàéòè îáúåêòû íà óðîâíå
  /// </summary>
  private void FindObjectsLevel()
  {
    LevelComplite = FindObjectOfType<LevelComplite>();
    ItemPickup = FindObjectOfType<ItemPickup>();
    KeyPickup = FindObjectOfType<KeyPickup>();

    KillingEnemy = FindObjectsOfType<KillingEnemy>();

    TotalNumberEnemytoLevel = KillingEnemy.Length;
  }

  /// <summary>
  /// Ïåðåäàòü äàííûå îáúåêòîâ
  /// </summary>
  private void TransferObjectData()
  {
    ItemPickup.GetComponent<SpriteRenderer>().sprite = _storingItemSprites.GetSprite(LevelData.IndexLevel);
    //_storingItemSprites

    //ItemPickup.GetComponent<SpriteRenderer>().sprite = LevelData.Item.SpriteObject;
    KeyPickup.GetComponent<SpriteRenderer>().sprite = LevelData.Key;
  }

  /// <summary>
  /// Âçÿòü êëþ÷
  /// </summary>
  private void TakeKey()
  {
    IsKey = true;
    LevelComplite.OpenDoor(LevelData);
  }

  /// <summary>
  /// Âçÿòü ïðåäìåò
  /// </summary>
  private void TakeItem()
  {
    IsItem = true;
  }

  /// <summary>
  /// Óáèòü âðàãà
  /// </summary>
  private void KillEnemy()
  {
    AddMurders();
    NumberKillEnemytoLevel++;
  }

  /// <summary>
  /// Çàéòè â äâåðü
  /// </summary>
  private void ThroughDoor()
  {
    CompleteLevel();
  }

  /// <summary>
  /// Çàâåðøèòü óðîâåíü
  /// </summary>
  public void CompleteLevel()
  {
    IsLevelComplite = true;

    if (IsItem)
    {
      if (!GameManager.CountCollectedItems.Contains(LevelData.IndexLevel))
      {
        GameManager.CountCollectedItems.Add(LevelData.IndexLevel);
      }
    }

    // Проверка ачивок должна быть в любом случае, т.к. они могут быть сброшены
    GameManager.UpdateAchievementsItems();

    GameManager.UnlockNextLevelGame(LevelData.IndexLevel + 1);

    EndGame(true);
  }

  /// <summary>
  /// Ñîáðàí ëè ïðåäìåò íà óðîâíå
  /// </summary>
  private void CollectedToLevel()
  {
    foreach (var indexItem in GameManager.CountCollectedItems)
    {
      if (indexItem != LevelData.IndexLevel)
        continue;

      //ItemPickup.gameObject.SetActive(false);
      ItemPickup.GetComponent<SpriteRenderer>().color = new Color32(90, 90, 90, 125);
      ItemPickup.GetComponent<BoxCollider2D>().enabled = false;
      break;
    }
  }

  /// <summary>
  /// Äîáàâèòü óáèéñòâà
  /// </summary>
  public void AddMurders()
  {
    GameManager.NumberMurders++;
    GameManager.UpdateAchievementsKills();
  }

  /// <summary>
  /// Ïåðåéòè â ãëàâíîå ìåíþ
  /// </summary>
  public void LoadMainMenu()
  {
    if (TransitionBetweenScenes.IsSceneTransition)
      return;

    if (IsLevelComplite)
    {
      IsPause = false;
      OnPause?.Invoke(IsPause);

      TransitionBetweenScenes.StartSceneChange("MainMenu");
    }
  }
  
  /// <summary>
  /// Ïåðåçàãðóçêà óðîâíÿ
  /// </summary>
  public void ReloadLevel()
  {
    if (TransitionBetweenScenes.IsSceneTransition)
      return;

    IsPause = false;
    if (IsLevelComplite)
    {
      IsPause = false;
      OnPause?.Invoke(IsPause);
      GameManager.NumberGamesPlayed++;

      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
  }

  /// <summary>
  /// Ñëåäóþùèé óðîâåíü
  /// </summary>
  public void NextLevel()
  {
    if (TransitionBetweenScenes.IsSceneTransition)
      return;

    if (IsLevelComplite)
    {
      if (SceneManager.GetActiveScene().buildIndex + 1 > SceneManager.sceneCountInBuildSettings - 1)
      {
        OnPause?.Invoke(IsPause);
        TransitionBetweenScenes.StartSceneChange("MainMenu");
        return;
      }

      OnPause?.Invoke(IsPause);
      GameManager.NumberGamesPlayed++;

      if (GameManager.CountCollectedItems.Count < 60 && SceneManager.GetActiveScene().buildIndex + 1 > 61)
      {
        OnPause?.Invoke(IsPause);
        TransitionBetweenScenes.StartSceneChange("MainMenu");
        return;
      }

      TransitionBetweenScenes.StartSceneChange(SceneManager.GetActiveScene().buildIndex + 1);
    }
  }

  /// <summary>
  /// Ñìåðòü èãðîêà
  /// </summary>
  private void OnPlayerDeath() => EndGame(false);

  /// <summary>
  /// Êîíåö èãðû
  /// </summary>
  private void EndGame(bool win)
  {
    GameIsEnding = true;
    IsPause = true;
    OnPause?.Invoke(IsPause);

    if (win)
    {
      RecordBestTimeCompleteLevel();
      OnLevelComplite?.Invoke();
    }
    else
    {
      IsPause = false;
      GameManager.NumberDeaths++;
      GameManager.UpdateAchievementsDeaths();
      StartCoroutine(RespawnPlayer());
    }
  }

  /// <summary>
  /// Çàïèñàòü ëó÷øåå âðåìÿ ïðîõîæäåíèÿ óðîâíÿ
  /// </summary>
  private void RecordBestTimeCompleteLevel()
  {
    if (GameManager.BestTimeCompliteLevels.ContainsKey(LevelData.IndexLevel))
    {
      if (GameManager.BestTimeCompliteLevels[LevelData.IndexLevel] > TimeOnLevel)
      {
        GameManager.BestTimeCompliteLevels[LevelData.IndexLevel] = TimeOnLevel;
      }
      BestTimeOnLevel = GameManager.BestTimeCompliteLevels[LevelData.IndexLevel];
    }
    else
    {
      BestTimeOnLevel = TimeOnLevel;
      GameManager.BestTimeCompliteLevels.Add(LevelData.IndexLevel, TimeOnLevel);
    }

    //Ачивка на прохождение всех уровней менее чем за 30 минут
    if (GameManager.CountUnlockedLevel > 60)
    {
      if (GameManager.GetGlobalBestTimeMainLevels() < 30 * 60)
      {
        GameManager.UpdateAchievement(GameManager.Achievement.SPEEDRUNNER);
      }
    }
  }

  /// <summary>
  /// Âîçðîæäåíèå èãðîêà
  /// </summary>
  private IEnumerator RespawnPlayer()
  {
    CameraShake.Shake(0.5f, 0.5f);
    IsPause = false;
    OnPause?.Invoke(IsPause);

    yield return new WaitForSeconds(1);

    while (IsPause)
    {
      yield return null;
    }

    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  //============================================================
}