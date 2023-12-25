using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Контроллер персанажа, который обрабатывает гравитацию и столкновения персонажа
/// Требует Collider2D и Rigidbody2D, чтобы функционировать
/// </summary>
public class PlayerController : MonoBehaviour
{
  [Header("Параметры")]
  [SerializeField, Tooltip("Исходные параметры")]
  private PlayerParameters _defaultParameters;

  [Header("Слои")]
  [SerializeField, Tooltip("Где может ходить игрок")]
  private LayerMask _groundMask = 0;
  [SerializeField, Tooltip("Слой передвижной платформы")]
  private LayerMask _movingPlatformMask = 0;

  [Header("Raycast")]
  [SerializeField, Tooltip("Количество лучей по горизонтали")]
  private int _numberOfHorizontalRays = 4;
  [SerializeField, Tooltip("Количество лучей по вертикали")]
  private int _numberOfVerticalRays = 4;
  [SerializeField, Tooltip("Дальность лучей")]
  private float _rayOffset = 0.05f;

  //------------------------------------------------------------

  private Vector2 _currentSpeed; // Текущая скорость
  private Vector2 _externalForce; // Внешняя Сила
  private Vector2 _newPosition; // Новая позиция
  private float _currentGravity = 0; // Текущая гравитация
  private bool _gravityActive = true; // Гравитация активна

  private Rect _rayBoundsRectangle; // Параметры лучей

  private const float _largeValue = 500000f;
  private const float _smallValue = 0.0001f;
  private const float _obstacleHeightToLerance = 0.05f;

  //============================================================

  /// <summary>
  /// Различные состояния игрока
  /// </summary>
  public PlayerState PlayerState { get; private set; }

  /// <summary>
  /// Хранение переопределения параметров
  /// </summary>
  private PlayerParameters OverrideParameters { get; set; }
  /// <summary>
  /// Текущие параметры
  /// </summary>
  public PlayerParameters CurrentParameters { get => OverrideParameters ?? DefaultParameters; }

  private Transform Transform { get; set; }
  private BoxCollider2D BoxCollider2D { get; set; }

  //------------------------------------------------------------

  #region Параметры
  /// <summary>
  /// Исходные параметры
  /// </summary>
  public PlayerParameters DefaultParameters { get => _defaultParameters; set => _defaultParameters = value; }
  #endregion

  #region Слои
  /// <summary>
  /// Где может ходить игрок
  /// </summary>
  public LayerMask GroundMask { get => _groundMask; set => _groundMask = value; }
  /// <summary>
  /// Слой передвижной платформы
  /// </summary>
  public LayerMask MovingPlatformMask { get => _movingPlatformMask; set => _movingPlatformMask = value; }
  #endregion

  #region Raycast
  /// <summary>
  /// Количество лучей по горизонтали
  /// </summary>
  public int NumberOfHorizontalRays { get => _numberOfHorizontalRays; set => _numberOfHorizontalRays = value; }
  /// <summary>
  /// Количество лучей по вертикали
  /// </summary>
  public int NumberOfVerticalRays { get => _numberOfVerticalRays; set => _numberOfVerticalRays = value; }
  /// <summary>
  /// Дальность лучей
  /// </summary>
  public float RayOffset { get => _rayOffset; set => _rayOffset = value; }
  #endregion

  /// <summary>
  /// Текущая скорость
  /// </summary>
  public Vector2 CurrentSpeed { get => _currentSpeed; private set => _currentSpeed = value; }

  /// <summary>
  /// Параметры лучей
  /// </summary>
  private Rect RayBoundsRectangle { get => _rayBoundsRectangle; set => _rayBoundsRectangle = value; }

  //------------------------------------------------------------

  /// <summary>
  /// Список Raycast
  /// </summary>
  private List<RaycastHit2D> ContactList { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// Центр коллайдера
  /// </summary>
  private Vector3 ColliderCenter
  {
    get
    {
      Vector3 colliderCenter = Vector3.Scale(transform.localScale, BoxCollider2D.offset);
      return colliderCenter;
    }
  }

  /// <summary>
  /// Положение коллайдера
  /// </summary>
  private Vector3 ColliderPosition
  {
    get
    {
      Vector3 colliderPosition = transform.position + ColliderCenter;
      return colliderPosition;
    }
  }

  /// <summary>
  /// Размер коллайдера
  /// </summary>
  private Vector3 ColliderSize
  {
    get
    {
      Vector3 colliderSize = Vector3.Scale(transform.localScale, BoxCollider2D.size);
      return colliderSize;
    }
  }

  /// <summary>
  /// Нижнее Положение
  /// </summary>
  private Vector3 BottomPosition
  {
    get
    {
      Vector3 colliderBottom = new Vector3(ColliderPosition.x, ColliderPosition.y - (ColliderSize.y / 2), ColliderPosition.z);
      return colliderBottom;
    }
  }

  //============================================================

  private void Awake()
  {
    BoxCollider2D = GetComponentInChildren<BoxCollider2D>();
  }

  private void Start()
  {
    Transform = transform;

    ContactList = new List<RaycastHit2D>();
    PlayerState = new PlayerState();

    GroundMask |= MovingPlatformMask;

    PlayerState.Reset();

  }

  private void Update()
  {
    
  }

  private void FixedUpdate()
  {
    EveryFrame();
  }

  //============================================================

  /// <summary>
  /// Рисует отладочный луч в 2D и делает фактический Raycast
  /// </summary>
  /// <param name="rayOriginPoint">Точка начала луча</param>
  /// <param name="rayDirection">Направление луча</param>
  /// <param name="rayDistance">Расстояние луча</param>
  public static RaycastHit2D RayCast(Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance, LayerMask mask, Color color, bool drawGizmo = false)
  {
    if (drawGizmo) { Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color); }

    return Physics2D.Raycast(rayOriginPoint, rayDirection, rayDistance, mask);
  }

  /// <summary>
  /// Установить силу, приложенную к персонажу
  /// </summary>
  public void SetForce(Vector2 force)
  {
    _currentSpeed = force;
    _externalForce = force;
  }

  /// <summary>
  /// Установить горизонтальную силу, приложенную к персонажу
  /// </summary>
  /// <param name="x">Значение скорости X</param>
  public void SetHorizontalForce(float x)
  {
    _currentSpeed.x = x;
    _externalForce.x = x;
  }

  /// <summary>
  /// Установить вертикальную силу, приложенную к персонажу
  /// </summary>
  /// <param name="y">Значение скорости Y</param>
  public void SetVerticalForce(float y)
  {
    _currentSpeed.y = y;
    _externalForce.y = y;
  }

  /// <summary>
  /// Добавить вертикальную силу, приложенную к персонажу
  /// </summary>
  /// <param name="y">Значение скорости Y</param>
  public void AddVerticalForce(float y)
  {
    _currentSpeed.y += y;
    _externalForce.y += y;
  }

  /// <summary>
  /// Каждый кадр, применяем гравитацию к нашему персонажу, затем проверяем с помощью Raycasts, если объект был поражен, и изменяем его новое положение
  /// Когда все проверки были сделаны, применяем эту новую позицию
  /// </summary>
  private void EveryFrame()
  {
    ContactList.Clear();

    _currentGravity = CurrentParameters.Gravity;

    if (_currentSpeed.y > 0)
      _currentGravity /= CurrentParameters.AscentMultiplier;

    if (_currentSpeed.y < 0)
      _currentGravity *= CurrentParameters.FallMultiplier;

    if (_gravityActive)
      _currentSpeed.y += (_currentGravity) * Time.deltaTime;

    // Инициализируем новую позицию, которую мы будем использовать во всех следующих вычислениях.
    _newPosition = CurrentSpeed * Time.deltaTime;

    PlayerState.WasGroundedLastFrame = PlayerState.IsCollidingBelow;
    PlayerState.WasTouchingTheCeilingLastFrame = PlayerState.IsCollidingAbove;
    PlayerState.Reset();

    // Инициализируем лучи.
    SetRaysParameters();
    // Бросаем лучи со всех сторон, чтобы проверить наклоны и столкновения
    CastRaysToTheSides();
    CastRaysBelow();
    CastRaysAbove();

    SetRaysParameters();

    // Перемещаем наше преобразование в следующую позицию.
    Transform.Translate(_newPosition, Space.World);

    // Вычисляем новую скорость.
    if (Time.deltaTime > 0)
    {
      _currentSpeed = _newPosition / Time.deltaTime;
    }

    // Удостоверяемся, что скорость не превышает максимальную скорость, указанную в параметрах.
    _currentSpeed.x = Mathf.Clamp(_currentSpeed.x, -CurrentParameters.MaxVelocity.x, CurrentParameters.MaxVelocity.x);
    _currentSpeed.y = Mathf.Clamp(_currentSpeed.y, -CurrentParameters.MaxVelocity.y, CurrentParameters.MaxVelocity.y);

    // Меняем состояния в зависимости от исхода движения.
    if (!PlayerState.WasGroundedLastFrame && PlayerState.IsCollidingBelow)
      PlayerState.JustGotGrounded = true;

    /*if (State.IsCollidingLeft || State.IsCollidingRight || State.IsCollidingBelow || State.IsCollidingAbove)
      OnCorgiColliderHit();*/

    _externalForce.x = 0;
    _externalForce.y = 0;
  }

  //============================================================

  /// <summary>
  /// Бросает лучи в стороны от персонажа, от его центральной оси
  /// Если мы попадаем в стену / склон, проверяем ее угол и перемещаемся или нет в соответствии с ним
  /// </summary>
  private void CastRaysToTheSides()
  {
    float movementDirection = 1;

    if ((_currentSpeed.x < 0) || (_externalForce.x < 0))
      movementDirection = -1;

    float horizontalRayLength = Mathf.Abs(_currentSpeed.x * Time.deltaTime) + RayBoundsRectangle.width / 2 + RayOffset * 2;

    Vector2 horizontalRayCastFromBottom = new Vector2(RayBoundsRectangle.center.x, RayBoundsRectangle.yMin + _obstacleHeightToLerance);

    Vector2 horizontalRayCastToTop = new Vector2(RayBoundsRectangle.center.x, RayBoundsRectangle.yMax - _obstacleHeightToLerance);

    RaycastHit2D[] hitsStorage = new RaycastHit2D[NumberOfHorizontalRays];


    for (int i = 0; i < NumberOfHorizontalRays; i++)
    {
      Vector2 rayOriginPoint = Vector2.Lerp(horizontalRayCastFromBottom, horizontalRayCastToTop, (float)i / (NumberOfHorizontalRays - 1));

      if (PlayerState.WasGroundedLastFrame && i == 0)
        hitsStorage[i] = RayCast(rayOriginPoint, movementDirection * (Vector2.right), horizontalRayLength, GroundMask, Color.red, CurrentParameters.DrawRaycastGizmos);
      else
        hitsStorage[i] = RayCast(rayOriginPoint, movementDirection * (Vector2.right), horizontalRayLength, GroundMask, Color.red, CurrentParameters.DrawRaycastGizmos);

      if (hitsStorage[i].distance > 0)
      {
        float hitAngle = Mathf.Abs(Vector2.Angle(hitsStorage[i].normal, Vector2.up));

        if (hitAngle > CurrentParameters.MaximumSlopeAngle) // Перед нами препядствие
        {
          if (movementDirection < 0)
            PlayerState.IsCollidingLeft = true;
          else
            PlayerState.IsCollidingRight = true;

          if (movementDirection < 0)
          {
            _newPosition.x = -Mathf.Abs(hitsStorage[i].point.x - horizontalRayCastFromBottom.x) + RayBoundsRectangle.width / 2 + RayOffset * 2;
          }
          else
          {
            _newPosition.x = Mathf.Abs(hitsStorage[i].point.x - horizontalRayCastFromBottom.x) - RayBoundsRectangle.width / 2 - RayOffset * 2;
          }

          ContactList.Add(hitsStorage[i]);
          _currentSpeed = new Vector2(0, _currentSpeed.y);
          break;
        }
      }
    }
  }

  /// <summary>
  /// Каждый кадр бросаем несколько лучей ниже нашего персонажа, чтобы проверить наличие столкновений с платформой
  /// </summary>
  private void CastRaysBelow()
  {
    if (_newPosition.y < -_smallValue)
      PlayerState.IsFalling = true;
    else
      PlayerState.IsFalling = false;

    if ((CurrentParameters.Gravity > 0) && (!PlayerState.IsFalling)) { return; }

    float rayLength = RayBoundsRectangle.height / 2 + RayOffset;

    if (PlayerState.OnAMovingPlatform)
    {
      rayLength *= 2;
    }

    if (_newPosition.y < 0)
      rayLength += Mathf.Abs(_newPosition.y);

    Vector2 verticalRayCastFromLeft = new Vector2(RayBoundsRectangle.xMin + _newPosition.x, RayBoundsRectangle.center.y + RayOffset);

    Vector2 verticalRayCastToRight = new Vector2(RayBoundsRectangle.xMax + _newPosition.x, RayBoundsRectangle.center.y + RayOffset);

    RaycastHit2D[] hitsStorage = new RaycastHit2D[NumberOfVerticalRays];

    float smallestDistance = _largeValue;

    int smallestDistanceIndex = 0;

    bool hitConnected = false;

    for (int i = 0; i < NumberOfVerticalRays; i++)
    {
      Vector2 rayOriginPoint = Vector2.Lerp(verticalRayCastFromLeft, verticalRayCastToRight, (float)i / (NumberOfVerticalRays - 1));

      if ((_newPosition.y > 0) && (!PlayerState.WasGroundedLastFrame))
        hitsStorage[i] = RayCast(rayOriginPoint, -Vector2.up, rayLength, GroundMask, Color.blue, CurrentParameters.DrawRaycastGizmos);
      else
        hitsStorage[i] = RayCast(rayOriginPoint, -Vector2.up, rayLength, GroundMask, Color.blue, CurrentParameters.DrawRaycastGizmos);

      if (Mathf.Abs(hitsStorage[smallestDistanceIndex].point.y - verticalRayCastFromLeft.y) < _smallValue)
        break;

      if (hitsStorage[i])
      {
        hitConnected = true;

        if (hitsStorage[i].distance < smallestDistance)
        {
          smallestDistanceIndex = i;
          smallestDistance = hitsStorage[i].distance;
        }
      }
    }
    if (hitConnected)
    {
      // Если персонаж прыгает на (1-полосную) платформу, но недостаточно высоко, мы ничего не делаем
      if (!PlayerState.WasGroundedLastFrame && (smallestDistance < RayBoundsRectangle.size.y / 2))
      {
        PlayerState.IsCollidingBelow = false;
        return;
      }

      PlayerState.IsFalling = false;
      PlayerState.IsCollidingBelow = true;

      // Если мы применяем внешнюю силу (прыжки...) мы только применяем это.
      if (_externalForce.y > 0)
      {
        _newPosition.y = _currentSpeed.y * Time.deltaTime;
        PlayerState.IsCollidingBelow = false;
      }
      // Если нет, мы просто скорректируем позицию на основе Hit raycast.
      else
      {
        _newPosition.y = -Mathf.Abs(hitsStorage[smallestDistanceIndex].point.y - verticalRayCastFromLeft.y) + RayBoundsRectangle.height / 2 + RayOffset;
      }

      if (!PlayerState.WasGroundedLastFrame && _currentSpeed.y > 0)
      {
        _newPosition.y += _currentSpeed.y * Time.deltaTime;
      }

      if (Mathf.Abs(_newPosition.y) < _smallValue)
        _newPosition.y = 0;
    }
    else
    {
      PlayerState.IsCollidingBelow = false;
    }
  }

  /// <summary>
  /// Если мы находимся в воздухе и движемся вверх, мы бросаем лучи над головой персонажа, чтобы проверить столкновения.
  /// </summary>
  private void CastRaysAbove()
  {
    if (_newPosition.y < 0) { return; }

    float rayLength = PlayerState.IsGrounded ? RayOffset : _newPosition.y;
    rayLength += RayBoundsRectangle.height / 2;

    bool hitConnected = false;

    Vector2 verticalRayCastStart = new Vector2(RayBoundsRectangle.xMin + _newPosition.x, RayBoundsRectangle.center.y + RayOffset);

    Vector2 verticalRayCastEnd = new Vector2(RayBoundsRectangle.xMax + _newPosition.x, RayBoundsRectangle.center.y + RayOffset);

    RaycastHit2D[] hitsStorage = new RaycastHit2D[NumberOfVerticalRays];

    float smallestDistance = _largeValue;

    for (int i = 0; i < NumberOfVerticalRays; i++)
    {
      Vector2 rayOriginPoint = Vector2.Lerp(verticalRayCastStart, verticalRayCastEnd, (float)i / (NumberOfVerticalRays - 1));

      hitsStorage[i] = RayCast(rayOriginPoint, Vector2.up, rayLength, GroundMask, Color.green, CurrentParameters.DrawRaycastGizmos);

      if (hitsStorage[i])
      {
        hitConnected = true;
        if (hitsStorage[i].distance < smallestDistance)
        {
          smallestDistance = hitsStorage[i].distance;
        }
      }
    }

    if (hitConnected)
    {
      _currentSpeed.y = 0;
      _newPosition.y = smallestDistance - RayBoundsRectangle.height / 2;

      if (PlayerState.IsGrounded && (_newPosition.y <= 0))
      {
        _newPosition.y = 0;
      }

      PlayerState.IsCollidingAbove = true;

      if (!PlayerState.WasTouchingTheCeilingLastFrame)
      {
        _newPosition.x = 0;
        _currentSpeed = new Vector2(0, _currentSpeed.y);
      }
    }
  }

  /// <summary>
  /// Создает прямоугольник с размером BoxCollider2D для удобства использования и рисует отладочные линии вдоль различных исходных осей Raycast
  /// </summary>
  public void SetRaysParameters()
  {
    RayBoundsRectangle = new Rect(BoxCollider2D.bounds.min.x, BoxCollider2D.bounds.min.y, BoxCollider2D.bounds.size.x, BoxCollider2D.bounds.size.y);

    Debug.DrawLine(new Vector2(RayBoundsRectangle.center.x, RayBoundsRectangle.yMin), new Vector2(RayBoundsRectangle.center.x, RayBoundsRectangle.yMax), Color.yellow);
    Debug.DrawLine(new Vector2(RayBoundsRectangle.xMin, RayBoundsRectangle.center.y), new Vector2(RayBoundsRectangle.xMax, RayBoundsRectangle.center.y), Color.yellow);
  }

  /// <summary>
  /// Включаем или отключаем гравитацию для персонажа
  /// </summary>
  /// <param name="parState">Если true, гравитация включается</param>
  public void GravityActive(bool parState)
  {
    if (parState)
      _gravityActive = true;
    else
      _gravityActive = false;
  }

  /// <summary>
  /// Срабатывает, когда Raycasts персонажа сталкиваются с чем-то
  /// </summary>
  private void OnCorgiColliderHit()
  {
    foreach (RaycastHit2D hit in ContactList)
    {
      Rigidbody2D body = hit.collider.attachedRigidbody;
      if (body == null || body.isKinematic)
        return;

      Vector3 pushDir = new Vector3(_externalForce.x, 0, 0);

      body.velocity = pushDir.normalized * CurrentParameters.Physics2DPushForce;
    }
  }

  //============================================================
}