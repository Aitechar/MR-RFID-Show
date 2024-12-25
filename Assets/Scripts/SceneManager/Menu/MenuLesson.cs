using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
namespace Menu
{
    public class MenuLesson : MonoBehaviour
    {
        [Header("BOX")]
        [SerializeField] private Image imageBox;
        [SerializeField] private TextMeshProUGUI buttonTextBox;
        [SerializeField] private TextMeshProUGUI descTextBox;


        [Header("CONTENT")]
        [SerializeField] private Sprite image;
        [SerializeField] private string buttonText;
        [TextArea(3, 10)][SerializeField] private string descText;

        [Header("SCENE")]
        [SerializeField] private SceneLoadManager.SceneType scene;

        private void Start()
        {
            UpdateBoxContent();
        }

        public void UpdateBoxContent()
        {
            imageBox.sprite = image;
            buttonTextBox.text = buttonText;
            descTextBox.text = descText;
        }

        public void ButtonOnLoadScene()
        {
            SceneLoadManager.Instance.LoadScene(scene);
        }
    }
# if UNITY_EDITOR
    [CustomEditor(typeof(MenuLesson))]
    public class MenuLessonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            if (GUILayout.Button("Update Content"))
            {
                var summon = (MenuLesson)target;
                summon.UpdateBoxContent();
            }
        }
    }
#endif
}