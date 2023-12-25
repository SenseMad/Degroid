using UnityEngine;

public class Jumper : MonoBehaviour
{
  [SerializeField, Header("Высота прыжка на батуте")]
  private float _jumpHeight = 15f;

  [SerializeField, Tooltip("Звук прыжка на батуте")]
  private AudioClip _jumpClip;

  private AudioSource audioSource;

  //============================================================

  private Animator Animator { get; set; }

  private GameManager GameManager { get; set; }

  private PlayerController2 PlayerController { get; set; }

  private PlayerManager2 PlayerManager { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// Высота прыжка на батуте
  /// </summary>
  public float JumpHeight { get => _jumpHeight; set => _jumpHeight = value; }

  //============================================================

  private void Awake()
  {
    audioSource = GetComponent<AudioSource>();

    Animator = GetComponent<Animator>();

    GameManager = GameManager.Instance;

    PlayerController = FindObjectOfType<PlayerController2>();

    PlayerManager = FindObjectOfType<PlayerManager2>();
  }

  //============================================================

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (LevelManager.Instance != null)
      if (LevelManager.Instance.IsPause) { return; }

    if (other.gameObject.tag == "Player")
    {
      if (Animator != null) { Animator.SetTrigger("Bounce"); }

      PlayerController.Rigidbody2D.velocity = new Vector2(PlayerController.Rigidbody2D.velocity.x, JumpHeight);
      PlayerController.IsDoubleJump = true;

      PlayerManager.VibrationLow();

      if (_jumpClip == null)
        return;

      if (audioSource.isPlaying)
        audioSource.Stop();

      audioSource.volume = (float)GameManager.SoundValue / 100;
      audioSource.clip = _jumpClip;
      audioSource.Play();
    }
  }

  //============================================================
}