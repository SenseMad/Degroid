using UnityEngine;

/// <summary>
/// Данный класс вышать на объекты которым можно будет нанести урон
/// </summary>
public class Damageable : MonoBehaviour
{
  [SerializeField, Tooltip("Количество урона")]
  private int _damage = 1;

  //============================================================

  /// <summary>
  /// Количество урона
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