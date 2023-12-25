using UnityEngine;

/// <summary>
/// Вращение пилы
/// </summary>
public class Saw : MonoBehaviour
{
  [SerializeField, Tooltip("Скорость пилы")]
  private float _moveSpeed = 500;

  //============================================================

  /// <summary>
  /// Скорость пилы
  /// </summary>
  public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }

  //============================================================

  private void Update()
  {
    transform.Rotate(new Vector3(0f, 0f, -MoveSpeed * Time.deltaTime));
  }

  //============================================================
}