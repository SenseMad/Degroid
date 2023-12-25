using UnityEngine;

/// <summary>
/// Движение врага
/// </summary>
public class SimpleWalkEnemy : MonoBehaviour
{
  [SerializeField, Tooltip("Скорость врага")]
  private float _speedEnemy;

  [SerializeField, Tooltip("Первоначальное направление")]
  private bool _lookingRight;

  [SerializeField, Tooltip("Проверка края")]
  private bool _edgeCheck;

  [SerializeField, Tooltip("Смещение обнаружения отверстия")]
  private Vector3 _holeDetectionOffset = new Vector3(0, 0, 0);

  [SerializeField, Tooltip("Высота луча проверки на отверстия")]
  private float _holeDetectionRaycastLength = 1f;

  //============================================================

  private PlayerController PlayerController { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// Скорость врага
  /// </summary>
  public float SpeedEnemy { get => _speedEnemy; set => _speedEnemy = value; }

  /// <summary>
  /// Первоначальное направление
  /// </summary>
  public bool LookingRight { get => _lookingRight; set => _lookingRight = value; }

  /// <summary>
  /// Проверка края
  /// </summary>
  public bool EdgeCheck { get => _edgeCheck; set => _edgeCheck = value; }

  /// <summary>
  /// Смещение обнаружения отверстия
  /// </summary>
  public Vector3 HoleDetectionOffset { get => _holeDetectionOffset; set => _holeDetectionOffset = value; }

  /// <summary>
  /// Высота луча проверки на отверстия
  /// </summary>
  public float HoleDetectionRaycastLength { get => _holeDetectionRaycastLength; set => _holeDetectionRaycastLength = value; }

  //------------------------------------------------------------

  /// <summary>
  /// Направление
  /// </summary>
  private Vector2 Direction { get; set; }

  //============================================================

  private void Awake()
  {
    PlayerController = GetComponent<PlayerController>();
  }

  private void Start()
  {
    Direction = LookingRight ? Vector2.right : -Vector2.right;
  }

  private void Update()
  {
    PlayerController.SetHorizontalForce(Direction.x * SpeedEnemy);

    CheckForWalls();

    if (EdgeCheck) { CheckForEdge(); }
  }

  //============================================================

  /// <summary>
  /// Рисует отладочный Луч в 2D и делает фактический raycast
  /// </summary>
  /// <param name="rayOriginPoint">Точка начала луча</param>
  /// <param name="rayDirection">Направление луча</param>
  /// <param name="rayDistance">Расстояние луча</param>
  private static RaycastHit2D RayCast(Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance, LayerMask mask, Color color, bool drawGizmo = false)
  {
    if (drawGizmo)
      Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);

    return Physics2D.Raycast(rayOriginPoint, rayDirection, rayDistance, mask);
  }

  /// <summary>
  /// Проверка стен
  /// </summary>
  private void CheckForWalls()
  {
    if ((Direction.x < 0 && PlayerController.PlayerState.IsCollidingLeft) || (Direction.x > 0 && PlayerController.PlayerState.IsCollidingRight))
      ChangeDirection();
  }

  /// <summary>
  /// Проверка на наличие отверстий
  /// </summary>
  private void CheckForEdge()
  {
    Vector2 raycastOrigin = new Vector2(transform.position.x + Direction.x * (HoleDetectionOffset.x + Mathf.Abs(GetComponent<BoxCollider2D>().bounds.size.x) / 2), transform.position.y + HoleDetectionOffset.y - (transform.localScale.y / 2));
    RaycastHit2D raycast = RayCast(raycastOrigin, Vector2.down, HoleDetectionRaycastLength, 1 << LayerMask.NameToLayer("Ground"), Color.yellow, true);

    if (!raycast)
      ChangeDirection();
  }

  /// <summary>
  /// Изменяет направление
  /// </summary>
  private void ChangeDirection()
  {
    Direction = -Direction;
    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
  }

  //============================================================
}