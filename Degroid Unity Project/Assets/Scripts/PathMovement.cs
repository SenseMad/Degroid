using UnityEngine;
using System.Collections.Generic;
using Unity.Collections;

public class PathMovement : MonoBehaviour
{
  [SerializeField, Tooltip("Зацикливание пути")]
  private bool _loopPath = true;

  [SerializeField, Tooltip("Точки пути")]
  private List<Vector3> _pointPath;

  [SerializeField, Tooltip("Начальное направление движения")]
  private MovementDirection _initialMovementDirection = MovementDirection.Ascending;

  [SerializeField, Tooltip("Тип движения")]
  private PossibleMovementType _movementType = PossibleMovementType.ConstantSpeed;

  [SerializeField, Tooltip("Скорость движения")]
  private float _speed = 1;

  [SerializeField, Tooltip("Ожидание для каждой точки")]
  private List<float> _delays;

  [SerializeField, Tooltip("Минимальное расстояние до точки")]
  private float _minDistanceToGoal = 0.01f;

  [SerializeField, Tooltip("Исходное положение")]
  private Vector3 _originalTransformPosition;

  [SerializeField, Tooltip("Внутренний флаг")]
  private bool _originalTransformPositionSet = false;

  [SerializeField, Tooltip("True, если находимся в меню")]
  private bool _isMenu = false;

  //------------------------------------------------------------

  private bool _active = false; // Активный
  private IEnumerator<Vector3> _currentPoint; // Текущая точка
  private int Direction = 1; // Направление
  private Vector3 _initialPosition; // Исходное положение
  private Vector3 _finalPosition; // Конечное положение
  private float _waiting = 0; // Ожидание
  private int _currentIndex; // Текущий индекс

  //============================================================

  /// <summary>
  /// Зацикливание пути
  /// </summary>
  public bool LoopPath { get => _loopPath; set => _loopPath = value; }

  /// <summary>
  /// Точки пути
  /// </summary>
  public List<Vector3> PointPath { get => _pointPath; set => _pointPath = value; }

  /// <summary>
  /// Начальное направление движения
  /// </summary>
  public MovementDirection InitialMovementDirection { get => _initialMovementDirection; set => _initialMovementDirection = value; }

  /// <summary>
  /// Тип движения
  /// </summary>
  public PossibleMovementType MovementType { get => _movementType; set => _movementType = value; }

  /// <summary>
  /// Скорость движения
  /// </summary>
  public float Speed { get => _speed; set => _speed = value; }

  /// <summary>
  /// Ожидание для каждой точки
  /// </summary>
  public List<float> Delays { get => _delays; set => _delays = value; }

  /// <summary>
  /// Минимальное расстояние до точки
  /// </summary>
  public float MinDistanceToGoal { get => _minDistanceToGoal; set => _minDistanceToGoal = value; }

  /// <summary>
  /// Внутренний флаг
  /// </summary>
  public bool OriginalTransformPositionSet { get => _originalTransformPositionSet; set => _originalTransformPositionSet = value; }

  //------------------------------------------------------------

  /// <summary>
  /// Текущая скорость
  /// </summary>
  public Vector3 CurrentSpeed { get; private set; }

  //============================================================

  private void Awake()
  {
    _originalTransformPosition = transform.position;
  }

  private void Start()
  {
    _active = true;

    // Если путь равен нулю, мы выходим
    if (PointPath == null || PointPath.Count < 1) { return; }

    // Устанавливаем наше начальное направление на основе настроек
    if (InitialMovementDirection == MovementDirection.Ascending)
      Direction = 1;
    else
      Direction = -1;

    // Инициализируем наш перечислитель путей
    _currentPoint = GetPathEnumerator();
    _currentPoint.MoveNext();

    // Начальное позиционирование
    if (!OriginalTransformPositionSet)
    {
      OriginalTransformPositionSet = true;
      _originalTransformPosition = transform.position;
    }
    transform.position = _originalTransformPosition + _currentPoint.Current;
  }

  private void Update()
  {
    if (!_isMenu)
    {
      if (LevelManager.Instance == null) { return; }
      if (LevelManager.Instance.IsPause) { return; }
    }

    // Если путь равен нулю, выходим
    if (PointPath == null || PointPath.Count < 1) { return; }

    // Ждем, пока сможем продолжить
    _waiting -= Time.deltaTime;
    if (_waiting > 0)
    {
      CurrentSpeed = Vector3.zero;
      return;
    }

    // Сохраняем наше начальное положение, чтобы вычислить текущую скорость в конце Update
    _initialPosition = transform.position;

    // Перемещаем наш объект
    MoveAlongThePath();

    // Решаем, достигли ли мы следующего пункта назначения или нет, если да, мы перемещаем наш пункт назначения в следующую точку
    float distanceSquared = (transform.position - (_originalTransformPosition + _currentPoint.Current)).sqrMagnitude;
    if (distanceSquared < MinDistanceToGoal * MinDistanceToGoal)
    {
      // Проверяем, нужно ли нам ожидать
      if (Delays.Count > _currentIndex)
      {
        _waiting = Delays[_currentIndex];
      }

      _currentPoint.MoveNext();
    }

    // Определим текущую скорость
    _finalPosition = transform.position;
    CurrentSpeed = (_finalPosition - _initialPosition) / Time.deltaTime;
  }

  //============================================================

  private static void DrawGizmoPoint(Vector3 position, float size, Color color)
  {
    Gizmos.color = color;
    Gizmos.DrawWireSphere(position, size);
  }

  /// <summary>
  /// Перемещает объект по пути в соответствии с заданным типом перемещения
  /// </summary>
  public void MoveAlongThePath()
  {
    if (MovementType == PossibleMovementType.ConstantSpeed)
    {
      transform.position = Vector3.MoveTowards(transform.position, _originalTransformPosition + _currentPoint.Current, Time.deltaTime * Speed);
    }
    else if (MovementType == PossibleMovementType.EaseOut)
    {
      transform.position = Vector3.Lerp(transform.position, _originalTransformPosition + _currentPoint.Current, Time.deltaTime * Speed);
    }
  }

  /// <summary>
  /// Возвращает текущую целевую точку в пути
  /// </summary>
  /// <returns>Перечислитель путей</returns>
  public IEnumerator<Vector3> GetPathEnumerator()
  {
    if (PointPath == null || PointPath.Count < 1) { yield break; }

    int index = 0;
    _currentIndex = index;
    while (true)
    {
      _currentIndex = index;
      yield return PointPath[index];

      if (PointPath.Count <= 1) { continue; }

      if (LoopPath)
      {
        index += Direction;

        if (index < 0)
        {
          index = PointPath.Count - 1;
        }
        else if (index > PointPath.Count - 1)
        {
          index = 0;
        }
      }
      else
      {
        if (index <= 0)
        {
          Direction = 1;
        }
        else if (index >= PointPath.Count - 1)
        {
          Direction = -1;
        }

        index += Direction;
      }
    }
  }

  //============================================================

  private void OnDrawGizmos()
  {
#if UNITY_EDITOR
    if (PointPath == null || PointPath.Count == 0) { return; }

    // Если мы еще не сохранили исходное положение объекта, мы делаем это
    if (OriginalTransformPositionSet == false)
    {
      _originalTransformPosition = transform.position;
      OriginalTransformPositionSet = true;
    }
    // Если мы не находимся в режиме выполнения и преобразование изменилось, мы обновляем нашу позицию
    if (transform.hasChanged && _active == false)
    {
      _originalTransformPosition = transform.position;
    }
    // Для каждой точки На пути
    for (int i = 0; i < PointPath.Count; i++)
    {
      // Рисуем зеленую точку
      DrawGizmoPoint(_originalTransformPosition + PointPath[i], 0.2f, Color.green);

      // Проводим линию к следующей точке пути
      if ((i + 1) < PointPath.Count)
      {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(_originalTransformPosition + PointPath[i], _originalTransformPosition + PointPath[i + 1]);
      }
      // Проводим линию от первой до последней точки, если мы зацикливаемся
      if ((i == PointPath.Count - 1) && LoopPath)
      {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(_originalTransformPosition + PointPath[0], _originalTransformPosition + PointPath[i]);
      }
    }
#endif
  }

  //============================================================

  /// <summary>
  /// Типы движения
  /// </summary>
  public enum PossibleMovementType
  {
    ConstantSpeed,
    EaseOut
  }

  /// <summary>
  /// Направления движения
  /// </summary>
  public enum MovementDirection
  {
    Ascending,
    Descending
  }

  //============================================================
}