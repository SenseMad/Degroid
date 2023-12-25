using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIChapterSelectButton : MonoBehaviour
{
  [SerializeField, Tooltip("����� �������� �����")]
  private TextMeshProUGUI _textChapterName;
  [SerializeField, Tooltip("����� ���������� ���������� ������� �� �����")]
  private TextMeshProUGUI _textNumberLevel;
  [SerializeField, Tooltip("Image ������ ������")]
  private Image _spriteLock;
  [SerializeField]
  private Image _background;
  [SerializeField]
  private List<Sprite> _sprite = new List<Sprite>();

  [Header("SECRET LEVEL")]
  [SerializeField, Tooltip("")]
  private GameObject _numberItems;
  [SerializeField, Tooltip("����� ���������� ��������� ���������")]
  private TextMeshProUGUI _numberItemsStaticText;
  [SerializeField, Tooltip("����� ���������� ��������� ���������")]
  private TextMeshProUGUI _numberItemsText;

  //============================================================

  public Button Button { get; set; }

  /// <summary>
  /// ����� �����
  /// </summary>
  public int NumberChapter { get; set; }

  public GameObject NumberItems { get => _numberItems; set => _numberItems = value; }
  public TextMeshProUGUI NumberItemsStaticText { get => _numberItemsStaticText; set => _numberItemsStaticText = value; }
  public TextMeshProUGUI NumberItemsText { get => _numberItemsText; set => _numberItemsText = value; }

  //============================================================



  //============================================================

  /// <summary>
  /// ������������� ������ ������ �����
  /// </summary>
  public void Initialize(int parNumberChapter, int parNumberLevels, int parNumberCompletedLevel)
  {
    NumberChapter = parNumberChapter;
    switch (NumberChapter)
    {
      case 1:
        _textChapterName.text = $"{GetKey("CHAPTER_1_KEY")}";
        break;
      case 2:
        _textChapterName.text = $"{GetKey("CHAPTER_2_KEY")}";
        break;
      case 3:
        _textChapterName.text = $"{GetKey("CHAPTER_3_KEY")}";
        break;
      case 4:
        _textChapterName.text = $"{GetKey("CHAPTER_4_KEY")}";
        break;
    }
    _textNumberLevel.text = $"{parNumberCompletedLevel}/{parNumberLevels}";
    _background.sprite = _sprite[NumberChapter - 1];
  }

  /// <summary>
  /// �������� ��� ������ ������ ������
  /// </summary>
  public void UpdateLockImage(bool parValue)
  {
    _spriteLock.gameObject.SetActive(parValue);
    _textNumberLevel.gameObject.SetActive(!parValue);

    if (parValue)
    {
      _spriteLock.color = ColorsGame.STANDART_COLOR;
      _textChapterName.color = ColorsGame.STANDART_COLOR;
    }
    else
    {
      _spriteLock.color = ColorsGame.STANDART_COLOR;
      _textChapterName.color = ColorsGame.STANDART_COLOR;
    }
  }

  /// <summary>
  /// �������� ��������� ��������� ������ ������ ������
  /// </summary>
  public void UpdateSelectImage(bool parValue)
  {
    if (parValue)
    {
      _textNumberLevel.color = ColorsGame.SELECTED_COLOR;
      _textChapterName.color = ColorsGame.SELECTED_COLOR;
    }
    else
    {
      _textNumberLevel.color = ColorsGame.STANDART_COLOR;
      _textChapterName.color = ColorsGame.STANDART_COLOR;
    }
  }
  
  public string GetKey(string parKey)
  {
    if (parKey == "")
      return "";

    string value = LocalisationSystem.Instance.GetLocalisedValue(parKey);
    value = value.TrimStart(' ', '"');
    value = value.Replace("\"", "");

    return value;
  }

  //============================================================
}