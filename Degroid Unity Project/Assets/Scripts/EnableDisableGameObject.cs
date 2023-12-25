using UnityEngine;

public class EnableDisableGameObject : MonoBehaviour
{
  [SerializeField] private GameObject _target;
  [SerializeField] private bool _enable = false;

#if UNITY_EDITOR
  private void Reset()
  {
    _target = gameObject;
  }
#endif

  private void Awake()
  {
#if UNITY_PS4    
    _target.SetActive(_enable);
#endif
  }
}
