using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace Aite.Tools
{
    public class AddButtonClickSound : EditorWindow
    {

        [MenuItem("Tools/Check and Add SoundButtonTool to Buttons")]
        public static void CheckAndAddSoundButtonTool()
        {
            // 获取所有场景中的游戏物体
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            // 遍历所有物体
            foreach (var obj in allObjects)
            {
                if (obj.TryGetComponent<Button>(out var button))
                {
                    if (obj.GetComponent<SoundButtonTool>() == null)
                    {
                        obj.AddComponent<SoundButtonTool>();
                        Debug.Log("Added SoundButtonTool to: " + obj.name);
                    }
                }
            }
            // 弹出提示
            Debug.Log("检查并添加按钮音效完成!");
        }
        [MenuItem("Tools/Check and Remove SoundButtonTool to Buttons")]
        public static void CheckRemoeSoundButtonTool()
        {
            // 获取所有场景中的游戏物体
            GameObject[] allObjects = FindObjectsOfType<GameObject>();

            foreach(var obj in allObjects)
            {
                if (obj.TryGetComponent<Button>(out var button))
                {
                    if (obj.TryGetComponent<SoundButtonTool>(out var buttonTool))
                    {
                        DestroyImmediate(buttonTool, true);
                        // obj.AddComponent<SoundButtonTool>();
                        Debug.Log("Remove SoundButtonTool to: " + obj.name);
                    }
                }
            }
            // 弹出提示
            Debug.Log("检查并移除按钮音效完成!");
        }
    }
}
