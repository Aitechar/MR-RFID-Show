using System.Collections.Generic;
using UnityEngine;


public class SettingManager : MonoBehaviour
{
    private static SettingManager _instance = null;
    public static SettingManager Instance { get => _instance; }

    [Header("音频")]
    // public AudioClip ButtonClip;
    public List<AudioClip> ButtonClips = new();
    public List<AudioClip> TypeClips = new();

    public AudioClip MenuShowClip;
    public AudioClip MenuHideClip;
    public AudioClip MenuChoiceChlip;

    public AudioClip ErrorClip;

    public bool TypingPlaySound = true;

    [Header("内容")]
    public bool AutoPlayFirstContent = true;


    [Header("标签")]
    public bool SummonSampleTag = false;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }

        _instance = this;
    }
}