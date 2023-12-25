using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
  [SerializeField, Tooltip("Музыка")]
  private AudioClip[] _audioClipMusic;

  [SerializeField, Tooltip("Звук интерфейса")]
  private AudioClip _audioClipSound;

  //------------------------------------------------------------

  private static AudioManager _instance;

  //============================================================

  public static AudioManager Instance
  {
    get
    {
      return _instance;
    }
  }

  //------------------------------------------------------------

  /// <summary>
  /// Музыка
  /// </summary>
  public AudioClip[] AudioClipMusic { get => _audioClipMusic; private set => _audioClipMusic = value; }

  /// <summary>
  /// Звук интерфейса
  /// </summary>
  public AudioClip AudioClipSound { get => _audioClipSound; private set => _audioClipSound = value; }

  //------------------------------------------------------------

  private AudioSource AudioSource { get; set; }

  private GameManager GameManager { get; set; }

  //============================================================

  /// <summary>
  /// Событие: Проиграть звук нажатия кнопки
  /// </summary>
  public CustomUnityEvent OnPlaySoundButton { get; } = new CustomUnityEvent();

  /// <summary>
  /// Событие: Проиграть звук
  /// </summary>
  public CustomUnityEvent<AudioClip> OnPlaySFX { get; } = new CustomUnityEvent<AudioClip>();

  //============================================================

  private void Awake()
  {
    AudioSource = GetComponent<AudioSource>();
    GameManager = GameManager.Instance;

    if (_instance != null && _instance != this)
    {
      Destroy(gameObject);
      return;
    }

    _instance = this;
    DontDestroyOnLoad(gameObject);
  }

  private void Start()
  {
    AudioSource.clip = GetRandomClip();
    AudioSource.Play();
  }

  private void OnEnable()
  {
    UpdateAudioSource(GameManager.MusicValue);

    OnPlaySoundButton.AddListener(PlaySoundButton);
    OnPlaySFX.AddListener(PlaySFX);

    GameManager.ChangeMusicValue.AddListener(UpdateAudioSource);
  }

  private void OnDisable()
  {
    OnPlaySoundButton.RemoveListener(PlaySoundButton);
    OnPlaySFX.RemoveListener(PlaySFX);

    GameManager.ChangeMusicValue.RemoveListener(UpdateAudioSource);
  }

  private void Update()
  {
    if (AudioClipMusic.Length > 0)
    {
      if (!AudioSource.isPlaying && AudioSource.time == AudioSource.clip.length)
      {
        AudioSource.clip = GetRandomClip();
        AudioSource.Play();
      }
    }
  }

  //============================================================

  /// <summary>
  /// Воспроизвести звук
  /// </summary>
  public AudioSource PlaySound(AudioClip Sfx, Vector3 Location)
  {
    GameObject tempAudio = new GameObject("SoundEffects");
    tempAudio.transform.position = Location;
    AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
    audioSource.clip = Sfx;
    audioSource.volume = (float)GameManager.SoundValue / 100;
    audioSource.Play();
    Destroy(tempAudio, Sfx.length);

    return audioSource;
  }

  /// <summary>
  /// Рандомная музыка без повторов
  /// </summary>
  private AudioClip GetRandomClip()
  {
    AudioClip clip;

    clip = AudioClipMusic[Random.Range(0, AudioClipMusic.Length)];

    while (clip == AudioSource.clip)
      clip = AudioClipMusic[Random.Range(0, AudioClipMusic.Length)];

    return clip;
  }

  /// <summary>
  /// Проиграть звук кнопки
  /// </summary>
  public void PlaySoundButton()
  {
    PlaySound(AudioClipSound, transform.position);
  }

  /// <summary>
  /// Проиграть звук
  /// </summary>
  public void PlaySFX(AudioClip audioClip)
  {
    PlaySound(audioClip, transform.position);
  }

  //============================================================

  private void UpdateAudioSource(int parValue)
  {
    AudioSource.volume = (float)parValue / 100 * 0.5f;
  }

  //============================================================
}