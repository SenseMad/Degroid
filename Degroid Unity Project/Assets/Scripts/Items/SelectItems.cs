using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectItems : MonoBehaviour
{
  [SerializeField, Tooltip("����� ���������� ��������� ���������")]
  private TextMeshProUGUI _textNumberCollectedItems;

  [SerializeField, Tooltip("������ �������")]
  private Panel _profilePanel;

  [SerializeField, Tooltip("���� ����� ����������� ������")]
  private RectTransform _spacer;

  [SerializeField, Tooltip("������ ��������")]
  private GameObject _objectItems;

  [SerializeField, Tooltip("������ ���������")]
  private List<LevelData> _listItems = new List<LevelData>();

  [SerializeField, Tooltip("������� ���������")]
  private StoringItemSprites _storingItemSprites;

  [SerializeField, Tooltip("")]
  private LevelSelectMenu _levelSelectMenu;

  [SerializeField, Tooltip("")]
  private GameObject _topPanel;

  //------------------------------------------------------------

  public List<GameObject> ItemsImage = new List<GameObject>();

  /// <summary>
  /// ���������� ��������� ���������
  /// </summary>
  private int numberItemsCollected;

  //============================================================

  private GameManager GameManager { get; set; }

  private PlayerInputHandler InputHandler { get; set; }

  private PanelController PanelController { get; set; }

  private AudioManager AudioManager { get; set; }

  //------------------------------------------------------------

  /// <summary>
  /// ������ �������
  /// </summary>
  public Panel ProfilePanel { get => _profilePanel; set => _profilePanel = value; }

  /// <summary>
  /// ���� ����� ����������� ������
  /// </summary>
  public RectTransform Spacer { get => _spacer; set => _spacer = value; }

  /// <summary>
  /// ������ ��������
  /// </summary>
  public GameObject ObjectItems { get => _objectItems; set => _objectItems = value; }

  /// <summary>
  /// ������ ���������
  /// </summary>
  public List<LevelData> ListItems { get => _listItems; set => _listItems = value; }

  //============================================================

  private void Awake()
  {
    GameManager = GameManager.Instance;

    InputHandler = PlayerInputHandler.Instance;

    PanelController = FindObjectOfType<PanelController>();
  }

  private void OnEnable()
  {
    _topPanel.SetActive(false);

    AddItemsList();

    InputHandler.AI_Player.UI.Back.performed += OnClosePanel;
    InputHandler.AI_Player.UI.Pause.performed += OnClosePanel;
  }

  private void OnDisable()
  {
    _topPanel.SetActive(true);

    RemoveItemsList();

    InputHandler.AI_Player.UI.Back.performed -= OnClosePanel;
    InputHandler.AI_Player.UI.Pause.performed -= OnClosePanel;
  }

  private void Start()
  {
    AudioManager = AudioManager.Instance;
  }

  //============================================================

  private void Sound()
  {
    AudioManager.OnPlaySoundButton?.Invoke();
  }

  private void OnClosePanel(InputAction.CallbackContext context)
  {
    if (PanelController.CurrentPanel != ProfilePanel) { return; }

    Sound();
    PanelController.DoNotCloseLastWindow();
  }

  private void AddItemsList()
  {
    foreach (var listItems in _levelSelectMenu.ListLevelData)
    {
      GameObject listLanguageInstance = Instantiate(ObjectItems, Spacer);
      Image imageItem = listLanguageInstance.GetComponentInChildren<Button>().GetComponent<Image>();

      imageItem.sprite = _storingItemSprites.GetSprite(listItems.IndexLevel);
      imageItem.color = new Color32(0, 0, 0, 100);

      for (int i = 0; i < GameManager.CountCollectedItems.Count; i++)
      {
        if (listItems.IndexLevel == GameManager.CountCollectedItems[i])
        {
          imageItem.color = new Color32(255, 255, 255, 255);
          numberItemsCollected++;
        }
      }

      ItemsImage.Add(listLanguageInstance);
    }

    _textNumberCollectedItems.text = $"{numberItemsCollected}/{_levelSelectMenu.ListLevelData.Count}";
  }

  private void RemoveItemsList()
  {
    numberItemsCollected = 0;

    for (int i = 0; i < ItemsImage.Count; i++)
    {
      Destroy(ItemsImage[i]);
    }

    ItemsImage = new List<GameObject>();
  }

  //============================================================
}