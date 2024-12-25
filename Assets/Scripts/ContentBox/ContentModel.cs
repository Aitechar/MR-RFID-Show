using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Aite.Tools;

namespace ContentBox
{
    public class ContentModel : MonoBehaviour
    {
        [SerializeField] private List<ContentModelPanel> panelList = new();
        [Header("文本框")]
        [SerializeField] private TextMeshProUGUI titleBox;
        [SerializeField] private TextMeshProUGUI descBox;
        [SerializeField] private TextMeshPro downText;
        private Typewriter _typewriter;

        [Header("面板")]
        [SerializeField] private Transform rotateObj;
        [SerializeField] private Transform rotateAxis;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float changeTime = 0.5f;
        [Range(0f, 180f)][SerializeField] private float offset = 0f;
        private Coroutine _changeCor = null;

        [SerializeField] private GameObject arrow;

        private void Start()
        {
            _typewriter = descBox.GetComponent<Typewriter>();
            foreach (var panel in panelList) panel.Content = this;

            arrow.SetActive(false);
            canvasGroup.alpha = 0;
        }

        private void FixedUpdate()
        {
            var cameraTrans = Camera.main.transform;
            /// 让下端字体一直朝着摄像头
            downText.transform.LookAt(cameraTrans);
            downText.transform.Rotate(180 * Vector3.up);

            /// 让面板一直朝着摄像头, 并且根据玩家与这个物体的位置进行移动
            var dirToTarget = rotateAxis.position - cameraTrans.position;
            Vector3 tangent = new(-dirToTarget.z, 0, dirToTarget.x);
            tangent = tangent.normalized;

            /// 局部旋转画布
            var targetRotation = Quaternion.LookRotation(tangent, Vector3.up);
            rotateObj.rotation = targetRotation;
            /// 修正
            var offsetRotation = Quaternion.Euler(0, offset + 90, 0);
            rotateObj.rotation *= offsetRotation;
        }

        /// <summary>
        /// 展示面板信息
        /// </summary>
        public void DisplayPanel(string title, string desc, bool arrayLeft, Vector3 arrayPos)
        {
            if (canvasGroup.alpha == 0)
            {
                if (_changeCor != null) StopCoroutine(_changeCor);
                _changeCor = StartCoroutine(HideShowCanvas(true));
            }

            titleBox.text = title;
            descBox.text = desc;
            _typewriter.OutputText();

            /// 设置箭头的位置与旋转
            arrow.transform.position = arrayPos;
            var currRotation = arrow.transform.eulerAngles;
            currRotation.z = arrayLeft ? 0f : 180;
            arrow.transform.eulerAngles = currRotation;
            arrow.SetActive(true);
        }

        /// <summary>
        /// 隐藏面板
        /// </summary>
        public void ClearPanel()
        {
            if (canvasGroup.alpha == 1)
            {
                if (_changeCor != null) StopCoroutine(_changeCor);
                _changeCor = StartCoroutine(HideShowCanvas(false));
            }

            titleBox.text = "";
            descBox.text = "";
            arrow.SetActive(false);
        }

        /// <summary>
        /// 用于显示/隐藏画布
        /// </summary>
        private IEnumerator HideShowCanvas(bool isShow)
        {
            var target = isShow ? 1f : 0f;
            var timer = changeTime;
            var startValue = canvasGroup.alpha;

            while (timer > 0)
            {
                timer -= Time.deltaTime;
                canvasGroup.alpha = math.lerp(startValue, target, 1 - timer / changeTime);
                yield return null;
            }

            canvasGroup.alpha = target;
            _changeCor = null;
        }
    }
}
