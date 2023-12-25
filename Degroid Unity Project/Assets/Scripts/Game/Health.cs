using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
  [SerializeField, Tooltip("������������ ��������")]
  private int _maxHealth = 1;

  [SerializeField, Tooltip("������ ������")]
  private List<GameObject> _vfxDeathPrefab;

  [SerializeField, Tooltip("���� ������")]
  private AudioClip _soundDeath;

  //------------------------------------------------------------

  private AudioManager audioManager;

  //============================================================

  private GameManager GameManager { get; set; }

  private PlayerManager2 PlayerManager2 { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// ������������ ��������
  /// </summary>
  public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }

  /// <summary>
  /// ������ ������
  /// </summary>
  public List<GameObject> VfxDeathPrefab { get => _vfxDeathPrefab; set => _vfxDeathPrefab = value; }

  //------------------------------------------------------------

  /// <summary>
  /// ������� ��������
  /// </summary>
  public int CurrentHealth { get; set; }

  //============================================================

  /// <summary>
  /// �������: ��������� �����
  /// </summary>
  public CustomUnityEvent<int> OnDamage { get; } = new CustomUnityEvent<int>();

  /// <summary>
  /// �������: ������
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
  /// ��������� �����
  /// </summary>
  public void TakeDamage(int damage)
  {
    CurrentHealth -= damage;

    OnDamage?.Invoke(damage);

    HandleDeath();
  }

  /// <summary>
  /// �������� �� ������
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
  /// ������� ������ ������
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