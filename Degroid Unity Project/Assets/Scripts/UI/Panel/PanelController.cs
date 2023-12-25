using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
  [SerializeField, Tooltip("Текущая панель")]
  private Panel _currentPanel;

  [SerializeField, Tooltip("Список всех открытых панелей")]
  private List<Panel> _listAllOpenPanels = new List<Panel>();

  //============================================================

  /// <summary>
  /// Текущая панель
  /// </summary>
  public Panel CurrentPanel { get => _currentPanel; set => _currentPanel = value; }

  /// <summary>
  /// Список всех открытых панелей
  /// </summary>
  public List<Panel> ListAllOpenPanels { get => _listAllOpenPanels; set => _listAllOpenPanels = value; }

  //============================================================

  private void Update()
  {
    if (_currentPanel == null)
    {
      if (LevelManager.Instance != null)
        LevelManager.Instance.IsPause = false;
    }
  }

  //============================================================

  /// <summary>
  /// Показать панель
  /// </summary>
  public void ShowPanel(Panel panel)
  {
    CurrentPanel = panel;
    InternalShowCurrentPanel();
  }

  /// <summary>
  /// Скрыть панель
  /// </summary>
  public void HidePanel(Panel panel)
  {
    CurrentPanel = panel;
    InternalHideCurrentPanel();
  }

  /// <summary>
  /// Открыть и закрыть панель
  /// </summary>
  public void ShowInHidePanel(Panel panel)
  {
    HidePanel(CurrentPanel);
    AddPanelList(panel);
  }

  public void SetActivePanel(Panel panel)
  {
    HidePanel(CurrentPanel);

    CurrentPanel = panel;

    ShowPanel(CurrentPanel);
  }

  /// <summary>
  /// Добавить панель в общий список
  /// </summary>
  public void AddPanelList(Panel panel)
  {
    ShowPanel(panel);
    ListAllOpenPanels.Add(panel);
  }

  /// <summary>
  /// Закрытие панелей по порядку
  /// </summary>
  public void ClosePanel()
  {
    if (ListAllOpenPanels.Count > 1)
    {
      ListAllOpenPanels.Remove(CurrentPanel);
      HidePanel(CurrentPanel);
      CurrentPanel = ListAllOpenPanels[ListAllOpenPanels.Count - 1];
      ShowPanel(CurrentPanel);
    }
    else
    {
      ListAllOpenPanels.Remove(CurrentPanel);
      HidePanel(CurrentPanel);
      CurrentPanel = null;
    }
  }

  /// <summary>
  /// Не закрывать последнее окно
  /// </summary>
  public void DoNotCloseLastWindow()
  {
    if (ListAllOpenPanels.Count > 1)
    {
      ListAllOpenPanels.Remove(CurrentPanel);
      HidePanel(CurrentPanel);
      CurrentPanel = ListAllOpenPanels[ListAllOpenPanels.Count - 1];
      ShowPanel(CurrentPanel);
    }
  }

  /// <summary>
  /// Закрыть все панели
  /// </summary>
  public void CloseAllPanels()
  {
    for (int i = 0; i < ListAllOpenPanels.Count; i++)
    {
      ClosePanel();
    }
    ClosePanel();

    if (CurrentPanel != null)
    {
      HidePanel(CurrentPanel);
      ListAllOpenPanels = new List<Panel>();
      CurrentPanel = null;
    }
  }

  //============================================================

  /// <summary>
  /// Показать панель
  /// </summary>
  private void InternalShowCurrentPanel()
  {
    if (CurrentPanel == null) { return; }
    CurrentPanel.ShowPanel();
  }

  /// <summary>
  /// Скрыть панель
  /// </summary>
  private void InternalHideCurrentPanel()
  {
    if (CurrentPanel == null) { return; }
    CurrentPanel.HidePanel();
  }

  //============================================================
}