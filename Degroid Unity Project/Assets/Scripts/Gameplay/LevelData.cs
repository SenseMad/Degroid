using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Data/Level Data")]
public class LevelData : ScriptableObject
{
  [SerializeField, Tooltip("Индекс уровня")]
  private int _indexLevel;

  [SerializeField, Tooltip("Данные предмета на уровне")]
  private Sprite _item;
  [SerializeField, Tooltip("Данные ключа на уровне")]
  private Sprite _key;
  [SerializeField, Tooltip("Данные двери на уровне")]
  private Sprite _doorStart;
  [SerializeField, Tooltip("Данные двери на уровне")]
  private Sprite _doorEnd;
  [SerializeField, Tooltip("Дверь открыта")]
  private Sprite _doorOpen;

  //============================================================

  /// <summary>
  /// Индекс уровня
  /// </summary>
  public int IndexLevel { get => _indexLevel; set => _indexLevel = value; }

  /// <summary>
  /// Данные предмета на уровне
  /// </summary>
  public Sprite Item { get => _item; set => _item = value; }

  /// <summary>
  /// Данные ключа на уровне
  /// </summary>
  public Sprite Key { get => _key; set => _key = value; }

  /// <summary>
  /// Данные двери на уровне
  /// </summary>
  public Sprite DoorStart { get => _doorStart; set => _doorStart = value; }
  /// <summary>
  /// Данные двери на уровне
  /// </summary>
  public Sprite DoorEnd { get => _doorEnd; set => _doorEnd = value; }
  /// <summary>
  /// Данные двери на уровне
  /// </summary>
  public Sprite DoorOpen { get => _doorOpen; set => _doorOpen = value; }

  //============================================================
}