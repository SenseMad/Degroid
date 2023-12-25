using UnityEngine;

public class KillEnemy : MonoBehaviour
{
  [SerializeField, Header("Сила отскока от врага")]
  private float _bounceOnEnemy = 10f;

  //============================================================

  private PlayerController2 PlayerController { get; set; }

  private Rigidbody2D Rigidbody2D { get; set; }

  //============================================================

  private void Awake()
  {
    PlayerController = FindObjectOfType<PlayerController2>();

    Rigidbody2D = transform.parent.GetComponent<Rigidbody2D>();
  }

  //============================================================

  public bool Death()
  {
    Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, _bounceOnEnemy);

    PlayerController.IsDoubleJump = false;

    if (LevelManager.Instance != null)
      LevelManager.Instance.OnKillingEnemy?.Invoke();

    return true;
  }

  //============================================================
}