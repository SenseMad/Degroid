using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager2 : MonoBehaviour
{
  private GameManager GameManager { get; set; }

  public Animator Animator { get; set; }
  public SpriteRenderer SpriteRenderer { get; set; }

  public Health Health { get; set; }
  public LevelManager LevelManager { get; set; }

  private Rigidbody2D Rigidbody2D { get; set; }

  //============================================================

  /// <summary>
  /// Состояния для анимации
  /// </summary>
  public CharState State
  {
    get => (CharState)Animator.GetInteger("State");
    set => Animator.SetInteger("State", (int)value);
  }

  //============================================================

  private void Awake()
  {
    GameManager = GameManager.Instance;

    Animator = GetComponent<Animator>();
    SpriteRenderer = GetComponent<SpriteRenderer>();

    Health = GetComponent<Health>();
    LevelManager = FindObjectOfType<LevelManager>();

    Rigidbody2D = GetComponent<Rigidbody2D>();
  }

  private void OnEnable()
  {
    Health.OnDeath.AddListener(OnDeath);

    LevelManager.OnPause.AddListener(AnimStatus);
  }

  private void OnDisable()
  {
    Health.OnDeath.RemoveListener(OnDeath);

    LevelManager.OnPause.RemoveListener(AnimStatus);
  }

  private void Update()
  {
    
  }

  //============================================================

  private void AnimStatus(bool parValue)
  {
    if (parValue)
    {
      Animator.StartPlayback();
    }
    else
    {
      Animator.StopPlayback();
    }
  }

  /// <summary>
  /// Вызвать событие смерти
  /// </summary>
  private void OnDeath()
  {
    gameObject.SetActive(false);
    LevelManager.OnDeath?.Invoke();

    Destroy(gameObject, 0.5f);
  }

  //============================================================

  /// <summary>
  /// Вибрация низкая
  /// </summary>
  public void VibrationLow()
  {
    if (!GameManager.VibrationOn) { return; }

    var gamepad = Gamepad.current;

    if (gamepad == null)
      return;

    gamepad.SetMotorSpeeds(0.1f, 0.3f);
    Invoke(nameof(StopVibration), 0.8f);

    /*foreach (Joystick joystick in Player.controllers.Joysticks)
    {
      if (joystick.supportsVibration)
      {
        if (joystick.vibrationMotorCount > 0)
        {
          joystick.SetVibration(0, 0.3f, 0.1f);
        }
        if (joystick.vibrationMotorCount > 1)
        {
          joystick.SetVibration(1, 0.3f, 0.1f);
        }
      }
    }*/
  }

  /// <summary>
  /// Вибрация средняя
  /// </summary>
  public void VibrationMedium()
  {
    if (!GameManager.VibrationOn) { return; }

    var gamepad = Gamepad.current;

    if (gamepad == null)
      return;

    gamepad.SetMotorSpeeds(0.2f, 0.5f);
    Invoke(nameof(StopVibration), 0.8f);

    /*foreach (Joystick joystick in Player.controllers.Joysticks)
    {
      if (joystick.supportsVibration)
      {
        if (joystick.vibrationMotorCount > 0)
        {
          joystick.SetVibration(0, 0.5f, 0.2f);
        }
        if (joystick.vibrationMotorCount > 1)
        {
          joystick.SetVibration(1, 0.5f, 0.2f);
        }
      }
    }*/
  }

  /// <summary>
  /// Вибрация высокая
  /// </summary>
  public void VibrationHeight()
  {
    if (!GameManager.VibrationOn) { return; }

    var gamepad = Gamepad.current;

    if (gamepad == null)
      return;

    gamepad.SetMotorSpeeds(0.5f, 0.1f);
    Invoke(nameof(StopVibration), 0.8f);

    /*foreach (Joystick joystick in Player.controllers.Joysticks)
    {
      if (joystick.supportsVibration)
      {
        if (joystick.vibrationMotorCount > 0)
        {
          joystick.SetVibration(0, 1f, 0.5f);
        }
        if (joystick.vibrationMotorCount > 1)
        {
          joystick.SetVibration(1, 1f, 0.5f);
        }
      }
    }*/
  }

  private void StopVibration()
  {
    var gamepad = Gamepad.current;
    gamepad.ResetHaptics();
  }

  //============================================================

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.transform.CompareTag("MovingPlatform"))
    {
      Rigidbody2D.interpolation = RigidbodyInterpolation2D.None;
      transform.parent = other.transform;
    }
  }

  private void OnCollisionExit2D(Collision2D other)
  {
    if (other.transform.CompareTag("MovingPlatform") && gameObject.activeInHierarchy)
    {
      Rigidbody2D.interpolation = RigidbodyInterpolation2D.Interpolate;
      transform.parent = null;
    }
  }

  private void OnCollisionStay2D(Collision2D other)
  {
    if (other.transform.CompareTag("MovingPlatform"))
    {
      Rigidbody2D.interpolation = RigidbodyInterpolation2D.None;
      transform.parent = other.transform;
    }

    /*if (other.gameObject.CompareTag("Enemy") && Rigidbody2D.velocity.y >= 0f)
    {
      Health.TakeDamage(1);
      return;
    }*/
  }

  //============================================================
}

/// <summary>
/// Состояния игрока
/// </summary>
public enum CharState
{
  Player_Idle,
  Player_Run,
  Player_Jump
}