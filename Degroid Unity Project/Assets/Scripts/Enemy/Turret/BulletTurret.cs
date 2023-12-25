using UnityEngine;

public class BulletTurret : MonoBehaviour
{
  /// <summary>
  /// Куда летит пуля
  /// </summary>
  private Transform Destination { get; set; }

  /// <summary>
  /// Скорость пули
  /// </summary>
  private float SpeedBullet { get; set; }

  //============================================================

  private void Update()
  {
    if (LevelManager.Instance != null)
      if (LevelManager.Instance.IsPause) { return; }

    transform.position = Vector3.MoveTowards(transform.position, Destination.position, Time.deltaTime * SpeedBullet);

    var distanceSquared = (Destination.transform.position - transform.position).sqrMagnitude;

    if (distanceSquared > 0.01f * 0.01f) { return; }

    Destroy(gameObject);
  }

  //============================================================

  public void Initialize(Transform destination, float speedBullet)
  {
    Destination = destination;
    SpeedBullet = speedBullet;
  }

  //============================================================
}