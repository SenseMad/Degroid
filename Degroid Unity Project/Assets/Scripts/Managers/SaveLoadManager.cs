using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Platform.Any;
using Platform.Any.Data;

public class SaveLoadManager : MonoBehaviour
{
  private readonly static string PATH = $"{Application.persistentDataPath}/SaveData.dat"; // Путь к сохранению

  private static SaveLoadManager _instance;

  //============================================================

  public static SaveLoadManager Instance
  {
    get
    {
      if (_instance == null)
      {
        GameObject obj = new GameObject("SaveLoadManager");
        _instance = obj.AddComponent<SaveLoadManager>();
        DontDestroyOnLoad(obj);
      }

      return _instance;
    }
  }

  //============================================================

  public void Awake()
  {
    if (_instance == null)
    {
      _instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(this);
      return;
    }

#if UNITY_PS4
    if (SaveLoad.Init())
    {
      Debug.Log("SaveLoad initialized!");
      SaveLoad.GameLoaded += SaveLoad_GameLoaded;
    }
    else
    {
      Debug.LogWarning("Error: SaveLoad not initialized!");
    }
#endif
  }

#if UNITY_PS4
  private void Update()
  {
    SaveLoad.Update();
  }

  private void OnDisable()
  {
    SaveLoad.Shutdown();
  }
#endif

  //============================================================

  public void SaveFile()
  {
    GameData data = GetGameData();

#if UNITY_PS4
    MemoryStream memoryStream = new MemoryStream();
    BinaryFormatter binaryFormatter = new BinaryFormatter();
    binaryFormatter.Serialize(memoryStream, data);

    SaveGameSlotDetails saveParams = new SaveGameSlotDetails()
    {
      Title = "Degroid SaveData",
      SubTitle = "Your game progress",
      Detail = "This is where your suffering is stored!"
    };

    SaveLoad.Save(memoryStream.GetBuffer(), saveParams);
#else
    FileStream fileStream = new FileStream(PATH, FileMode.Create);
    BinaryFormatter binaryFormatter = new BinaryFormatter();

    binaryFormatter.Serialize(fileStream, data);
    fileStream.Close();
#endif
  }

  //============================================================

  public void ResetAndSaveFile()
  {
    ResetGameData();
    SaveFile();
  }

  public static void ResetGameData()
  {
    SetGameData(new GameData());
  }

  //============================================================

  public void LoadFile()
  {
#if UNITY_PS4

    SaveLoad.Load();
#else
    if (File.Exists(PATH))
    {
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      FileStream fileStream = new FileStream(PATH, FileMode.Open);
      GameData data = (GameData)binaryFormatter.Deserialize(fileStream);
      fileStream.Close();
      
      SetGameData(data);
    }
#endif
  }

#if UNITY_PS4
  private void SaveLoad_GameLoaded(LocalUserId parUserId, byte[] parData, int parErrorCode)
  {
    if (parErrorCode == 0)
    {
      MemoryStream input = new MemoryStream(parData);
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      GameData data = (GameData)binaryFormatter.Deserialize(input);
      SetGameData(data);
    }
  }
#endif

  //============================================================

  [System.Serializable]
  public class GameData
  {
    public Language CurrentLanguage = Language.English;
    public int MusicValue = 25;
    public int SoundValue = 25;
#if !UNITY_PS4
    public bool FullScreenValue = true;
    public bool VSyncValue = true;
#endif
    public bool VibrationOn = true;

    public int CountUnlockedLevel = 1;
    public int NumberDeaths = 0;
    public int NumberMurders = 0;
    public int NumberJumps = 0;
    public int NumberGamesPlayed = 0;
    public List<int> CountCollectedItems = new List<int>();
    public Dictionary<int, float> BestTimeCompliteLevels = new Dictionary<int, float>();
    public List<int> DialogsViewed = new List<int>();
  }

  private static GameData GetGameData()
  {
    GameManager gameManager = GameManager.Instance;
    return new GameData()
    {
      CurrentLanguage = gameManager.CurrentLanguage,
      MusicValue = gameManager.MusicValue,
      SoundValue = gameManager.SoundValue,
#if !UNITY_PS4
      FullScreenValue = gameManager.FullScreenValue,
      VSyncValue = gameManager.VSyncValue,
#endif
      VibrationOn = gameManager.VibrationOn,

      CountUnlockedLevel = gameManager.CountUnlockedLevel,
      NumberDeaths = gameManager.NumberDeaths,
      NumberMurders = gameManager.NumberMurders,
      NumberJumps = gameManager.NumberJumps,
      NumberGamesPlayed = gameManager.NumberGamesPlayed,
      CountCollectedItems = gameManager.CountCollectedItems,
      BestTimeCompliteLevels = gameManager.BestTimeCompliteLevels,
      DialogsViewed = gameManager.DialogsViewed,
    };
  }

  private static void SetGameData(GameData parData)
  {
    GameManager gameManager = GameManager.Instance;
    gameManager.CurrentLanguage = parData.CurrentLanguage;
    gameManager.MusicValue = parData.MusicValue;
    gameManager.SoundValue = parData.SoundValue;
#if !UNITY_PS4
    gameManager.FullScreenValue = parData.FullScreenValue;
    gameManager.VSyncValue = parData.VSyncValue;
#endif
    gameManager.VibrationOn = parData.VibrationOn;

    gameManager.CountUnlockedLevel = parData.CountUnlockedLevel;
    gameManager.NumberDeaths = parData.NumberDeaths;
    gameManager.NumberMurders = parData.NumberMurders;
    gameManager.NumberJumps = parData.NumberJumps;
    gameManager.NumberGamesPlayed = parData.NumberGamesPlayed;
    gameManager.CountCollectedItems = parData.CountCollectedItems ?? new List<int>();
    gameManager.BestTimeCompliteLevels = parData.BestTimeCompliteLevels ?? new Dictionary<int, float>();
    gameManager.DialogsViewed = parData.DialogsViewed ?? new List<int>();
  }
  //============================================================
}