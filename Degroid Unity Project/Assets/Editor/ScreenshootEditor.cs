using UnityEditor;
using UnityEngine;

public class ScreenshootEditor
{
  [MenuItem("Tools/Screenshot")]
  public static void Screenshot()
  {
    string name = $"Screenshots/screenshot{System.DateTime.Now.ToString("MM-dd-yy (HH-mm-ss)")}.png";
    ScreenCapture.CaptureScreenshot(name);
  }
}
