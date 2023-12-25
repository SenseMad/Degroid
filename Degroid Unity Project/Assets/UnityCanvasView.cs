using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PSME_View.Canvas
{
  /// <summary>
  /// Отображение "Холст"
  /// </summary>
  public class UnityCanvasView : MonoBehaviour
  {
    private RenderTexture renderTexture;

    private void Start()
    {
      renderTexture = new RenderTexture(1920, 1080, 24);
    }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.L))
      {
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes($"{Application.dataPath}/{Random.Range(0, 10000)}.png", bytes);

        RenderTexture.active = null;
        Destroy(texture);
      }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
      if (source.width != renderTexture.width || source.height != renderTexture.height)
      {
        if (renderTexture != null)
          DestroyImmediate(renderTexture);
        renderTexture = new RenderTexture(source.width, source.height, 24);
      }

      Graphics.Blit(source, renderTexture);
      Graphics.Blit(source, destination);
    }

    //==========================================================================
  }
}
