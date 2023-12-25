using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using Platform.Any.Requests;
using Platform.Any.Data;
using Platform.Any;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class GameManager : MonoBehaviour
{
  public const int MAX_ITEMS = 60;

  private static GameManager _instance;

  //------------------------------------------------------------

  private int _musicValue = 25; // Çíà÷åíèå ìóçûêè
  private int _soundValue = 25; // Çíà÷åíèå çâóêîâ
#if !UNITY_PS4
  private bool _fullScreenValue = true; // Ïîëíûé ýêðàí
  private bool _vSyncValue = true; // Âåðòèêàëüíàÿ ñèíõðîíèçàöèÿ
#endif
  private bool _vibrationOn = true; // Âèáðàöèÿ ãåéìïàäà

  private int _countUnlockedLevel = 1; // Êîëè÷åñòâî îòêðûòûõ óðîâíåé

  private int _numberDeaths = 0; // Êîëè÷åñòâî ñìåðòåé
  private int _numberMurders = 0; // Êîëè÷åñòâî óáèéñòâ
  private int _numberJumps = 0; // Êîëè÷åñòâî ïðûæêîâ
  private int _numberGamesPlayed = 0; // Êîëè÷åñòâî ñûãðàííûõ èãð

  //============================================================

  public static GameManager Instance
  {
    get
    {
      if (_instance == null)
      {
        GameObject obj = new GameObject("GameManager");
        _instance = obj.AddComponent<GameManager>();
        DontDestroyOnLoad(obj);
      }

      return _instance;
    }
  }

  //private Achievement Achievement { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// Ñïèñîê ðàçðåøåíèé ýêðàíà
  /// </summary>
  public List<Resolution> Resolutions { get; set; } = new List<Resolution>();

  /// <summary>
  /// Òåêóùåå âûáðàííîå ðàçðåøåíèå ýêðàíà
  /// </summary>
  internal int CurrentSelectedResolution { get; set; }

  /// <summary>
  /// Çíà÷åíèå ìóçûêè
  /// </summary>
  internal int MusicValue
  {
    get => _musicValue;
    set
    {
      _musicValue = value;
      ChangeMusicValue?.Invoke(value);
    }
  }

  /// <summary>
  /// Çíà÷åíèå çâóêîâ
  /// </summary>
  internal int SoundValue
  {
    get => _soundValue;
    set
    {
      _soundValue = value;
      ChangeSoundValue?.Invoke(value);
    }
  }

#if !UNITY_PS4
  /// <summary>
  /// Çíà÷åíèå ïîëíûé ýêðàí
  /// </summary>
  internal bool FullScreenValue
  {
    get => _fullScreenValue;
    set
    {
      _fullScreenValue = value;
      ChangeFullScreenValue?.Invoke(value);
    }
  }
  /// <summary>
  /// Çíà÷åíèå âåðòèêàëüíàÿ ñèíõðîíèçàöèÿ
  /// </summary>
  internal bool VSyncValue
  {
    get => _vSyncValue;
    set
    {
      _vSyncValue = value;
      QualitySettings.vSyncCount = Convert.ToInt16(value);
      //ChangeVSyncValue?.Invoke(value);
    }
  }
#endif
  /// <summary>
  /// Âèáðàöèÿ ãåéìïàäà
  /// </summary>
  internal bool VibrationOn
  {
    get => _vibrationOn;
    set
    {
      _vibrationOn = value;
      ChangeVibrationOn?.Invoke(value);
    }
  }

  /// <summary>
  /// Êîëè÷åñòâî îòêðûòûõ óðîâíåé
  /// </summary>
  internal int CountUnlockedLevel
  {
    get => _countUnlockedLevel;
    set
    {
      _countUnlockedLevel = value;
      ChangeCountUnlockedLevel?.Invoke(value);
    }
  }

  /// <summary>
  /// Òåêóùèé ÿçûê
  /// </summary>
  public Language CurrentLanguage
  {
    get => LocalisationSystem.CurrentLanguage;
    set
    {
      LocalisationSystem.CurrentLanguage = value;
      ChangeLanguage?.Invoke(value);
    }
  }

  //------------------------------------------------------------

  /// <summary>
  /// Êîëè÷åñòâî ñìåðòåé
  /// </summary>
  internal int NumberDeaths
  {
    get => _numberDeaths;
    set
    {
      _numberDeaths = value;
      ChangeNumberDeaths?.Invoke(value);
    }
  }

  /// <summary>
  /// Êîëè÷åñòâî óáèéñòâ
  /// </summary>
  internal int NumberMurders
  {
    get => _numberMurders;
    set
    {
      _numberMurders = value;
      ChangeNumberMurders?.Invoke(value);
    }
  }

  /// <summary>
  /// Êîëè÷åñòâî ïðûæêîâ
  /// </summary>
  internal int NumberJumps
  {
    get => _numberJumps;
    set
    {
      _numberJumps = value;
      ChangeNumberJumps?.Invoke(value);
    }
  }

  /// <summary>
  /// Количество сыгранных игр
  /// </summary>
  internal int NumberGamesPlayed
  {
    get => _numberGamesPlayed;
    set
    {
      _numberGamesPlayed = value;
      ChangeNumberGamesPlayed?.Invoke(value);
    }
  }

  /// <summary>
  /// Собранные предметы по индексу уровня
  /// </summary>
  public List<int> CountCollectedItems { get; set; } = new List<int>();

  /// <summary>
  /// Лучшее время прохождения уровней
  /// </summary>
  public Dictionary<int, float> BestTimeCompliteLevels = new Dictionary<int, float>();

  /// <summary>
  /// Диалоги просмотрены (Добавляем индекс уровня, чтобы отключить диалог на уровне)
  /// </summary>
  public List<int> DialogsViewed = new List<int>();

  /// <summary>
  /// Открытые главы
  /// </summary>
  public bool[] OpenChapters { get; set; } = new bool[4] { true, false, false, false };

  /// <summary>
  /// Количество завершенных глав уровня
  /// </summary>
  public Dictionary<int, Tes> NumberCompletedLevelChapters = new Dictionary<int, Tes>()
  {
    { 1, new Tes(20, 0) },
    { 2, new Tes(20, 0) },
    { 3, new Tes(20, 0) },
    { 4, new Tes(8, 0) },
  };

  public class Tes
  {
    /// <summary>
    /// Номер уровня
    /// </summary>
    public int numberLevels;
    /// <summary>
    /// Количество пройденных уровней
    /// </summary>
    public int numberCompletedLevels;

    public Tes(int parNumberLevels, int parNumberCompletedLevels)
    {
      numberLevels = parNumberLevels;
      numberCompletedLevels = parNumberCompletedLevels;
    }
  }

  //============================================================

  private TransitionBetweenScenes TransitionBetweenScenes { get; set; }

  //============================================================

  /// <summary>
  /// Событие: Изменение значения музыки
  /// </summary>
  public CustomUnityEvent<int> ChangeMusicValue { get; } = new CustomUnityEvent<int>();
  /// <summary>
  /// Событие: Изменение значения звуков
  /// </summary>
  public CustomUnityEvent<int> ChangeSoundValue { get; } = new CustomUnityEvent<int>();
#if !UNITY_PS4
  /// <summary>
  /// Ñîáûòèå: Èçìåíåíèå çíà÷åíèÿ ïîëíîãî ýêðàíà
  /// </summary>
  public CustomUnityEvent<bool> ChangeFullScreenValue { get; } = new CustomUnityEvent<bool>();
  /// <summary>
  /// Ñîáûòèå: Èçìåíåíèå çíà÷åíèÿ âåðòèêàëüíîé ñèíõðîíèçàöèè
  /// </summary>
  public CustomUnityEvent<bool> ChangeVSyncValue { get; } = new CustomUnityEvent<bool>();
#endif
  /// <summary>
  /// Ñîáûòèå: Èçìåíåíèå ïàðàìåòðà âèáðàöèÿ ãåéìïàäà
  /// </summary>
  public CustomUnityEvent<bool> ChangeVibrationOn { get; } = new CustomUnityEvent<bool>();

  /// <summary>
  /// Ñîáûòèå: Èçìåíåíèå êîëè÷åñòâî îòêðûòûõ óðîâíåé
  /// </summary>
  public CustomUnityEvent<int> ChangeCountUnlockedLevel { get; } = new CustomUnityEvent<int>();
  /// <summary>
  /// Ñîáûòèå: Èçìåíåíèå ìîíåò
  /// </summary>
  public CustomUnityEvent<float> ChangePlayerCoins { get; } = new CustomUnityEvent<float>();
  /// <summary>
  /// Ñîáûòèå: Èçìåíåíèå îïûòà
  /// </summary>
  public CustomUnityEvent<int> ChangePlayerExperience { get; } = new CustomUnityEvent<int>();
  /// <summary>
  /// Ñîáûòèå: Èçìåíåíèå óðîâíÿ
  /// </summary>
  public CustomUnityEvent<int> ChangePlayerLevel { get; } = new CustomUnityEvent<int>();

  /// <summary>
  /// Ñîáûòèå: Èçìåíåíèå êîëè÷åñòâà ñìåðòåé
  /// </summary>
  public CustomUnityEvent<int> ChangeNumberDeaths { get; } = new CustomUnityEvent<int>();
  /// <summary>
  /// Ñîáûòèå: Èçìåíåíèå êîëè÷åñòâà óáèéñòâ
  /// </summary>
  public CustomUnityEvent<int> ChangeNumberMurders { get; } = new CustomUnityEvent<int>();
  /// <summary>
  /// Ñîáûòèå: Èçìåíåíèå êîëè÷åñòâî ñûãðàííûõ èãð
  /// </summary>
  public CustomUnityEvent<int> ChangeNumberJumps { get; } = new CustomUnityEvent<int>();
  /// <summary>
  /// Ñîáûòèå: Èçìåíåíèå êîëè÷åñòâî ñûãðàííûõ èãð
  /// </summary>
  public CustomUnityEvent<int> ChangeNumberGamesPlayed { get; } = new CustomUnityEvent<int>();

  /// <summary>
  /// Ñîáûòèå: Èçìåíåíèå ÿçûêà
  /// </summary>
  public CustomUnityEvent<Language> ChangeLanguage { get; } = new CustomUnityEvent<Language>();

  //============================================================

  private void Awake()
  {
    if (_instance == null)
    {
      _instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
      return;
    }

    TransitionBetweenScenes = FindObjectOfType<TransitionBetweenScenes>();


    StartCoroutine(Init());
    InvokeRepeating(nameof(SetColorDualShock), 0, 3f);
    /*try
    {
      SteamClient.Init(480, true);
    }
    catch
    {
      Debug.Log("Îøèáêà ïîäêëþ÷åíèÿ ê Steam");
    }*/
  }

  private void SetColorDualShock()
  {
#if UNITY_PS4
    Screen.fullScreen = true;
#else
    if (Gamepad.current is DualShockGamepad gamepad)
    {
      gamepad.SetLightBarColor(new Color(0.3686275f, 0.7333333f, 0.6627451f));
    }
#endif
  }

  private IEnumerator Init()
  {
    bool initScene = SceneManager.GetActiveScene().name == "InitScene";

    yield return new WaitForSeconds(0.5f);

    //установка системного языка
    switch (Main.SystemLanguage)
    {
      case Platform.Any.Data.SystemLanguage.JAPANESE:
        CurrentLanguage = Language.Japan;
        break;

      case Platform.Any.Data.SystemLanguage.FRENCH:
      case Platform.Any.Data.SystemLanguage.FRENCH_CA:
        CurrentLanguage = Language.French;
        break;

      case Platform.Any.Data.SystemLanguage.SPANISH:
      case Platform.Any.Data.SystemLanguage.SPANISH_LA:
        CurrentLanguage = Language.Spanish;
        break;

      case Platform.Any.Data.SystemLanguage.GERMAN:
        CurrentLanguage = Language.German;
        break;

      case Platform.Any.Data.SystemLanguage.RUSSIAN:
        CurrentLanguage = Language.Russian;
        break;

      case Platform.Any.Data.SystemLanguage.CHINESE_T:
      case Platform.Any.Data.SystemLanguage.CHINESE_S:
        CurrentLanguage = Language.Chinese;
        break;

      case Platform.Any.Data.SystemLanguage.PORTUGUESE_PT:
      case Platform.Any.Data.SystemLanguage.PORTUGUESE_BR:
        CurrentLanguage = Language.Portuguese;
        break;

      default:
        CurrentLanguage = Language.English;
        break;
    }

    //Инициализация API платформы
    if (Main.Init())
    {
      Debug.Log("Platform initialized!");

      yield return new WaitForSeconds(1f);

      //Профили
      if (Profiles.Init())
      {
        Debug.Log("Profiles initialized!");
      }
      else
      {
        Debug.LogWarning("Error: Profiles not initialized!");
      }

      var data1 = Profiles.GetPrimaryUserDetails();
      Debug.Log($"PrimaryUser: Slot={Profiles.GetPrimaryUserSlot()} UserId={data1.UserId.Value} Name={data1.UserName} Primary={data1.IsPrimaryUser}");

      yield return new WaitForSeconds(1f);

      //Достижения
      if (Achievements.Init())
      {
        Debug.Log("Achievements initialized");

        if (Achievements.InitAchievements())
        {
          yield return new WaitWhile(() => Achievements.CheckAchievementsInitialized());
          Debug.Log("User Achievements initialized");

          if (Achievements.CacheAchievements())
          {
            Debug.Log("CacheAchievements");
          }
          else
          {
            Debug.LogWarning("Error: CacheAchievements");
          }
        }
        else
        {
          Debug.LogWarning("Error: User Achievements not initialized");
        }
      }
      else
      {
        Debug.LogWarning("Error: Achievements not initialized");
      }
    }
    else
    {
      Debug.LogWarning("Error: Platform not initialized!");
      yield return new WaitForSeconds(2f);

    }

    yield return new WaitForSeconds(2f);

    UploadData();

    yield return new WaitForSeconds(2f);

    //Если это первая сцена - переходим в главное меню
    if (initScene)
    {
      TransitionBetweenScenes.StartSceneChange("MainMenu");
    }
  }

  private void Start()
  {
#if UNITY_PS4
    Screen.fullScreen = true;
#else
    CreateResolutions();

    Screen.fullScreen = FullScreenValue;
    QualitySettings.vSyncCount = Convert.ToInt16(VSyncValue);
    Screen.SetResolution(Resolutions[CurrentSelectedResolution].width, Resolutions[CurrentSelectedResolution].height, FullScreenValue, Resolutions[CurrentSelectedResolution].refreshRate);
#endif
  }

  private void Update()
  {
    Main.Update();
    SaveLoad.Update();
  }

  private void OnApplicationQuit()
  {
#if !UNITY_PS4
    SaveData();
#endif

    Main.Shutdown();
    /*try
    {
      SteamClient.Shutdown();
    }
    catch { }*/
  }

#if !UNITY_PS4
  private void OnApplicationPause(bool pause)
  {
    if (pause)
      SaveData();
    else
      UploadData();
  }
#endif

  //============================================================

  /// <summary>
  /// Ñîõðàíèòü äàííûå èãðû
  /// </summary>
  public void SaveData()
  {
    SaveLoadManager.Instance.SaveFile();
  }

  /// <summary>
  /// Çàãðóçèòü äàííûå èãðû
  /// </summary>
  public void UploadData()
  {
    SaveLoadManager.Instance.LoadFile();
  }
#if !UNITY_PS4
  /// <summary>
  /// Ñîçäàíèå ðàçðåøåíèé ýêðàíà
  /// </summary>
  private void CreateResolutions()
  {
    Resolution[] resolutions = Screen.resolutions;
    HashSet<Tuple<int, int>> newResolutions = new HashSet<Tuple<int, int>>();
    Dictionary<Tuple<int, int>, int> maxRefreshRates = new Dictionary<Tuple<int, int>, int>();

    for (int i = 0; i < resolutions.Length; i++)
    {
      var resolution = new Tuple<int, int>(resolutions[i].width, resolutions[i].height);
      newResolutions.Add(resolution);

      if (!maxRefreshRates.ContainsKey(resolution)) {
        maxRefreshRates.Add(resolution, resolutions[i].refreshRate);
      } else {
        maxRefreshRates[resolution] = resolutions[i].refreshRate;
      }
    }

    foreach (var resolution in newResolutions)
    {
      Resolution newResolution = new Resolution();
      newResolution.width = resolution.Item1;
      newResolution.height = resolution.Item2;

      if (maxRefreshRates.TryGetValue(resolution, out int refreshRate))
      {
        newResolution.refreshRate = refreshRate;
      }

      Resolutions.Add(newResolution);
    }

    for (int i = 0; i < Resolutions.Count; i++)
    {
      if (Resolutions[i].width == Screen.width & Resolutions[i].height == Screen.height)
      {
        CurrentSelectedResolution = i;
      }
    }
  }
#endif
  /// <summary>
  /// Îòêðûòèå ñëåäóþùåãî óðîâíÿ èãðû
  /// </summary>
  /// <param name="parCurrentLevel">Èíäåêñ òåêóùåãî óðîâíÿ</param>
  public void UnlockNextLevelGame(int parCurrentLevel)
  {
    if (parCurrentLevel == CountUnlockedLevel)
    {
      CountUnlockedLevel++;
    }
    //Проверка ачивок должна быть в любом случае, т.к. они могут быть сброшены
    UpdateAchievementsLevels();

    SaveData();
  }

  /// <summary>
  /// Ïîëó÷èòü îòêðûòûå ãëàâû
  /// </summary>
  public void GetOpenChapters()
  {
    if (CountUnlockedLevel > 20)
    {
      OpenChapters[1] = true;
      OpenChapters[2] = false;
    }

    if (CountUnlockedLevel > 40)
    {
      OpenChapters[1] = true;
      OpenChapters[2] = true;
    }

    if (CountUnlockedLevel > 60)
    {
      OpenChapters[1] = true;
      OpenChapters[2] = true;
      OpenChapters[3] = true;
    }
  }

  public void GetNumberCompletedLevelChapters()
  {
    NumberCompletedLevelChapters[1].numberCompletedLevels = Mathf.Clamp(CountUnlockedLevel - 1, 0, 20);
    NumberCompletedLevelChapters[2].numberCompletedLevels = Mathf.Clamp(CountUnlockedLevel - 1 - 20, 0, 20);
    NumberCompletedLevelChapters[3].numberCompletedLevels = Mathf.Clamp(CountUnlockedLevel - 1 - 40, 0, 20);
    NumberCompletedLevelChapters[4].numberCompletedLevels = Mathf.Clamp(CountUnlockedLevel - 1 - 60, 0, 8);
    /*
    if (CountUnlockedLevel < 21)
    {
      NumberCompletedLevelChapters[1].numberCompletedLevels = CountUnlockedLevel - 1;
    }

    if (CountUnlockedLevel > 20 && CountUnlockedLevel < 41)
    {
      NumberCompletedLevelChapters[1].numberCompletedLevels = 20;
      NumberCompletedLevelChapters[2].numberCompletedLevels = CountUnlockedLevel - 1 - 20;
    }

    if (CountUnlockedLevel > 40 && CountUnlockedLevel < 61)
    {
      NumberCompletedLevelChapters[1].numberCompletedLevels = 20;
      NumberCompletedLevelChapters[2].numberCompletedLevels = 20;
      NumberCompletedLevelChapters[3].numberCompletedLevels = CountUnlockedLevel - 1 - 40;
    }

    if (CountUnlockedLevel > 60 && CountUnlockedLevel < 69)
    {
      NumberCompletedLevelChapters[1].numberCompletedLevels = 20;
      NumberCompletedLevelChapters[2].numberCompletedLevels = 20;
      NumberCompletedLevelChapters[3].numberCompletedLevels = 20;
      NumberCompletedLevelChapters[4].numberCompletedLevels = CountUnlockedLevel - 1 - 60;
    }
    else
    {
      NumberCompletedLevelChapters[1].numberCompletedLevels = 20;
      NumberCompletedLevelChapters[2].numberCompletedLevels = 20;
      NumberCompletedLevelChapters[3].numberCompletedLevels = 20;
      NumberCompletedLevelChapters[4].numberCompletedLevels = 8;
    }*/
  }
  
  public float GetGlobalBestTimeMainLevels()
  {
    return GetGlobalBestTimeSecretLevels(0, 60);
  }
  public float GetGlobalBestTimeSecretLevels()
  {
    return GetGlobalBestTimeSecretLevels(60, 8);
  }
  public float GetGlobalBestTimeSecretLevels(int parStartIndex, int parCount)
  {
    float time = 0;
    for (int i = parStartIndex; i < parStartIndex + parCount; i++)
    {
      if(BestTimeCompliteLevels.TryGetValue(i, out float t))
      {
        time += t;
      }
    }

    return time;
  }

  public void OnDeleteSave()
  {
    _countUnlockedLevel = 1;
    _numberDeaths = 0;
    _numberMurders = 0;
    _numberJumps = 0;
    _numberGamesPlayed = 0;

    CountCollectedItems = new List<int>();
    BestTimeCompliteLevels = new Dictionary<int, float>();
    DialogsViewed = new List<int>();
    OpenChapters = new bool[4] { true, false, false, false };

    for (int i = 0; i < NumberCompletedLevelChapters.Count; i++)
    {
      NumberCompletedLevelChapters[i + 1].numberCompletedLevels = 0;
    }

    SaveData();
  }

  /// <summary>
  /// Получить просмотренные диалоги
  /// </summary>
  /// <param name="parIndexLevel">Индекс уровня</param>
  public bool GetDialogsViewed(int parIndexLevel)
  {
    return DialogsViewed.Contains(parIndexLevel);
  }

  /// <summary>
  /// Добавить в просмотренные диалоги
  /// </summary>
  /// <param name="parIndexLevel">Индекс уровня</param>
  public void AddDialogsViewed(int parIndexLevel)
  {
    if (GetDialogsViewed(parIndexLevel))
      return;

    DialogsViewed.Add(parIndexLevel);
  }

  //============================================================

  /// <summary>
  /// Öâåò ñëîæíîñòè EASY
  /// </summary>
  public Color32 ColorDifficultyEasy()
  {
    return new Color32(46, 232, 23, 255);
  }

  /// <summary>
  /// Öâåò ñëîæíîñòè MEDIUM
  /// </summary>
  public Color32 ColorDifficultyMedium()
  {
    return new Color32(235, 225, 20, 255);
  }

  /// <summary>
  /// Öâåò ñëîæíîñòè HARD
  /// </summary>
  public Color32 ColorDifficultyHard()
  {
    return new Color32(233, 38, 22, 255);
  }

  //============================================================

  /// <summary>
  /// Îáíîâèòü èíôîðìàöèþ î êîëè÷åñòâå ñìåðòåé
  /// </summary>
  public void UpdateAchievementsDeaths()
  {
    if(NumberDeaths >= 1000)
    {
      UpdateAchievement(Achievement.DEATHS_1000);
    }
    if (NumberDeaths >= 750)
    {
      UpdateAchievement(Achievement.DEATHS_750);
    }
    if (NumberDeaths >= 500)
    {
      UpdateAchievement(Achievement.DEATHS_500);
    }
    if (NumberDeaths >= 250)
    {
      UpdateAchievement(Achievement.DEATHS_250);
    }
    if (NumberDeaths >= 100)
    {
      UpdateAchievement(Achievement.DEATHS_100);
    }
    if (NumberDeaths >= 50)
    {
      UpdateAchievement(Achievement.DEATHS_50);
    }
    if (NumberDeaths >= 25)
    {
      UpdateAchievement(Achievement.DEATHS_25);
    }
    if (NumberDeaths >= 1)
    {
      UpdateAchievement(Achievement.DEATH_1);
    }
  }

  /// <summary>
  /// Îáíîâèòü èíôîðìàöèþ î ïðîéäåííûõ óðîâíÿõ
  /// </summary>
  public void UpdateAchievementsLevels()
  {
    //главы
    if (CountUnlockedLevel > 20)
    {
      UpdateAchievement(Achievement.CHAPTER_1);
    }
    if (CountUnlockedLevel > 40)
    {
      UpdateAchievement(Achievement.CHAPTER_2);
    }
    if (CountUnlockedLevel > 60)
    {
      UpdateAchievement(Achievement.CHAPTER_3);
    }

    if (CountUnlockedLevel > 60 && CountCollectedItems.Count >= 60)
    {
      UpdateAchievement(Achievement.OPEN_SEC_LEVELS);
    }
    if (CountUnlockedLevel > 68)
    {
      UpdateAchievement(Achievement.END_SEC_LEVELS);
    }

    //Уровни
    if (CountUnlockedLevel > 60)
    {
      UpdateAchievement(Achievement.LEVELS_60);
    }
    if (CountUnlockedLevel > 50)
    {
      UpdateAchievement(Achievement.LEVELS_50);
    }
    if (CountUnlockedLevel > 40)
    {
      UpdateAchievement(Achievement.LEVELS_40);
    }
    if (CountUnlockedLevel > 30)
    {
      UpdateAchievement(Achievement.LEVELS_30);
    }
    if (CountUnlockedLevel > 20)
    {
      UpdateAchievement(Achievement.LEVELS_20);
    }
    if (CountUnlockedLevel > 10)
    {
      UpdateAchievement(Achievement.LEVELS_10);
    }
    if (CountUnlockedLevel > 1)
    {
      UpdateAchievement(Achievement.LEVEL_1);
    }
  }

  /// <summary>
  /// Îáíîâèòü èíôîðìàöèþ î ñîáðàííûõ ïðåäìåòàõ
  /// </summary>
  public void UpdateAchievementsItems()
  {
    int count = CountCollectedItems.Count;

    if (count >= MAX_ITEMS)
    {
      UpdateAchievement(Achievement.ALL_ITEMS);
    }
    if (count >= 50)
    {
      UpdateAchievement(Achievement.ITEMS_50);
    }
    if (count >= 25)
    {
      UpdateAchievement(Achievement.ITEMS_25);
    }
    if (count >= 10)
    {
      UpdateAchievement(Achievement.ITEMS_10);
    }
    if (count >= 1)
    {
      UpdateAchievement(Achievement.ITEM_1);
    }
  }

  /// <summary>
  /// Îáíîâèòü èíôîðìàöèþ î óáèòûõ âðàãàõ
  /// </summary>
  public void UpdateAchievementsKills()
  {
    if (NumberMurders >= 1000)
    {
      UpdateAchievement(Achievement.KILLS_1000);
    }
    if (NumberMurders >= 500)
    {
      UpdateAchievement(Achievement.KILLS_500);
    }
    if (NumberMurders >= 100)
    {
      UpdateAchievement(Achievement.KILLS_100);
    }
    if (NumberMurders >= 50)
    {
      UpdateAchievement(Achievement.KILLS_50);
    }
    if (NumberMurders >= 25)
    {
      UpdateAchievement(Achievement.KILLS_25);
    }
    if (NumberMurders >= 1)
    {
      UpdateAchievement(Achievement.KILL_1);
    }
  }
  public void UpdateAchievementsJumps()
  {
    if (NumberJumps >= 5000)
    {
      UpdateAchievement(Achievement.JUMPS_5000);
    }
    if (NumberJumps >= 1000)
    {
      UpdateAchievement(Achievement.JUMPS_1000);
    }
    if (NumberJumps >= 500)
    {
      UpdateAchievement(Achievement.JUMPS_500);
    }
    if (NumberJumps >= 100)
    {
      UpdateAchievement(Achievement.JUMPS_100);
    }
    if (NumberJumps >= 1)
    {
      UpdateAchievement(Achievement.JUMP_1);
    }
  }

  public enum Achievement
  {
    ALL_ACHIEVEMENTS = 0,

    DEATH_1 = 1,
    DEATHS_25 = 2,
    DEATHS_50 = 3,
    DEATHS_100 = 4,
    DEATHS_250 = 5,
    DEATHS_500 = 6,
    DEATHS_750 = 7,
    DEATHS_1000 = 8,

    ITEM_1 = 9,
    ITEMS_10 = 10,
    ITEMS_25 = 11,
    ITEMS_50 = 12,
    ALL_ITEMS = 13,

    KILL_1 = 14,
    KILLS_25 = 15,
    KILLS_50 = 16,
    KILLS_100 = 17,
    KILLS_500 = 18,
    KILLS_1000 = 19,

    LEVEL_1 = 20,
    LEVELS_10 = 21,
    LEVELS_20 = 22,
    LEVELS_30 = 23,
    LEVELS_40 = 24,
    LEVELS_50 = 25,
    LEVELS_60 = 26,

    OPEN_SEC_LEVELS = 27,
    END_SEC_LEVELS = 28,

    CHAPTER_1 = 29,
    CHAPTER_2 = 30,
    CHAPTER_3 = 31,

    SPEEDRUNNER = 32,

    JUMP_1 = 33,
    JUMPS_100 = 34,
    JUMPS_500 = 35,
    JUMPS_1000 = 36,
    JUMPS_5000 = 37,

    CHARACTER_BREAK = 38,
#if !UNITY_PS4
    HELLO = 39,
#endif
  }

  public void UpdateAchievement(Achievement parAchievement)
  {
    Debug.Log($"UpdateAchievement({parAchievement})");
    Achievements.UnlockAchievement((int)parAchievement);
    /*
    var request = new UnlockAchievement_Request((int)parAchievement, AchievementUnlocked);
    if(request.Execute())
    {
      Debug.Log($"UnlockAchievement_Request: Achievement={parAchievement} UserId={request.UserId} RequestId={request.RequestId}");
    }
    else
    {
      Debug.LogWarning($"Error UnlockAchievement_Request: Achievement={parAchievement} UserId={request.UserId} RequestId={request.RequestId}");
    }*/
  }
  /*
  private void AchievementUnlocked (uint parRequestId, LocalUserId parUserId, bool parError)
  {
    if(parError)
    {
      Debug.LogWarning($"Error AchievementUnlocked: UserId={parUserId} RequestId={parRequestId}");
    }
    else
    {
      Debug.Log($"AchievementUnlocked: UserId={parUserId} RequestId={parRequestId}");
    }
  }*/

  //============================================================
}