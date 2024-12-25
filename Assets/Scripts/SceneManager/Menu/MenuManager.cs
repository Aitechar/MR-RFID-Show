using UnityEngine;
using Aite.Tools;


public class MenuManager : MonoBehaviour
{
    private static MenuManager _instance = null;
    public static MenuManager Instance { get => _instance; }

    [SerializeField] private Vector3 offset = new();
    [SerializeField] private float rotateAulge = 5f;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Typewriter typewriter;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetButtonDown("XRI_Left_MenuButton") || Input.GetButtonDown("XRI_Right_MenuButton") || Input.GetKeyDown(KeyCode.Space))
        {
            UpdateCanvasPos();
        }
    }


    /// <summary>
    ///  返回目录
    /// </summary>
    public void ButtonOnBackToCate()
    {
        SceneLoadManager.Instance.LoadScene(SceneLoadManager.SceneType.MR_Menu);
        UpdateCanvasPos();
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ButtonOnQuit()
    {
        SceneLoadManager.Instance.Quit();
    }

    /// <summary>
    /// 调出菜单的逻辑
    /// </summary>
    private void UpdateCanvasPos()
    {
        var cameraTrans = Camera.main.transform;

        // 计算菜单位置：摄像头的位置 + 偏移量
        var menuPos = cameraTrans.position + (cameraTrans.right * offset.x) + (cameraTrans.forward * offset.z);

        // 将菜单tp过去
        transform.position = menuPos;
        // 改变菜单的Active
        canvas.enabled = !canvas.enabled;

        if (canvas.enabled)
        {
            // 面板朝向
            transform.LookAt(cameraTrans);
            transform.Rotate(180 * Vector3.up);
            transform.Rotate(rotateAulge * Vector3.right);

            // 打印效果
            typewriter.OutputText();

            // 播放音效
            AudioManager.Instance.PlayOneShot(AudioManager.ClipEnum.MenuOn);
        }
        else
        {
            // 播放音效
            AudioManager.Instance.PlayOneShot(AudioManager.ClipEnum.MenuOff);
        }
    }

}