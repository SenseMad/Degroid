using UnityEngine;

public class TurretManager : MonoBehaviour
{
  [SerializeField, Tooltip("Куда стреляет турель")]
  private Transform _destination;

  [SerializeField, Tooltip("Объект пули")]
  private BulletTurret _bulletTurretPrefab;

  [SerializeField, Tooltip("Скорость пули")]
  private float _speedBullet;

  [SerializeField, Tooltip("Задержка между выстрелами")]
  private float _fireSecond;

  //------------------------------------------------------------

  private float _nextShotInSecond;

  //============================================================

  /// <summary>
  /// Точка выстрела
  /// </summary>
  public Transform Destination { get => _destination; set => _destination = value; }

  /// <summary>
  /// Объект пули
  /// </summary>
  public BulletTurret BulletTurretPrefab { get => _bulletTurretPrefab; set => _bulletTurretPrefab = value; }

  /// <summary>
  /// Скорость пули
  /// </summary>
  public float SpeedBullet { get => _speedBullet; set => _speedBullet = value; }

  /// <summary>
  /// Задержка между выстрелами
  /// </summary>
  public float FireSecond { get => _fireSecond; set => _fireSecond = value; }

  //============================================================

  private void Update()
  {
    if (LevelManager.Instance != null)
      if (LevelManager.Instance.IsPause) { return; }

    if ((_nextShotInSecond -= Time.deltaTime) > 0) { return; }

    _nextShotInSecond = FireSecond;

    var bullet = Instantiate(BulletTurretPrefab, transform.position, transform.rotation);

    bullet.Initialize(Destination, SpeedBullet);
  }

  //============================================================

  private void OnDrawGizmos()
  {
    if (Destination == null) { return; }

    Gizmos.color = Color.red;
    Gizmos.DrawLine(transform.position, Destination.position);
  }

  //============================================================
}