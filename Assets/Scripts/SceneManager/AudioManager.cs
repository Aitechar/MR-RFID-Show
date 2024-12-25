using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// 枚举类型
    /// </summary>
    public enum ClipEnum
    {
        Button,
        MenuOn,
        MenuOff,
        MenuChoice,
        Type,
        Error,
    }

    private static AudioManager _instance = null;
    private AudioSource _audioSource;

    public static AudioManager Instance { get => _instance; }
    private SettingManager Setting => SettingManager.Instance;

    [SerializeField] private int typeCount = 3;
    private int _typeCounter = 0;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }

        _instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayOneShot(ClipEnum clipEnum)
    {
        AudioClip targetClip = null;
        switch (clipEnum)
        {
            case ClipEnum.Button:
                {
                    var index = Random.Range(0, Setting.ButtonClips.Count);
                    targetClip = Setting.ButtonClips[index];
                    break;
                }
            case ClipEnum.Type:
                {
                    var index = Random.Range(0, Setting.TypeClips.Count);
                    targetClip = Setting.TypeClips[index];
                    break;
                }
            case ClipEnum.MenuOn:
                {
                    targetClip = Setting.MenuShowClip;
                    break;
                }
            case ClipEnum.MenuOff:
                {
                    targetClip = Setting.MenuHideClip;
                    break;
                }
            case ClipEnum.MenuChoice:
                {
                    targetClip = Setting.MenuChoiceChlip;
                    break;
                }
            case ClipEnum.Error:
                {
                    targetClip = Setting.ErrorClip;
                    break;
                }
        }
        if (targetClip != null) _audioSource.PlayOneShot(targetClip);
    }

    /// <summary>
    /// 计数器打字声
    /// </summary>
    public void PlayTypeShow()
    {
        _typeCounter--;
        if (_typeCounter < 0)
        {
            _typeCounter = typeCount;
            PlayOneShot(ClipEnum.Type);
        }
    }
}