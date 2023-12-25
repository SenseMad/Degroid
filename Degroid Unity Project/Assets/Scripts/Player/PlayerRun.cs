using UnityEngine;

public class PlayerRun : MonoBehaviour
{
  [SerializeField, Tooltip("�������� ������")]
  private float _playerSpeed = 10f;

  //============================================================

  private PlayerManager PlayerManager { get; set; }
  private PlayerController PlayerController { get; set; }
  private PlayerInputHandler PlayerInputHandler { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// �������� ������
  /// </summary>
  public float PlayerSpeed { get => _playerSpeed; set => _playerSpeed = value; }

  //------------------------------------------------------------

  /// <summary>
  /// �������������� �����������
  /// </summary>
  private float HorizontalMove { get; set; }

  /// <summary>
  /// ������� �������������� ��������
  /// </summary>
  private float CurrentHorizontalSpeed { get; set; }

  //============================================================

  private void Awake()
  {
    PlayerManager = GetComponent<PlayerManager>();
    PlayerController = GetComponent<PlayerController>();
    PlayerInputHandler = GetComponent<PlayerInputHandler>();
  }

  private void Update()
  {
    HorozontalMove();

    HorizontalMovement();
  }

  //============================================================

  /// <summary>
  /// �������������� �����������
  /// </summary>
  public void HorozontalMove()
  {
    HorizontalMove = PlayerInputHandler.GetMoveHorizontalInput();
  }

  /// <summary>
  /// ���������� ��� Update(), ������������ �������������� �����������.
  /// </summary>
  private void HorizontalMovement()
  {
    if (LevelManager.Instance != null)
      if (LevelManager.Instance.IsPause) { return; }

    if (HorizontalMove > 0.1f)
    {
      CurrentHorizontalSpeed = HorizontalMove;

      if (!PlayerManager.IsFacingRight)
        PlayerManager.Flip();

      if (PlayerController.PlayerState.IsGrounded)
      {
        /*if (PlayerManager.Animator.runtimeAnimatorController != null)
          PlayerManager.State = CharState.Player_Run;*/
      }
    }
    else if (HorizontalMove < -0.1f)
    {
      CurrentHorizontalSpeed = HorizontalMove;

      if (PlayerManager.IsFacingRight)
        PlayerManager.Flip();

      if (PlayerController.PlayerState.IsGrounded)
      {
        /*if (PlayerManager.Animator.runtimeAnimatorController != null)
          PlayerManager.State = CharState.Player_Run;*/
      }
    }
    else
    {
      CurrentHorizontalSpeed = 0;
      if (PlayerController.PlayerState.IsGrounded)
      {
        /*if (PlayerManager.Animator.runtimeAnimatorController != null)
          PlayerManager.State = CharState.Player_Idle;*/
      }
    }

    PlayerController.SetHorizontalForce(CurrentHorizontalSpeed * PlayerSpeed);
  }

  //============================================================
}