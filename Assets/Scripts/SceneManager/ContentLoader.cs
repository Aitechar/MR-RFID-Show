using UnityEngine;
using ContentBox;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using UnityEditor;
using System.IO;
using Unity.Collections;
using TMPro;


public class ContentLoader : MonoBehaviour
{
    [SerializeField] private string JsonPath = "Json/Lesson";

    /// 保存所有要展示的内容
    public string title;
    [SerializeReference][HideInInspector] public List<Content> Contents;
    [SerializeField][ReadOnly] private int curIndex = 0;

    // 文本展示管理器
    [SerializeField] private ContentTxtManager txtManager;
    // 文本便签管理器
    [SerializeField] private TextTagManager tagManager;
    // 图片展示管理器
    [SerializeField] private ContentPhotoManager photoManager;
    // 模型展示管理器
    [SerializeField] private ContentModelManager modelManager;

    private bool AutoPlayFirst => SettingManager.Instance.AutoPlayFirstContent;

    private void Start()
    {
        if (AutoPlayFirst) ButtonOnShowNextContent();
        if (title != null) tagManager.SummonTag(title);
    }

    /// <summary>
    /// 显示下一条信息
    /// </summary>
    public void ButtonOnShowNextContent()
    {
        if (curIndex >= Contents.Count) return;

        // 避免第0项是图片或者模型的情况
        while (curIndex < Contents.Count && Contents[curIndex].ContentType != ContentBoxEnum.Txt)
        {
            // TODO 生成对应的图片与模型
            Debug.Log($"{curIndex}为 图片/模型!");
            curIndex++;
        }

        if (curIndex >= Contents.Count) return;

        if (Contents[curIndex].ContentType == ContentBoxEnum.Txt && Contents[curIndex] is TxtContent txtContent)
            txtManager.ShowNextText(txtContent.Txt);
        curIndex++;

        while (curIndex < Contents.Count && Contents[curIndex].ContentType != ContentBoxEnum.Txt)
        {
            // 图片生成图像
            if (Contents[curIndex].ContentType == ContentBoxEnum.Photo && Contents[curIndex] is PhotoContent photoContent)
            {
                photoManager.InstantiateAtRow(photoContent);
            }

            // 生成对应的图片与模型
            if (Contents[curIndex].ContentType == ContentBoxEnum.Model && Contents[curIndex] is ModelContent modelContent)
            {
                modelManager.InstantiateModel(modelContent.Prefabs);
            }

            Debug.Log($"{curIndex}为 图片/模型!");
            curIndex++;
        }
    }

    /// <summary>
    /// 显示上一条信息
    /// </summary>
    public void ButtonOnShowLastTextContent()
    {
        if (txtManager.ShowLastText())
        {
            curIndex--;
            while (curIndex >= 0 && Contents[curIndex].ContentType != ContentBoxEnum.Txt) curIndex--;
        }
    }

    /// <summary>
    /// 将当前信息收录在Tag
    /// </summary>
    public void ButtonOnSaveTextToTag()
    {
        var text = txtManager.CurText;
        
        if (text != null) tagManager.SummonTag(text);
        else if (title != null) tagManager.SummonTag(title);
    }

    private static List<string> GetTextList(List<Content> Contents)
    {
        List<string> res = new();
        foreach (var content in Contents)
        {
            if (content.ContentType == ContentBoxEnum.Txt && content is TxtContent txtContent)
                res.Add(txtContent.Txt);
        }
        return res;
    }

    /// <summary>
    /// 从Json中获取内容Box
    /// </summary>
    public static Document GetContentFromJson(string JsonPath)
    {

        // TODO 把这里改成直接从Object内加载，而不是从Resouces加载json文件
        var jsonAsset = Resources.Load<TextAsset>(JsonPath);
        if (jsonAsset == null) throw new Exception($"尝试从资源内加载 {JsonPath} 时发生了一个错误");

        var jsonSetting = new JsonSerializerSettings();
        jsonSetting.Converters.Add(new ContentBoxConverter());

        try
        {
            var jsonObj = JsonConvert.DeserializeObject<Document>(jsonAsset.text, jsonSetting);
            foreach (var box in jsonObj.Content)
            {
                Debug.Log($"获取到一个Box: {box.ContentType}, {box.display()}");
                box.LoadAsset();
            }
            // Debug.Log($"加载{JsonPath} 完成");
            return jsonObj;
        }
        catch (Exception e)
        {
            throw new Exception($"尝试从资源内加载 {JsonPath} 时发生了一个错误: {e}");
        }
    }

    /// <summary>
    /// 将信息保存回json中
    /// </summary>
    public static void SaveContentToJson(string JsonPath, string title, List<Content> contents)
    {
        string path = Path.Combine(Application.dataPath, "Resources", $"{JsonPath}.json");

        var doc = new Document() { Title = title, Content = contents };

        var jsonSetting = new JsonSerializerSettings();
        jsonSetting.Converters.Add(new ContentBoxConverter());

        try
        {
            var jsonStr = JsonConvert.SerializeObject(doc, Formatting.Indented, jsonSetting);
            File.WriteAllText(path, jsonStr);
            Debug.Log("JSON file updated successfully.");
        }
        catch (Exception e)
        {
            throw new Exception($"写入Json信息时发生了一个错误:{e}");
        }
    }

    #region 序列化相关

    public class Document
    {
        public string Title { get; set; }
        public List<Content> Content { get; set; }
    }
    public void LoadContents(bool clear = false)
    {
        if (Contents == null || clear)
        {
            var doc = GetContentFromJson(JsonPath);
            title = doc.Title;
            Contents = doc.Content;
        }
    }
    public void ClearContents() { Contents = null; }
    public void SaveContents() { SaveContentToJson(JsonPath, title, Contents); }

    #endregion

}

#if UNITY_EDITOR
[CustomEditor(typeof(ContentLoader))]
public class LessionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        var summon = (ContentLoader)target;

        // 清理，加载与保存
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear"))
            summon.ClearContents();
        if (GUILayout.Button("Load"))
            summon.LoadContents(true);
        if (GUILayout.Button("Save"))
            summon.SaveContents();
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        // 展示Content
        if (summon.Contents != null && summon.Contents.Count > 0)
        {
            EditorGUILayout.LabelField(summon.title);
            var width = GUILayout.Width(200);
            // GUILayout.BeginHorizontal();
            // GUILayout.Label("描述", width);
            // GUILayout.Label("路径");
            // GUILayout.EndHorizontal();

            foreach (var content in summon.Contents)
            {
                switch (content.ContentType)
                {
                    case ContentBoxEnum.Txt: break;
                    case ContentBoxEnum.Photo:
                        {
                            if (content is PhotoContent photoContent)
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Label(photoContent.Desc);

                                var sprite = (Sprite)EditorGUILayout.ObjectField(obj: photoContent.Photo, typeof(Sprite), false, width);
                                if (sprite != photoContent.Photo)
                                {
                                    var path = AssetDatabase.GetAssetPath(sprite);
                                    if (path.StartsWith("Assets/Resources/"))
                                    {
                                        path = RemoveExtension(path);
                                        photoContent.Path = path[(path.IndexOf("Resources") + "Resources ".Length)..];
                                        photoContent.Photo = sprite;
                                        summon.SaveContents();
                                    }
                                }
                                GUILayout.EndHorizontal();
                            }
                            break;
                        }
                    case ContentBoxEnum.Model:
                        {
                            if (content is ModelContent modelContent)
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Label(modelContent.Desc);
                                var prefab = (GameObject)EditorGUILayout.ObjectField(obj: modelContent.Prefabs, typeof(GameObject), false, width);
                                if (prefab != modelContent.Prefabs)
                                {
                                    var path = AssetDatabase.GetAssetPath(prefab);

                                    if (path.StartsWith("Assets/Resources/"))
                                    {
                                        path = RemoveExtension(path);
                                        Debug.Log("修改了目标的模型");
                                        modelContent.Path = path[(path.IndexOf("Resources") + "Resources ".Length)..];
                                        modelContent.Prefabs = prefab;
                                        summon.SaveContents();
                                    }
                                }
                                GUILayout.EndHorizontal();
                            }
                            break;
                        }
                    default: break;
                }
            }
        }
    }


    private static string RemoveExtension(string path)
    {
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
        string directoryPath = Path.GetDirectoryName(path);

        // 如果有目录，则拼接路径和文件名
        return Path.Combine(directoryPath, fileNameWithoutExtension);
    }
}
#endif