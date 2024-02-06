using UnityEngine;
using UnityEngine.UI;

public class CartelController : MonoBehaviour
{
    public Text billboardText;


    private void Start()
    {
        UpdateText("Jose Luis");
    }

    // Method to update the text on the billboard
    public void UpdateText(string newText)
    {
        billboardText.text = newText;
    }
}
