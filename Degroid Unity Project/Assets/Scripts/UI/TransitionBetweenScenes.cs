using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionBetweenScenes : MonoBehaviour
{
  [SerializeField, Tooltip("Время смены сцены")]
  private float _sceneChangeTime = 1.0f;

  //------------------------------------------------------------

  /// <summary>
  /// True, если происходит переход сцены
  /// </summary>
  public bool IsSceneTransition { get; private set; }

  private CanvasGroup canvasGroup;

  //============================================================

  private void Awake()
  {
    canvasGroup = GetComponent<CanvasGroup>();
  }

  //============================================================

  /// <summary>
  /// Запустить смену сцены
  /// </summary>
  /// <param name="parNameScene">Название сцены</param>
  public void StartSceneChange(string parNameScene)
  {
    StartCoroutine(SceneFading(parNameScene));
  }

  /// <summary>
  /// Запустить смену сцены
  /// </summary>
  /// <param name="parNameScene">Индекс сцены</param>
  public void StartSceneChange(int parIndexScene)
  {
    StartCoroutine(SceneFading(parIndexScene));
  }

  //============================================================

  /// <summary>
  /// Затухание сцены
  /// </summary>
  private IEnumerator SceneFading(string parNameScene)
  {
    IsSceneTransition = true;

    //canvasGroup.alpha = 1.0f;

    while (canvasGroup.alpha < 1)
    {
      canvasGroup.alpha += Time.deltaTime;
      yield return null;
    }

    yield return new WaitForSeconds(_sceneChangeTime);

    //SceneManager.LoadScene(parNameScene);
    var asyncOperation = SceneManager.LoadSceneAsync(parNameScene, LoadSceneMode.Single);
    asyncOperation.allowSceneActivation = true;
  }

  /// <summary>
  /// Затухание сцены
  /// </summary>
  private IEnumerator SceneFading(int parIndexScene)
  {
    IsSceneTransition = true;

    //canvasGroup.alpha = 1.0f;

    while (canvasGroup.alpha < 1)
    {
      canvasGroup.alpha += Time.deltaTime;
      yield return null;
    }

    yield return new WaitForSeconds(_sceneChangeTime);

    //SceneManager.LoadScene(parIndexScene);
    var asyncOperation = SceneManager.LoadSceneAsync(parIndexScene, LoadSceneMode.Single);
    asyncOperation.allowSceneActivation = true;
  }

  private IEnumerator FadeOutAndLoadNextScene()
  {
    canvasGroup.alpha = 1;

    while (canvasGroup.alpha > 0)
    {
      canvasGroup.alpha -= Time.deltaTime / _sceneChangeTime;
      yield return null;
    }
  }

  //============================================================
}