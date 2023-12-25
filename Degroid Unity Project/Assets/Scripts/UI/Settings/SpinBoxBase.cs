using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class SpinBoxBase : MonoBehaviour
{
  [SerializeField] protected TextMeshProUGUI _fieldNameText;
  [SerializeField] protected Image _leftArrow;
  [SerializeField] protected Image _rightArrow;

  [SerializeField] private bool _enableLeft;
  [SerializeField] private bool _enableRight;
  [SerializeField] private bool isSelected;

  //------------------------------------------------------------

  private readonly float timeMoveNextValue = 0.2f; // Время перехода к следующему значения
  private float nextimeMoveNextValue = 0.0f;

  //------------------------------------------------------------

  private PlayerInputHandler inputHandler;
  
  //============================================================

  public bool EnableLeft
  {
    get => _enableLeft;
    set
    {
      _enableLeft = value;
      _leftArrow.color = _enableLeft ? ColorsGame.SELECTED_COLOR : ColorsGame.DISABLE_COLOR;
    }
  }
  public bool EnableRight
  {
    get => _enableRight;
    set
    {
      _enableRight = value;
      _rightArrow.color = _enableRight ? ColorsGame.SELECTED_COLOR : ColorsGame.DISABLE_COLOR;
    }
  }

  public bool IsSelected
  {
    get => isSelected;
    set
    {
      isSelected = value;
      if (isSelected) { OnSelected(); }
      else { OnDeselected(); }
    }
  }

  //============================================================

/*#if UNITY_EDITOR
  protected virtual void OnValidate()
  {
    EnableLeft = EnableLeft;
    EnableRight = EnableRight;
    IsSelected = IsSelected;
  }
#endif*/

  protected virtual void Awake()
  {
    inputHandler = PlayerInputHandler.Instance;
  }

  protected virtual void Update()
  {
    if (_leftArrow == null && _rightArrow == null) { return; }
    if (!IsSelected) { return; }

    if (Time.time > nextimeMoveNextValue)
    {
      nextimeMoveNextValue = Time.time + timeMoveNextValue;
      if (inputHandler.GetChangingValuesInput() > 0)
      {
        if (_enableRight)
        {
          OnRight();
        }
      }

      if (inputHandler.GetChangingValuesInput() < 0)
      {
        if (_enableLeft)
        {
          OnLeft();
        }
      }
    }

    if (inputHandler.GetChangingValuesInput() == 0)
    {
      nextimeMoveNextValue = Time.time;
    }
  }

  //============================================================

  protected abstract void OnLeft();
  protected abstract void OnRight();

  protected virtual void OnSelected()
  {
    _fieldNameText.color = ColorsGame.SELECTED_COLOR;

    if (_leftArrow == null && _rightArrow == null) { return; }
    _leftArrow.color = _enableLeft ? ColorsGame.SELECTED_COLOR : ColorsGame.DISABLE_COLOR;
    _rightArrow.color = _enableRight ? ColorsGame.SELECTED_COLOR : ColorsGame.DISABLE_COLOR;
  }

  protected virtual void OnDeselected()
  {
    _fieldNameText.color = ColorsGame.STANDART_COLOR;

    if (_leftArrow == null && _rightArrow == null) { return; }
    _leftArrow.color = _enableLeft ? ColorsGame.STANDART_COLOR : ColorsGame.DISABLE_COLOR;
    _rightArrow.color = _enableRight ? ColorsGame.STANDART_COLOR : ColorsGame.DISABLE_COLOR;
  }

  //============================================================
}