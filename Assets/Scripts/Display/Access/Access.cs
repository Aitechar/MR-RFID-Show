using UnityEngine;
using UnityEngine.Playables;

namespace Aite.Display
{
    public class Access : MonoBehaviour
    {
        [SerializeField] private Animator _accessAnim;
        [SerializeField] private PlayableDirector _doorDir;
        
        [SerializeField] PlayableDirector _dir;

        public void ButtonOnReplayDir()
        {
            _dir.time = 0;
            _dir.Play();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Admin Card"))
            {
                AudioManager.Instance.PlayOneShot(AudioManager.ClipEnum.Button);
                // Debug.Log("Right Enter!");
                _accessAnim.SetTrigger("Right");
                _doorDir.Play();
            }
            else if (collision.gameObject.CompareTag("Vistor Card"))
            {
                AudioManager.Instance.PlayOneShot(AudioManager.ClipEnum.Error);                

                // Debug.Log("Wrong Enter");
                _accessAnim.SetTrigger("Wrong");
            }

        }
    }

}