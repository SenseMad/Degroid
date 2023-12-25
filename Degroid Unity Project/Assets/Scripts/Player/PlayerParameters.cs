using System;
using UnityEngine;

/// <summary>
/// Параметры для класса PlayerController
/// Здесь определяем максимальную скорость, предел наклона и гравитацию
/// </summary>
[Serializable]
public class PlayerParameters
{
  [Header("Движение")]
  [Tooltip("Максимальная скорость")]
  public Vector2 MaxVelocity = new Vector2(100f, 100f);
  [Tooltip("Максимальный угол по которому можно ходить"), Range(0, 90)]
  public float MaximumSlopeAngle = 45;

  [Space(10), Header("Гравитация")]
  [Tooltip("Гравитация")]
  public float Gravity = -30;
  [Tooltip("Множитель, применяемый к гравитации персонажа при спуске")]
  public float FallMultiplier = 1.5f;
  [Tooltip("Множитель, применяемый к силе тяжести персонажа при движении вверх")]
  public float AscentMultiplier = 1f;

  [Space(10), Header("Physics2D")]
  [Tooltip("Сила, приложенная к объектам, с которыми сталкивается персонаж")]
  public float Physics2DPushForce = 2.0f;

  [Space(10), Header("Отображение")]
  [Tooltip("True, если отображать лучи")]
  public bool DrawRaycastGizmos = true;
}