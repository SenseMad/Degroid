using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
  [SerializeField, Tooltip("������� ������")]
  private Panel _currentPanel;

  [SerializeField, Tooltip("������ ���� �������� �������")]
  private List<Panel> _listAllOpenPanels = new List<Panel>();

  //============================================================

  /// <summary>
  /// ������� ������
  /// </summary>
  public Panel CurrentPanel { get => _currentPanel; set => _currentPanel = value; }

  /// <summary>
  /// ������ ���� �������� �������
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
  /// �������� ������
  /// </summary>
  public void ShowPanel(Panel panel)
  {
    CurrentPanel = panel;
    InternalShowCurrentPanel();
  }

  /// <summary>
  /// ������ ������
  /// </summary>
  public void HidePanel(Panel panel)
  {
    CurrentPanel = panel;
    InternalHideCurrentPanel();
  }

  /// <summary>
  /// ������� � ������� ������
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
  /// �������� ������ � ����� ������
  /// </summary>
  public void AddPanelList(Panel panel)
  {
    ShowPanel(panel);
    ListAllOpenPanels.Add(panel);
  }

  /// <summary>
  /// �������� ������� �� �������
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
  /// �� ��������� ��������� ����
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
  /// ������� ��� ������
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
  /// �������� ������
  /// </summary>
  private void InternalShowCurrentPanel()
  {
    if (CurrentPanel == null) { return; }
    CurrentPanel.ShowPanel();
  }

  /// <summary>
  /// ������ ������
  /// </summary>
  private void InternalHideCurrentPanel()
  {
    if (CurrentPanel == null) { return; }
    CurrentPanel.HidePanel();
  }

  //============================================================
}