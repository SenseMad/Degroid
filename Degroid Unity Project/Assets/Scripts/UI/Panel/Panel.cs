using UnityEngine;

public class Panel : MonoBehaviour
{
  [SerializeField, Tooltip("������ ������")]
  private GameObject _objectPanel;

  [SerializeField, Tooltip("True, ���� ������ ��������")]
  private bool _isShown;

  //============================================================

  /// <summary>
  /// ������ ������
  /// </summary>
  public GameObject ObjectPanel { get => _objectPanel; set => _objectPanel = value; }

  /// <summary>
  /// True, ���� ������ ��������
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
  /// �������� ������
  /// </summary>
  public void ShowPanel()
  {
    IsShown = true;
  }

  /// <summary>
  /// ������ ������
  /// </summary>
  public void HidePanel()
  {
    IsShown = false;
  }

  //============================================================
}