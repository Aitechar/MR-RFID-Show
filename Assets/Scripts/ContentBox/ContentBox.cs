using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ContentBox
{
    public enum ContentBoxEnum
    {
        Txt,
        Photo,
        Model
    }
    
    public static class ContentFac
    {
        public static string defaultPhotoPath = "Sprite/Default Photo";
        public static string defaultModelPath = "Prefabs/Default Model";

        private static Sprite _defaultPhoto; 
        private static GameObject _defaultModel; 

        public static Sprite GetSprite(PhotoContent photoContent)
        {
            var res = Resources.Load<Sprite>(photoContent.Path);
            if(res == null)
            {
                Debug.LogWarning($"加载Content图像资源时出错, Path: {photoContent.Path}, 替换为默认资源");
                if(_defaultPhoto == null) _defaultPhoto = Resources.Load<Sprite>(defaultPhotoPath);
                res = _defaultPhoto;
            }
            return res;
        }

        public static GameObject GetModel(ModelContent ModelContent)
        {
            var res = Resources.Load<GameObject>(ModelContent.Path);
            if (res == null)
            {
                Debug.LogWarning($"加载Content模型资源时出错, Path: {ModelContent.Path}, 替换为默认资源");
                if (_defaultModel == null) _defaultModel = Resources.Load<GameObject>(defaultModelPath);
                res = _defaultModel;
            }
            return res;
        }
    }

    [Serializable]
    public abstract class Content
    {
        public ContentBoxEnum ContentType;

        public abstract void LoadAsset();

        public abstract string display();
    }

    [Serializable]
    public class TxtContent : Content
    {
        public string Txt;
        public TxtContent(string txt) => (ContentType, Txt) = (ContentBoxEnum.Txt, txt);

        public override string display()
        {
            return Txt;
        }

        public override void LoadAsset() {}
    }

    [Serializable]
    public class PhotoContent : Content
    {
        public string Path;         // 路径
        public string Desc;         // 图片名称
        public float ScaleFactor;     // 缩放系数
        public int BoxPos;          // 移动盒子的位置

        public Sprite Photo;

        public PhotoContent(string path, string desc, float scale, int boxPos) => 
            (ContentType, Path, Desc, ScaleFactor, BoxPos) = (ContentBoxEnum.Photo, path, desc, scale, boxPos);

        public override string display() { return Path; }

        public override void LoadAsset()
        {
            Photo = ContentFac.GetSprite(this);
        }
    }

    [Serializable]
    public class ModelContent : Content
    {
        public string Path;
        public string Desc;

        public GameObject Prefabs;

        public ModelContent(string path, string desc) => (ContentType, Path, Desc) = (ContentBoxEnum.Model, path, desc);

        public override string display() { return Path; }

        public override void LoadAsset()
        {
            Prefabs = ContentFac.GetModel(this);
        }
    }


    /// <summary>
    /// Json ContentBox
    /// </summary>
    public class ContentBoxConverter : JsonConverter<Content>
    {
        public override void WriteJson(JsonWriter writer, Content value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Type");
            writer.WriteValue(value.ContentType);
            switch (value.ContentType)
            {
                case ContentBoxEnum.Txt:
                    {
                        if (value is TxtContent txtContent)
                        {
                            writer.WritePropertyName("Text");
                            writer.WriteValue(txtContent.Txt);
                        }
                        break;
                    }
                case ContentBoxEnum.Photo:
                    {
                        if (value is PhotoContent photoContent)
                        {
                            writer.WritePropertyName("Path");
                            writer.WriteValue(photoContent.Path);
                            writer.WritePropertyName("Desc");
                            writer.WriteValue(photoContent.Desc);
                            writer.WritePropertyName("ScaleFactor");
                            writer.WriteValue(photoContent.ScaleFactor);
                            writer.WritePropertyName("BoxPos");
                            writer.WriteValue(photoContent.BoxPos);
                        }
                        break;
                    }
                case ContentBoxEnum.Model:
                    {
                        if (value is ModelContent modelContent)
                        {
                            writer.WritePropertyName("Path");
                            writer.WriteValue(modelContent.Path);
                            writer.WritePropertyName("Desc");
                            writer.WriteValue(modelContent.Desc);
                        }
                        break;
                    }
            }
            writer.WriteEndObject();
        }

        public override Content ReadJson(JsonReader reader, Type objectType, Content existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var contentJson = JObject.Load(reader);

            return (int)contentJson["Type"] switch
            {
                0 => new TxtContent((string)contentJson["Text"]),

                1 => new PhotoContent((string)contentJson["Path"], 
                                      (string)contentJson["Desc"], 
                                      (float)contentJson["ScaleFactor"], 
                                      (int)contentJson["BoxPos"]),
                                      
                2 => new ModelContent((string)contentJson["Path"], 
                                      (string)contentJson["Desc"]),
                _ => throw new Exception($"序列化Content时,遇到了一个错误的类型： {(string)contentJson["Type"]}")
            };
        }
    }
}