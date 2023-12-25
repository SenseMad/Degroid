using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
  private static PlayerInputHandler _instance;

  //============================================================

  public AI_Player AI_Player { get; private set; }

#if UNITY_PS4
  public bool EnterButtonCross { get; private set; }
#endif
  //============================================================

  public static PlayerInputHandler Instance
  {
    get
    {
      if (_instance == null)
      {
        var singletonObject = new GameObject($"{typeof(PlayerInputHandler)}");
        _instance = singletonObject.AddComponent<PlayerInputHandler>();
        DontDestroyOnLoad(singletonObject);
      }

      return _instance;
    }
  }

  //============================================================

  private void Awake()
  {
    AI_Player = new AI_Player();

#if UNITY_PS4
    // 0 means circle, 1 means cross.  See https://ps4.siedev.net/resources/documents/SDK/10.000/SystemService-Reference/0001.html
    EnterButtonCross = (Application.isEditor ? 1 : UnityEngine.PS4.Utility.GetSystemServiceParam(UnityEngine.PS4.Utility.SystemServiceParamId.EnterButtonAssign)) == 1;
    //string fullPath = "<PS4DualShockGamepad>/" + buttonName;
    if(!EnterButtonCross)
    {
      Debug.Log($"Enter button param is 0, adding binding (Select = <Gamepad>/buttonEast, Back = <Gamepad>/buttonSouth)");
      //Удаляем прошлый InputAction и добавляем новый
      AI_Player.UI.Select.ChangeBindingWithPath("<Gamepad>/buttonSouth").Erase();
      AI_Player.UI.Select.AddBinding("<Gamepad>/buttonEast");

      AI_Player.UI.Back.ChangeBindingWithPath("<Gamepad>/buttonEast").Erase();
      AI_Player.UI.Back.AddBinding("<Gamepad>/buttonSouth");
    }
#endif

    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;

    if (_instance == null)
    {
      _instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
      return;
    }
  }

  private void OnEnable()
  {
    AI_Player.Enable();
  }

  private void OnDisable()
  {
    AI_Player.Disable();
  }

  //============================================================

  /// <summary>
  /// True, если можно использовать управление
  /// </summary>
  public bool CanProcessInput()
  {
    return AI_Player != null && Cursor.lockState == CursorLockMode.Locked && !LevelManager.Instance.GameIsEnding && !LevelManager.Instance.IsPause && (DialogSystem.Instance != null ? !DialogSystem.Instance.IsDialogStarted : true);
  }

  //============================================================

  /// <summary>
  /// Получить кнопки перемещения (По горизонтали)
  /// </summary>
  public float GetMoveHorizontalInput()
  {
    return CanProcessInput() ? AI_Player.Player.Move.ReadValue<Vector2>().x : 0f;
  }

  /// <summary>
  /// Получить кнопки навигации (По вертикали)
  /// </summary>
  public float GetNavigationInput()
  {
    return AI_Player != null ? AI_Player.Player.Move.ReadValue<Vector2>().y : 0f;
  }

  /// <summary>
  /// Получить кнопки изменения значений (По горизонтали)
  /// </summary>
  public float GetChangingValuesInput()
  {
    return AI_Player != null ? AI_Player.Player.Move.ReadValue<Vector2>().x : 0f;
  }

  //============================================================

  public bool GetButtonBack()
  {
    return AI_Player.UI.Back.ReadValue<float>() == 1;
  }

  //============================================================

  public bool GetButtonKeyLeft()
  {
    return CanProcessInput() && AI_Player.Player.KeyLeft.ReadValue<float>() == 1;
  }

  public bool GetButtonKeyRight()
  {
    return CanProcessInput() && AI_Player.Player.KeyRight.ReadValue<float>() == 1;
  }

  //============================================================
}