/// <summary>
/// Различные состояния, которые вы можете использовать, чтобы проверить, если ваш персонаж делает что-то в текущем кадре
/// </summary>
public class PlayerState
{
  /// <summary>
  /// True, если персонаж сталкивается справа
  /// </summary>
  public bool IsCollidingRight { get; set; }

  /// <summary>
  /// True, если персонаж сталкивается слева
  /// </summary>
  public bool IsCollidingLeft { get; set; }

  /// <summary>
  /// True, если персонаж сталкивается сверху
  /// </summary>
  public bool IsCollidingAbove { get; set; }

  /// <summary>
  /// True, если персонаж сталкивается снизу
  /// </summary>
  public bool IsCollidingBelow { get; set; }
  
  /// <summary>
  /// True, если персонаж на земле
  /// </summary>
  public bool IsGrounded { get => IsCollidingBelow; }

  /// <summary>
  /// True, если персонаж падает прямо сейчас
  /// </summary>
  public bool IsFalling { get; set; }

  /// <summary>
  /// True, если персонаж был заземлен в прошлом кадре
  /// </summary>
  public bool WasGroundedLastFrame { get; set; }

  /// <summary>
  /// True, если персонаж касался потолка в прошлом кадре
  /// </summary>
  public bool WasTouchingTheCeilingLastFrame { get; set; }

  /// <summary>
  /// True, если персонаж только что заземлился
  /// </summary>
  public bool JustGotGrounded { get; set; }

  /// <summary>
  /// True, если прыгаем в данный момент
  /// </summary>
  public bool IsJumping { get; set; }

  /// <summary>
  /// True, если движемся на платформе
  /// </summary>
  public bool OnAMovingPlatform { get; set; }

  /// <summary>
  /// Сброс всех состояний столкновения
  /// </summary>
  public void Reset()
  {
    IsCollidingLeft = false;
    IsCollidingRight = false;
    IsCollidingAbove = false;
    JustGotGrounded = false;
    IsFalling = true;
  }
}