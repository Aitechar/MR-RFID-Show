using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Aite.Tools;

public class ContentTxtManager : MonoBehaviour
{
    // [SerializeField] private int ShowCnt = 3;
    // [SerializeField] private float ShowMaxSize = 15f;
    // [SerializeField] private float ShowMinSize = 10f;

    [SerializeField] private Transform textGUIBox;

    // 现在正在展示的<TextGUI组件， 打字机组件>
    private List<Tuple<TextMeshProUGUI, Typewriter>> _textComs = new();
    private List<string> _showTextStk = new();

    // 当前的文本, 没有文本返回空
    public string CurText { get => _showTextStk.Count > 0 ? _showTextStk[^1] : null; }

    private void Awake()
    {
        InitCompList();
    }

    /// <summary>
    /// 展示下一句话
    /// </summary>
    public void ShowNextText(string text)
    {
        /// 先完成当前文本的打印
        // _textComs[0].Item2.CompleteOutput();

        for (int i = _textComs.Count - 1; i > 0; i--)
        { _textComs[i].Item1.text = _textComs[i - 1].Item1.text; }

        _textComs[0].Item2.OutputText(text);
        _showTextStk.Add(text);
    }

    /// <summary>
    /// 回溯上一句话
    /// </summary>
    public bool ShowLastText()
    {
        if (_showTextStk.Count <= 0) return false;

        for (int i = 0; i < _textComs.Count - 1; i++)
            _textComs[i].Item1.text = _textComs[i + 1].Item1.text;

        _showTextStk.RemoveAt(_showTextStk.Count - 1);
        _textComs[^1].Item1.text = (_showTextStk.Count >= _textComs.Count) ? _showTextStk[^_textComs.Count] : "";
        return true;
    }

    /// <summary>
    /// 注册组件信息到List中
    /// </summary>
    private void InitCompList()
    {
        int chileCount = textGUIBox.childCount;
        for (int i = 0; i < chileCount; i++)
        {
            var obj = textGUIBox.GetChild(i).gameObject;
            _textComs.Add(new(obj.GetComponent<TextMeshProUGUI>(), obj.GetComponent<Typewriter>()));
        }
    }
}
