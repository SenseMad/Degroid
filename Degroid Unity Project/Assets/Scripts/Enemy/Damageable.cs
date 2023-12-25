using UnityEngine;

/// <summary>
/// ������ ����� ������ �� ������� ������� ����� ����� ������� ����
/// </summary>
public class Damageable : MonoBehaviour
{
  [SerializeField, Tooltip("���������� �����")]
  private int _damage = 1;

  //============================================================

  /// <summary>
  /// ���������� �����
  /// </summary>
  public int Damage { get => _damage; set => _damage = value; }

  //============================================================

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (!other.CompareTag("Player")) { return; }

    var health = other.GetComponent<Health>();

    if (health)
    {
      health.TakeDamage(Damage);
    }
  }

  //============================================================
}