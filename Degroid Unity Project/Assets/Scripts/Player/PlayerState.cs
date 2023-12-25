/// <summary>
/// ��������� ���������, ������� �� ������ ������������, ����� ���������, ���� ��� �������� ������ ���-�� � ������� �����
/// </summary>
public class PlayerState
{
  /// <summary>
  /// True, ���� �������� ������������ ������
  /// </summary>
  public bool IsCollidingRight { get; set; }

  /// <summary>
  /// True, ���� �������� ������������ �����
  /// </summary>
  public bool IsCollidingLeft { get; set; }

  /// <summary>
  /// True, ���� �������� ������������ ������
  /// </summary>
  public bool IsCollidingAbove { get; set; }

  /// <summary>
  /// True, ���� �������� ������������ �����
  /// </summary>
  public bool IsCollidingBelow { get; set; }
  
  /// <summary>
  /// True, ���� �������� �� �����
  /// </summary>
  public bool IsGrounded { get => IsCollidingBelow; }

  /// <summary>
  /// True, ���� �������� ������ ����� ������
  /// </summary>
  public bool IsFalling { get; set; }

  /// <summary>
  /// True, ���� �������� ��� �������� � ������� �����
  /// </summary>
  public bool WasGroundedLastFrame { get; set; }

  /// <summary>
  /// True, ���� �������� ������� ������� � ������� �����
  /// </summary>
  public bool WasTouchingTheCeilingLastFrame { get; set; }

  /// <summary>
  /// True, ���� �������� ������ ��� ����������
  /// </summary>
  public bool JustGotGrounded { get; set; }

  /// <summary>
  /// True, ���� ������� � ������ ������
  /// </summary>
  public bool IsJumping { get; set; }

  /// <summary>
  /// True, ���� �������� �� ���������
  /// </summary>
  public bool OnAMovingPlatform { get; set; }

  /// <summary>
  /// ����� ���� ��������� ������������
  /// </summary>
  public void Reset()
  {
    IsCollidingLeft = false;
    IsCollidingRight = false;
    IsCollidingAbove = false;
    JustGotGrounded = false;
    IsFalling = true;
  }
}