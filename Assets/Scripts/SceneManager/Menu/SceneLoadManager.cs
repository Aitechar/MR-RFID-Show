using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private static SceneLoadManager _instance = null;
    public static SceneLoadManager Instance { get => _instance; }

    public enum SceneType
    {
        MR_Menu,
        MR_Learn,
        RFID_1,
        RFID_3,
        RFID_4,
    }

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

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadScene(SceneType scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
}