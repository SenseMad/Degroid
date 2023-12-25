using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Случайное изменение фона
/// </summary>
public class RandomBackgroundChange : MonoBehaviour
{
  [SerializeField, Tooltip("Список фонов")]
  private List<Sprite> _listBackgrounds;

  //------------------------------------------------------------

  private LevelManager levelManager;
  private SpriteRenderer spriteRenderer;

  //============================================================

  private void Awake()
  {
    levelManager = LevelManager.Instance;

    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  private void Start()
  {
    ChangeBackground();
  }

  //============================================================

  /// <summary>
  /// Изменить фон
  /// </summary>
  private void ChangeBackground()
  {
    if (_listBackgrounds.Count == 0)
      return;

    if (levelManager == null)
    {
      levelManager = LevelManager.Instance;
    }

    int value = levelManager.LevelData.IndexLevel % _listBackgrounds.Count;

    spriteRenderer.sprite = _listBackgrounds[value];
  }

  //============================================================
}