using UnityEngine;

public class EnableBlood : MonoBehaviour
{
  [SerializeField, Tooltip("������ ����� � ������")]
  private Sprite _spriteObject;

  //============================================================

  private GameManager GameManager { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// ������ ����� � ������
  /// </summary>
  public Sprite SpriteObject { get => _spriteObject; set => _spriteObject = value; }

  //------------------------------------------------------------

  /// <summary>
  /// True, ���� ��� �����
  /// </summary>
  private bool IsSpiteHurt { get; set; }

  //============================================================

  private void Awake()
  {
    GameManager = GameManager.Instance;
  }

  //============================================================

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Player"))
    {
      if (IsSpiteHurt) { return; }

      IsSpiteHurt = true;
      gameObject.GetComponent<SpriteRenderer>().sprite = SpriteObject;
    }
  }

  //============================================================
}