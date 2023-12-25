using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
  [SerializeField, Tooltip("����� ������� �� ������")]
  private TextMeshProUGUI _textTime;
  [SerializeField, Tooltip("����� ������ ������")]
  private TextMeshProUGUI _textNumberLevel;

  [SerializeField, Tooltip("����� ���������� ������")]
  private Text _textNumberEnemy;

  [SerializeField, Tooltip("������ �����")]
  private GameObject _objectEnemy;

  [SerializeField, Tooltip("������ �����")]
  private GameObject _objectKey;

  [SerializeField, Tooltip("������ ��������")]
  private GameObject _objectItem;

  //============================================================

  private LevelManager LevelManager { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// ����� ���������� ������
  /// </summary>
  public Text TextNumberEnemy { get => _textNumberEnemy; set => _textNumberEnemy = value; }

  /// <summary>
  /// ������ �����
  /// </summary>
  public GameObject ObjectEnemy { get => _objectEnemy; set => _objectEnemy = value; }

  /// <summary>
  /// ������ �����
  /// </summary>
  public GameObject ObjectKey { get => _objectKey; set => _objectKey = value; }

  /// <summary>
  /// ������ ��������
  /// </summary>
  public GameObject ObjectItem { get => _objectItem; set => _objectItem = value; }

  //============================================================

  private void Awake()
  {
    LevelManager = FindObjectOfType<LevelManager>();
  }

  private void OnEnable()
  {
    LevelManager.ChangingTimeOnLevel.AddListener(UpdateTextTime);
    LevelManager.ChangeNumberEnemytoLevel.AddListener(UpdateTextNumberEnemy);
    LevelManager.OnKeyPickup.AddListener(UpdateObjectKey);
    LevelManager.OnItemPickup.AddListener(UpdateObjectItem);
  }

  private void OnDisable()
  {
    LevelManager.ChangingTimeOnLevel.RemoveAllListeners();
    LevelManager.ChangeNumberEnemytoLevel.RemoveAllListeners();
    LevelManager.OnKeyPickup.RemoveAllListeners();
    LevelManager.OnItemPickup.RemoveAllListeners();
  }

  private void Start()
  {
    _textNumberLevel.text = $"LEVEL {LevelManager.LevelData.IndexLevel + 1}";

    //ObjectKey.GetComponent<Image>().sprite = LevelManager.LevelData.Key.SpriteObject;
    //ObjectItem.GetComponent<Image>().sprite = LevelManager.LevelData.Item.SpriteObject;
  }

  //============================================================

  /// <summary>
  /// �������� ����� �������
  /// </summary>
  private void UpdateTextTime(float parValue)
  {
    float min = (int)parValue / 60;
    string sec = (parValue % 60).ToString("0.0").Replace(',', '.');
    _textTime.text = $"{min} : {sec}";
  }

  /// <summary>
  /// �������� ����� ���������� ������
  /// </summary>
  private void UpdateTextNumberEnemy(int parValue)
  {
    //if (!ObjectEnemy.activeSelf) { ObjectEnemy.SetActive(true); }

    //TextNumberEnemy.text = $"{parValue}";
  }

  /// <summary>
  /// �������� ������ �����
  /// </summary>
  private void UpdateObjectKey()
  {
    //ObjectKey.SetActive(true);
  }

  /// <summary>
  /// �������� ������ ��������
  /// </summary>
  private void UpdateObjectItem()
  {
    //ObjectItem.SetActive(true);
  }

  //============================================================
}