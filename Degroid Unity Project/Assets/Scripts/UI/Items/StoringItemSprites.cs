using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "StoringItemSprites", menuName = "Data/Storing Item Sprites")]
public class StoringItemSprites : ScriptableObject
{
  [SerializeField, Tooltip("Спрайты предметов (Номер индекса соответствует индексу уровня)")]
  public List<Sprite> _itemSprites;

  //============================================================

  /// <summary>
  /// Получить все спрайты
  /// </summary>
  public List<Sprite> GetAllSprites() => _itemSprites;

  //============================================================

  /// <summary>
  /// Получить спрайт по индексу
  /// </summary>
  /// <param name="parIndex">Индекс спрайта</param>
  public Sprite GetSprite(int parIndex)
  {
    return _itemSprites[parIndex];
  }

  //============================================================
}