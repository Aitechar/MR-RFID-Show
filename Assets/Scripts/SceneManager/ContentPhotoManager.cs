using System.Collections.Generic;
using ContentBox;
using UnityEngine;

public class ContentPhotoManager : MonoBehaviour
{
    // TODO 后续改成ScriptTable 配置表获取
    [SerializeField] private GameObject photoContentPrefabs;

    // Dic <内容物, 游戏对象>
    private readonly Dictionary<PhotoContent, ContentPhoto> _photoContents = new();

    // 正在画廊上展示的游戏对象
    private readonly List<ContentPhoto> _rowList = new();

    // 游离在外面的游戏对象
    private readonly List<ContentPhoto> _outsideList = new();

    [SerializeField] private Transform rowBeginTran;
    [SerializeField] private int rowCount = 10;  // Row上的数目, [可选]超出的图片会被释放
    [SerializeField] private float rowGap = 10;  // Row上图片的间距
    [SerializeField] private bool summonRight = true;

    private void Update()
    {
        UpdateRowPos();
    }

    // 在画廊上生成一个图片预制体
    public void InstantiateAtRow(PhotoContent photoContent)
    {
        if (_photoContents.ContainsKey(photoContent))
        {
            Debug.Log("已经实例化的图片内容");
            return;
        }

        var obj = Instantiate(photoContentPrefabs, parent: rowBeginTran);
        obj.SetActive(false);
        var photoScp = obj.GetComponent<ContentPhoto>();
        _photoContents.Add(photoContent, photoScp);

        // 添加，然后移除溢出
        _rowList.Insert(0, photoScp);
        if (_rowList.Count > rowCount)
        {
            var remove = _rowList[^1];
            remove.gameObject.SetActive(false);
            _rowList.Remove(remove);
            _photoContents.Remove(remove.Content);
            Destroy(remove.gameObject, 1f);
        }

        // 更新值
        photoScp.UpdateValue(photoContent, this);
        obj.SetActive(true);
    }

    // 将画廊上的图片变成自由移动的状态
    public void OutPutRowPhoto(ContentPhoto scp)
    {
        if (!_rowList.Contains(scp)) return;

        _rowList.Remove(scp);
        _outsideList.Add(scp);
    }

    // 将游离的图片插入回画廊的开头
    public void InRowPhoto(ContentPhoto scp)
    {
        if (!_outsideList.Contains(scp)) return;
        _outsideList.Remove(scp);
        _rowList.Insert(0, scp);
    }

    // 更新画廊上所有的图片的坐标
    private void UpdateRowPos()
    {
        Vector3 pos = Vector3.zero;

        for (int i = 0; i < _rowList.Count; i++)
        {
            // Debug.Log(rowList[i].Width);
            var append = rowGap + _rowList[i].Width / 2 + (i != 0 ? _rowList[i - 1].Width / 2 : 0);

            pos.x += (summonRight ? 1 : -1) * append;
            _rowList[i].transform.localPosition = pos;
        }


    }
}
