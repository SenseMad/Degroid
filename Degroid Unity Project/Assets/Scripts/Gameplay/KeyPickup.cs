using UnityEngine;

/// <summary>
/// Подобрать ключ
/// </summary>
public class KeyPickup : Pickup
{
  //============================================================

  protected override void OnPicked(PlayerManager2 player)
  {
    PlayerManager2 playerManager = player.GetComponent<PlayerManager2>();

    if (playerManager)
    {
      playerManager.LevelManager.OnKeyPickup?.Invoke();

      PlayPickupFeedback();
      gameObject.SetActive(false);
      Destroy(gameObject, 1f);
    }
  }

  //============================================================
}