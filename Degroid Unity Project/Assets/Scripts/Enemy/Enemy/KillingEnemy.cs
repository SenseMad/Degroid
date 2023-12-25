using UnityEngine;

/// <summary>
/// Добавить этот класс на врага, чтобы иметь возможность его убивать
/// </summary>
public class KillingEnemy : MonoBehaviour
{
  [SerializeField, Tooltip("Количество лучей")]
  private int _numberOfRays = 5;

  [SerializeField, Tooltip("На сколько подбросит")]
  private Vector2 _knockbackForce = new Vector2(0f, 15f);

  [SerializeField, Tooltip("Кто может наносить урон")]
  private LayerMask _playerMask;

  [SerializeField, Tooltip("Количество урона")]
  private int _damagePerStomp;

  [SerializeField, Tooltip("Сбрасывать прыжки при убийстве врага?")]
  private bool resetNumberOfJumpsOnStomp = false;

  //============================================================

  private BoxCollider2D BoxCollider2D { get; set; }

  private Health Health { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// Количество лучей
  /// </summary>
  public int NumberOfRays { get => _numberOfRays; set => _numberOfRays = value; }

  /// <summary>
  /// На сколько подбросит
  /// </summary>
  public Vector2 KnockbackForce { get => _knockbackForce; set => _knockbackForce = value; }

  /// <summary>
  /// Кто может наносить урон
  /// </summary>
  public LayerMask PlayerMask { get => _playerMask; set => _playerMask = value; }

  /// <summary>
  /// Количество урона
  /// </summary>
  public int DamagePerStomp { get => _damagePerStomp; set => _damagePerStomp = value; }

  /// <summary>
  /// Сбрасывать прыжки при убийстве врага?
  /// </summary>
  public bool ResetNumberOfJumpsOnStomp { get => resetNumberOfJumpsOnStomp; set => resetNumberOfJumpsOnStomp = value; }

  //------------------------------------------------------------

  private Vector2 VerticalRayCastStart;
  private Vector2 VerticalRayCastEnd;
  private RaycastHit2D[] RaycastHit2D { get; set; }

  //============================================================

  private void Awake()
  {
    BoxCollider2D = GetComponent<BoxCollider2D>();
    Health = GetComponent<Health>();
  }

  private void OnEnable()
  {
    Health.OnDeath.AddListener(OnDeath);
  }

  private void OnDisable()
  {
    Health.OnDeath.RemoveAllListeners();
  }

  private void Start()
  {
    RaycastHit2D = new RaycastHit2D[NumberOfRays];
  }

  private void Update()
  {
    CastRaysAbove();
  }

  //============================================================

  /// <summary>
  /// Рисует отладочный Луч в 2D и делает фактический raycast
  /// </summary>
  /// <param name="rayOriginPoint">Точка начала луча</param>
  /// <param name="rayDirection">Направление луча</param>
  /// <param name="rayDistance">Расстояние луча</param>
  public static RaycastHit2D RayCast(Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance, LayerMask mask, Color color, bool drawGizmo = false)
  {
    if (drawGizmo)
      Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);

    return Physics2D.Raycast(rayOriginPoint, rayDirection, rayDistance, mask);
  }

  /// <summary>
  /// Бросает лучи выше врага
  /// </summary>
  private void CastRaysAbove()
  {
    float rayLength = 0.5f;

    bool hitConnected = false;
    int hitConnectedIndex = 0;

    VerticalRayCastStart.x = BoxCollider2D.bounds.min.x;
    VerticalRayCastStart.y = BoxCollider2D.bounds.max.y;
    VerticalRayCastEnd.x = BoxCollider2D.bounds.max.x;
    VerticalRayCastEnd.y = BoxCollider2D.bounds.max.y;

    for (int i = 0; i < NumberOfRays; i++)
    {
      Vector2 rayOriginPoint = Vector2.Lerp(VerticalRayCastStart, VerticalRayCastEnd, (float)i / (float)(NumberOfRays - 1));
      RaycastHit2D[i] = RayCast(rayOriginPoint, Vector2.up, rayLength, PlayerMask, Color.black, true);

      if (RaycastHit2D[i])
      {
        hitConnected = true;
        hitConnectedIndex = i;
        break;
      }
    }

    if (hitConnected)
    {
      if (BoxCollider2D.bounds.max.y > RaycastHit2D[hitConnectedIndex].collider.bounds.min.y) { return; }

      PlayerController controller = RaycastHit2D[hitConnectedIndex].collider.gameObject.GetComponentNoAlloc<PlayerController>();

      if (controller != null)
      {
        if (controller.CurrentSpeed.y >= 0) { return; }

        PerformStomp(controller);
      }
    }
  }

  private void PerformStomp(PlayerController controller)
  {
    controller.SetForce(KnockbackForce);

    if (Health != null)
    {
      Health.TakeDamage(DamagePerStomp);

      Debug.Log("!");
      if (Health.CurrentHealth <= 0)
      {
        var playerManager = controller.GetComponent<PlayerManager2>();
        playerManager.LevelManager.OnKillingEnemy?.Invoke();
      }
    }

    PlayerJump playerJump = controller.gameObject.GetComponentNoAlloc<PlayerJump>();

    if (playerJump != null)
    {
      playerJump.ResetJumpButtonReleased();

      if (ResetNumberOfJumpsOnStomp)
        playerJump.ResetNumberOfJumps();
    }
  }

  /// <summary>
  /// Вызвать событие смерти
  /// </summary>
  private void OnDeath()
  {
    gameObject.SetActive(false);

    Destroy(gameObject, 1f);
  }

  //============================================================
}