using UnityEngine;

/// <summary>
/// �������� ����
/// </summary>
public class Saw : MonoBehaviour
{
  [SerializeField, Tooltip("�������� ����")]
  private float _moveSpeed = 500;

  //============================================================

  /// <summary>
  /// �������� ����
  /// </summary>
  public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }

  //============================================================

  private void Update()
  {
    transform.Rotate(new Vector3(0f, 0f, -MoveSpeed * Time.deltaTime));
  }

  //============================================================
}