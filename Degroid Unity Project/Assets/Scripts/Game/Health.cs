using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
  [SerializeField, Tooltip("Максимальное здоровье")]
  private int _maxHealth = 1;

  [SerializeField, Tooltip("Эффект смерти")]
  private List<GameObject> _vfxDeathPrefab;

  [SerializeField, Tooltip("Звук смерти")]
  private AudioClip _soundDeath;

  //------------------------------------------------------------

  private AudioManager audioManager;

  //============================================================

  private GameManager GameManager { get; set; }

  private PlayerManager2 PlayerManager2 { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// Максимальное здоровье
  /// </summary>
  public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }

  /// <summary>
  /// Эффект смерти
  /// </summary>
  public List<GameObject> VfxDeathPrefab { get => _vfxDeathPrefab; set => _vfxDeathPrefab = value; }

  //------------------------------------------------------------

  /// <summary>
  /// Текущее здоровье
  /// </summary>
  public int CurrentHealth { get; set; }

  //============================================================

  /// <summary>
  /// Событие: Нанесение урона
  /// </summary>
  public CustomUnityEvent<int> OnDamage { get; } = new CustomUnityEvent<int>();

  /// <summary>
  /// Событие: Смерть
  /// </summary>
  public CustomUnityEvent OnDeath { get; } = new CustomUnityEvent();

  //============================================================

  private void Awake()
  {
    GameManager = GameManager.Instance;

    audioManager = AudioManager.Instance;

    PlayerManager2 = FindObjectOfType<PlayerManager2>();

    CurrentHealth = MaxHealth;
  }

  //============================================================

  /// <summary>
  /// Получение урона
  /// </summary>
  public void TakeDamage(int damage)
  {
    CurrentHealth -= damage;

    OnDamage?.Invoke(damage);

    HandleDeath();
  }

  /// <summary>
  /// Проверка на смерть
  /// </summary>
  private void HandleDeath()
  {
    if (CurrentHealth <= 0)
    {
      PlayerManager2.VibrationMedium();

      if (_soundDeath != null)
        audioManager.OnPlaySFX?.Invoke(_soundDeath);

      CreateEffectDeath();
      OnDeath?.Invoke();
    }
  }

  /// <summary>
  /// Создать эффект смерти
  /// </summary>
  public void CreateEffectDeath()
  {
    if (VfxDeathPrefab != null)
    {
      foreach (var deathPrefab in VfxDeathPrefab)
      {
        var deathInstance = Instantiate(deathPrefab, transform.position, transform.rotation);

        Destroy(deathInstance, 2f);
      }
    }
  }

  //============================================================
}