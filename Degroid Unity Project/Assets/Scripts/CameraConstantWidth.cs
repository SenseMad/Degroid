using UnityEngine;

/// <summary>
/// Keeps constant camera width instead of height, works for both Orthographic & Perspective cameras
/// Made for tutorial https://youtu.be/0cmxFjP375Y
/// </summary>
public class CameraConstantWidth : MonoBehaviour
{
  public Vector2 DefaultResolution = new Vector2(720, 1280);
  [Range(0f, 1f)] public float WidthOrHeight = 0;

  //------------------------------------------------------------

  private float initialSize;
  private float targetAspect;

  private float initialFov;
  private float horizontalFov = 120f;

  //============================================================

  private Camera Camera { get; set; }

  //============================================================

  private void Awake()
  {
    Camera = GetComponent<Camera>();
  }

  private void Start()
  {
    initialSize = Camera.orthographicSize;

    targetAspect = DefaultResolution.x / DefaultResolution.y;

    initialFov = Camera.fieldOfView;
    horizontalFov = CalcVerticalFov(initialFov, 1 / targetAspect);
  }

  private void Update()
  {
    if (Camera.orthographic)
    {
      float constantWidthSize = initialSize * (targetAspect / Camera.aspect);
      Camera.orthographicSize = Mathf.Lerp(constantWidthSize, initialSize, WidthOrHeight);
    }
    else
    {
      float constantWidthFov = CalcVerticalFov(horizontalFov, Camera.aspect);
      Camera.fieldOfView = Mathf.Lerp(constantWidthFov, initialFov, WidthOrHeight);
    }
  }

  //============================================================

  private float CalcVerticalFov(float hFovInDeg, float aspectRatio)
  {
    float hFovInRads = hFovInDeg * Mathf.Deg2Rad;

    float vFovInRads = 2 * Mathf.Atan(Mathf.Tan(hFovInRads / 2) / aspectRatio);

    return vFovInRads * Mathf.Rad2Deg;
  }

  //============================================================
}