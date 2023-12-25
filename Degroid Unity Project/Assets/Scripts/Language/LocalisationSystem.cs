using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class LocalisationSystem : MonoBehaviour
{
  private static Language _currentLanguage = Language.English;

  private static Dictionary<string, string> localisedRU;
  private static Dictionary<string, string> localisedEN;
  private static Dictionary<string, string> localisedFR;
  private static Dictionary<string, string> localisedJA;
  private static Dictionary<string, string> localisedGE;
  private static Dictionary<string, string> localisedSP;
  private static Dictionary<string, string> localisedPO;
  private static Dictionary<string, string> localisedCH;

  public static bool IsInit;

  private static LocalisationSystem _instance;

  //============================================================

  [SerializeField]
  private FontAsset _fontAsset;

  //============================================================

  /// <summary>
  /// Текущий язык
  /// </summary>
  public static Language CurrentLanguage
  {
    get => _currentLanguage;
    set
    {
      _currentLanguage = value;
      ChangeCurrentLanguage?.Invoke(value);
    }
  }

  public static LocalisationSystem Instance
  {
    get
    {
      if (_instance == null) { _instance = FindObjectOfType<LocalisationSystem>(); }
      return _instance;
    }
  }

  //============================================================

  /// <summary>
  /// Событие: Изменение текущего языка
  /// </summary>
  public static CustomUnityEvent<Language> ChangeCurrentLanguage { get; } = new CustomUnityEvent<Language>();

  //============================================================

  private void Awake()
  {
    if (_instance != null && _instance != this)
    {
      Destroy(this);
      return;
    }
    _instance = this;
  }

  //============================================================

  /// <summary>
  /// Получить шрифт
  /// </summary>
  public TMP_FontAsset GetFont()
  {
    return _fontAsset.GetFont(_currentLanguage);
  }

  public void Init()
  {
    CSVLoader csvLoader = new CSVLoader();
    csvLoader.LoadCSV();

    localisedRU = csvLoader.GetDictionaryValues("ru");
    localisedEN = csvLoader.GetDictionaryValues("en");
    localisedFR = csvLoader.GetDictionaryValues("fr");
    localisedJA = csvLoader.GetDictionaryValues("ja");
    localisedGE = csvLoader.GetDictionaryValues("ge");
    localisedSP = csvLoader.GetDictionaryValues("sp");
    localisedPO = csvLoader.GetDictionaryValues("po");
    localisedCH = csvLoader.GetDictionaryValues("ch");

    IsInit = true;
  }

  public string GetLocalisedValue(string key)
  {
    if (!IsInit) { Init(); }

    string value = key;

    switch (CurrentLanguage)
    {
      case Language.Russian:
        localisedRU.TryGetValue(key, out value);
        break;
      case Language.English:
        localisedEN.TryGetValue(key, out value);
        break;
      case Language.French:
        localisedFR.TryGetValue(key, out value);
        break;
      case Language.Japan:
        localisedJA.TryGetValue(key, out value);
        break;
      case Language.German:
        localisedGE.TryGetValue(key, out value);
        break;
      case Language.Spanish:
        localisedSP.TryGetValue(key, out value);
        break;
      case Language.Portuguese:
        localisedPO.TryGetValue(key, out value);
        break;
      case Language.Chinese:
        localisedCH.TryGetValue(key, out value);
        break;
    }

    return value;
  }

  /// <summary>
  /// Получить шрифт локализации
  /// </summary>
  public TMP_FontAsset GetLocalizationFont(Language parLanguage)
  {
    return _fontAsset.GetFont(parLanguage);
  }

  //============================================================
}

public enum Language
{
  /// <summary>
  /// Китайский
  /// </summary>
  Chinese,
  /// <summary>
  /// Английский
  /// </summary>
  English,
  /// <summary>
  /// Французский
  /// </summary>
  French,
  /// <summary>
  /// Немецкий
  /// </summary>
  German,
  /// <summary>
  /// Японский
  /// </summary>
  Japan,
  /// <summary>
  /// Португальский
  /// </summary>
  Portuguese,
  /// <summary>
  /// Русский
  /// </summary>
  Russian,
  /// <summary>
  /// Испанский
  /// </summary>
  Spanish,
}