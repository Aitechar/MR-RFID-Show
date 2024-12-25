using System;
using TMPro;
using UnityEngine;

namespace ContentBox
{
    public class TextTag : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textBox;

        public string Text { get; set; } = "";
        public bool IsHandel { get; set; } = false;

        public TextTagManager tagManager;

        public void SetText(string text)
        {
            Text = text;
            _textBox.text = Text;
        }

        /// <summary>
        /// 被抓取以后触发回调， 释放为IsHandel状态
        /// </summary>
        public void SelectEnter()
        {
            if (!IsHandel) return;

            if (tagManager == null)
            {
                Debug.LogWarning($"Text Tag: {gameObject.name} 尝试释放改成isHandel状态时发生了一个错误，因为其找不到Tag管理器");
                return;
            }

            tagManager.OnTagSelect(this);
            IsHandel = false;
        }
    }
}