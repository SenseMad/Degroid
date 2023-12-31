using UnityEngine;

/// <summary>
/// ��������� �������
/// </summary>
public class ItemPickup : Pickup
{
  //============================================================

  protected override void OnPicked(PlayerManager2 player)
  {
    PlayerManager2 playerManager = player.GetComponent<PlayerManager2>();

    if (playerManager)
    {
      playerManager.LevelManager.OnItemPickup?.Invoke();

      PlayPickupFeedback();
      gameObject.SetActive(false);
      Destroy(gameObject, 1f);
    }
  }

  //============================================================
}