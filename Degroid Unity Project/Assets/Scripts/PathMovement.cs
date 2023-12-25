using UnityEngine;
using System.Collections.Generic;
using Unity.Collections;

public class PathMovement : MonoBehaviour
{
  [SerializeField, Tooltip("������������ ����")]
  private bool _loopPath = true;

  [SerializeField, Tooltip("����� ����")]
  private List<Vector3> _pointPath;

  [SerializeField, Tooltip("��������� ����������� ��������")]
  private MovementDirection _initialMovementDirection = MovementDirection.Ascending;

  [SerializeField, Tooltip("��� ��������")]
  private PossibleMovementType _movementType = PossibleMovementType.ConstantSpeed;

  [SerializeField, Tooltip("�������� ��������")]
  private float _speed = 1;

  [SerializeField, Tooltip("�������� ��� ������ �����")]
  private List<float> _delays;

  [SerializeField, Tooltip("����������� ���������� �� �����")]
  private float _minDistanceToGoal = 0.01f;

  [SerializeField, Tooltip("�������� ���������")]
  private Vector3 _originalTransformPosition;

  [SerializeField, Tooltip("���������� ����")]
  private bool _originalTransformPositionSet = false;

  [SerializeField, Tooltip("True, ���� ��������� � ����")]
  private bool _isMenu = false;

  //------------------------------------------------------------

  private bool _active = false; // ��������
  private IEnumerator<Vector3> _currentPoint; // ������� �����
  private int Direction = 1; // �����������
  private Vector3 _initialPosition; // �������� ���������
  private Vector3 _finalPosition; // �������� ���������
  private float _waiting = 0; // ��������
  private int _currentIndex; // ������� ������

  //============================================================

  /// <summary>
  /// ������������ ����
  /// </summary>
  public bool LoopPath { get => _loopPath; set => _loopPath = value; }

  /// <summary>
  /// ����� ����
  /// </summary>
  public List<Vector3> PointPath { get => _pointPath; set => _pointPath = value; }

  /// <summary>
  /// ��������� ����������� ��������
  /// </summary>
  public MovementDirection InitialMovementDirection { get => _initialMovementDirection; set => _initialMovementDirection = value; }

  /// <summary>
  /// ��� ��������
  /// </summary>
  public PossibleMovementType MovementType { get => _movementType; set => _movementType = value; }

  /// <summary>
  /// �������� ��������
  /// </summary>
  public float Speed { get => _speed; set => _speed = value; }

  /// <summary>
  /// �������� ��� ������ �����
  /// </summary>
  public List<float> Delays { get => _delays; set => _delays = value; }

  /// <summary>
  /// ����������� ���������� �� �����
  /// </summary>
  public float MinDistanceToGoal { get => _minDistanceToGoal; set => _minDistanceToGoal = value; }

  /// <summary>
  /// ���������� ����
  /// </summary>
  public bool OriginalTransformPositionSet { get => _originalTransformPositionSet; set => _originalTransformPositionSet = value; }

  //------------------------------------------------------------

  /// <summary>
  /// ������� ��������
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

    // ���� ���� ����� ����, �� �������
    if (PointPath == null || PointPath.Count < 1) { return; }

    // ������������� ���� ��������� ����������� �� ������ ��������
    if (InitialMovementDirection == MovementDirection.Ascending)
      Direction = 1;
    else
      Direction = -1;

    // �������������� ��� ������������� �����
    _currentPoint = GetPathEnumerator();
    _currentPoint.MoveNext();

    // ��������� ����������������
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

    // ���� ���� ����� ����, �������
    if (PointPath == null || PointPath.Count < 1) { return; }

    // ����, ���� ������ ����������
    _waiting -= Time.deltaTime;
    if (_waiting > 0)
    {
      CurrentSpeed = Vector3.zero;
      return;
    }

    // ��������� ���� ��������� ���������, ����� ��������� ������� �������� � ����� Update
    _initialPosition = transform.position;

    // ���������� ��� ������
    MoveAlongThePath();

    // ������, �������� �� �� ���������� ������ ���������� ��� ���, ���� ��, �� ���������� ��� ����� ���������� � ��������� �����
    float distanceSquared = (transform.position - (_originalTransformPosition + _currentPoint.Current)).sqrMagnitude;
    if (distanceSquared < MinDistanceToGoal * MinDistanceToGoal)
    {
      // ���������, ����� �� ��� �������
      if (Delays.Count > _currentIndex)
      {
        _waiting = Delays[_currentIndex];
      }

      _currentPoint.MoveNext();
    }

    // ��������� ������� ��������
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
  /// ���������� ������ �� ���� � ������������ � �������� ����� �����������
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
  /// ���������� ������� ������� ����� � ����
  /// </summary>
  /// <returns>������������� �����</returns>
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

    // ���� �� ��� �� ��������� �������� ��������� �������, �� ������ ���
    if (OriginalTransformPositionSet == false)
    {
      _originalTransformPosition = transform.position;
      OriginalTransformPositionSet = true;
    }
    // ���� �� �� ��������� � ������ ���������� � �������������� ����������, �� ��������� ���� �������
    if (transform.hasChanged && _active == false)
    {
      _originalTransformPosition = transform.position;
    }
    // ��� ������ ����� �� ����
    for (int i = 0; i < PointPath.Count; i++)
    {
      // ������ ������� �����
      DrawGizmoPoint(_originalTransformPosition + PointPath[i], 0.2f, Color.green);

      // �������� ����� � ��������� ����� ����
      if ((i + 1) < PointPath.Count)
      {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(_originalTransformPosition + PointPath[i], _originalTransformPosition + PointPath[i + 1]);
      }
      // �������� ����� �� ������ �� ��������� �����, ���� �� �������������
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
  /// ���� ��������
  /// </summary>
  public enum PossibleMovementType
  {
    ConstantSpeed,
    EaseOut
  }

  /// <summary>
  /// ����������� ��������
  /// </summary>
  public enum MovementDirection
  {
    Ascending,
    Descending
  }

  //============================================================
}