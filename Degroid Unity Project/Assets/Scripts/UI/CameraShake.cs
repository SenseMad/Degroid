using UnityEngine;

/// <summary>
/// Даанный скрипт необъодимо поместить на камеру, для тряски экрана если это необходимо.
/// </summary>
public class CameraShake : MonoBehaviour
{
  public enum ShakeMode { OnlyX, OnlyY, OnlyZ, XY, XZ, XYZ };

  //============================================================

  private static Transform Transform;
  private static float elapsed, i_Duration, i_Power, percentComplete;
  private static ShakeMode i_Mode;
  private static Vector3 originalPos;

  //============================================================

  private void Start()
  {
    percentComplete = 1;
    Transform = GetComponent<Transform>();
  }

  private void Update()
  {
    if (elapsed < i_Duration)
    {
      elapsed += Time.deltaTime;
      percentComplete = elapsed / i_Duration;
      percentComplete = Mathf.Clamp01(percentComplete);
      Vector3 rnd = Random.insideUnitSphere * i_Power * (1f - percentComplete);

      switch (i_Mode)
      {
        case ShakeMode.XYZ:
          Transform.localPosition = originalPos + rnd;
          break;
        case ShakeMode.OnlyX:
          Transform.localPosition = originalPos + new Vector3(rnd.x, 0, 0);
          break;
        case ShakeMode.OnlyY:
          Transform.localPosition = originalPos + new Vector3(0, rnd.y, 0);
          break;
        case ShakeMode.OnlyZ:
          Transform.localPosition = originalPos + new Vector3(0, 0, rnd.z);
          break;
        case ShakeMode.XY:
          Transform.localPosition = originalPos + new Vector3(rnd.x, rnd.y, 0);
          break;
        case ShakeMode.XZ:
          Transform.localPosition = originalPos + new Vector3(rnd.x, 0, rnd.z);
          break;
      }
    }
  }

  //============================================================

  /// <summary>
  /// Тряска экрана
  /// </summary>
  /// <param name="duration">Продолжительность</param>
  /// <param name="power">Сила</param>
  public static void Shake(float duration, float power)
  {
    if (percentComplete == 1) 
      originalPos = Transform.localPosition;

    i_Mode = ShakeMode.XYZ;
    elapsed = 0;
    i_Duration = duration;
    i_Power = power;
  }

  /// <summary>
  /// Тряска экрана + режим
  /// </summary>
  /// <param name="duration">Продолжительность</param>
  /// <param name="power">Сила</param>
  /// <param name="mode">Режим</param>
  public static void Shake(float duration, float power, ShakeMode mode)
  {
    if (percentComplete == 1) 
      originalPos = Transform.localPosition;

    i_Mode = mode;
    elapsed = 0;
    i_Duration = duration;
    i_Power = power;
  }

  //============================================================
}