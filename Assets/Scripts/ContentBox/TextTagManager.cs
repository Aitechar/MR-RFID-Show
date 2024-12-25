using System.Collections.Generic;
using UnityEngine;

namespace ContentBox
{
    public class TextTagManager : MonoBehaviour
    {
        /// SerializeField
        [SerializeField] private GameObject tagPrefabs;
        [SerializeField] private Transform summonTagTransfrom;

        /// Private
        private Dictionary<string, TextTag> tagDic = new();
        private TextTag nowHoldTag;

        public void SummonTag(string text)
        {
            /// 重复的String标签，尝试跳过
            if (!SettingManager.Instance.SummonSampleTag && tagDic.ContainsKey(text))
            {
                Debug.Log("标签管理器: 尝试生成一个已经存在的标签, 由于设置的原因跳过此次生成");
                return;
            }
            
            /// 先前生成的标签没有被移动，直接更新
            if (nowHoldTag != null && nowHoldTag.IsHandel)
            {
                tagDic.Remove(nowHoldTag.Text);
                nowHoldTag.SetText(text);
                tagDic.Add(text, nowHoldTag);
            }
            /// 否则在目标位置生成一个新的标签
            else
            {
                nowHoldTag = GameObject.Instantiate(tagPrefabs, summonTagTransfrom).GetComponent<TextTag>();
                nowHoldTag.SetText(text);
                nowHoldTag.IsHandel = true;
                nowHoldTag.tagManager = this;

                tagDic.Add(text, nowHoldTag);
            }

        }

        /// <summary>
        /// Tag被移动时的回调
        /// </summary>
        public void OnTagSelect(TextTag tag)
        {
            if (tag.transform.position == nowHoldTag.transform.position)
            {
                nowHoldTag = null;
            }
        }
    }
}