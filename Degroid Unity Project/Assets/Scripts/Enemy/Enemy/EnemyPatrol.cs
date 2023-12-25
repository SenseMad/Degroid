using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
  [SerializeField, Tooltip("Скорость перемещения врага")]
  private float _moveSpeed;
  [SerializeField, Tooltip("True, есть Enemy смотрит вправо")]
  private bool _moveRight;

  [Header("Проверка стен")]
  [SerializeField, Tooltip("Проверка стены")]
  private Transform _wallCheck;
  [SerializeField, Tooltip("")]
  private float _wallCheckRadius = 0.5f;
  [SerializeField, Tooltip("Слои стен которые будет видеть враг, чтобы развернуться")]
  private LayerMask _whatIsWall;
  [SerializeField]
  private LayerMask _whatIsPlayer;

  [Header("Проверка границы")]
  [SerializeField, Tooltip("Проверка границы")]
  private Transform _edgeCheck;

  [Header("Raycast")]
  [SerializeField, Tooltip("Дальность лучей")]
  private float _rayOffset = 0.05f;

  //------------------------------------------------------------

  private BoxCollider2D boxCollider;

  private Health health;

  private bool hittingWall;
  private bool notAtEdge;

  private Vector3 colliderBottomCenterPosition;
  private Vector3 colliderLeftCenterPosition;
  private Vector3 colliderRightCenterPosition;
  private Vector3 colliderTopCenterPosition;

  //============================================================

  private Rigidbody2D Rigidbody2D { get; set; }

  /// <summary>
  /// Нижнее положение коллайдера
  /// </summary>
  public Vector3 ColliderBottomPosition
  {
    get
    {
      colliderBottomCenterPosition.x = boxCollider.bounds.center.x;
      colliderBottomCenterPosition.y = boxCollider.bounds.min.y;
      colliderBottomCenterPosition.z = 0;

      return colliderBottomCenterPosition;
    }
  }

  /// <summary>
  /// Левое положение коллайдера
  /// </summary>
  public Vector3 ColliderLeftPosition
  {
    get
    {
      colliderLeftCenterPosition.x = boxCollider.bounds.min.x;
      colliderLeftCenterPosition.y = boxCollider.bounds.center.y;
      colliderLeftCenterPosition.z = 0;

      return colliderLeftCenterPosition;
    }
  }

  /// <summary>
  /// Верхнее положение коллайдера
  /// </summary>
  public Vector3 ColliderTopPosition
  {
    get
    {
      colliderTopCenterPosition.x = boxCollider.bounds.center.x;
      colliderTopCenterPosition.y = boxCollider.bounds.max.y;
      colliderTopCenterPosition.z = 0;

      return colliderTopCenterPosition;
    }
  }

  /// <summary>
  /// Правое положение коллайдера
  /// </summary>
  public Vector3 ColliderRightPosition
  {
    get
    {
      colliderRightCenterPosition.x = boxCollider.bounds.max.x;
      colliderRightCenterPosition.y = boxCollider.bounds.center.y;
      colliderRightCenterPosition.z = 0;

      return colliderTopCenterPosition;
    }
  }

  //============================================================

  private void Awake()
  {
    Rigidbody2D = GetComponent<Rigidbody2D>();

    boxCollider = GetComponent<BoxCollider2D>();

    health = GetComponentInChildren<Health>();
  }

  private void Update()
  {
    CheckHitPlayer();
  }

  private void FixedUpdate()
  {
    if (LevelManager.Instance != null)
      if (LevelManager.Instance.IsPause) { return; }

    hittingWall = Physics2D.OverlapCircle(_wallCheck.position, _wallCheckRadius, _whatIsWall);
    notAtEdge = Physics2D.OverlapCircle(_edgeCheck.position,  _wallCheckRadius, _whatIsWall);

    if (hittingWall || !notAtEdge) { _moveRight = !_moveRight; }
    if (_moveRight)
    {
      transform.localScale = new Vector3(1f, 1f, 1f);
      Rigidbody2D.velocity = new Vector2(_moveSpeed, Rigidbody2D.velocity.y);
      return;
    }
    transform.localScale = new Vector3(-1f, 1f, 1f);
    Rigidbody2D.velocity = new Vector2(-_moveSpeed, Rigidbody2D.velocity.y);
  }

  //============================================================

  /// <summary>
  /// Определить попадание игрока
  /// </summary>
  private void CheckHitPlayer()
  {
    RaycastHit2D leftHit = Physics2D.Raycast(ColliderTopPosition - new Vector3(boxCollider.size.x / 2f, 0, 0), Vector3.up, _rayOffset, _whatIsPlayer);

    RaycastHit2D centerHit = Physics2D.Raycast(ColliderTopPosition, Vector3.up, _rayOffset, _whatIsPlayer);

    RaycastHit2D rightHit = Physics2D.Raycast(ColliderTopPosition - new Vector3(-boxCollider.size.x / 2f, 0, 0), Vector3.up, _rayOffset, _whatIsPlayer);

    if (leftHit.collider)
    {
      GetRaycastHit2D(leftHit);
    }
    else if (centerHit.collider)
    {
      GetRaycastHit2D(centerHit);
    }
    else if (rightHit.collider)
    {
      GetRaycastHit2D(rightHit);
    }
  }

  private void GetRaycastHit2D(RaycastHit2D raycastHit2D)
  {
    if (raycastHit2D.collider.TryGetComponent(out KillEnemy killEnemy))
    {
      var playerController = raycastHit2D.collider.GetComponentInParent<PlayerController2>();
      if (!playerController)
        return;

      if (playerController.Rigidbody2D.velocity.y > 0)
        return;

      if (!killEnemy.Death())
        return;

      health.TakeDamage(1);
      gameObject.SetActive(false);
    }
  }

  private void ChectHitLeftPlayer()
  {
    RaycastHit2D leftHit = Physics2D.Raycast(new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.max.y), Vector3.left, 0.05f, _whatIsPlayer);
    Debug.DrawRay(new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.max.y), Vector2.left * 0.05f, Color.red);

    RaycastHit2D centerHit = Physics2D.Raycast(new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.center.y), Vector3.left, 0.05f, _whatIsPlayer);
    Debug.DrawRay(new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.center.y), Vector2.left * 0.05f, Color.red);

    RaycastHit2D rightHit = Physics2D.Raycast(new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.min.y), Vector3.left, 0.05f, _whatIsPlayer);
    Debug.DrawRay(new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.min.y), Vector2.left * 0.05f, Color.red);

    if (leftHit.collider)
    {
      if (leftHit.collider.CompareTag("Player") && leftHit.collider.TryGetComponent(out Health health))
      {
        health.TakeDamage(1);
      }
    }
    else if (centerHit.collider)
    {
      if (centerHit.collider.CompareTag("Player") && centerHit.collider.TryGetComponent(out Health health))
      {
        health.TakeDamage(1);
      }
    }
    else if (rightHit.collider)
    {
      if (rightHit.collider.CompareTag("Player") && rightHit.collider.TryGetComponent(out Health health))
      {
        health.TakeDamage(1);
      }
    }
  }

  private void ChectHitRightPlayer()
  {
    RaycastHit2D leftHit = Physics2D.Raycast(new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.max.y), Vector3.right, 0.05f, _whatIsPlayer);
    Debug.DrawRay(new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.max.y), Vector2.right * 0.05f, Color.blue);

    RaycastHit2D centerHit = Physics2D.Raycast(new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.center.y), Vector3.right, 0.05f, _whatIsPlayer);
    Debug.DrawRay(new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.center.y), Vector2.right * 0.05f, Color.blue);

    RaycastHit2D rightHit = Physics2D.Raycast(new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.min.y), Vector3.right, 0.05f, _whatIsPlayer);
    Debug.DrawRay(new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.min.y), Vector2.right * 0.05f, Color.blue);
  }

  //============================================================

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.transform.tag == "MovingPlatform")
    {
      transform.parent = other.transform;
    }
  }

  private void OnCollisionExit2D(Collision2D other)
  {
    if (other.transform.tag == "MovingPlatform" && gameObject.activeInHierarchy)
    {
      transform.parent = null;
    }
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(_wallCheck.position, _wallCheckRadius);
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(_edgeCheck.position, _wallCheckRadius);
  }

  //============================================================
}