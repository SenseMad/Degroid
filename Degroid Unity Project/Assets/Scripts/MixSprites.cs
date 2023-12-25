using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixSprites : MonoBehaviour
{
  [SerializeField, Tooltip("Спрайты предметов (Номер индекса соответствует индексу уровня)")]
  public List<Sprite> _itemSprites;

  public List<Sprite> ItemSprites1 = new List<Sprite>();

  //============================================================

  private void Awake()
  {
    //ChangeUpdate();
  }

  //============================================================

  private void ChangeUpdate()
  {
    List<Sprite> tempList = new List<Sprite>();
    List<Sprite> sprites = new List<Sprite>();

    tempList = _itemSprites;

    int num = tempList.Count;

    while (num > 0)
    {
      int rand = Random.Range(0, num);
      sprites.Add(tempList[rand]);
      tempList.RemoveAt(rand);
      num--;
    }

    ItemSprites1 = sprites;
  }

  //============================================================

}