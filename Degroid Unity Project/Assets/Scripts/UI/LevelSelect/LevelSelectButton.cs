using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
  [SerializeField, Tooltip("Текст индекса уровня")]
  private TextMeshProUGUI _indexLevelText;

  [SerializeField, Tooltip("Image замока кнопки")]
  private Image _lockImage;

  [SerializeField, Tooltip("Image выделения кнопки")]
  private Image _selectImage;

  //============================================================

  public Button Button { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// Текст индекса уровня
  /// </summary>
  public TextMeshProUGUI IndexLevelText { get => _indexLevelText; set => _indexLevelText = value; }

  //------------------------------------------------------------

  /// <summary>
  /// Индекс уровня
  /// </summary>
  public int IndexLevel { get; set; }

  /// <summary>
  /// True, если уровень открыт
  /// </summary>
  public bool IsLevelOpen { get; set; }

  //============================================================

  private void Awake()
  {
    Button = GetComponent<Button>();
  }

  //============================================================

  /// <summary>
  /// Обновить вид кнопки выбора уровня
  /// </summary>
  public void UpdateLockImage(bool parValue)
  {
    _lockImage.gameObject.SetActive(parValue);
  }

  /// <summary>
  /// Обновить состояние выделения кнопки выбора уровня
  /// </summary>
  public void UpdateSelectImage(bool parValue)
  {
    _selectImage.gameObject.SetActive(parValue);
  }

  /// <summary>
  /// Инициализация кнопки выбора уровня
  /// </summary>
  public void Initialize(LevelData levelData)
  {
    IndexLevelText.text = $"{levelData.IndexLevel + 1}";
    IndexLevel = levelData.IndexLevel + 1;
  }

  /// <summary>
  /// Загрузить уровень
  /// </summary>
  public void LoadLevel()
  {
    if (IndexLevel > SceneManager.sceneCountInBuildSettings - 1) { return; }

    GameManager.Instance.NumberGamesPlayed++;
  }

  //============================================================



  //============================================================
}