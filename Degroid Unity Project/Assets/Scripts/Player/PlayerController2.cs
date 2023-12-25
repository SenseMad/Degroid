using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2 : MonoBehaviour
{
  [Header("Параметры игрока")]
  [SerializeField, Tooltip("Скорость игрока")]
  private float _moveSpeed = 8f;
  [SerializeField, Tooltip("Высота прыжка")]
  private float _jumpHeight = 20f;
  [SerializeField, Tooltip("Скорость падения"), Range(0.0f, -50.0f)]
  private float _rateOfFall = -14.0f;

  [Header("Проверка земли")]
  [SerializeField, Tooltip("По каким слоям можно ходить")]
  private LayerMask _whatIsGround;
  [SerializeField, Tooltip("")]
  private GameObject _gameObjectKill;

  [Header("ЗВУКИ")]
  [SerializeField, Tooltip("Звук шагов")]
  private AudioClip _soundFootsteps;
  [SerializeField, Tooltip("Звук прыжка")]
  private AudioClip _soundJump;

  [Header("Параметры лучей")]
  [SerializeField, Tooltip("Ширина луча проверки земли")]
  private float _rayWidth = 0.8f;
  [SerializeField, Tooltip("Длина луча проверки земли")]
  private float _rayLength = 0.15f;

  //------------------------------------------------------------

  private AudioSource audioSource;

  private float moveVelocity;

  private float tempTimeBreakup; // Временное время разрыва игрока

  //============================================================

  public Rigidbody2D Rigidbody2D { get; set; }

  private GameManager GameManager { get; set; }

  public PlayerInputHandler PlayerInputHandler { get; set; }

  private PlayerManager2 PlayerManager2 { get; set; }

  private BoxCollider2D BoxCollider2D;

  //------------------------------------------------------------

  /// <summary>
  /// True, если стоим на земле
  /// </summary>
  private bool IsGrounded { get; set; }

  private bool isJumping { get; set; }

  /// <summary>
  /// Время нахождения в воздухе
  /// </summary>
  private float TimeSpentAir { get; set; }

  /// <summary>
  /// True, если был использован второй прыжок
  /// </summary>
  public bool IsDoubleJump { get; set; }

  //============================================================

  private void Awake()
  {
    PlayerInputHandler = PlayerInputHandler.Instance;

    audioSource = GetComponent<AudioSource>();

    GameManager = GameManager.Instance;

    Rigidbody2D = GetComponent<Rigidbody2D>();

    PlayerManager2 = GetComponent<PlayerManager2>();

    BoxCollider2D = _gameObjectKill.GetComponent<BoxCollider2D>();
  }

  private void OnEnable()
  {
    PlayerInputHandler.AI_Player.Player.Jump.performed += OnJumpStart;
    PlayerInputHandler.AI_Player.Player.Jump.canceled += OnJumpStop;

    PlayerInputHandler.AI_Player.UI.Reload.performed += CharacterBreakup;

    if (LevelManager.Instance != null)
      LevelManager.Instance.OnPause.AddListener(UpdatePlayer);
  }

  private void OnDisable()
  {
    PlayerInputHandler.AI_Player.Player.Jump.performed -= OnJumpStart;
    PlayerInputHandler.AI_Player.Player.Jump.canceled -= OnJumpStop;

    PlayerInputHandler.AI_Player.UI.Reload.performed -= CharacterBreakup;

    if (LevelManager.Instance != null)
      LevelManager.Instance.OnPause.RemoveListener(UpdatePlayer);
  }

  private void Update()
  {
    if (LevelManager.Instance != null)
      if (LevelManager.Instance.IsPause) { return; }

    CharacterBreakup();

    HandleInput();

    // Ограничить скорость падения
    if (Rigidbody2D.velocity.y < _rateOfFall)
    {
      Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, _rateOfFall);
    }

    /*BoxCollider2D.size = new Vector3(0.75f, Mathf.Clamp(0.5f - Rigidbody2D.velocity.y * Time.deltaTime, 0.5f, float.MaxValue));
    BoxCollider2D.offset = new Vector2(0, -0.5f - BoxCollider2D.size.y * 0.5f);*/
  }

  //============================================================

  /// <summary>
  /// Разрыв персонажа
  /// </summary>
  private void CharacterBreakup()
  {
    if (PlayerInputHandler.GetButtonKeyLeft() && PlayerInputHandler.GetButtonKeyRight())
    {
      tempTimeBreakup += Time.deltaTime;

      if (tempTimeBreakup >= 1.0f)
      {
        PlayerManager2.Health.TakeDamage(1);
        GameManager.UpdateAchievement(GameManager.Achievement.CHARACTER_BREAK);
      }

      return;
    }

    tempTimeBreakup = 0;
  }

  private void CharacterBreakup(InputAction.CallbackContext context)
  {
    if (LevelManager.Instance.IsPause)
      return;

    PlayerManager2.Health.TakeDamage(1);
    GameManager.UpdateAchievement(GameManager.Achievement.CHARACTER_BREAK);
  }

  private void UpdatePlayer(bool parValue)
  {
    if (parValue)
    {
      Rigidbody2D.gravityScale = 0;
      Rigidbody2D.velocity = new Vector2(0, 0);
      return;
    }

    Rigidbody2D.gravityScale = 7;
  }

  private void HandleInput()
  {
    Vector2 a = new Vector2(transform.position.x - _rayWidth / 2f, transform.position.y - _rayWidth / 2f);
    float d = _rayWidth / 2f;

    for (int i = 0; i < 3; i++)
    {
      Debug.DrawRay(a + Vector2.right * d * (float)i, -Vector2.up * _rayLength, Color.red);
      if (!Physics2D.Raycast(a + Vector2.right * d * (float)i, -Vector2.up, _rayLength, _whatIsGround))
      {
        TimeSpentAir += Time.deltaTime;
      }
      else
      {
        TimeSpentAir = 0f;
        IsGrounded = true;
        IsDoubleJump = false;
      }
    }

    if (TimeSpentAir >= 0.3f)
    {
      IsGrounded = false;
    }

    moveVelocity = _moveSpeed * PlayerInputHandler.GetMoveHorizontalInput();
    Rigidbody2D.velocity = new Vector2(moveVelocity, Rigidbody2D.velocity.y);

    if (IsGrounded && (moveVelocity > 0f || moveVelocity < 0))
    {
      if (PlayerManager2.Animator.runtimeAnimatorController != null)
        PlayerManager2.State = CharState.Player_Run;

      if (audioSource.isPlaying && audioSource.clip == _soundJump && !isJumping)
        audioSource.Stop();

      if (_soundFootsteps != null)
        SoundSFX(_soundFootsteps);
    }
    else if (IsGrounded && moveVelocity == 0f)
    {
      if (PlayerManager2.Animator.runtimeAnimatorController != null)
        PlayerManager2.State = CharState.Player_Idle;

      if (audioSource.isPlaying && (audioSource.clip == _soundFootsteps || (!isJumping && audioSource.clip == _soundJump)))
        audioSource.Stop();
    }
    else
    {
      if (PlayerManager2.Animator.runtimeAnimatorController != null) 
        PlayerManager2.State = CharState.Player_Jump;
    }

    if (Rigidbody2D.velocity.x > 0f)
    {
      transform.localScale = new Vector3(0.8f, 0.8f, 1f);
      return;
    }

    if (Rigidbody2D.velocity.x < 0f)
    {
      transform.localScale = new Vector3(-0.8f, 0.8f, 1f);
    }
  }

  private void SoundSFX(AudioClip audioClip)
  {
    if (audioSource.isPlaying)
      return;

    audioSource.volume = (float)GameManager.SoundValue / 100;
    audioSource.clip = audioClip;
    audioSource.Play();
  }

  /// <summary>
  /// Начало прыжка
  /// </summary>
  private void OnJumpStart(InputAction.CallbackContext context)
  {
    if (!PlayerInputHandler.CanProcessInput())
      return;

    if (IsGrounded)
      JumpStart();

    if (!IsDoubleJump && !IsGrounded)
    {
      JumpStart();
      IsDoubleJump = true;
    }
  }

  private void JumpStart()
  {
    isJumping = true;
    if (PlayerManager2.Animator.runtimeAnimatorController != null)
    {
      PlayerManager2.State = CharState.Player_Jump;
    }

    if (audioSource.isPlaying && audioSource.clip == _soundFootsteps)
      audioSource.Stop();

    if (audioSource.isPlaying && audioSource.clip == _soundJump)
      audioSource.Stop();

    if (_soundJump != null)
      SoundSFX(_soundJump);

    Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, _jumpHeight);
    GameManager.Instance.NumberJumps++;
    GameManager.Instance.UpdateAchievementsJumps();
  }

  /// <summary>
  /// Остановка прыжка
  /// </summary>
  private void OnJumpStop(InputAction.CallbackContext context)
  {
    Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, Rigidbody2D.velocity.y * 0.5f);

    isJumping = false;
  }

  //============================================================
}