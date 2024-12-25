using UnityEngine;

public class ContentModelManager : MonoBehaviour
{
    [SerializeField] private GameObject _emptyModel;

    private GameObject _lastSummonObject;

    private void Start()
    {
        _emptyModel.SetActive(true);
    }

    /// <summary>
    /// 在指定位置上生成对应的物品
    /// </summary>
    public void InstantiateModel(GameObject model)
    {
        CheckDestoryLastModel();
        _lastSummonObject = Instantiate(model, transform.position, Quaternion.identity, transform);

        _emptyModel.SetActive(false);
    }

    private void CheckDestoryLastModel()
    {
        if (_lastSummonObject != null)
            Destroy(_lastSummonObject);
    }
}