using TMPro;
using UnityEngine;

public class UIListen : MonoBehaviour
{
    public TextMeshProUGUI statusText;

    public void OnEnable()
    {
        ButtonEvent.onButtonPressed += UpdateText;
    }
    public void OnDisable()
    {
        ButtonEvent.onButtonPressed -= UpdateText;
    }
    void UpdateText()
    {
        statusText.text = "Button Pressed";
    }
    //does the button know if the text exists? NO
    //but now it is listening!

}
