using System.Collections.Generic;
using Aite.Tools;
using TMPro;
using UnityEngine;

public class ContentModelQuestion : MonoBehaviour
{
    [TextArea(1, 3)][SerializeField] private string quastion;
    [TextArea(1, 3)][SerializeField] private string explain;

    [SerializeField] private TextMeshProUGUI quastionBox;
    [SerializeField] private TextMeshProUGUI explainBox;

    [SerializeField] private List<GameObject> choisesList = new();
    [SerializeField] private GameObject rightAns;

    [SerializeField] private Material wrongMaterial;
    [SerializeField] private Material rightMaterial;

    private void Start()
    {
        quastionBox.text = quastion;
        explainBox.text = "";
    }

    public void ShowAns()
    {
        foreach (var choice in choisesList)
        {
            var render = choice.GetComponent<Renderer>();
            render.material = wrongMaterial;
            if (choice == rightAns) render.material = rightMaterial;
        }
        explainBox.GetComponent<Typewriter>().OutputText(explain);
    }
}