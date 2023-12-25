using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
  [SerializeField, Tooltip("Список скинов")]
  private List<GameObject> _listSkins;

  //============================================================

  private GameManager GameManager { get; set; }

  public Animator Animator { get; set; }
  public SpriteRenderer SpriteRenderer { get; set; }

  private Health Health { get; set; }
  public LevelManager LevelManager { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// True, если смотрим вправо
  /// </summary>
  public bool IsFacingRight { get; set; } = true;

  /// <summary>
  /// Состояния для анимации
  /// </summary>
  /*public CharState State
  {
    get => (CharState)Animator.GetInteger("State");
    set => Animator.SetInteger("State", (int)value);
  }*/

  /// <summary>
  /// Список скинов
  /// </summary>
  public List<GameObject> ListSkins { get => _listSkins; set => _listSkins = value; }

  //============================================================

  private void Awake()
  {
    GameManager = GameManager.Instance;

    Animator = GetComponent<Animator>();
    SpriteRenderer = GetComponent<SpriteRenderer>();

    Health = GetComponent<Health>();
    LevelManager = FindObjectOfType<LevelManager>();
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
    IsFacingRight = transform.localScale.x > 0;
  }

  //============================================================

  /// <summary>
  /// Поворот персонажа
  /// </summary>
  public void Flip()
  {
    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    IsFacingRight = transform.localScale.x > 0;
  }

  /// <summary>
  /// Вызвать событие смерти
  /// </summary>
  private void OnDeath()
  {
    gameObject.SetActive(false);
    LevelManager.OnDeath?.Invoke();
  }

  //============================================================

  /// <summary>
  /// Вибрация низкая
  /// </summary>
  public void VibrationLow()
  {
    /*if (!GameManager.VibrationOn) { return; }

    foreach (Joystick joystick in Player.controllers.Joysticks)
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
    /*if (!GameManager.VibrationOn) { return; }

    foreach (Joystick joystick in Player.controllers.Joysticks)
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
    /*if (!GameManager.VibrationOn) { return; }

    foreach (Joystick joystick in Player.controllers.Joysticks)
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

  //============================================================

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.transform.CompareTag("MovingPlatform"))
    {
      GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.None;
      transform.parent = other.transform;
    }
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    if (other.transform.CompareTag("MovingPlatform"))
    {
      GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
      transform.parent = null;
    }
  }

  private void OnTriggerStay2D(Collider2D other)
  {
    if (other.transform.CompareTag("MovingPlatform"))
    {
      GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.None;
      transform.parent = other.transform;
    }
  }

  //============================================================
}

/// <summary>
/// Состояния игрока
/// </summary>
public enum CharState2
{
  Player_Idle,
  Player_Run,
  Player_Jump
}