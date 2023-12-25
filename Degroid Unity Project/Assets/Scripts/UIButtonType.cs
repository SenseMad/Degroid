using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIButtonType : MonoBehaviour
{
  public enum ButtonType
  {
    Move,
    Jump,
    EnterDoor,

    Pause,
    Select,
    Reload,
    Back,
  }

  [SerializeField]
  private ButtonType _type;
  [SerializeField]
  private ButtonIcons _icons;

  [SerializeField, Tooltip("Клавиатура")]
  private Sprite _keyboardButton;

  [SerializeField, Tooltip("Джостик Xbox")]
  private Sprite _controllerXboxButton;

  [SerializeField, Tooltip("Джостик PS4")]
  private Sprite _controllerPS4Button;

  //============================================================

  private Image SpriteRenderer { get; set; }
  private SpriteRenderer SpriteRenderer1 { get; set; }

  private PlayerInputHandler PlayerInputHandler { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// Клавиатура
  /// </summary>
  public Sprite KeyboardButton { get => _keyboardButton; set => _keyboardButton = value; }

  /// <summary>
  /// Джостик Xbox
  /// </summary>
  public Sprite ControllerXboxButton { get => _controllerXboxButton; set => _controllerXboxButton = value; }

  /// <summary>
  /// Джостик PS4
  /// </summary>
  public Sprite ControllerPS4Button { get => _controllerPS4Button; set => _controllerPS4Button = value; }

  //============================================================

  private void Awake()
  {
    SpriteRenderer = GetComponent<Image>();
    SpriteRenderer1 = GetComponent<SpriteRenderer>();

    PlayerInputHandler = PlayerInputHandler.Instance;
  }

  private void Update()
  {
    UpdateIconButton();
  }

  //============================================================

  private void SetSprite(Sprite parSprite)
  {
    if (SpriteRenderer != null)
      SpriteRenderer.sprite = parSprite;
    else if (SpriteRenderer1 != null)
      SpriteRenderer1.sprite = parSprite;
  }

  private void UpdateIconButton()
  {
    Sprite sprite = null;

#if UNITY_STANDALONE_WIN
    var lastDevice = InputSystem.devices[InputSystem.devices.Count - 1];

    //lastDevice.description.interfaceName
    if (lastDevice is Keyboard || lastDevice is Mouse)
    {
      sprite = _icons.GetSprite("Keyboard", _type.ToString());
      //SetSprite(KeyboardButton);
    }
    else if (lastDevice is Gamepad)
    {
      if (lastDevice is UnityEngine.InputSystem.XInput.XInputController)
      {
        sprite = _icons.GetSprite("Xbox", _type.ToString());
        //SetSprite(ControllerXboxButton);
      }
      else if (lastDevice is UnityEngine.InputSystem.DualShock.DualShockGamepad)
      {
        sprite = _icons.GetSprite("DualShock", _type.ToString());
        //SetSprite(ControllerPS4Button);
      }
    }
    else if (lastDevice is Joystick)
    {
      sprite = _icons.GetSprite("Joystick", _type.ToString());
      //SetSprite(ControllerXboxButton);
    }
#elif UNITY_PS4
    ButtonType type = _type;
    if (!PlayerInputHandler.EnterButtonCross)
    {
      switch (type)
      {
        case ButtonType.Select:
          type = ButtonType.Back;
          break;
        case ButtonType.Back:
          type = ButtonType.Select;
          break;
      }
    }
    sprite = _icons.GetSprite("DualShock", type.ToString());
    //SetSprite(ControllerPS4Button);
#endif
    SetSprite(sprite);
    /*
    var lastDevice = InputSystem.devices[InputSystem.devices.Count - 1];
    
    if (lastDevice is Gamepad)
    {
      if (SpriteRenderer != null)
        SpriteRenderer.sprite = ControllerPS4Button;
      else if (SpriteRenderer1 != null)
        SpriteRenderer1.sprite = ControllerPS4Button;
      return;
    }
    else if (lastDevice is Joystick)
    {
      if (SpriteRenderer != null)
        SpriteRenderer.sprite = ControllerXboxButton;
      else if (SpriteRenderer1 != null)
        SpriteRenderer1.sprite = ControllerXboxButton;
      return;
    }
    else if (lastDevice is Keyboard || lastDevice is Mouse)
    {
      if (SpriteRenderer != null)
        SpriteRenderer.sprite = KeyboardButton;
      else if (SpriteRenderer1 != null)
        SpriteRenderer1.sprite = KeyboardButton;
      return;
    }*/
  }

  //============================================================
}