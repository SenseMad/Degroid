using UnityEngine;

public class PlayerJump : MonoBehaviour
{
  [Header("������")]
  [SerializeField, Tooltip("������ ������")]
  private float _jumpHeight = 3f;
  [SerializeField, Tooltip("���������� �������")]
  private int _numberJumps = 2;

  [Header("������������ ������")]
  [SerializeField, Tooltip("������������ ������� ������")]
  private bool _durationPressTime = true;
  [SerializeField, Tooltip("����� ���������� � �������")]
  private float _minimumAirTime = 0f;
  [SerializeField, Tooltip("�� ������� ������ ������ ������")]
  private float _jumpReleaseForceFactor = 2f;

  private float _nextJumpInSecond; // ��������� ������ ����� ...

  //============================================================

  private PlayerManager PlayerManager { get; set; }
  private PlayerController PlayerController { get; set; }
  private PlayerInputHandler PlayerInputHandler { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// ������ ������
  /// </summary>
  public float JumpHeight { get => _jumpHeight; set => _jumpHeight = value; }

  /// <summary>
  /// ���������� �������
  /// </summary>
  public int NumberJumps { get => _numberJumps; set => _numberJumps = value; }

  /// <summary>
  /// ������������ ������� ������
  /// </summary>
  public bool DurationPressTime { get => _durationPressTime; set => _durationPressTime = value; }

  /// <summary>
  /// ����� ���������� � �������
  /// </summary>
  public float MinimumAirTime { get => _minimumAirTime; set => _minimumAirTime = value; }

  /// <summary>
  /// �� ������� ������ ������ ������
  /// </summary>
  public float JumpReleaseForceFactor { get => _jumpReleaseForceFactor; set => _jumpReleaseForceFactor = value; }

  //------------------------------------------------------------

  /// <summary>
  /// ������� ���������� �������
  /// </summary>
  private int CurrentNumberJumps { get; set; } = 0;

  /// <summary>
  /// ����� ������� ������ ������
  /// </summary>
  private float JumpButtonPressTime { get; set; } = 0;

  /// <summary>
  /// True, ���� ������ ������ ����������
  /// </summary>
  private bool JumpButtonPressed { get; set; } = false;

  /// <summary>
  /// True, ���� ������ ������ ��������
  /// </summary>
  private bool JumpButtonReleased { get; set; } = false;

  /// <summary>
  /// True, ���� ����� �������
  /// </summary>
  private bool CanJump { get; set; } = true;

  //============================================================

  private void Awake()
  {
    PlayerManager = GetComponent<PlayerManager>();
    PlayerController = GetComponent<PlayerController>();
    PlayerInputHandler = GetComponent<PlayerInputHandler>();
  }

  private void Start()
  {
    ResetNumberOfJumps();
  }

  private void Update()
  {
    EveryFrame();

    /*if (CanJump)
    {
      if (PlayerInputHandler.GetJumpInputDown()) { JumpStart(); }
    }

    if (PlayerInputHandler.GetJumpInputUp()) { JumpStop(); }*/

    /*if (!PlayerController.PlayerState.IsGrounded)
    {
      if (PlayerManager.Animator.runtimeAnimatorController != null)
        PlayerManager.State = CharState.Player_Jump;
    }*/
  }

  //============================================================

  /// <summary>
  /// ������ ��� ������ ����
  /// </summary>
  private void EveryFrame()
  {
    if (PlayerController.PlayerState.JustGotGrounded)
    {
      CurrentNumberJumps = NumberJumps;
      PlayerController.PlayerState.IsJumping = false;
      CanJump = true;
    }

    // ���� ������������ ��������� ������ ������, � �������� ������� ����� � ���������� ������� ������ � ������� ��������������� ������, �� �� ���������� �������, ��������� ���� ����
    if ((JumpButtonPressTime != 0) && (Time.time - JumpButtonPressTime >= MinimumAirTime) && (PlayerController.CurrentSpeed.y > Mathf.Sqrt(Mathf.Abs(PlayerController.CurrentParameters.Gravity)))
    && JumpButtonReleased && (!JumpButtonPressed) && PlayerController.PlayerState.IsJumping && CanJump)
    {
      JumpButtonReleased = false;
      if (DurationPressTime)
      {
        JumpButtonPressTime = 0;
        if (JumpReleaseForceFactor == 0)
          PlayerController.SetVerticalForce(0f);
        else
        {
          if (CurrentNumberJumps <= 0)
          {
            CanJump = false;
          }
          PlayerController.AddVerticalForce(-PlayerController.CurrentSpeed.y / JumpReleaseForceFactor);
        }
      }
    }
  }

  /// <summary>
  /// ������ ������
  /// </summary>
  public void JumpStart()
  {
    if (CurrentNumberJumps <= 0) { return; }

    if (CurrentNumberJumps - 1 < 0) { return; }

    CurrentNumberJumps -= 1;
    PlayerController.PlayerState.IsJumping = true;

    SetJumpFlags();

    PlayerController.SetVerticalForce(Mathf.Sqrt(2f * JumpHeight * Mathf.Abs(PlayerController.CurrentParameters.Gravity)));
  }

  /// <summary>
  /// ����������� ������
  /// </summary>
  public void JumpStop()
  {
    JumpButtonPressed = false;
    JumpButtonReleased = true;
  }

  /// <summary>
  /// ����� ������ ��������
  /// </summary>
  public void SetJumpFlags()
  {
    JumpButtonPressTime = Time.time;
    JumpButtonPressed = true;
    JumpButtonReleased = false;
  }

  /// <summary>
  /// ���������� ���� ���������� ������ ��������
  /// </summary>
  public void ResetJumpButtonReleased()
  {
    JumpButtonReleased = false;
  }

  /// <summary>
  /// ���������� ���������� �������
  /// </summary>
  public void ResetNumberOfJumps()
  {
    CurrentNumberJumps = NumberJumps;
  }

  //============================================================
}