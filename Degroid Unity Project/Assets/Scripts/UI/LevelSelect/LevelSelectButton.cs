using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
  [SerializeField, Tooltip("����� ������� ������")]
  private TextMeshProUGUI _indexLevelText;

  [SerializeField, Tooltip("Image ������ ������")]
  private Image _lockImage;

  [SerializeField, Tooltip("Image ��������� ������")]
  private Image _selectImage;

  //============================================================

  public Button Button { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// ����� ������� ������
  /// </summary>
  public TextMeshProUGUI IndexLevelText { get => _indexLevelText; set => _indexLevelText = value; }

  //------------------------------------------------------------

  /// <summary>
  /// ������ ������
  /// </summary>
  public int IndexLevel { get; set; }

  /// <summary>
  /// True, ���� ������� ������
  /// </summary>
  public bool IsLevelOpen { get; set; }

  //============================================================

  private void Awake()
  {
    Button = GetComponent<Button>();
  }

  //============================================================

  /// <summary>
  /// �������� ��� ������ ������ ������
  /// </summary>
  public void UpdateLockImage(bool parValue)
  {
    _lockImage.gameObject.SetActive(parValue);
  }

  /// <summary>
  /// �������� ��������� ��������� ������ ������ ������
  /// </summary>
  public void UpdateSelectImage(bool parValue)
  {
    _selectImage.gameObject.SetActive(parValue);
  }

  /// <summary>
  /// ������������� ������ ������ ������
  /// </summary>
  public void Initialize(LevelData levelData)
  {
    IndexLevelText.text = $"{levelData.IndexLevel + 1}";
    IndexLevel = levelData.IndexLevel + 1;
  }

  /// <summary>
  /// ��������� �������
  /// </summary>
  public void LoadLevel()
  {
    if (IndexLevel > SceneManager.sceneCountInBuildSettings - 1) { return; }

    GameManager.Instance.NumberGamesPlayed++;
  }

  //============================================================



  //============================================================
}