using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FontAsset : MonoBehaviour
{
  [SerializeField, Tooltip("������� �����")]
  private TMP_FontAsset _russianFont;
  [SerializeField, Tooltip("���������� �����")]
  private TMP_FontAsset _englishFont;
  [SerializeField, Tooltip("����������� �����")]
  private TMP_FontAsset _frenchFont;
  [SerializeField, Tooltip("�������� �����")]
  private TMP_FontAsset _japanFont;
  [SerializeField, Tooltip("�������� �����")]
  private TMP_FontAsset _germanFont;
  [SerializeField, Tooltip("��������� �����")]
  private TMP_FontAsset _spanishFont;
  [SerializeField, Tooltip("������������� �����")]
  private TMP_FontAsset _portugueseFont;
  [SerializeField, Tooltip("��������� �����")]
  private TMP_FontAsset _chineseFont;

  //============================================================

  /// <summary>
  /// �������� ����� ��� �����
  /// </summary>
  public TMP_FontAsset GetFont(Language language)
  {
    return language switch
    {
      Language.Russian => _russianFont,
      Language.English => _englishFont,
      Language.French => _frenchFont,
      Language.Japan => _japanFont,
      Language.German => _germanFont,
      Language.Spanish => _spanishFont,
      Language.Portuguese => _portugueseFont,
      Language.Chinese => _chineseFont,
      _ => _englishFont,
    };
  }

  //============================================================
}