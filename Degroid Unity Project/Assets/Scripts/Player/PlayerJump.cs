using UnityEngine;

public class PlayerJump : MonoBehaviour
{
  [Header("Прыжок")]
  [SerializeField, Tooltip("Высота прыжка")]
  private float _jumpHeight = 3f;
  [SerializeField, Tooltip("Количество прыжков")]
  private int _numberJumps = 2;

  [Header("Длительность прыжка")]
  [SerializeField, Tooltip("Длительность нажатия кнопки")]
  private bool _durationPressTime = true;
  [SerializeField, Tooltip("Время нахождения в воздухе")]
  private float _minimumAirTime = 0f;
  [SerializeField, Tooltip("На сколько делить высоту прыжка")]
  private float _jumpReleaseForceFactor = 2f;

  private float _nextJumpInSecond; // Следующий прыжок через ...

  //============================================================

  private PlayerManager PlayerManager { get; set; }
  private PlayerController PlayerController { get; set; }
  private PlayerInputHandler PlayerInputHandler { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// Высота прыжка
  /// </summary>
  public float JumpHeight { get => _jumpHeight; set => _jumpHeight = value; }

  /// <summary>
  /// Количество прыжков
  /// </summary>
  public int NumberJumps { get => _numberJumps; set => _numberJumps = value; }

  /// <summary>
  /// Длительность нажатия кнопки
  /// </summary>
  public bool DurationPressTime { get => _durationPressTime; set => _durationPressTime = value; }

  /// <summary>
  /// Время нахождения в воздухе
  /// </summary>
  public float MinimumAirTime { get => _minimumAirTime; set => _minimumAirTime = value; }

  /// <summary>
  /// На сколько делить высоту прыжка
  /// </summary>
  public float JumpReleaseForceFactor { get => _jumpReleaseForceFactor; set => _jumpReleaseForceFactor = value; }

  //------------------------------------------------------------

  /// <summary>
  /// Текущее количество прыжков
  /// </summary>
  private int CurrentNumberJumps { get; set; } = 0;

  /// <summary>
  /// Время нажатия кнопки прыжка
  /// </summary>
  private float JumpButtonPressTime { get; set; } = 0;

  /// <summary>
  /// True, если кнопка прыжка нажимается
  /// </summary>
  private bool JumpButtonPressed { get; set; } = false;

  /// <summary>
  /// True, если кнопка прыжка отпущена
  /// </summary>
  private bool JumpButtonReleased { get; set; } = false;

  /// <summary>
  /// True, если можно прыгать
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
  /// Делаем это каждый кадр
  /// </summary>
  private void EveryFrame()
  {
    if (PlayerController.PlayerState.JustGotGrounded)
    {
      CurrentNumberJumps = NumberJumps;
      PlayerController.PlayerState.IsJumping = false;
      CanJump = true;
    }

    // Если пользователь отпускает кнопку прыжка, и персонаж прыгает вверх и достаточно времени прошло с момента первоначального прыжка, то мы прекращаем прыгать, применяем силу вниз
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
  /// Начало прыжка
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
  /// Прекращение прыжка
  /// </summary>
  public void JumpStop()
  {
    JumpButtonPressed = false;
    JumpButtonReleased = true;
  }

  /// <summary>
  /// Сброс флагов перехода
  /// </summary>
  public void SetJumpFlags()
  {
    JumpButtonPressTime = Time.time;
    JumpButtonPressed = true;
    JumpButtonReleased = false;
  }

  /// <summary>
  /// Сбрасывает флаг отпущенной кнопки перехода
  /// </summary>
  public void ResetJumpButtonReleased()
  {
    JumpButtonReleased = false;
  }

  /// <summary>
  /// Сбрасываем количество прыжков
  /// </summary>
  public void ResetNumberOfJumps()
  {
    CurrentNumberJumps = NumberJumps;
  }

  //============================================================
}