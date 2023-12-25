using UnityEngine;
using UnityEngine.InputSystem;

public class EndCredits : MonoBehaviour
{
  [SerializeField, Tooltip("Скорость прокрутки")]
  private float _scrollSpeed = 5;
  [SerializeField, Tooltip("Объект который нужно прокручивать")]
  private RectTransform _content;
  [SerializeField, Tooltip("")]
  private RectTransform _xz;

  [SerializeField, Tooltip("True, если титры запущены")]
  private bool _isCreditsRunning = false;

  [SerializeField, Tooltip("")]
  private GameObject _topPanel;

  //------------------------------------------------------------

  private Vector2 startPos;
  private RectTransform parent;

  //============================================================

  private PlayerInputHandler InputHandler { get; set; }

  private AudioManager AudioManager { get; set; }

  private PanelController PanelController { get; set; }

  //============================================================

  private void Awake()
  {
    InputHandler = PlayerInputHandler.Instance;

    AudioManager = AudioManager.Instance;

    PanelController = FindObjectOfType<PanelController>();
  }

  private void Start()
  {
    startPos = _content.anchoredPosition;

    parent = _content.parent as RectTransform;
  }

  private void OnEnable()
  {
    _isCreditsRunning = true;

    _topPanel.SetActive(false);

    InputHandler.AI_Player.UI.Back.performed += OnDoNotCloseLastWindow;
    InputHandler.AI_Player.UI.Pause.performed += OnDoNotCloseLastWindow;
  }

  private void OnDisable()
  {
    _isCreditsRunning = false;
    _content.anchoredPosition = startPos;

    _topPanel.SetActive(true);

    InputHandler.AI_Player.UI.Back.performed -= OnDoNotCloseLastWindow;
    InputHandler.AI_Player.UI.Pause.performed -= OnDoNotCloseLastWindow;
  }

  private void Update()
  {
    if (!_isCreditsRunning)
      return;

    float targetY = parent.rect.max.y + _content.rect.size.y;
    _content.localPosition = Vector3.MoveTowards(_content.localPosition, Vector3.up * targetY, _scrollSpeed * Time.deltaTime);

    if (_content.localPosition.y == targetY)
    {
      PanelController.DoNotCloseLastWindow();
    }
  }

  //============================================================

  private void Sound()
  {
    AudioManager.OnPlaySoundButton?.Invoke();
  }

  public void OnDoNotCloseLastWindow(InputAction.CallbackContext context)
  {
    Sound();

    PanelController.DoNotCloseLastWindow();
  }

  //============================================================
}