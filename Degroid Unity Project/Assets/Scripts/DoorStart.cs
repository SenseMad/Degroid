using UnityEngine;

public class DoorStart : MonoBehaviour
{
  private LevelManager levelManager;

  //============================================================

  private void Start()
  {
    levelManager = LevelManager.Instance;

    gameObject.GetComponent<SpriteRenderer>().sprite = levelManager.LevelData.DoorStart;
  }

  //============================================================
}