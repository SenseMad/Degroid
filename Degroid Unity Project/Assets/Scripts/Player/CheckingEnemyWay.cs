using UnityEngine;

/// <summary>
/// �������� ���� �� ����
/// </summary>
public class CheckingEnemyWay : MonoBehaviour
{
  private bool enemyDetected;

  //============================================================

  /// <summary>
  /// True, ���� ���� ���������
  /// </summary>
  public bool EnemyDetected { get; private set; }

  //============================================================

  private void Update()
  {
    if (!enemyDetected)
    {
      EnemyDetected = false;
      return;
    }

    EnemyDetected = true;
  }

  //============================================================

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Enemy"))
    {
      enemyDetected = true;
    }
  }

  private void OnTriggerStay2D(Collider2D other)
  {
    if (other.CompareTag("Enemy"))
    {
      enemyDetected = true;
    }
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    if (other.CompareTag("Enemy"))
    {
      enemyDetected = false;
    }
  }

  //============================================================
}