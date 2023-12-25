using UnityEngine;

public class Pickup : MonoBehaviour
{
  [SerializeField, Tooltip("Частота, с которой элемент будет перемещаться вверх и вниз")]
  private float _verticalBorFrequency = 2f;

  [SerializeField, Tooltip("Расстояние, на котором элемент будет перемещаться вверх и вниз")]
  private float _bobbingAmount = 0.3f;

  [SerializeField, Tooltip("VFX, эффект при взятии")]
  private GameObject _pickupVfxPrefab;

  [SerializeField, Tooltip("Звук сбора предмета")]
  private AudioClip audioClip;

  //------------------------------------------------------------

  private Vector3 StartPosition;

  //============================================================

  private AudioManager audioManager;

  private PlayerManager2 PlayerManager2 { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// Частота, с которой элемент будет перемещаться вверх и вниз
  /// </summary>
  public float VerticalBorFrequency { get => _verticalBorFrequency; set => _verticalBorFrequency = value; }

  /// <summary>
  /// Расстояние, на котором элемент будет перемещаться вверх и вниз
  /// </summary>
  public float BobbingAmount { get => _bobbingAmount; set => _bobbingAmount = value; }

  /// <summary>
  /// VFX, эффект при взятии
  /// </summary>
  public GameObject PickupVfxPrefab { get => _pickupVfxPrefab; set => _pickupVfxPrefab = value; }

  //============================================================

  private void Awake()
  {
    audioManager = AudioManager.Instance;
  }

  protected virtual void Start()
  {
    StartPosition = transform.position;

    PlayerManager2 = FindObjectOfType<PlayerManager2>();
  }

  private void Update()
  {
    if (LevelManager.Instance == null) { return; }
    if (LevelManager.Instance.IsPause) { return; }

    float bobbingAnimationPhase = ((Mathf.Sin(Time.time * VerticalBorFrequency) * 0.5f) + 0.5f) * BobbingAmount;

    transform.position = StartPosition + Vector3.up * bobbingAnimationPhase;
  }

  //============================================================

  protected virtual void OnPicked(PlayerManager2 playerManager)
  {
    PlayPickupFeedback();
  }

  public void PlayPickupFeedback()
  {
    PlayerManager2.VibrationLow();

    audioManager.OnPlaySFX?.Invoke(audioClip);

    //AudioSource.PlayClipAtPoint(audioClip, transform.position);

    //Destroy(gameObject, 2.0f);

    /*if (PickupVfxPrefab)
    {
      var pickupVfx = Instantiate(PickupVfxPrefab, transform.position, transform.rotation);

      Destroy(pickupVfx, 1f);
    }*/
  }

  //============================================================

  private void OnTriggerEnter2D(Collider2D collision)
  {
    PlayerManager2 pickingPlayer = collision.GetComponent<PlayerManager2>();

    if (pickingPlayer)
    {
      OnPicked(pickingPlayer);
    }
  }

  //============================================================
}