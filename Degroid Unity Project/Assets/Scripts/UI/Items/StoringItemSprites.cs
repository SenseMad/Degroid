using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "StoringItemSprites", menuName = "Data/Storing Item Sprites")]
public class StoringItemSprites : ScriptableObject
{
  [SerializeField, Tooltip("������� ��������� (����� ������� ������������� ������� ������)")]
  public List<Sprite> _itemSprites;

  //============================================================

  /// <summary>
  /// �������� ��� �������
  /// </summary>
  public List<Sprite> GetAllSprites() => _itemSprites;

  //============================================================

  /// <summary>
  /// �������� ������ �� �������
  /// </summary>
  /// <param name="parIndex">������ �������</param>
  public Sprite GetSprite(int parIndex)
  {
    return _itemSprites[parIndex];
  }

  //============================================================
}