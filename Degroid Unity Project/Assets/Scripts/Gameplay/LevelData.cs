using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Data/Level Data")]
public class LevelData : ScriptableObject
{
  [SerializeField, Tooltip("������ ������")]
  private int _indexLevel;

  [SerializeField, Tooltip("������ �������� �� ������")]
  private Sprite _item;
  [SerializeField, Tooltip("������ ����� �� ������")]
  private Sprite _key;
  [SerializeField, Tooltip("������ ����� �� ������")]
  private Sprite _doorStart;
  [SerializeField, Tooltip("������ ����� �� ������")]
  private Sprite _doorEnd;
  [SerializeField, Tooltip("����� �������")]
  private Sprite _doorOpen;

  //============================================================

  /// <summary>
  /// ������ ������
  /// </summary>
  public int IndexLevel { get => _indexLevel; set => _indexLevel = value; }

  /// <summary>
  /// ������ �������� �� ������
  /// </summary>
  public Sprite Item { get => _item; set => _item = value; }

  /// <summary>
  /// ������ ����� �� ������
  /// </summary>
  public Sprite Key { get => _key; set => _key = value; }

  /// <summary>
  /// ������ ����� �� ������
  /// </summary>
  public Sprite DoorStart { get => _doorStart; set => _doorStart = value; }
  /// <summary>
  /// ������ ����� �� ������
  /// </summary>
  public Sprite DoorEnd { get => _doorEnd; set => _doorEnd = value; }
  /// <summary>
  /// ������ ����� �� ������
  /// </summary>
  public Sprite DoorOpen { get => _doorOpen; set => _doorOpen = value; }

  //============================================================
}