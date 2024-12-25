using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
namespace ContentBox
{
    [RequireComponent(typeof(XRSimpleInteractable))]
    [RequireComponent(typeof(BoxCollider))]
    public class ContentModelPanel : MonoBehaviour
    {
        public ContentModel Content { get; set; }

        private XRBaseInteractable grab;

        [SerializeField] private string title;
        [TextArea(3, 10)][SerializeField] private string desc;

        [SerializeField] private bool arrayLeft = true;
        [SerializeField] private Transform arrayPos;

        private void Start()
        {
            // grab = GetComponent<XRBaseInteractable>();

            // grab.hoverEntered.AddListener(HoverOnEnter);
            // grab.hoverExited.AddListener(HoverOnExit);
        }

        private void OnDestroy()
        {
            // if (grab != null)
            // {
            //     grab.hoverEntered.RemoveListener();
            //     grab.hoverExited.RemoveListener();
            // }
        }

        public void HoverOnEnter()
        {
            if (Content == null) return;
            Content.DisplayPanel(title, desc, arrayLeft, arrayPos.position);
        }

        public void HoverOnExit()
        {
            if (Content == null) return;
            Content.ClearPanel();
        }


    }

}
