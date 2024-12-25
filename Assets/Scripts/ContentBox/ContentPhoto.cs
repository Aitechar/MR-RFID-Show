using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ContentBox
{
    public class ContentPhoto : MonoBehaviour
    {
        public static ContentPhotoManager manager;

        public PhotoContent Content { get; private set; }

        [Header("移动盒子")]
        [SerializeField] private BoxLocalPos boxLocalType = BoxLocalPos.DownRight;

        [SerializeField] private Transform RightDownBoxTran;
        [SerializeField] private Vector3 RightDownOffset = new();
        [Space]
        [SerializeField] private Transform DownBoxTran;
        [SerializeField] private Vector3 DownBoxOffset = new();

        [Header("图片显示")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Rect spriteRect;
        private float pixelPreUnit = 0.1f;
        public float Width { get => spriteRect.width / pixelPreUnit * transform.localScale.x; }

        [Header("射线显示")]
        [SerializeField] private LineRenderer lineRender;
        [SerializeField] private Vector3 lineOffset = new();

        [Header("文本显示")]
        [SerializeField] private TextMeshPro textGui;
        [SerializeField] private float textHeightOffset = 0.5f;
        private Transform textTrans;

        [Header("动画")]
        [SerializeField] private Animator anim;
        public bool isRotate = false;

        private void Start()
        {
            // 获取相关参数
            GetScripts();
        }
        private void Update()
        {
            TryMove();
        }

        # region 图片相关内容物

        /// <summary>
        /// 更新图片的值
        /// </summary>
        /// <param name="content"></param>
        public void UpdateValue(PhotoContent content, ContentPhotoManager manager)
        {
            ContentPhoto.manager = ContentPhoto.manager != null ? ContentPhoto.manager : manager;

            Content = content;
            // 图像
            spriteRenderer.sprite = Content.Photo;
            // 文本
            textGui.text = Content.Desc;
            gameObject.name = $"{Content.Desc} - PhotoContent Object";
            // 盒子位置
            boxLocalType = Content.BoxPos == 1 ? BoxLocalPos.Down : BoxLocalPos.DownRight;
            // 缩放
            transform.localScale = new Vector3(Content.ScaleFactor, Content.ScaleFactor, Content.ScaleFactor);

            // XR Grab 插件初始化
            SetXRGrabScripts();
        }

        /// <summary>
        /// 获取对应的组件
        /// </summary>
        public void GetScripts()
        {
            spriteRect = spriteRenderer.sprite.rect;
            pixelPreUnit = spriteRenderer.sprite.pixelsPerUnit;

            textTrans = textGui.transform;
        }

        /// <summary>
        /// 将Box 射线与文本移动到指定位置
        /// </summary>
        public void TryMove()
        {
            var width = spriteRect.width / pixelPreUnit;
            var height = spriteRect.height / pixelPreUnit;

            switch (boxLocalType)
            {
                // 右下角
                case BoxLocalPos.DownRight:
                    {
                        DownBoxTran.gameObject.SetActive(false);
                        RightDownBoxTran.gameObject.SetActive(true);

                        RightDownBoxTran.localPosition = new Vector3(width / 2 + RightDownOffset.x, -height / 2 + RightDownOffset.y, RightDownOffset.z);

                        var boxLocalPos = RightDownBoxTran.localPosition + lineOffset;
                        lineRender.positionCount = 3;
                        lineRender.SetPosition(0, boxLocalPos + new Vector3(-width, 0, 0));
                        lineRender.SetPosition(1, boxLocalPos);
                        lineRender.SetPosition(2, boxLocalPos + new Vector3(0, height, 0));

                        break;
                    }
                // 正下方
                case BoxLocalPos.Down:
                    {
                        RightDownBoxTran.gameObject.SetActive(false);
                        DownBoxTran.gameObject.SetActive(true);

                        DownBoxTran.localPosition = new Vector3(DownBoxOffset.x, DownBoxOffset.y - height / 2, DownBoxOffset.z);

                        var boxLocalPos = DownBoxTran.localPosition + lineOffset;
                        lineRender.positionCount = 3;
                        lineRender.SetPosition(0, boxLocalPos + new Vector3(-width / 2, 0, 0));
                        lineRender.SetPosition(1, boxLocalPos + new Vector3(0, 0, 0));
                        lineRender.SetPosition(2, boxLocalPos + new Vector3(width / 2, 0, 0));
                        break;
                    }
            }

            // 文字位置同步
            textTrans.localPosition = new Vector3(0, -height / 2 + textHeightOffset, 0);
            // 动画旋转
            anim.SetBool("Rotate", isRotate);
        }
        #endregion

        # region XR交互相关

        private void SetXRGrabScripts()
        {
            var scp = GetComponent<XRGrabInteractable>();
            var targetTrans = boxLocalType switch
            {
                BoxLocalPos.DownRight => RightDownBoxTran,
                BoxLocalPos.Down => DownBoxTran,
                _ => throw new System.Exception(),
            };
            // 设置碰撞检测 与抓取位置
            scp.colliders.Clear();
            scp.colliders.Add(targetTrans.GetComponent<Collider>());
            
            scp.attachTransform = targetTrans;
        }

        /// <summary>
        /// 悬停进入
        /// </summary>
        public void OnHoverEnter()
        {
            Debug.Log("Hover Enter");
        }

        /// <summary>
        /// 悬停离开
        /// </summary>
        public void OnHoverExit()
        {

        }

        /// <summary>
        /// 扳机进入
        /// </summary>
        public void OnSelectEnter()
        {
            
        }

        /// <summary>
        /// 扳机离开
        /// 使其不再受到manager的控制
        /// </summary>
        public void OnSelectExit()
        {
            manager.OutPutRowPhoto(this);
        }

        # endregion
    }

    public enum BoxLocalPos
    {
        DownRight,
        Down,
    }

# if UNITY_EDITOR
    [CustomEditor(typeof(ContentPhoto))]
    public class ContentPhotoEditor : Editor
    {
        private ContentPhoto Target => (ContentPhoto)target;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            if (GUILayout.Button("Move Box To Target"))
            {
                Target.GetScripts();
                Target.TryMove();
            }
        }
    }
# endif

}