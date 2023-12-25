using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
  [SerializeField, Tooltip("Текст процента загрузки сцены")]
  private Text _progressText;

  [SerializeField, Tooltip("")]
  private Image _progressBar;

  //============================================================

  private static LoadScene Instance;

  private Animator Animator { get; set; }

  private AsyncOperation LoadSceneOperation { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// Текст процента загрузки сцены
  /// </summary>
  public Text ProgressText { get => _progressText; set => _progressText = value; }

  /// <summary>
  /// 
  /// </summary>
  public Image ProgressBar { get => _progressBar; set => _progressBar = value; }

  //------------------------------------------------------------

  public static bool ShouldPlayOpeningAnimation { get; set; } = false;
  public static bool IsActive { get; set; }

  //============================================================

  private void Awake()
  {
    Instance = this;

    Animator = GetComponent<Animator>();

    if (ShouldPlayOpeningAnimation)
    {
      Instance.Animator.SetTrigger("SceneEnd");
    }
  }

  private void Update()
  {
    if (LoadSceneOperation != null)
    {
      ProgressText.text = $"{Mathf.RoundToInt(LoadSceneOperation.progress * 100)}%";
      ProgressBar.fillAmount = Mathf.Lerp(ProgressBar.fillAmount, LoadSceneOperation.progress, Time.deltaTime * 5);
    }
  }

  //============================================================

  public static void SwitchToScene(string sceneName)
  {
    if (IsActive) { return; }
    Instance.Animator.SetTrigger("SceneStart");

    Instance.LoadSceneOperation = SceneManager.LoadSceneAsync(sceneName);
    Instance.LoadSceneOperation.allowSceneActivation = false;
    IsActive = true;
  }

  public static void SwitchToScene(int sceneIndex)
  {
    if (IsActive) { return; }
    Instance.Animator.SetTrigger("SceneStart");

    Instance.LoadSceneOperation = SceneManager.LoadSceneAsync(sceneIndex);
    Instance.LoadSceneOperation.allowSceneActivation = false;
    IsActive = true;
  }

  //============================================================

  public void OnAnimationOver()
  {
    ShouldPlayOpeningAnimation = true;
    LoadSceneOperation.allowSceneActivation = true;
    IsActive = false;
  }

  //============================================================
}