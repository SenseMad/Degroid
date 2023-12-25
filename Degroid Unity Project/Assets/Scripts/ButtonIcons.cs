using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "ButtonIcons", menuName = "ButtonIcons")]
public class ButtonIcons : ScriptableObject
{
  [System.Serializable]
  public class DeviceButtons
  {
    [System.Serializable]
    public struct Button
    {
      public string Name;
      public Sprite Sprite;
    }

    [SerializeField]
    private string _name;
    [SerializeField]
    private List<Button> _buttons = new List<Button>();

    //private Dictionary<string, Button> _buttonsTable = null;

    public string Name
    {
      get { return _name; }
    }
    /*
    public void Init()
    {
      _buttonsTable = new Dictionary<string, Button>(_buttons.Count);
      foreach (var button in _buttons)
      {
        _buttonsTable[button.Name] = button;
      }
    }*/

    public Sprite GetSprite(string parButtonName)
    {
      return _buttons.FirstOrDefault((b) => b.Name == parButtonName).Sprite;
      //return _buttonsTable[parButtonName].Sprite;
    }
  }

  [SerializeField]
  private List<DeviceButtons> _buttons = new List<DeviceButtons>();
 /* private Dictionary<string, DeviceButtons> _buttonsTable = null;

  private void Init()
  {
    _buttonsTable = new Dictionary<string, DeviceButtons>(_buttons.Count);
    foreach (var button in _buttons)
    {
      button.Init();
      _buttonsTable[button.Name] = button;
    }
  }
  */
  public Sprite GetSprite(string parDeviceName, string parButtonName)
  {
    //Init();
    return _buttons.FirstOrDefault((b) => b.Name == parDeviceName)?.GetSprite(parButtonName);
    //return _buttonsTable[parDeviceName].GetSprite(parButtonName);
  }
}
