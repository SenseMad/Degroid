using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LazerManager : MonoBehaviour
{
  [SerializeField, Tooltip("Задержка после выстрела")]
  private float _delayAfterFiring = 1f;

  [SerializeField, Tooltip("Перезарядка после выстрела")]
  private float _reloadingAfterFiring = 4f;

  [SerializeField, Tooltip("Точка выстрела")]
  private Transform _pointShot;

  [SerializeField, Tooltip("")]
  private LayerMask _layerMask;

  //------------------------------------------------------------

  private float tempTime = 0f;

  //============================================================


  /// <summary>
  /// LineRenderer
  /// </summary>
  private LineRenderer LineRenderer { get; set; }

  //============================================================

  private void Awake()
  {
    LineRenderer = GetComponent<LineRenderer>();
  }

  private void Start()
  {
    LineRenderer.enabled = true;
  }

  private void Update()
  {
    if (LevelManager.Instance != null)
      if (LevelManager.Instance.IsPause) { return; }

    if (LineRenderer.enabled)
    {
      RaycastHit2D[] hits = Physics2D.RaycastAll(_pointShot.position, transform.right, 100, _layerMask);
      for (int i = 0; i < hits.Length; i++)
      {
        if (!hits[i].collider)
        {
          LineRenderer.SetPosition(1, transform.position + transform.right * 100);
          continue;
        }

        LineRenderer.SetPosition(1, hits[i].point);
        if (hits[i].collider.CompareTag("Player"))
        {
          var health = hits[i].collider.GetComponent<Health>();
          if (health)
          {
            health.TakeDamage(1);
          }
        }
      }
      LineRenderer.SetPosition(0, transform.position);

      LaserControll(_delayAfterFiring, false);
    }
    else
    {
      LaserControll(_reloadingAfterFiring, true);
    }
  }

  //============================================================

  private void LaserControll(float time, bool parValue)
  {
    if (tempTime < time)
    {
      tempTime += Time.deltaTime;
      return;
    }

    tempTime = 0;
    LineRenderer.enabled = parValue;
  }

  //============================================================
}