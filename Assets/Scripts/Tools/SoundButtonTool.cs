using UnityEngine;
using UnityEngine.UI;

namespace Aite.Tools
{

    [RequireComponent(typeof(Button))]
    public class SoundButtonTool : MonoBehaviour
    {
        public bool canPlay = true;
        public AudioManager.ClipEnum soundType = AudioManager.ClipEnum.Button;

        void ClickSound()
        {
            if (canPlay) AudioManager.Instance.PlayOneShot(soundType);
            
        }

        private void Awake()
        {
            if (TryGetComponent<Button>(out var btn))
            {
                btn.onClick.AddListener(ClickSound);
            }
        }
    }
}
