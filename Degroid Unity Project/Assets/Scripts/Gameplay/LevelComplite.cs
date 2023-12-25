using UnityEngine;
using UnityEngine.InputSystem;

public class LevelComplite : MonoBehaviour
{
  [SerializeField]
  private GameObject _doorEnter;

  //------------------------------------------------------------

  /// <summary>
  /// True, если стоим возле двери
  /// </summary>
  private bool isNearDoor;

  //private UIButtonType uIButtonType;

  //============================================================

  private PlayerInputHandler PlayerInputHandler { get; set; }

  private BoxCollider2D BoxCollider2D { get; set; }

  //============================================================

  private void Awake()
  {
    BoxCollider2D = GetComponent<BoxCollider2D>();

    PlayerInputHandler = PlayerInputHandler.Instance;
  }

  private void OnEnable()
  {
    PlayerInputHandler.AI_Player.Player.EnterDoor.performed += OnEnterDoor;
  }

  private void OnDisable()
  {
    PlayerInputHandler.AI_Player.Player.EnterDoor.performed -= OnEnterDoor;
  }

  //============================================================

  public void CloseDoor(LevelData levelData)
  {
    gameObject.GetComponent<SpriteRenderer>().sprite = levelData.DoorEnd;
  }

  /// <summary>
  /// Открыть дверь
  /// </summary>
  public void OpenDoor(LevelData levelData)
  {
    BoxCollider2D.enabled = true;
    gameObject.GetComponent<SpriteRenderer>().sprite = levelData.DoorOpen;
  }

  private void OnEnterDoor(InputAction.CallbackContext context)
  {
    if (!isNearDoor)
      return;

    var levelManager = LevelManager.Instance;

    if (levelManager.IsPause)
      return;

    if (levelManager.IsLevelComplite)
      return;

    levelManager.OnThroughDoor?.Invoke();
  }

  //============================================================

  private void OnTriggerEnter2D(Collider2D collision)
  {
    PlayerManager2 playerManager = collision.GetComponent<PlayerManager2>();

    if (playerManager)
    {
      //playerManager.LevelManager.OnThroughDoor?.Invoke();
      isNearDoor = true;
      _doorEnter.SetActive(true);
    }
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    PlayerManager2 playerManager = collision.GetComponent<PlayerManager2>();

    if (playerManager)
    {
      isNearDoor = false;
      _doorEnter.SetActive(false);
    }
  }

  //============================================================
}