using UnityEngine;

public class Panel : MonoBehaviour
{
  [SerializeField, Tooltip("Объект панели")]
  private GameObject _objectPanel;

  [SerializeField, Tooltip("True, если панель показана")]
  private bool _isShown;

  //============================================================

  /// <summary>
  /// Объект панели
  /// </summary>
  public GameObject ObjectPanel { get => _objectPanel; set => _objectPanel = value; }

  /// <summary>
  /// True, если панель показана
  /// </summary>
  public bool IsShown
  {
    get => _isShown;
    private set
    {
      _isShown = value;
      ObjectPanel?.SetActive(value);
    }
  }

  //============================================================

  /// <summary>
  /// Показать панель
  /// </summary>
  public void ShowPanel()
  {
    IsShown = true;
  }

  /// <summary>
  /// Скрыть панель
  /// </summary>
  public void HidePanel()
  {
    IsShown = false;
  }

  //============================================================
}