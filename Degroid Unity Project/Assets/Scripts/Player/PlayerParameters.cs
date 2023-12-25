using System;
using UnityEngine;

/// <summary>
/// ��������� ��� ������ PlayerController
/// ����� ���������� ������������ ��������, ������ ������� � ����������
/// </summary>
[Serializable]
public class PlayerParameters
{
  [Header("��������")]
  [Tooltip("������������ ��������")]
  public Vector2 MaxVelocity = new Vector2(100f, 100f);
  [Tooltip("������������ ���� �� �������� ����� ������"), Range(0, 90)]
  public float MaximumSlopeAngle = 45;

  [Space(10), Header("����������")]
  [Tooltip("����������")]
  public float Gravity = -30;
  [Tooltip("���������, ����������� � ���������� ��������� ��� ������")]
  public float FallMultiplier = 1.5f;
  [Tooltip("���������, ����������� � ���� ������� ��������� ��� �������� �����")]
  public float AscentMultiplier = 1f;

  [Space(10), Header("Physics2D")]
  [Tooltip("����, ����������� � ��������, � �������� ������������ ��������")]
  public float Physics2DPushForce = 2.0f;

  [Space(10), Header("�����������")]
  [Tooltip("True, ���� ���������� ����")]
  public bool DrawRaycastGizmos = true;
}