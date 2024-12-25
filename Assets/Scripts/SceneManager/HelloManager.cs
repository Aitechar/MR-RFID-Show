using UnityEngine;
using Aite.Tools;

public class HelloManager : MonoBehaviour
{
    [SerializeField] private Typewriter helloType;
    void Start()
    {
        helloType.OutputText("你好，RFID_");
    }
}
