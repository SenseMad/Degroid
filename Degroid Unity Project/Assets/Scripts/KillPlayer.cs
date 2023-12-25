using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
  private List<Collider2D> colliders = new List<Collider2D>();

  //============================================================

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (!colliders.Contains(other))
    {
      colliders.Add(other);
    }

    for (int i = 0; i < colliders.Count; i++)
    {
      if (!colliders[i].CompareTag("Player"))
        return;

      /*if (colliders[i].GetComponent<KillEnemy>())
        return;*/

      if (colliders[i].TryGetComponent(out Health health))
      {
        if (health)
        {
          health.TakeDamage(1);
        }
      }
    }
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    if (colliders.Contains(other))
    {
      colliders.Remove(other);
    }
  }

  //============================================================
}