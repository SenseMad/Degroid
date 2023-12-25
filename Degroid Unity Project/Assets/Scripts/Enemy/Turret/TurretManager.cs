using UnityEngine;

public class TurretManager : MonoBehaviour
{
  [SerializeField, Tooltip("���� �������� ������")]
  private Transform _destination;

  [SerializeField, Tooltip("������ ����")]
  private BulletTurret _bulletTurretPrefab;

  [SerializeField, Tooltip("�������� ����")]
  private float _speedBullet;

  [SerializeField, Tooltip("�������� ����� ����������")]
  private float _fireSecond;

  //------------------------------------------------------------

  private float _nextShotInSecond;

  //============================================================

  /// <summary>
  /// ����� ��������
  /// </summary>
  public Transform Destination { get => _destination; set => _destination = value; }

  /// <summary>
  /// ������ ����
  /// </summary>
  public BulletTurret BulletTurretPrefab { get => _bulletTurretPrefab; set => _bulletTurretPrefab = value; }

  /// <summary>
  /// �������� ����
  /// </summary>
  public float SpeedBullet { get => _speedBullet; set => _speedBullet = value; }

  /// <summary>
  /// �������� ����� ����������
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